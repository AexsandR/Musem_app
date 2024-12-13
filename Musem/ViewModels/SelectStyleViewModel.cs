using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Musem.ViewModels
{
    public class SelectStyleViewModel: BaseViewModel
    {
        double _size = 300;
        string _path = "";
        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                OnPropertyChanged();
            }
        }
        public double Size
        {
            get => _size;
            set
            {
                if (value >= 0)
                    _size = value;
                else
                    _size = 0;
                OnPropertyChanged();
            }
        }
    }
}
