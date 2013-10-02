using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AuthorModel
    {
        public string Name { get; set; }

        public string OrigName { get; set; }

        public string Country { get; set; }

        public string ImageUrl { get; set; }

        public IEnumerable<BookModel> Books { get; set; }
    }
}
