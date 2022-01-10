using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;

namespace Bookstore
{
    public class BookstoreDataRepository
    {
        private CloudStorageAccount storageAccount;
        private CloudTable table;

        public BookstoreDataRepository()
        {
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("BookstoreDataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(storageAccount.TableEndpoint.AbsoluteUri), storageAccount.Credentials);
            table = tableClient.GetTableReference("BookTable");
            table.CreateIfNotExists();
        }

        public IQueryable<Book> GetAllBooks()
        {
            var bookList = from x in table.CreateQuery<Book>() where x.PartitionKey == "Book" select x;
            if (bookList.ToList().Count > 0)
            {
                return bookList;
            }
            else
            {
                Trace.WriteLine("There are no books in the database.");
                return bookList;
            }
        }

        public IQueryable<Book> GetBookById(string bookId)
        {
            var book = from x in table.CreateQuery<Book>() where x.PartitionKey == "Book" && x.RowKey == bookId select x;
            if (book.ToList().Count > 0)
            {
                return book;
            }
            else
            {
                Trace.WriteLine($"Book with id: {bookId} does not exists.");
                return book;
            }
        }

        public bool AddBook(Book newBook)
        {
            if (!GetAllBooks().ToList().Contains(newBook))
            {
                TableOperation insertOperation = TableOperation.Insert(newBook);
                table.Execute(insertOperation);
                return true;
            }
            else
            {
                Trace.WriteLine($"Book with id: {newBook.BookId} already exists.");
                return false;
            }
        }

        public bool RemoveBook(string bookId)
        {
            Book bookForDeletion = GetBookById(bookId).FirstOrDefault();
            if (bookForDeletion != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(bookForDeletion);
                table.Execute(deleteOperation);
                return true;
            }
            else
            {
                Trace.WriteLine($"Book with id: {bookId} could not be removed because it does not exist.");
                return false;
            }
        }

        public bool UpdateBook(Book updatedBook)
        {
            if (GetBookById(updatedBook.BookId) != null)
            {
                TableOperation updateOperation = TableOperation.Replace(updatedBook);
                table.Execute(updateOperation);
                return true;
            }
            else
            {
                Trace.WriteLine($"Book with id: {updatedBook.BookId} could not be updated because it does not exist.");
                return false;
            }
        }
    }
}
