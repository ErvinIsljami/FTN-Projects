using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore
{
    public class Bookstore : IBookstore
    {
        private static List<Book> bookstoreRepository = new List<Book>
        {
            new Book() {BookId = "book1", Count = 10, Price = 50},
            new Book() {BookId = "book2", Count = 15, Price = 30}
        };

        private static string bookForSaleId;
        private static int numberOfBooksForSale;


        public bool Prepare()
        {
            Book bookForSale = bookstoreRepository.FirstOrDefault(x => x.BookId == bookForSaleId);

            if (bookForSale != null)
            {
                if (bookForSale.Count - numberOfBooksForSale > 0)
                {
                    Book bookWithUpdatedCount = new Book()
                    {
                        Price = bookForSale.Price,
                        BookId = bookForSaleId + "prep",
                        Count = bookForSale.Count - numberOfBooksForSale
                    };
                    bookstoreRepository.Add(bookWithUpdatedCount);
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

        public void Commit()
        {
            Book oldBook = bookstoreRepository.FirstOrDefault(x => x.BookId == bookForSaleId);
            Book newBook = bookstoreRepository.FirstOrDefault(x => x.BookId == bookForSaleId + "prep");

            if (oldBook != null && newBook != null)
            {
                Console.WriteLine($"Old book[{oldBook.BookId}] state: {oldBook.Count}");
                oldBook.Count = newBook.Count;
                bookstoreRepository.Remove(newBook);
                Console.WriteLine($"New book[{oldBook.BookId}] state: {oldBook.Count}");
            }
            else
            {
                Console.WriteLine("Either oldBook or newBook is null");
            }
        }

        public void Rollback()
        {
            if (bookstoreRepository.FirstOrDefault(x => x.BookId == bookForSaleId + "prep") != null)
            {
                Book bookToDelete = bookstoreRepository.FirstOrDefault(x => x.BookId == bookForSaleId + "prep");
                bookstoreRepository.Remove(bookToDelete);
            }
        }

        public void ListAvailableItems()
        {
            if (bookstoreRepository.Count > 0)
            {
                foreach (var book in bookstoreRepository)
                {
                    Console.WriteLine($"Book: {book.BookId}, Price: {book.Price}");
                }
            }
            else
            {
                Console.WriteLine("No books in bank.");
            }
        }

        public void EnlistPurchase(string bookId, int count)
        {
            bookForSaleId = bookId;
            numberOfBooksForSale = count;
        }

        public double GetItemPrice(string bookId)
        {
            Book book = bookstoreRepository.FirstOrDefault(x => x.BookId == bookId);
            if (book != null)
            {
                return book.Price;
            }
            else
            {
                return -1;
            }
        }
    }
}
