using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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
using System.Windows.Threading;
using System.Configuration;
using ControlPanel.Managers;
namespace ControlPanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Other windows
        Settings settingsWindow;

        // Private variables
        DispatcherTimer ClockTimer;
        private int deviceCount;
        private int currentDeviceId;
        private static List<int> qDevices;
        private bool WindowFullscreen;
        private bool NPRIsRunning;
        private Process NPRProcess;
        private AlarmManager alarmManager;

        public MainWindow()
        {
            InitializeComponent();

            ClockTimer = new DispatcherTimer();
            ClockTimer.Tick += ClockTimer_Tick;
            ClockTimer.Interval = new TimeSpan(0, 0, 1);
            ClockTimer.Start();
            NPRIsRunning = false;
            alarmManager = new AlarmManager();

            // Start in full screen
            WindowFullscreen = true;

            // Get and populate audio sources
            qDevices = new List<int>();
            // Count sound-devices
            foreach (var tuple in GetDevices())
            {
                deviceCount += 1;
                cmbox_AudioSource.Items.Add(tuple.Item2);
            }
        }

        private void ClockTimer_Tick(object sender, object e)
        {
            this.lbl_Time.Content = DateTime.Now.ToString("h:mm");
            this.lbl_AMPM.Content = DateTime.Now.ToString("tt");
        }

        public static T NextOf<T>(IList<T> list, T item)
        {
            return list[(list.IndexOf(item) + 1) == list.Count ? 0 : (list.IndexOf(item) + 1)];
        }

        //Gets the ID of the next sound device in the list
        private int nextId()
        {
            if (qDevices.Count > 0) currentDeviceId = NextOf(qDevices, currentDeviceId);
            return currentDeviceId;
        }



        #region Tray events

        private void PopulateDeviceList(object sender, EventArgs e)
        {
            // Clear the combo box ever time it populates
            this.cmbox_AudioSource.Items.Clear();

            // All all active devices
            foreach (var tuple in GetDevices())
            {
                var id = tuple.Item1;
                var deviceName = tuple.Item2;
                var isInUse = qDevices.Contains(id);
                object newSource = new object();

                this.cmbox_AudioSource.Items.Add(deviceName);
                //var item = new MenuItem { Checked = isInUse, Text = deviceName + " (" + id + ")" };
               // item.Click += (s, a) => AddDeviceToList(id);

               // trayMenu.MenuItems.Add(item);
            }
        }

        private static void AddDeviceToList(int id)
        {
            if (qDevices.Contains(id)) qDevices.Remove(id);
            else qDevices.Add(id);
        }

        #endregion

        #region EndPointController.exe interaction

        private static IEnumerable<Tuple<int, string, bool>> GetDevices()
        {
            string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            var p = new Process
            {
                StartInfo =
                                {
                                    UseShellExecute = false,
                                    RedirectStandardOutput = true,
                                    CreateNoWindow = true,
                                    FileName = dir + "\\EndPointController.exe",
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

        private void cmbox_AudioSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var tuple in GetDevices().Where(tuple => cmbox_AudioSource.SelectedValue.ToString() == tuple.Item2))
            {

                SelectDevice(tuple.Item1);
                break;
            }   
        }

        private void btn_Snooze_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Dismiss_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btn_Settings_Click(object sender, RoutedEventArgs e)
        {
            // Initialize settings
            settingsWindow = new Settings();
            settingsWindow.Show();
            
        }

        private bool NPR_CheckServiceRunning()
        {
            var NprProcesses = Process.GetProcesses().
                                 Where(pr => pr.ProcessName.Contains("NPROne"));

            if(NprProcesses.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void NPR_Toggle()
        {
            string mainAudioProgram = ConfigurationSettings.AppSettings.Get("MainAudioProgram");
            if (NPRIsRunning)
            {

                var NprProcesses = Process.GetProcesses().
                                 Where(pr => pr.ProcessName.Contains("NPROne"));

                foreach (var process in NprProcesses)
                {
                    process.Kill();
                }
                NPRIsRunning = false;

            }
            else
            {
                NPRProcess = new Process
                {
                    StartInfo =
                                {
                                    UseShellExecute = true,
                                    RedirectStandardOutput = false,
                                    CreateNoWindow = false,
                                    FileName = mainAudioProgram

                                }
                };
                try
                {
                    NPRProcess.Start();
                    NPRIsRunning = true;

                }
                catch (Exception ex)
                {

                }
            }
        }

        private void btn_CloseNpr_Click(object sender, RoutedEventArgs e)
        {
            NPR_Toggle();
        }

        private void btn_Fullscreen_Click(object sender, RoutedEventArgs e)
        {
            if(WindowFullscreen)
            {
                this.WindowState = WindowState.Normal;
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.ResizeMode = ResizeMode.CanResize;
                WindowFullscreen = false;
            }
            else
            {
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
                this.ResizeMode = ResizeMode.NoResize;
                WindowFullscreen = true;
            }
        }

        private void Image_GotFocus(object sender, RoutedEventArgs e)
        {

        }

    }
}
