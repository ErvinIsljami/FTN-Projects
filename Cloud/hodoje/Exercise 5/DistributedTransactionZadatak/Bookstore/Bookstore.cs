using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Diagnostics;
using Microsoft.WindowsAzure.Storage.Table;

namespace Bookstore
{
    public class Bookstore : IBookstore
    {
        private static BookstoreDataRepository bookstoreDataRepository;
        private static string bookForSaleId;
        private static int numberOfBooksForSale;

        public Bookstore()
        {
            bookstoreDataRepository = new BookstoreDataRepository();
        }

        public void Commit()
        {
            Book oldBook = bookstoreDataRepository.GetBookById(bookForSaleId).FirstOrDefault();
            Book newBook = bookstoreDataRepository.GetBookById(bookForSaleId + "prep").FirstOrDefault();

            if (oldBook != null && newBook != null)
            {
                oldBook.Count = newBook.Count;

                bookstoreDataRepository.UpdateBook(oldBook);
                bookstoreDataRepository.RemoveBook(newBook.BookId);
            }
            else
            {
                Trace.WriteLine("Either oldBook or newBook is null.");   
            }
        }

        public void EnlistPurchase(string bookId, int count)
        {
            bookForSaleId = bookId;
            numberOfBooksForSale = count;
        }

        public double GetItemPrice(string bookId)
        {
            var book = bookstoreDataRepository.GetBookById(bookId).FirstOrDefault();
            if (book != null)
            {
                return book.Price;
            }
            else
            {
                return -1;
            }
        }

        public void ListAvailableItems()
        {
            foreach(var b in bookstoreDataRepository.GetAllBooks().ToList())
            {
                Trace.WriteLine($"Book: {b.BookId}, Price: {b.Price}");
            }
        }

        public bool Prepare()
        {
            Book bookForSale = bookstoreDataRepository.GetBookById(bookForSaleId).FirstOrDefault();

            if (bookForSale != null)
            {
                if (bookForSale.Count - numberOfBooksForSale > 0)
                {
                    // We need to set all the properties or it will fail.
                    Book bookWithUpdatedCount = new Book
                    {
                        PartitionKey = bookForSale.PartitionKey,
                        RowKey = bookForSaleId + "prep",
                        Price = bookForSale.Price,
                        BookId = bookForSaleId + "prep",
                        Count = bookForSale.Count - numberOfBooksForSale
                    };
                    bookstoreDataRepository.AddBook(bookWithUpdatedCount);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void Rollback()
        {
            if (bookstoreDataRepository.GetBookById(bookForSaleId + "prep") != null)
            {
                bookstoreDataRepository.RemoveBook(bookForSaleId + "prep");
            }
        }
    }
}
