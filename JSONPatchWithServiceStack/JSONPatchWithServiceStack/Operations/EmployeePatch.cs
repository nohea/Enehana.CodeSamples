using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JSONPatchWithServiceStack.Operations
{
    [Route("/employees/{Id}", "PATCH")]
    public class EmployeePatch : JsonPatchRequest
    {
        public long Id { get; set; }
    }
}
