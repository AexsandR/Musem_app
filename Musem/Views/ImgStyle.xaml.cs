using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Musem.Views
{
    /// <summary>
    /// Логика взаимодействия для ImgStyle.xaml
    /// </summary>
    public partial class ImgStyle : UserControl
    {
        public ImgStyle()
        {
            InitializeComponent();
        }



        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Path.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register("Path", typeof(string), typeof(ImgStyle), new PropertyMetadata(""));



        public string NameStyle
        {
            get { return (string)GetValue(NameStyleProperty); }
            set { SetValue(NameStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NameStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameStyleProperty =
            DependencyProperty.Register("NameStyle", typeof(string), typeof(ImgStyle), new PropertyMetadata("123"));


    }
}
