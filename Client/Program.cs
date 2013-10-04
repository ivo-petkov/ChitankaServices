using Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            //var books = BooksPersister.GetAllBooksByLetter("%D0%90");
            //foreach (var item in books)
            //{
            //    Console.WriteLine(item.Title);
            //}

            var authors = AuthorsPersister.GetByLetterFirstName("%D0%90");
            foreach (var item in authors)
            {
                Console.WriteLine(item.Name);
                Console.WriteLine(item.OrigName);
                Console.WriteLine(item.Country);
                Console.WriteLine(item.ImageUrl);
                Console.WriteLine();
            }            
        }
    }
}
