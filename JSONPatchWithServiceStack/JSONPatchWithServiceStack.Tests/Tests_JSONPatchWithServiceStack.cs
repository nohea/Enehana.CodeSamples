using JSONPatchWithServiceStack.Types;
using ServiceStack.Common.Web;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JSONPatchWithServiceStack.Tests
{
    // http://xunit.github.io/docs/running-tests-in-vs.html
    public class Tests_JSONPatchWithServiceStack : JSONPatchWithServiceStack.App_Start.AppHostHttpListener
    {
        public Tests_JSONPatchWithServiceStack()
        {
        }

        [Fact]
        public void Test_GET_empty_PASS()
        {
            var restClient = new JsonServiceClient(serviceUrl);
            var emps = restClient.Get<List<Employee>>("/employees");

            Assert.NotNull(emps);
            Assert.Empty(emps);
        }

        [Fact]
        public void Test_GET_two_PASS()
        {
            var restClient = new JsonServiceClient(serviceUrl);

            var newemp1 = new Employee() { Id = 1, Name = "Joe", StartDate = new DateTime(2015, 1, 2), };
            var newemp2 = new Employee() { Id = 2, Name = "Julie", StartDate = new DateTime(2012, 12, 2), };
            restClient.Post<object>("/employees", newemp1);
            restClient.Post<object>("/employees", newemp2);

            var emps = restClient.Get<List<Employee>>("/employees");

            Assert.NotNull(emps);
            Assert.NotEmpty(emps);
            Assert.Equal(2, emps.Count);
        }

        [Fact]
        public void Test_PATCH_PASS()
        {
            var restClient = new JsonServiceClient(serviceUrl);

            // register callback to grab the Location: header when executed
            string lastResponseLocation = "";
            HttpStatusCode lastResponseStatusCode = 0;
            restClient.LocalHttpWebResponseFilter = httpRes =>
            {
                lastResponseLocation = httpRes.Headers[HttpHeaders.Location];
                lastResponseStatusCode = httpRes.StatusCode;
            };

            // dummy data
            var newemp1 = new Employee()
            {
                Id = 123,
                Name = "Kimo",
                StartDate = new DateTime(2015, 7, 2),
                CubicleNo = 4234,
                Email = "test1@example.com",
            };
            restClient.Post<object>("/employees", newemp1);

            var emps = restClient.Get<List<Employee>>("/employees");

            var emp = emps.First();

            var empPatch = new Operations.EmployeePatch();
            empPatch.Add(new Operations.JsonPatchElement()
            {
                op = "replace",
                path = "/title",
                value = "Kahuna Laau Lapaau",
            });

            empPatch.Add(new Operations.JsonPatchElement()
            {
                op = "replace",
                path = "/cubicleno",
                value = "32",
            });

            restClient.Patch<object>(string.Format("/employees/{0}", emp.Id), empPatch);

            var empAfterPatch = restClient.Get<Employee>(string.Format("/employees/{0}", emp.Id));

            Assert.NotNull(empAfterPatch);
            // patched
            Assert.Equal("Kahuna Laau Lapaau", empAfterPatch.Title);
            Assert.Equal("32", empAfterPatch.CubicleNo.ToString());
            // unpatched
            Assert.Equal("test1@example.com", empAfterPatch.Email);
        }

        [Fact]
        public void Test_PATCH_unsupported_cast_FAIL()
        {
            var restClient = new JsonServiceClient(serviceUrl);

            // dummy data
            var newemp1 = new Employee()
            {
                Id = 123,
                Name = "Kimo",
                StartDate = new DateTime(2015, 7, 2),
                CubicleNo = 4234,
                Email = "test1@example.com",
            };
            restClient.Post<object>("/employees", newemp1);

            var emps = restClient.Get<List<Employee>>("/employees");
            var emp = emps.First();

            var empPatch = new Operations.EmployeePatch();

            // double not currently supported by this example code
            empPatch.Add(new Operations.JsonPatchElement()
            {
                op = "replace",
                path = "/othernumber",
                value = "3.1415927",
            });

            Assert.Throws<WebServiceException>(delegate
            {
                // InvalidCastException
                restClient.Patch<object>(string.Format("/employees/{0}", emp.Id), empPatch);
            });
        }

        [Fact]
        public void Test_PATCH_unsupported_cast_PASS()
        {
            var restClient = new JsonServiceClient(serviceUrl);

            // dummy data
            var newemp1 = new Employee()
            {
                Id = 123,
                Name = "Kimo",
                StartDate = new DateTime(2015, 7, 2),
                CubicleNo = 4234,
                Email = "test1@example.com",
            };
            restClient.Post<object>("/employees", newemp1);

            var emps = restClient.Get<List<Employee>>("/employees");
            var emp = emps.First();

            var empPatch = new Operations.EmployeePatch();

            // float not currently supported by this example code
            empPatch.Add(new Operations.JsonPatchElement()
            {
                op = "replace",
                path = "/longitude",
                value = "2.123",
            });

            restClient.Patch<object>(string.Format("/employees/{0}", emp.Id), empPatch);
            
        }

    }
}
