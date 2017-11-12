using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppDataFieldItemsApp1.Model
{
    /// <summary>
    ///  This is a simple class for the book.
    /// </summary>
    public class Book
    {
        /// <summary>
        ///  Name of the book.
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// Author of the book
        /// </summary>
        public string Author { set; get; }
        /// <summary>
        /// ISBN of the book
        /// </summary>
        public string ISBN { set; get; }
        /// <summary>
        ///  Rate of the book.
        /// </summary>
        public int Rate { set; get; }
    }
}
