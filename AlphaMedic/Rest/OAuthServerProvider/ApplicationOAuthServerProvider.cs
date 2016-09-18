using Microsoft.Owin.Security.OAuth;
using Rest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;

namespace Rest.OAuthServerProvider
{
    public class ApplicationOAuthServerProvider :
        OAuthAuthorizationServerProvider
    {
        private HttpClient client;
        private string urlParameters = "utc.json";

        public ApplicationOAuthServerProvider()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://chronic.heroku.com");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.UserAgent.Add(
            //    new ProductInfoHeaderValue(
            //    "Only a test!"));
        }

        private class TimeResponce
        {
            public string dateString { get; set; }
        }


        private string getchar(long number)
        {
            var a = number % 16;
            if (a < 10)
            {
                return a.ToString();
            }
            switch (a)
            {
                case 10: return "A";
                case 11: return "B";
                case 12: return "C";
                case 13: return "D";
                case 14: return "E";
                case 15: return "F";
            }
            return string.Empty;
        }

        private string hash(long number)
        {
            var res = string.Empty;
            long rest = number;
            for (var i = 0; i < 5; i++)
            {
                var a = rest / (long)Math.Pow(16, 4 - i);
                rest = rest % (long)Math.Pow(16, 4 - i);
                res += getchar(a);
            }
            return res;
        }

         
        public override async Task ValidateClientAuthentication(
            OAuthValidateClientAuthenticationContext context)
        {
            string key;
            var userName = context.Parameters.Get("username");
            var userProvider = new UserProvider(new Models.AlphaMedicContext.AlphaMedicContext());
            var user = await userProvider.FindByEmailAsync(userName);
            var mAuth = await userProvider.FindMAuthByIdAsync(user.UserId);
            try
            {
                key = context.Parameters.Get("client");
                if (key == null) throw new Exception();
            }
            catch (Exception)
            {
                context.SetError(
                    "invalid_grant",
                    "type of client is undefined"
                    );
                context.Rejected();
                return;
            }
            switch (key)
            {
                case "desktop":
                    {                        
                        if (mAuth == null|| (bool)!mAuth.IsUseAuth)
                        {
                            await Task.FromResult(context.Validated());
                            return;
                        }
                        else
                        {
                            var mac = long.Parse(mAuth.MacAdress);
                            var currentMinute = DateTime.Parse(DateTime.Now.ToString("g")).Ticks;

                            var hash = this.hash(currentMinute / mac);
                            if (hash == context.Parameters.Get("code"))
                            {
                                await Task.FromResult(context.Validated());
                                return;
                            }
                            else
                            {
                                context.SetError(
                                    "invalid_grant",
                                    "The authentification code is invalid"
                                    );
                                context.Rejected();
                                return;
                            }
                        }
                    }
                case "android":
                    {                       
                        if (mAuth == null)
                        {
                            try
                            {
                                var macAdress = context.Parameters.Get("mac");
                                if (macAdress == null)
                                    throw new Exception();
                                
                                await userProvider.AddMobileAuthentificatorAsync(
                                    new MobileAuthentificator
                                    {
                                        UserId = user.UserId,
                                        IsUseAuth = false,
                                        MacAdress = macAdress
                                    }
                                    );
                                await Task.FromResult(context.Validated());
                                return;
                            }
                            catch (Exception)
                            {
                                context.SetError(
                                    "invalid_grant",
                                    "client data is undefined"
                                );
                                context.Rejected();
                                return;
                            }
                        }
                        break;                      
                    }
                    await Task.FromResult(context.Validated());
            }
           
            HttpResponseMessage result = client.GetAsync(urlParameters).Result;
            if (result.IsSuccessStatusCode)
            {
                TimeResponce time = result.Content.ReadAsAsync<TimeResponce>().Result;
            }
            //var currentMinute = DateTime.Parse(DateTime.Now.ToString("g")).Ticks;            
            //var hash = this.hash(currentMinute);
            //if (hash == context.Parameters.First(x => x.Key == "code").Value[0])
            //{
            //    await Task.FromResult(context.Validated());
            //}
            //else
            //{
            //    context.SetError(
            //        "invalid_grant",
            //        "The user name or password is incorrect or user account is inactive."
            //        );
            //    context.Rejected();
            //    return;
            //}
            await Task.FromResult(context.Validated());
        }

        public override async Task GrantResourceOwnerCredentials(
            OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userProvider = new UserProvider(new Models.AlphaMedicContext.AlphaMedicContext());
            var user = await userProvider.FindByEmailAsync(context.UserName);
            if (user == null || user.Password != context.Password || user.Active == false)
            {
                context.SetError(
                    "invalid_grant",
                    "The user name or password is incorrect or user account is inactive."
                    );
                context.Rejected();
                return;
            }

            //var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            var identity = new ClaimsIdentity("JWT");
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim("id", user.UserId.ToString()));
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, user.UserClaim.ClaimValue));


            context.Validated(identity);
        }

        public override Task GrantAuthorizationCode(OAuthGrantAuthorizationCodeContext context)
        {

            return base.GrantAuthorizationCode(context);
        }
    }
}