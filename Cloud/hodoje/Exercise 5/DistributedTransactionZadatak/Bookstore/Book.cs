using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore
{
    public class Book : TableEntity
    {
        public string BookId { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }

        public Book() { }

        public Book(string bookId)
        {
            PartitionKey = "Book";
            RowKey = bookId;
        }
    }
}
