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
    public class AuthorsController : ApiController
    {
        //
        // GET: /Authors/

        // api/authors/firstname?letter=
        [HttpGet]
        [ActionName("firstname")]
        public HttpResponseMessage GetBooksByFirstNameLetter(string letter)
        {
            var authors = AuthorsPersister.GetByLetterFirstName(letter);
            return Request.CreateResponse(HttpStatusCode.OK, authors);
        }

        // api/authors/lastname?letter=
        [HttpGet]
        [ActionName("lastname")]
        public HttpResponseMessage GetBooksByLastNameLetter(string letter)
        {
            var authors = AuthorsPersister.GetByLetterLastName(letter);
            return Request.CreateResponse(HttpStatusCode.OK, authors);
        }

        // api/authors/books?url=
        [HttpGet]
        [ActionName("books")]
        public HttpResponseMessage GetAuthorBooks(string url)
        {
            var authors = AuthorsPersister.GetAuthorBooks(url);
            return Request.CreateResponse(HttpStatusCode.OK, authors);
        }

        // api/authors?query=
        [HttpGet]
        public HttpResponseMessage GetSearchedAuthors(string query)
        {
            var authors = AuthorsPersister.GetSearchResults(query);
            return Request.CreateResponse(HttpStatusCode.OK, authors);
        }

    }
}
