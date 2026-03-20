using DTPortal.Core.Constants;
using DTPortal.Core.Domain.Models;
using DTPortal.Core.Domain.Models.RegistrationAuthority;
using DTPortal.Core.Domain.Repositories;
using DTPortal.Core.Domain.Services;
using DTPortal.Core.Domain.Services.Communication;
using DTPortal.Core.DTOs;
using DTPortal.Core.Enums;
using DTPortal.Core.Exceptions;
using DTPortal.Core.Utilities;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using OtpNet;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Attribute = DTPortal.Core.DTOs.Attribute;

namespace DTPortal.Core.Services
{
    public class CredentialService : ICredentialService
    {
        private readonly ILogger<CredentialService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOrganizationService _organizationService;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _environment;
        private readonly IUserDataService _userDataService;
        private readonly ISelfServiceConfigurationService _selfServiceConfigurationService;
        private readonly ICacheClient _cacheClient;
        private readonly Helper _helper;
        private readonly MessageConstants Constants;
        private readonly IMessageLocalizer _messageLocalizer;
        private readonly IGlobalConfiguration _globalConfiguration;
        private readonly string _accessTokenHeaderName;
        public CredentialService(ILogger<CredentialService> logger,
            IUnitOfWork unitOfWork,
            HttpClient httpClient,
            IConfiguration configuration,
            IOrganizationService organizationService,
            IHttpClientFactory httpClientFactory,
            IEmailSender emailSender,
            ISelfServiceConfigurationService selfServiceConfigurationService,
            IWebHostEnvironment environment,
            IUserDataService userDataService,
            ICacheClient cacheClient, IGlobalConfiguration globalConfiguration,
            IMessageLocalizer messageLocalizer,
            Helper helper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            httpClient.BaseAddress = new Uri(configuration["APIServiceLocations:CredentialOfferBaseAddress"]);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _accessTokenHeaderName = "Authorization";
            _client = httpClient;
            _organizationService = organizationService;
            _httpClientFactory = httpClientFactory;
            _emailSender = emailSender;
            _environment = environment;
            _userDataService = userDataService;
            _selfServiceConfigurationService = selfServiceConfigurationService;
            _cacheClient = cacheClient;
            _globalConfiguration = globalConfiguration;
            _messageLocalizer = messageLocalizer;
            _helper = helper;

            var errorConfiguration = _globalConfiguration.
            GetErrorConfiguration();
            if (null == errorConfiguration)
            {
                _logger.LogError("Get Error Configuration failed");
                throw new NullReferenceException();
            }

            Constants = errorConfiguration.Constants;
            if (null == Constants)
            {
                _logger.LogError("Get Error Configuration failed");
                throw new NullReferenceException();
            }
        }


