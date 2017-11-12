using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TestAppDataFieldItemsApp1
{
    // Simple class.
    public class ViewModel: BindableBase
    {
        private object _source;
        private IEnumerable<string> _labels;

        public ViewModel()
       {
            Model.Book book = new Model.Book();
            book.Author = "Pirandello";
            book.ISBN = "182928";
            book.Name = "Il Fu mattia pascal";
            book.Rate = 9;
            SourceName = book;
            ChangedItem = new DelegateCommand<object>(modelCommand);
            List<string> list = new List<string>();
            list.Add("Autor");
            list.Add("ISBN");
            list.Add("Nombre");
            list.Add("Rata");
            Labels = list;
       }
        public void modelCommand(object value)
        {
            MessageBox.Show("New");
        }
        public IEnumerable<string> Labels
        {
            get { return _labels; }
            set { _labels = value; RaisePropertyChanged(); }
        }
        public object SourceName
        {
            get { return _source; }
            set { _source = value; RaisePropertyChanged(); }
        }
        public ICommand ChangedItem { set; get; }
    }
}
