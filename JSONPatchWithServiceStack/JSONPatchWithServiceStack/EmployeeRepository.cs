using JSONPatchWithServiceStack.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JSONPatchWithServiceStack
{
    // from https://github.com/ServiceStack/ServiceStack.Examples/blob/master/src/StarterTemplates/StarterTemplates.Common/TodoService.cs

    /// <summary>
    /// In-memory repository, so we can run the TODO app without any dependencies
    /// Registered in Funq as a singleton, injected on every request
    /// </summary>
    public class EmployeeRepository
    {
        private readonly List<Employee> employees = new List<Employee>();

        public List<Employee> GetAll()
        {
            return employees;
        }

        public Employee GetById(long id)
        {
            return employees.FirstOrDefault(x => x.Id == id);
        }

        public Employee Store(Employee emp)
        {
            if (emp.Id == default(long))
            {
                emp.Id = employees.Count == 0 ? 1 : employees.Max(x => x.Id) + 1;
            }
            else
            {
                for (var i = 0; i < employees.Count; i++)
                {
                    if (employees[i].Id != emp.Id) continue;

                    employees[i] = emp;
                    return emp;
                }
            }

            employees.Add(emp);
            return emp;
        }

        public void DeleteById(long id)
        {
            employees.RemoveAll(x => x.Id == id);
        }
    }
}