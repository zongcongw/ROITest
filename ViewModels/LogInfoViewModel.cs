using Prism.Events;
using Prism.Mvvm;
using Rehm2024.Common.Models;
using Rehm2024.Event;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rehm2024.ViewModels
{
    public class LogInfoViewModel: BindableBase
    {
        public LogInfoViewModel(IEventAggregator aggregator)
        {
            aggregator.GetEvent<MessagesEvent>().Subscribe(OnMessageReceived);

        }

        private void OnMessageReceived(string obj)
        {
            
            
            System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                if (Items.Count > 10) { Items.RemoveAt(0); }
                obj = DateTime.Now.ToString() + "--" + obj;
                Items.Add($"{obj}");
            }));
        }
   

        private ObservableCollection<string> _items = new ObservableCollection<string>();
        public ObservableCollection<string> Items
        {
            get { return _items; }
            set { _items = value; RaisePropertyChanged(); }
        }


       
     
    }
}
