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

namespace WpfPomodoro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Timer t;
        private TimeSpan ts = new TimeSpan(0,0,25,0);
        public MainWindow()
        {
            InitializeComponent();
            t = new Timer(1000);
            t.Elapsed += t_Elapsed;
        }

        public string Counter
        {
            get {
                OnPropertyChanged(new PropertyChangedEventArgs("Counter"));

                return ts.ToString();
            }
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, e);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            t.Start();
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            ts = ts.Add(new TimeSpan(0, 0, 0, -1));
        }
    }
}
