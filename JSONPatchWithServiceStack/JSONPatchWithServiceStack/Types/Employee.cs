using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JSONPatchWithServiceStack.Types
{
    [Route("/employees", "GET,POST")]
    [Route("/employees/{Id}", "GET,PUT")]
    public class Employee
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public int? CubicleNo { get; set; }
        public DateTime StartDate { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
    }

}