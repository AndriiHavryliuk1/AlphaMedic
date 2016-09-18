using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Rest.Models;
using Rest.Models.AlphaMedicContext;
using Rest.OAuthServerProvider;
using System;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System.Configuration;

namespace Rest
{
    public partial class Startup
    {
        private void ConfigureAuth(IAppBuilder app)
        {

            //var issuer = "http://jwtauthzsrv.azurewebsites.net";
            //var audience = "099153c2625149bc8ecb3e85e03f0022";
            //var secret = TextEncodings.Base64Url.Decode("IxrAjDoa2FqElO7IhrSrUJELhUckePEPVpaePlS_Xaw");


            // Configure the db context and user manager to use a single instance per request
            //   app.CreatePerOwinContext(AlphaMedicContext.Create);
            // app.CreatePerOwinContext<User>(Users.Create);

            var issuer = "http://localhost:63741";
            var secret = TextEncodings.Base64Url.Decode(
                ConfigurationManager.AppSettings["as:AudienceSecret"]);


            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            var OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                // Provider = new CustomOAuthProvider(),
                AccessTokenFormat = new CustomJwtFormat(issuer),
                // Only do this for demo!!
                AllowInsecureHttp = true
            };
            app.UseOAuthAuthorizationServer(OAuthOptions);
            // app.UseOAuthBearerAuthentication(
            //       new OAuthBearerAuthenticationOptions());




            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { ConfigurationManager.AppSettings["as:AudienceId"] },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                       new SymmetricKeyIssuerSecurityTokenProvider(issuer, secret)
                    }
                }
                );
        }
    }
}