using HtmlAgilityPack;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class AuthorPersister
    {
        private static string baseUrl = "http://chitanka.info/authors/";

        private static string goodreadsKey = "Zf0fSMtDhxS4rDFCcNsw";

        public static IEnumerable<AuthorModel> GetByLetterFirstName(string letter)
        {
            string responseHtml = HttpRequester.GetHtml(baseUrl + "first-name/" + letter);
            List<AuthorModel> authors = new List<AuthorModel>();

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(responseHtml);

            HtmlNodeCollection pagesUrlsNodes = document.DocumentNode.SelectNodes("//dl[@class='menu buttonmenu']//a");
            var list = pagesUrlsNodes.ToList();
            List<string> pagesUrlsList = new List<string>();
            pagesUrlsList.Add(baseUrl + "first-name/" + letter);

            foreach (var node in list)
            {
                string url = "http://chitanka.info" + node.Attributes["href"].Value.ToString();
                if (!pagesUrlsList.Contains(url))
                {
                    pagesUrlsList.Add(url);
                }
            }

            foreach (var pageUrl in pagesUrlsList)
            {
                string html = HttpRequester.GetHtml(pageUrl);
                HtmlDocument pageHtml = new HtmlDocument();
                pageHtml.LoadHtml(html);

                var authorLiNodes = pageHtml.DocumentNode.SelectNodes("//ul[@class='superlist']/li").ToList();

                foreach (var node in authorLiNodes)
                {
                    string rootUrl = "http://chitanka.info";
                    string authorUrl = rootUrl + node.SelectSingleNode(".//a[@itemprop='url']").Attributes["href"].Value.ToString();
                    //HtmlDocument authorHtml = new HtmlDocument();
                    //authorHtml.LoadHtml(HttpRequester.GetHtml(authorUrl));

                    //if ( authorHtml.DocumentNode.SelectNodes("//ul[@class='superlist booklist']") == null)
                    //{
                    //    continue;
                    //}

                    string name = "", latinName = "", country = "", imageUrl = "";
                    var authorBooks = new List<BookModel>();

                    try
                    {
                        name = node.SelectSingleNode(".//a[@itemprop='url']").InnerText;

                        latinName = node.SelectSingleNode(".//dd[@class='orig-name']").InnerText;

                        country = node.SelectSingleNode(".//dd[@class='country']/a").InnerText;

                        //imageUrl = authorHtml.DocumentNode.SelectSingleNode(".//a[@class='image']").FirstChild.Attributes["src"].Value.ToString();

                        //authorBooks = BooksPersister.GetBooksList(authorHtml);
                        
                    }
                    catch (Exception)
                    { 
                    }

                    AuthorModel newAuthor = new AuthorModel
                    {
                        Name = name,
                        OrigName = latinName,
                        Country = country,
                        Books = authorBooks,
                        ImageUrl = imageUrl
                    };
                    authors.Add(newAuthor);
                }
            }

            return authors;
        }
    }
}
