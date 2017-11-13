using Autodesk.Forge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;

namespace forgeViewerTest.Controllers
{
    public class TokenController : ApiController
    {
        [HttpGet]
        [Route("api/forge/token")]

        public async Task<HttpResponseMessage> GetToken()
        {
            TwoLeggedApi oauthApi = new TwoLeggedApi();
            dynamic bearer = await oauthApi.AuthenticateAsync(WebConfigurationManager.AppSettings["FORGE_CLIENT_ID"],
                WebConfigurationManager.AppSettings["FORGE_CLIENT_SECRET"],
                "client_credentials", new Scope[] { Scope.DataRead }
                );

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(bearer.access_token, System.Text.Encoding.UTF8, "text/plain");
            return response;
        }
    }
}
