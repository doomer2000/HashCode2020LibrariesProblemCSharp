using System;
using System.Collections.Generic;
using System.Text;

namespace Hash.Models
{
    public class Library
    {
        public int Id { get; set; }
        public List<Book> Books { get; set; }
        public int BooksPerDay { get; set; }
        public int InitTime { get; set; }
    }
}
