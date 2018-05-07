using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace EmployeeData.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    public class EmployeeApiController : ApiController
    {
        public IEnumerable<Employee> Get()
        {
            using (EmployeeDBEntities dbentities = new EmployeeDBEntities())
            {
                return dbentities.Employees.ToList();
            }
        }

        protected readonly EmployeeDBEntities entities = new EmployeeDBEntities();
               
        [HttpPost]
        public HttpResponseMessage Post([FromBody] Employee employee)
        {
            entities.Employees.Add(employee);
            //entities.SaveChanges();
            using (HttpClient client = new HttpClient())
            {
                var response = Request.CreateResponse(HttpStatusCode.Created, employee);
                response.Content = new StringContent(JsonConvert.SerializeObject(entities.SaveChanges()), Encoding.UTF8,
                    "application/json");
                return response;
            }
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int code)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.code == code);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Employee with Id = " + code.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.Employees.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPut]
        public HttpResponseMessage Put(int code,[FromBody] Employee employee)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.code == code);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Employee with Id " + code.ToString() + " not found to update");
                    }
                    else
                    {
                        entity.name = employee.name;
                        entity.gender = employee.gender;
                        entity.annualSalary = employee.annualSalary;
                        entity.dateOfBirth = employee.dateOfBirth;

                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpOptions]
        public HttpResponseMessage Options([FromBody] Employee employee)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                entities.Employees.Add(employee);
                entities.SaveChanges();

                var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                message.Headers.Location = new Uri(Request.RequestUri +
                    employee.code.ToString());

                return message;
            }
        }
    }
    }
