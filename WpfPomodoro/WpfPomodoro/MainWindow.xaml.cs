using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
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
using todoistsharp;

namespace WpfPomodoro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TimeSpan _ts = new TimeSpan(0, 0, 25, 0);
        //private TimeSpan _ts = new TimeSpan(0, 0, 0, 30);
        //private const int TotalSeconds = 30;
        private const int TotalSeconds = 1500;
        private DispatcherTimer _counter;

        public MainWindow()
        {
            InitializeComponent();
            TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Normal;

            GetToDoIstTasks();
        }

        private void GetToDoIstTasks()
        {
            Todoist td = new Todoist();
            td.Login(ConfigurationManager.AppSettings.Get("todoistUsername"), ConfigurationManager.AppSettings.Get("todoistPassword"));
            var p = td.GetUncompletedItems(td.GetProjects().FirstOrDefault());
            p.OrderBy(o => o.due_date).ThenBy(o => o.priority).ThenBy(o => o.item_order);

            foreach(var a in p)
            {
                lb.Items.Add(a.content);
            }
        }

        private void UpdateToDoIstTask()
        {
            Todoist td = new Todoist();
            td.Login(ConfigurationManager.AppSettings.Get("todoistUsername"), ConfigurationManager.AppSettings.Get("todoistPassword"));
            var tasks = td.GetUncompletedItems(td.GetProjects().FirstOrDefault());
            var t = tasks.Single(p => p.content.Equals(lb.SelectedItem.ToString()));
            if (t.content.Contains("☐"))
            {
                int tmp = t.content.IndexOf("☐");
                t.content = t.content.Substring(0, tmp) + "☑" + t.content.Substring(tmp + 1);
            }
            else
            {
                t.content += " ☑"; //" &#2611;";
            }
            td.UpdateItem(t.id, t.content, t.date_string, t.priority, t.indent, t.item_order, t.collapsed);
            GetToDoIstTasks();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _counter = new DispatcherTimer();
            _counter.Tick += new EventHandler(counterOne_Tick);
            _counter.Interval = new TimeSpan(0, 0, 0, 1);

            _counter.Start();

            MediaPlayer mp = new MediaPlayer();
            mp.Volume = 1.0;
            mp.Open(new Uri("Sounds/wc3-peon-work-work.mp3", UriKind.Relative));
            mp.Play();

            lb.IsEnabled = false;
        }

        private void counterOne_Tick(object sender, EventArgs e)
        {
            _ts = _ts.Add(new TimeSpan(0, 0, 0, -1));
            time.Text = new DateTime(_ts.Ticks).ToString("mm:ss");
            TaskbarItemInfo.ProgressValue = (((TotalSeconds - _ts.TotalSeconds) * 100) / TotalSeconds) / 100;

            if(_ts <= new TimeSpan(0,0,0))
            {
                TaskbarItemInfo.ProgressValue = 1.0;
                TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Indeterminate;

                _counter.Stop();

                Console.Beep();
                MediaPlayer mp = new MediaPlayer();
                mp.Open(new Uri("Sounds/ring.wav", UriKind.Relative));
                mp.Volume = 1.0;
                mp.Play();
                UpdateToDoIstTask();

                lb.IsEnabled = true;
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

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://todoist.com/");
        }

        private void lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnStart.IsEnabled = true;
        }
    }
}