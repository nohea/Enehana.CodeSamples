using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.Configuration;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;

namespace SSHelloWorldAuth
{
    public class AcmeCredentialsAuthProvider : CredentialsAuthProvider
    {
        public AcmeCredentialsAuthProvider(AppSettings appSettings) : base(appSettings) { }

        public override bool TryAuthenticate(IServiceBase authService, string userName, string password)
        {
            //Add here your custom auth logic (database calls etc)
            //Return true if credentials are valid, otherwise false
            if (userName == "testuser" && password == "Test2#4%")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void OnAuthenticated(IServiceBase authService, IAuthSession session, IOAuthTokens tokens, Dictionary<string, string> authInfo)
        {
            //Fill the IAuthSession with data which you want to retrieve in the app eg:
            session.FirstName = "some_firstname_from_db";
            //...

            //Important: You need to save the session!
            authService.SaveSession(session, SessionExpiry);
        }
    }
}
