using HtmlAgilityPack;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Data
{
    public class AuthorsPersister
    {
        private static string baseUrl = "http://chitanka.info/authors/";
        private static string goodreadsBaseUrl = "http://www.goodreads.com/";
        private static string goodreadsKey = "Zf0fSMtDhxS4rDFCcNsw";

        public static IEnumerable<AuthorModel> GetByLetterFirstName(string letter)
        {
            string mainUrl = baseUrl + "first-name/" + letter;
            return GetAuthors(mainUrl);            
        }

        public static IEnumerable<AuthorModel> GetByLetterLastName(string letter)
        {
            string mainUrl = baseUrl + "last-name/" + letter;
            return GetAuthors(mainUrl);
        }

        private static IEnumerable<AuthorModel> GetAuthors(string mainUrl)
        {
            string responseHtml = HttpRequester.GetHtml(mainUrl);
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(responseHtml);
            List<AuthorModel> authors = new List<AuthorModel>();
            HtmlNodeCollection pagesUrlsNodes = document.DocumentNode.SelectNodes("//dl[@class='menu buttonmenu']//a");
            var list = pagesUrlsNodes.ToList();
            List<string> pagesUrlsList = new List<string>();
            pagesUrlsList.Add(mainUrl);

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

                GetAuthorsFromListNodes(authorLiNodes, authors);
            }

            return authors;
        }
  
        private static void GetAuthorsFromListNodes(List<HtmlNode> authorLiNodes, List<AuthorModel> authors)
        {
            foreach (var node in authorLiNodes)
            {
                string rootUrl = "http://chitanka.info";
                string name = null, origName = null, country = null, imageUrl = null, info = null, booksUrl = null;

                try
                {
                    booksUrl = rootUrl + node.SelectSingleNode(".//a[@itemprop='url']").Attributes["href"].Value.ToString();
                    name = node.SelectSingleNode(".//a[@itemprop='url']").InnerText;
                    origName = node.SelectSingleNode(".//dd[@class='orig-name']").InnerText;
                    country = node.SelectSingleNode(".//dd[@class='country']/a").InnerText;
                    //string id = GetGoodreadsId(origName, name);
                    //if (!string.IsNullOrEmpty(id))
                    //{
                    //    imageUrl = GetAuthorImage(id);
                    //    info = GetAuthorInfo(id);
                    //}
                }
                catch (Exception)
                {
                }

                AuthorModel newAuthor = new AuthorModel();
                newAuthor.Name = name != null ? name : newAuthor.Name;
                newAuthor.OrigName = origName != null ? origName : newAuthor.OrigName;
                newAuthor.Country = country != null ? country : newAuthor.Country;
                newAuthor.BooksUrl = booksUrl;
                newAuthor.Info = info != null ? info : newAuthor.Info;
                newAuthor.ImageUrl = imageUrl != null ? imageUrl : newAuthor.ImageUrl;

                authors.Add(newAuthor);
            }
        }

        public static IEnumerable<BookModel> GetAuthorBooks(string authorUrl )
        {
            List<BookModel> books = new List<BookModel>();
            string responseHtml = HttpRequester.GetHtml(authorUrl);
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(responseHtml);
            books = BooksPersister.GetBooksList(document);
            return books;
        }

        public static IEnumerable<AuthorModel> GetSearchResults(string searchQuery)
        {
            string url = "http://chitanka.info/search?q=" + searchQuery;
            string responseHtml = HttpRequester.GetHtml(url);
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(responseHtml);

            List<AuthorModel> authors = new List<AuthorModel>();
            HtmlNode h2node = document.DocumentNode.SelectNodes("//h2").FirstOrDefault();
            if (h2node.InnerText != "Личности")
            {
                return authors;
            }

            var liNodes = h2node.NextSibling.SelectNodes(".//li").ToList();
            GetAuthorsFromListNodes(liNodes, authors);

            return authors;
        }

        private static string GetAuthorInfo(string id)
        {
            string url = goodreadsBaseUrl + "author/show.xml?id=" + id + "&key=" + goodreadsKey;
            string xmlString = HttpRequester.GetXml(url);
            XmlDocument doc = GetXmlDocumet(xmlString);
            string authorInfo = doc.SelectSingleNode("//about").InnerText;
            authorInfo = Regex.Replace(authorInfo, "<.*?>", string.Empty);

            return authorInfo;
        }

        private static string GetAuthorImage(string id)
        {
            string url = goodreadsBaseUrl + "author/show.xml?id=" + id + "&key=" + goodreadsKey;
            string xmlString = HttpRequester.GetXml(url);
            XmlDocument doc = GetXmlDocumet(xmlString);
            string authorImageUrl = doc.SelectSingleNode("//image_url").InnerText;

            return authorImageUrl;
        }

        private static string GetGoodreadsId(string origName, string name)
        {
            string id = string.Empty;
            id = TryGetId(origName);

            if (string.IsNullOrEmpty(id))
            {
                id = TryGetId(name);
            }

            return id;
        }


        private static string TryGetId(string authorName)
        {
            string id = string.Empty;
            string url = goodreadsBaseUrl + "api/author_url/" + authorName + "?key=" + goodreadsKey;
            string xmlString = HttpRequester.GetXml(url);
            XmlDocument doc = GetXmlDocumet(xmlString);
            XmlNode authorNode = doc.SelectSingleNode("//author");
            if (authorNode != null)
            {
                id = authorNode.Attributes["id"].Value.ToString();
            }

            return id;
        }


        private static XmlDocument GetXmlDocumet(string xmlString)
        {
            XmlDocument doc = new XmlDocument();

            using (StringReader s = new StringReader(xmlString))
            {
                doc.Load(s);
            }

            return doc;
        }
    }
}
