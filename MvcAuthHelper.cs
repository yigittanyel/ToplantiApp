using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToplantiApp.Models;

namespace ToplantiApp
{
    public class MvcAuthHelper
    {
        private readonly HttpContextBase _httpContext;
        private readonly OutlookAuthData _authData;
        private readonly WebServerClient _client;
        private readonly OutgoingWebResponse _response;

        public MvcAuthHelper(HttpContextBase httpContext, OutlookAuthData authData)
        {
            _httpContext = httpContext;
            _authData = authData;

            var server = new AuthorizationServerDescription
            {
                AuthorizationEndpoint = new Uri("https://login.live.com/oauth20_authorize.srf"),
                TokenEndpoint = new Uri("https://login.live.com/oauth20_token.srf"),
                ProtocolVersion = ProtocolVersion.V20
            };

            _client = new WebServerClient(server, _authData.ClientId, _authData.ClientSecret);
            _response = _client.PrepareRequestUserAuthorization(_authData.Scopes, _authData.RedirectUri);
        }
    
            public bool IsAuthorized()
            {
                return !String.IsNullOrEmpty(_httpContext.Request.QueryString["code"]);
            }

            public object Authorize()
            {
                return _response.AsActionResultMvc5();
            }

            public string GetAccessToken()
            {
                return _client.ProcessUserAuthorization().AccessToken;
            }
    }
}