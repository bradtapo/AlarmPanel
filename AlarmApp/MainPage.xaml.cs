using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AlarmApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    
    
    public sealed partial class MainPage : Page
    {
        DispatcherTimer ClockTimer;
        public MainPage()
        {
            this.InitializeComponent();
            ClockTimer = new DispatcherTimer();
            ClockTimer.Tick += ClockTimer_Tick;
            ClockTimer.Interval = new TimeSpan(0, 0, 1);
            ClockTimer.Start();
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
    }
}
