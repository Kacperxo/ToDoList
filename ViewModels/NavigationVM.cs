using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.ViewModels
{
    public class NavigationVM : BaseViewModel
    {
        private readonly MainWindowVM _root;

        public NavigationVM(MainWindowVM root)
        {
            _root = root;
        }   
    }
}
