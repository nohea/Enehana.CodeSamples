using Funq;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JSONPatchWithServiceStack.App_Start
{
    public class AppHostHttpListener : AppHostHttpListenerBase
    {
        // to run on your local machine, run as Administrator on command prompt:
        // netsh http add urlacl url=http://127.0.0.1:57429/ user=DOMAIN\user
        // Otherwise, you have to run Visual Studio as Administrator and then run the tests.
        private const string BaseUrl = "http://127.0.0.1:57429/";

        protected string serviceUrl = "http://localhost:57429";

        /// <summary>
        /// Initializes a new instance of your ServiceStack application, with the specified name and assembly containing the services.
        /// </summary>
        public AppHostHttpListener()
            : base("JSON Patch with ServiceStack Host Self-Hosted", typeof(EmployeeService).Assembly)
        {
            Initialize();
        }

        public virtual void Initialize()
        {
            // uncomment to use Fiddler inspection
            // jsonServiceUrl = "http://localhost.fiddler:61234";

            Instance = null;

            Init();
            try
            {
                Start(BaseUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error trying to run ConsoleHost: " + ex.Message);
            }
        }

        /// <summary>
        /// Configure the container with the necessary routes for your ServiceStack application.
        /// </summary>
        /// <param name="container">The built-in IoC used with ServiceStack.</param>
        public override void Configure(Container container)
        {
            JsConfig.DateHandler = JsonDateHandler.ISO8601;

            container.Register(new EmployeeRepository());
        }
    }
}