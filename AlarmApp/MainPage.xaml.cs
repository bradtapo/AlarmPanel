using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Text;
using System.Globalization;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AlarmApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

   
    public sealed partial class MainPage : Page
    {
        DispatcherTimer ClockTimer;
        private int deviceCount;
        private int currentDeviceId;
        private static List<int> qDevices;

        public MainPage()
        {
            this.InitializeComponent();
            ClockTimer = new DispatcherTimer();
            ClockTimer.Tick += ClockTimer_Tick;
            ClockTimer.Interval = new TimeSpan(0, 0, 1);
            ClockTimer.Start();

            // Get and populate audio sources
            qDevices =  new List<int>();
            // Count sound-devices
            foreach (var tuple in GetDevices())
            {
                deviceCount += 1;
            }
        }

        private void ClockTimer_Tick(object sender, object e)
        {
            this.txtBox_Time.Text = DateTime.Now.ToString("hh:mm");
            this.txtBox_AMPM.Text = DateTime.Now.ToString("tt");
        }
        
        private void btn_Snooze_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Dismiss_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Source_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_CloseNPR_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtBox_Time_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        #region EndPointController.exe interaction

        private static IEnumerable<Tuple<int, string, bool>> GetDevices()
        {
            var p = new Process
            {
                StartInfo =
                                {
                                    UseShellExecute = false,
                                    RedirectStandardOutput = true,
                                    CreateNoWindow = true,
                                    FileName = "EndPointController.exe",
                                    Arguments = "-f \"%d|%ws|%d|%d\""
                                }
            };
            p.Start();
            p.WaitForExit();
            var stdout = p.StandardOutput.ReadToEnd().Trim();

            var devices = new List<Tuple<int, string, bool>>();

            foreach (var line in stdout.Split('\n'))
            {
                var elems = line.Trim().Split('|');
                var deviceInfo = new Tuple<int, string, bool>(int.Parse(elems[0]), elems[1], elems[3].Equals("1"));
                devices.Add(deviceInfo);
            }

            return devices;
        }

        private static void SelectDevice(int id)
        {
            
            var p = new Process
            {
                StartInfo =
                                {
                                    UseShellExecute = false,
                                    RedirectStandardOutput = true,
                                    CreateNoWindow = true,
                                    FileName = "EndPointController.exe",
                                    StandardOutputEncoding = Encoding.UTF8,
                                    Arguments = id.ToString(CultureInfo.InvariantCulture)
                                }
            };
            p.Start();
            p.WaitForExit();
        }
        #endregion
    }
}
