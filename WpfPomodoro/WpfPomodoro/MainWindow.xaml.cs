using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfPomodoro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private TimeSpan _ts = new TimeSpan(0, 0, 25, 0);
        private TimeSpan _ts = new TimeSpan(0, 0, 0, 30);
        private int _totalSeconds = 30;
        //private int _totalSeconds = 1500;
        private DispatcherTimer _counter;

        public MainWindow()
        {
            InitializeComponent();
            TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Normal;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _counter = new DispatcherTimer();
            _counter.Tick += new EventHandler(counterOne_Tick);
            _counter.Interval = new TimeSpan(0, 0, 0, 1);

            _counter.Start();

            //http://msdn.microsoft.com/en-us/library/system.windows.media.mediaplayer.volume.aspx
            MediaPlayer mp = new MediaPlayer();
            mp.Volume = 1.0;
            mp.Open(new Uri("Sounds/wc3-peon-work-work.mp3", UriKind.Relative));
            mp.Play();
        }

        private void counterOne_Tick(object sender, EventArgs e)
        {
            _ts = _ts.Add(new TimeSpan(0, 0, 0, -1));
            time.Text = string.Format("{0}:{1}", _ts.Minutes, _ts.Seconds);
            TaskbarItemInfo.ProgressValue = (((_totalSeconds - _ts.TotalSeconds) * 100) / _totalSeconds) / 100;

            if(_ts <= new TimeSpan(0,0,0))
            {
                TaskbarItemInfo.ProgressValue = 1.0;
                TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Indeterminate;

                _counter.Stop();

                MediaPlayer mp = new MediaPlayer();
                mp.Open(new Uri("Sounds/ring.wav", UriKind.Relative));
                mp.Play();
            }
        }

        private void MainWindow_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}