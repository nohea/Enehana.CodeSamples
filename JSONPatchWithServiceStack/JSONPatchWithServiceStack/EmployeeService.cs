using JSONPatchWithServiceStack.Operations;
using JSONPatchWithServiceStack.Types;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

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
                // read from request dto properties
                var properties = emp.GetType().GetProperties();

                // update values which are specified to update only
                foreach (var op in dto)
                {
                    string fieldName = op.path.Replace("/", "").ToLower(); // assume leading /slash only for example

                    // patch field is in type
                    if (properties.ToList().Where(x => x.Name.ToLower() == fieldName).Count() > 0)
                    {
                        var persistentProperty = properties.ToList().Where(x => x.Name.ToLower() == fieldName).First();

                        // update property on persistent object
                        // i'm sure this can be improved, but you get the idea...
                        if (persistentProperty.PropertyType == typeof(string))
                        {
                            persistentProperty.SetValue(emp, op.value, null);
                        }
                        else if (persistentProperty.PropertyType == typeof(int))
                        {
                            int valInt = 0;
                            if (Int32.TryParse(op.value, out valInt))
                            {
                                persistentProperty.SetValue(emp, valInt, null);
                            }
                        }
                        else if (persistentProperty.PropertyType == typeof(int?))
                        {
                            int valInt = 0;
                            if (op.value == null)
                            {
                                persistentProperty.SetValue(emp, null, null);
                            }
                            else if (Int32.TryParse(op.value, out valInt))
                            {
                                persistentProperty.SetValue(emp, valInt, null);
                            }
                        }
                        else if (persistentProperty.PropertyType == typeof(DateTime))
                        {
                            DateTime valDt = default(DateTime);
                            if (DateTime.TryParse(op.value, out valDt))
                            {
                                persistentProperty.SetValue(emp, valDt, null);
                            }
                        }
                        else
                        {
                            throw new InvalidCastException(string.Format("type {0} conversion not supported yet", persistentProperty.PropertyType.ToString()));
                        }

                    }
                }

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
