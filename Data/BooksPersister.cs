using HtmlAgilityPack;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class BooksPersister
    {
        private static string baseUrl = "http://chitanka.info/books/";

        public static IEnumerable<BookModel> GetAllBooksByLetter(string letter)
        {
            string responseHtml = HttpRequester.GetHtml(baseUrl + "alpha/" + letter);
            List<BookModel> allBooksList = new List<BookModel>();

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(responseHtml);

            HtmlNodeCollection pagesUrlsNodes = document.DocumentNode.SelectNodes("//dl[@class='menu buttonmenu']//a");

            var list = pagesUrlsNodes.ToList();
            List<string> pagesUrlsList = new List<string>();
            pagesUrlsList.Add(baseUrl + "alpha/" + letter);
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

                allBooksList.AddRange(GetBooksList(pageHtml));                
            }

            return allBooksList;
        }

        public static List<BookModel> GetBooksList(HtmlDocument pageHtml)
        {
            List<BookModel> books = new List<BookModel>();
            var booksLiNodes = pageHtml.DocumentNode.SelectNodes("//ul[@class='superlist booklist']/li").ToList();

            foreach (var node in booksLiNodes)
            {
                string rootUrl = "http://chitanka.info";
                string infoUrl = "", imageUrl = "", title = "", author = "", 
                genre = "", epubUrl = "", txtUrl = "";
                     
                try
                {
                    genre = node.SelectSingleNode(".//a[@rel='category tag']").InnerText;
                    if (genre == "Разкази в картинки")
                    {
                        continue;
                    }
                    infoUrl = rootUrl + node.SelectSingleNode(".//a[@class='booklink']").Attributes["href"].Value.ToString();
                    imageUrl = node.SelectSingleNode(".//img[@class='cover']").Attributes["src"].Value.ToString();
                    title = node.SelectSingleNode(".//i[@itemprop='name']").InnerText;
                    author = node.SelectSingleNode(".//a[@itemprop='name']").InnerText;
                    epubUrl = rootUrl + node.SelectSingleNode(".//a[@class='dl dl-epub action']").Attributes["href"].Value.ToString();
                    txtUrl = rootUrl + node.SelectSingleNode(".//a[@class='dl dl-txt action']").Attributes["href"].Value.ToString();
                }
                catch (NullReferenceException e)
                { 
                }

                BookModel newBook = new BookModel
                {
                    InfoUrl = infoUrl,
                    ImageUrl = imageUrl,
                    Title = title,
                    Author = author,
                    Category = genre,
                    EpubDownloadUrl = epubUrl,
                    TxtDownloadUrl = txtUrl
                };

                books.Add(newBook);
            }

            return books;
        }

        //public static IEnumerable<BookModel> GetBooksCategory(string categoryName)
        //{
        //    string responseHtml = HttpRequester.GetHtml(baseUrl + "category/" + categoryName);
        //    List<BookModel> books = new List<BookModel>();

        //    HtmlDocument document = new HtmlDocument();
        //    document.LoadHtml(responseHtml);
        //}

        public static string GetAnnotation(string bookUrl)
        {
            string responseHtml = HttpRequester.GetHtml(bookUrl);            
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(responseHtml);
            var annotationFieldset = document.DocumentNode.SelectSingleNode("//fieldset[@class='annotation']");
            if (annotationFieldset == null)
            {
                return "Книгата няма налична анотация.";
            }
            
            var pNodesList = annotationFieldset.SelectNodes(".//p").ToList();
            StringBuilder annotation = new StringBuilder();

            foreach (var node in pNodesList)
            {
                annotation.Append(node.InnerText);
                annotation.Append(Environment.NewLine);
            }

            string annotationStr = annotation.ToString();
            return annotationStr;
        }

        public static IEnumerable<BookModel> GetSearchResults(string searchQuery)
        {
            string url = "http://chitanka.info/search?q=" + searchQuery;
            string responseHtml = HttpRequester.GetHtml(url);
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(responseHtml);
            List<BookModel> booksList = new List<BookModel>();
            booksList.AddRange(GetBooksList(document));
            return booksList;
        }
    }
}
