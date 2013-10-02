using ChitankaServices.Attributes;
using Data;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ValueProviders;

namespace ChitankaServices.Controllers
{
    public class BooksController : ApiController
    {
        
        [HttpGet]
        [ActionName("all")]
        public HttpResponseMessage GetBooksByLetter(string letter)
        {
            var books = BooksPersister.GetAllBooksByLetter(letter);
            return Request.CreateResponse(HttpStatusCode.OK, books);
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

    }
}
