using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //    Console.WriteLine(item.Tite);
            //}

            var authors = AuthorPersister.GetByLetterFirstName("%D0%90");
            foreach (var item in authors)
            {
                Console.WriteLine(item.Name);
                Console.WriteLine(item.OrigName);
                Console.WriteLine(item.Country);
                Console.WriteLine();
            }
        }
    }
}
