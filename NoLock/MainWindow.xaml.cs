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
using System.Runtime.InteropServices;
using System.Timers;



namespace NoLock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public const uint ES_CONTINUOUS = 0x80000000;
        public const uint ES_SYSTEM_REQUIRED = 0x00000001;
        public const uint ES_DISPLAY_REQUIRED = 0x00000002;
        public const int ONE_HOUR = 3600000;
        public const int TWO_HOURS = 7200000;
        public const int FOUR_HOURS = 14400000;
        public int timeToRun = 0;

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint SetThreadExecutionState([In] uint esFlags);
        private static Timer runTimer;

        public MainWindow()
        {
            InitializeComponent();
            lblStatus.Foreground = new SolidColorBrush(Colors.Red);
            lblStatus.Content = "Stopped";
            lblTimer.Content = "";
            runTimer = new System.Timers.Timer(timeToRun);
            runTimer.Elapsed += onTimedEvent;
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            startRunning();
        }

        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            stopRunning();
        }

        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (radBtnOneHour.IsChecked == true)
            {
            timeToRun = ONE_HOUR;
            }
            else if (radBtnTwoHours.IsChecked == true)
            { 
                timeToRun = TWO_HOURS;
            }
            else
            { 
                timeToRun = FOUR_HOURS;
            }
        }
        private void stopRunning()
        {
            // Relase the screenlock
            SetThreadExecutionState(ES_CONTINUOUS);
            // Call the UI changes with invoke as this method does not own the UI elements
            this.Dispatcher.Invoke(() =>
            {
                lblStatus.Content = "Stopped";
                lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                lblTimer.Content = "";
            });
            
        }

        private void startRunning()
        {
            SetThreadExecutionState(ES_CONTINUOUS | ES_DISPLAY_REQUIRED);
            lblStatus.Content = "Running";
            lblStatus.Foreground = new SolidColorBrush(Colors.Green);
            //lblTimer.Content = "Time " + timeToRun;
            runTimer.Interval = timeToRun;
            runTimer.Start();
        }
        private void onTimedEvent(Object source, ElapsedEventArgs e)
        {
            stopRunning();
        }
    }
}
