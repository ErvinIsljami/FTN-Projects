using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace Bookstore
{
    public class BookStore : IBookstore
    {
        string bookName;
        int numberOfBooks;
        BookTableHelper tableHelper;
        public BookStore()
        {
            tableHelper = new BookTableHelper();
            Book k = new Book(10, 1000, "MaliPrinc");
            Book k2 = new Book(10, 1000, "AnaKarenjina");
            Book k3 = new Book(10, 1000, "NaDriniCuprija");
            tableHelper.AddElement(k);
            tableHelper.AddElement(k2);
            tableHelper.AddElement(k3);
        }
        public void Commit()
        {
            tableHelper.Update(bookName, numberOfBooks);
        }

        public void EnlistPurchase(string bookID, int count)
        {
            bookName = bookID;
            numberOfBooks = count;
        }

        public double GetItemPrice(string bookID)
        {
            return tableHelper.GetPrice(bookID);
        }

        public void ListAvailableItems()
        {
            BookTableHelper helper = new BookTableHelper();
            List<Book> lista = helper.RetrieveAllElements() as List<Book>;
            foreach(Book b in lista)
            {
                Trace.TraceInformation("Knjiga: " + b.Name);
            }

        }

        public bool Prepare()
        {
            if (tableHelper.GetCount(bookName) > numberOfBooks)
                return true;
            else
                return false;

        }

        public void Rollback()
        {
            Trace.TraceInformation("Rollback...");
            //ne mora nista
        }
    }
}
