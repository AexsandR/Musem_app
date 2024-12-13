using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Musem.Models
{
    public class ImgStyle
    {
        int _column = 0;
        double _size = 300;

        public int Column
        {
            get => _column;
            set
            {
                if (value >= 0)
                    _column = value;
                else
                    _column = 0;
            }
        }
        public string Path
        {
            get { return "../Data/" + NameStyle + ".png"; }
        }
        public string NameStyle { get; set; } = "Style";
        

    }
}