        public async Task<ServiceResult> GetCredentialOfferByUidAsync(string Id, string token)
        {
            try
            {
                if (_client.DefaultRequestHeaders.Contains(_accessTokenHeaderName))
                {
                    _client.DefaultRequestHeaders.Remove(_accessTokenHeaderName);
                }

                _client.DefaultRequestHeaders.Add(_accessTokenHeaderName, $"Bearer {token}");
                HttpResponseMessage response = await _client.GetAsync($"api/Credential/GetCredentialOfferByUid/{Id}");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    APIResponse apiResponse = JsonConvert.DeserializeObject<APIResponse>(await response.Content.ReadAsStringAsync());
                    if (apiResponse.Success)
                    {
                        return new ServiceResult(true, _messageLocalizer.GetMessage(Constants.CredentialListSuccess), apiResponse.Result);
                    }
                    else
                    {
                        _logger.LogError(apiResponse.Message);
                        return new ServiceResult(false, _messageLocalizer.GetMessage(Constants.InternalError));
                    }
                }
                else
                {
                    _logger.LogError($"The request with URI={response.RequestMessage.RequestUri} failed " +
                          $"with status code={response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ServiceResult(false, _messageLocalizer.GetMessage(Constants.InternalError));
              
            }
            return null;

        }
        //public async Task<ServiceResult> GetCredentialOfferByUid(string Id, string token)
        //{
        //    Accesstoken accessToken = null;
        //    try
        //    {
        //        accessToken = await _cacheClient.Get<Accesstoken>("AccessToken",
        //            token);
        //        if (null == accessToken)
        //        {
        //            _logger.LogError("Access token not recieved from cache." +
        //                "Expired or Invalid access token");
        //            return new ServiceResult(false, _messageLocalizer.GetMessage(Constants.UnAuthorized));
        //        }
        //    }
        //    catch (CacheException ex)
        //    {
        //        _logger.LogError("Failed to get Access Token Record");
        //        ErrorResponseDTO error = new ErrorResponseDTO();
        //        error.error = "Internal Error";
        //        error.error_description = _helper.GetRedisErrorMsg(
        //            ex.ErrorCode, ErrorCodes.REDIS_ACCESS_TOKEN_GET_FAILED);
        //        return new ServiceResult(false, error.error_description);
        //    }
        //    try
        //    {


        //        var credential = GetCredentialOfferByUidAsync(Id,token);

        //        if (credential == null)
        //        {
        //            _logger.LogInformation(Id + " : Credential Data Not Found");
        //            return new ServiceResult(false, _messageLocalizer.GetMessage(Constants.CredentialNotFound));
        //        }

        //        JObject json = JObject.Parse(credential);

        //        var obj1 = json[credential.OrganizationId]["supportedCredentials"][0]["credentialType"];

        //        var jsonObject = obj1.ToString();

        //        var attributeslist = json[credential.OrganizationId]["supportedCredentials"][0][jsonObject];

        //        var attributes = JsonConvert.DeserializeObject<List<Attribute>>(attributeslist.ToString());

        //        var supportedCredentials = new Dictionary<string, object>();

        //        supportedCredentials["credentialId"] = json[credential.OrganizationId]["supportedCredentials"][0]["credentialId"].ToString();

        //        supportedCredentials["credentialType"] = json[credential.OrganizationId]["supportedCredentials"][0]["credentialType"].ToString();

        //        supportedCredentials["trustUrl"] = json[credential.OrganizationId]["supportedCredentials"][0]["trustUrl"].ToString();

        //        supportedCredentials["isoNamespace"] = json[credential.OrganizationId]["supportedCredentials"][0]["isoNamespace"].ToString();

        //        var typeToken = json[credential.OrganizationId]["supportedCredentials"][0]["type"];

        //        var typeList = typeToken.ToObject<List<string>>();

        //        supportedCredentials["type"] = typeList;

        //        supportedCredentials["schema"] = json[credential.OrganizationId]["supportedCredentials"][0]["schema"].ToString();

        //        var typeToken1 = json[credential.OrganizationId]["supportedCredentials"][0]["format"];

        //        var typeList1 = typeToken1.ToObject<List<string>>();

        //        supportedCredentials["format"] = typeList1;

        //        supportedCredentials["proofType"] = json[credential.OrganizationId]["supportedCredentials"][0]["proofType"].ToString();

        //        var revocation = new Revocation()
        //        {
        //            Type = json[credential.OrganizationId]["supportedCredentials"][0]["revocation"]["type"].ToString(),

        //            RevocationListURL = json[credential.OrganizationId]["supportedCredentials"][0]["revocation"]["revocationListURL"].ToString()
        //        };

        //        supportedCredentials["revocation"] = revocation;

        //        supportedCredentials[jsonObject] = attributes;

        //        Dictionary<string, object> CredentialDetails = new Dictionary<string, object>();

        //        CredentialDetails["id"] = json[credential.OrganizationId]["id"].ToString();

        //        CredentialDetails["issuerName"] = json[credential.OrganizationId]["IssuerName"].ToString();

        //        CredentialDetails["issuerKey"] = json[credential.OrganizationId]["issuerKey"].ToString();

        //        CredentialDetails["issuerCertificateChain"] = json[credential.OrganizationId]["issuerCertificateChain"].ToString();

        //        CredentialDetails["supportedCredentials"] = supportedCredentials;

        //        return new ServiceResult(true, _messageLocalizer.GetMessage(Constants.CredentialListSuccess), CredentialDetails);
        //    }
        //    catch (Exception error)
        //    {
        //        _logger.LogError("Get Credential By Id::Database exception: {0}", error);

        //        return new ServiceResult(false, _messageLocalizer.GetMessage(Constants.InternalError));
        //    }
        //}



    }
}