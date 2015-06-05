using JSONPatchWithServiceStack.Operations;
using JSONPatchWithServiceStack.Types;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using JSONPatchWithServiceStack.Extensions;

namespace JSONPatchWithServiceStack
{
    // from https://github.com/ServiceStack/ServiceStack.Examples/blob/master/src/StarterTemplates/StarterTemplates.Common/TodoService.cs

    public class EmployeeService : Service
    {
        public EmployeeService(EmployeeRepository repository)
        {
            this.Repository = repository;
        }

        public EmployeeRepository Repository { get; set; }  //Injected by IOC

        public object Get(Employee request)
        {
            if (request.Id == default(long))
                return Repository.GetAll();

            return Repository.GetById(request.Id);
        }

        public object Post(Employee emp)
        {
            return Repository.Store(emp);
        }

        public void Delete(Employee request)
        {
            Repository.DeleteById(request.Id);
        }

        public object Patch(EmployeePatch dto)
        {
            // partial updates

            // get from persistent data store by id from routing path
            var emp = Repository.GetById(dto.Id);

            if (emp != null)
            {
                // update values which are specified to update only
                emp.populateFromJsonPatch(dto);

                // update
                Repository.Store(emp);

            }
            
            // return HTTP Code and Location: header for the new resource
            // 204 No Content; The request was processed successfully, but no response body is needed.
            return new HttpResult()
            {
                StatusCode = HttpStatusCode.NoContent,
                Location = base.Request.AbsoluteUri,
                Headers = {
                    // allow jquery ajax in firefox to read the Location header - CORS
                    { "Access-Control-Expose-Headers", "Location" },
                }
            };
        }



    }
}
