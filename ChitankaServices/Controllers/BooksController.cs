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
        //
        // GET: /Books/
        //public IEnumerable<string> GetAllByLetter(char letter)        
        //{
        //    //string rootUrl = "http://chitanka.info/books/alpha/" + letter;
        //    //string responseHtml = await HttpRequester.Get<string>(rootUrl);
        //    //HtmlDocument document = new HtmlDocument();
        //    //document.LoadHtml(responseHtml);            
            
        //    //HtmlNode contentMenu = (from d in document.DocumentNode.Descendants()
        //    //                        where d.Name == "dl" && d.Attributes["class"].Value == "menu buttonmenu"
        //    //                        select d).First();

        //    //IEnumerable<string> pagesUrlsList = from d in contentMenu.Descendants() 
        //    //                                  where d.Name == "a"
        //    //                                  select d.Attributes["href"].Value.ToString();

        //    //pagesUrlsList = pagesUrlsList.ToList().Distinct();


        //    //foreach (var url in pagesUrlsList)
        //    //{
        //    //    string currHtmlResponse = await HttpRequester.Get<string>(url);
        //    //    HtmlDocument currDocument = new HtmlDocument();
        //    //    currDocument.LoadHtml(currHtmlResponse);

        //    //    HtmlNode booksUl = (from d in document.DocumentNode.Descendants()
        //    //                        where d.Name == "ul" && d.Attributes["class"].Value == "superlist booklist"
        //    //                        select d).First();
                
    
        //    //}
        //}

        

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
