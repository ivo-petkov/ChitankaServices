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

        public string BooksUrl { get; set; }

        public string Info { get; set; }

        public AuthorModel()
        {
            this.Name = string.Empty;
            this.OrigName = string.Empty;
            this.Country = string.Empty;
            this.BooksUrl = null;
            this.ImageUrl = "http://www.goodreads.com/assets/nophoto/user/u_200x266-312f5971f6b4a667fe0e83c852b56858.png";
            this.Info = "Няма налична информация за автора.";
        }
    }
}
