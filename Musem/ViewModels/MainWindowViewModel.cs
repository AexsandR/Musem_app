using System;
using System.Linq;
using System.Threading.Tasks;
using Quobject.SocketIoClientDotNet.Client;
using System.Windows.Media.Imaging;
using Musem.Modules;
using System.IO;
using System.Timers;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Musem.ViewModels
{
    public class MainWindowViewModel: BaseViewModel
    {

        const string urlLocalHost = "http://127.0.0.1:5000";
        //const string urlLocalHost = "http://192.168.7.138:5000";
        //const string urlLocalHost = "http://192.168.1.119:5000";

        string urlNgrok;
        bool connectClient = false;
        Dictionary<string, string> texts;
        string _textStyle = "";
        double _heightImg = 950;
        const double minHeightImg = 548;
        const double maxHeightImg = 950;
        int codeAccept;
        double speedAnimationSelect = 25;
        double speedAnimationStyle = 15;
        int timeLoad;
        string[] stylesName = { "Аниме", "Пиксель-арт", "Реализм", "Импрессионизм" };
        double widthCellGridStyles;
        int heightCellGridStyles = 400;
        (int, int) sizeSc = (1920, 1080);
        double angle;
        int _sizeStyleImg = 300;
        double _transparencyImage = 1;
        double _transparencyImageStyle = 1;
        double speedOpacity = 0.05;

        Socket socket;
        Thickness _posStyle;
        Models.ImgStyle _imgStyle;
        BitmapImage _bitmapImageQrCode;
        BitmapImage _bitmapImage;
        Visibility _visibilityQrCode = Visibility.Visible;
        Visibility _visibilityText = Visibility.Hidden;

        Requests requests = new Requests();
        Thickness _posGridStyles;
        ObservableCollection<Models.ImgStyle> _styles = new ObservableCollection<Models.ImgStyle>();
        ProgressBarViewModel _progressBarVM = new ProgressBarViewModel();

        Timer timerAnimationStyle = new Timer(12);
        Timer timerAnimationSelect = new Timer(12);
        Timer timerAnimationAppearanceImage = new Timer(12);

        public string TextStyle
        {
            get => _textStyle;
            set
            {
                _textStyle = value;
                OnPropertyChanged();
            }
        }
        public ProgressBarViewModel ProgressBarVM
        {
            get => _progressBarVM;
        }
        public ObservableCollection<Models.ImgStyle> Styles
        {
            get => _styles;
            set
            {
                _styles = value;
                OnPropertyChanged();
            }
        }
        public Thickness PosStyle
        {
            get => _posStyle;
            set
            {
                _posStyle = value;
                OnPropertyChanged();
            }
        }
        public int SizeStyleImg => _sizeStyleImg;
        public double TransparencyImage
        {
            get => _transparencyImage;
            set
            {
                if (value > 1)
                    _transparencyImage = 1;
                else if (value < 0)
                    _transparencyImage = 0;
                else
                    _transparencyImage = value;
                OnPropertyChanged();
            }
        }
        public double TransparencyImageStyle
        {
            get => _transparencyImageStyle;
            set
            {
                if (value > 1)
                    _transparencyImageStyle = 1;
                else if (value < 0)
                    _transparencyImageStyle = 0;
                else
                    _transparencyImageStyle = value;
                OnPropertyChanged();
            }
        }
        public Models.ImgStyle ImgStyle
        {
            get => _imgStyle;
            set
            {
                _imgStyle = value;
                OnPropertyChanged();
            }
        }
        public BitmapImage BitmapImageQrCode
        {
            get => _bitmapImageQrCode;
            set
            {
                _bitmapImageQrCode = value;
                OnPropertyChanged();
            }
        }
        public BitmapImage BitmapImageSc
        {
            get => _bitmapImage;
            set
            {
                _bitmapImage = value;
                OnPropertyChanged();
            }
        }
        public Visibility VisibilityQrCode
        {
            get => _visibilityQrCode;
            set
            {
                _visibilityQrCode = value;
                OnPropertyChanged();
            }
        }
        public Visibility VisibilitText
        {
            get => _visibilityText;
            set
            {
                _visibilityText = value;
                OnPropertyChanged();
            }
        }
        public Thickness PostGridStyles
        {
            get => _posGridStyles;
            set
            {
                if (value.Bottom < 45)
                    _posGridStyles = value;
                
                else
                    _posGridStyles = new Thickness(0, 0, 0, 45);
                OnPropertyChanged();
            }
        }
        public double HeightImg
        {
            get => _heightImg;
            set
            {
                if (value < minHeightImg)
                    _heightImg = minHeightImg;
                else if (value >= 0)
                    _heightImg = value;
                else
                    _heightImg = 0;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel() 
        {
            SocketOperationode();
            widthCellGridStyles = 1920 / stylesName.Length;
            PostGridStyles = new Thickness(0,0,0, -heightCellGridStyles);
            LoadImgStart();
            timerAnimationStyle.Elapsed += AnimationStyles;
            timerAnimationStyle.AutoReset = true;
            timerAnimationSelect.Elapsed += AnimationSelect;
            Modules.TextStyle.SetTextStyle(urlLocalHost);
            SetTextStyle();



        }

        private void LoadImgStart()
        {
            BitmapImageSc = new BitmapImage(new Uri(@"../Data/startImage1.jpg", UriKind.Relative));
        }
        private void loadStyles(string styleException = "")
        {
            var styles = new ObservableCollection<Models.ImgStyle>();
            for (int index = 0; index < stylesName.Length; index++)
            {
                if (stylesName[index] == styleException)
                    continue;
                styles.Add(new Models.ImgStyle() { Column = index, NameStyle = stylesName[index] });
            }
            Styles = styles;
        }
        private void SocketOperationode()
        {
            socket = IO.Socket(urlLocalHost);

            socket.On(Socket.EVENT_CONNECT, () =>
            {
                socket.Emit("connect_client", "c# на связи");

            });

            socket.On(Socket.EVENT_MESSAGE, (data) =>
            {
                //MessageBox.Show(data.ToString());
                if (int.Parse(data.ToString()) == codeAccept && connectClient)
                {
                    StartAnimationAppearanceImage();
                    SetQrCode("http://api.qrserver.com/v1/create-qr-code/?");

                }
            });

            socket.On("connect_user", (data) => {
                //MessageBox.Show(data.ToString() + "\n код экрана" + codeAccept.ToString() + "\n" + connectClient.ToString());
                if (int.Parse(data.ToString()) == codeAccept && !connectClient)
                {
                    StartAnimationDisplayingStyles();
                    connectClient = true;
                }
            });


            socket.On("get_ngrok_url", (data) => {
                urlNgrok = data.ToString();
                SetQrCode("http://api.qrserver.com/v1/create-qr-code/?");
            });

            socket.On("set_time", (data) => {
                if(codeAccept == int.Parse(data.ToString().Split()[1]))
                {
                    timeLoad = int.Parse(data.ToString().Split()[0]);
                    ProgressBarVM.StartAnimationСompletion((double)timeLoad);
                }

            });

            socket.On("SwitchImg", (data) => {
                if (codeAccept == int.Parse(data.ToString().Split()[1]))
                {
                    StartAnimationSelect(data.ToString().Split()[0]);
                }
            });
            socket.On(Socket.EVENT_DISCONNECT, () => {
                socket.Close();
                socket.Connect();
            });
        }
        private void StartAnimationSelect(string style)
        {
            ImgStyle = Styles.Where(el => el.NameStyle == style).First();
            loadStyles(style);
            PosStyle = new Thickness((widthCellGridStyles - SizeStyleImg) / 2 + widthCellGridStyles * ImgStyle.Column , 0, 0, (heightCellGridStyles - SizeStyleImg) / 2);
            angle = Math.PI / 2 -  Math.Atan(((sizeSc.Item1 - SizeStyleImg) / 2 - PosStyle.Left) / ((sizeSc.Item2 + SizeStyleImg - 100) / 2 - PosStyle.Bottom));
            TransparencyImageStyle = 1;
            speedAnimationStyle *= -1;
            timerAnimationSelect.Enabled = true;
            timerAnimationStyle.Enabled = true;

        }
        private void StartAnimationDisplayingStyles()
        {
            VisibilitText = Visibility.Hidden;
            PostGridStyles = new Thickness(0, 0, 0, -heightCellGridStyles); //нужно для костыля
            loadStyles();
            VisibilityQrCode ^= Visibility.Hidden;

            timerAnimationStyle.Enabled = true;

        }
        private void StartAnimationAppearanceImage()
        {
            VisibilitText = Visibility.Visible;
            HeightImg = maxHeightImg;
            TextStyle = texts[ImgStyle.NameStyle];
            SetImg();
            timerAnimationAppearanceImage.AutoReset = true;
            timerAnimationAppearanceImage.Elapsed += AnimationAppearanceImage;
            timerAnimationAppearanceImage.Enabled = true;
        }

        private void AnimationAppearanceImage(object sender, ElapsedEventArgs e)
        {
            TransparencyImageStyle -= speedOpacity;
            TransparencyImage += speedOpacity;
            ProgressBarVM.TransparencyProgressbar -= speedOpacity;
            if(TransparencyImageStyle == 0)
            {
                timerAnimationAppearanceImage.Enabled = false;
                VisibilityQrCode = Visibility.Visible;
                connectClient = false;
           }
        }

        private void AnimationSelect(object sender, ElapsedEventArgs e)
        {
            PosStyle = new Thickness(PosStyle.Left + speedAnimationSelect * Math.Cos(angle), 0, 0, PosStyle.Bottom + speedAnimationSelect * Math.Sin(angle));
            TransparencyImage -= speedOpacity;
            ProgressBarVM.TransparencyProgressbar += speedOpacity / 2;

            if (PosStyle.Bottom > (sizeSc.Item2 + SizeStyleImg - 100) / 2)
            {
                timerAnimationSelect.Enabled = false;
                PosStyle = new Thickness((sizeSc.Item1 - SizeStyleImg) / 2, 0, 0, (sizeSc.Item2 + SizeStyleImg - 100) / 2);
            }
        }


        private void AnimationStyles(object sender, ElapsedEventArgs e)
        {
            if(speedAnimationStyle < 0 && PostGridStyles.Bottom <= -heightCellGridStyles)
            {
                PostGridStyles = new Thickness(0, 0, 0, -999); // костыль хз почему он у меня уезжает вверх панель со стилями
                speedAnimationStyle *= -1;
                timerAnimationStyle.Enabled = false;
                return;
            }            
            else if(speedAnimationStyle > 0 && PostGridStyles.Bottom == 45)
            {
                timerAnimationStyle.Enabled = false;
                return;
            }
            if (speedAnimationStyle > 0)
                HeightImg -= speedAnimationStyle;
            PostGridStyles = new Thickness(0, 0, 0, PostGridStyles.Bottom + speedAnimationStyle);
        }
        
        private async Task SetQrCode(string url)
        {
           
            var code = await requests.GetCodetAsync(urlLocalHost + "/get_code_connect");
            codeAccept = code;
            var res = await GetBitMapImage(url + $"data=http://{urlNgrok + "/musem/" + codeAccept}&size=300x300");
            BitmapImageQrCode = res;
            //MessageBox.Show("новый код " + codeAccept.ToString());
        }
        private async Task SetImg()
        {
            var res = await GetBitMapImage(urlLocalHost + "/get_img/" + codeAccept.ToString());
            BitmapImageSc = res;
        }
        public async Task SetTextStyle()
        {
            var http = new Requests();
            var animeText = await http.GetTextAsync(urlLocalHost + "/get_text/anime");
            var pixelart = await http.GetTextAsync(urlLocalHost + "/get_text/pixelart");
            var realism = await http.GetTextAsync(urlLocalHost + "/get_text/realism");
            var impressionism = await http.GetTextAsync(urlLocalHost + "/get_text/impressionism");
            texts = new Dictionary<string, string>()
                {
                    {"Аниме", animeText },
                    {"Пиксель-арт", pixelart },
                    {"Реализм", realism },
                    {"Импрессионизм", impressionism },

                };
        }
        private async Task<BitmapImage> GetBitMapImage(string url)
        {

            byte[] binary = await requests.GetRequestResponceContentAsync(url);
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = new MemoryStream(binary);
            bitmapImage.EndInit();
            bitmapImage.Freeze(); // Замораживаем изображение для использования в разных потоках
            return bitmapImage;



        }

    }
}
