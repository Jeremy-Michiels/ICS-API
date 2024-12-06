using System.Text.Json;
using Flurl.Http;
using ITCCLMBSSA_API.Models;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace ITCCLMBSSA_API.Controllers{

    [ApiController]
    [Route("[controller]")]
    public class XOutlookApiController : ControllerBase{

        [HttpGet]
        [Route("GetBearerToken")]
        public async Task<AccessTokenReturn> GetBearerToken(){
            var app = PublicClientApplicationBuilder.Create(Conf.clientId)
            .WithRedirectUri("http://localhost")
            .Build();
            Console.WriteLine("App Gemaakt");
            string[] scopes = 
            {
                "https://graph.microsoft.com/user.read"
            };

            var tenantId = new Uri(Conf.tenantId);

            var bToken = await app.AcquireTokenInteractive(scopes)
                .WithTenantIdFromAuthority(tenantId)
                .ExecuteAsync();
            Console.WriteLine("Bearer token verkregen");

            //     string json = await Flurl.Url.Parse("https://graph.microsoft.com/v1.0/me")
            //         .WithOAuthBearerToken(bToken.AccessToken)
            //         .GetStringAsync();

            // Console.WriteLine(json);
            return new AccessTokenReturn{
                AccessToken = bToken.AccessToken,
                userName = bToken.Account.Username,
                expiresOn = bToken.ExpiresOn.DateTime,
            };
        }
        internal async Task<Models.GetSchedule.Return> GetSchedule(Models.GetSchedule.Post postItem, [FromHeader] string Bearer){
            string json = await Flurl.Url.Parse("https://graph.microsoft.com/v1.0/me/calendar/getschedule")
                    .WithOAuthBearerToken(Bearer)
                    .WithHeader("Prefer", "outlook.timezone=" + (char)34 + "Pacific Standard Time"+ (char)34)
                    .PostJsonAsync(postItem)
                    .ReceiveString();
            var item = JsonSerializer.Deserialize<Models.GetSchedule.Return>(json);

            return item;
        }
        internal async Task<Models.PostEvent.Return> PostEvent(Models.PostEvent.Post postItem, [FromHeader] string Bearer){
            string json = await Flurl.Url.Parse("https://graph.microsoft.com/v1.0/me/events")
                    .WithOAuthBearerToken(Bearer)
                    .PostJsonAsync(postItem)
                    .ReceiveString();
            var item = JsonSerializer.Deserialize<Models.PostEvent.Return>(json);
            return item;
        }
    }
}
