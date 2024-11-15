using ImTools;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rehm2024.Common.Models
{
    public class ItemsMenu:BindableBase
    {
        private string title;

        /// <summary>
        /// 参数名称
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string nameSpace;

        /// <summary>
        /// 参数命名空间
        /// </summary>
        public string NameSpace
        {
            get { return nameSpace; }
            set { nameSpace = value;  RaisePropertyChanged(); }
        }
    }
}


