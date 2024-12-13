using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows;

namespace Musem.ViewModels
{
    public class ProgressBarViewModel: BaseViewModel
    {
        double _valueProgressBar = 0;
        string _stringValueProgressBar = "0";
        double _transparencyProgressbar = 0;
        double speed;


        Timer timer;

        public double TransparencyProgressbar
        {
            get => _transparencyProgressbar;
            set
            {
                if (value > 1)
                    _transparencyProgressbar = 1;
                else if (value < 0)
                    _transparencyProgressbar = 0;
                else
                    _transparencyProgressbar = value;
                OnPropertyChanged();
            }
        }


        public double ValueProgressBar
        {
            get => _valueProgressBar;
            set
            {
                if (value < 0)
                    _valueProgressBar = 0;
                if (value > 100)
                    _valueProgressBar = 100;
                else
                {
                    _valueProgressBar = value;
                    StringValueProgressBar = ((int)value).ToString();
                }
                OnPropertyChanged();
            }
        }
        public string StringValueProgressBar
        {
            get => _stringValueProgressBar + "%";
            set
            {
                _stringValueProgressBar = value;
                OnPropertyChanged();
            }
        }

        public void StartAnimationСompletion(double time)
        {
            ValueProgressBar = 0;
            timer = new Timer(1000);
            speed = 100 / time;
            timer.AutoReset = true;
            timer.Elapsed += AnimationСompletion;
            timer.Enabled = true;
        }

        private void AnimationСompletion(object sender, ElapsedEventArgs e)
        {
            ValueProgressBar += speed;
            if (ValueProgressBar == 100)
            {
                timer.Enabled = false;
                StringValueProgressBar = "100";
            }
        }
    }
}
