using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPanel.Models
{
    public class Alarm
    {
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public string ReminderDetail { get; set; }
        public bool Repeating { get; set; }
        public bool OneTime { get; set; }
        public bool Daily { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public string OfflineAlarmSound { get; set; }
        public int SnoozeInterval { get; set; }
        public bool MuteOnLightSense { get; set; }
        public bool Enabled { get; set; }
        public Alarm ()
        {
            Name = String.Empty;
            Time = DateTime.Now;
            ReminderDetail = String.Empty;
            Repeating = false;
            OneTime = true;
            Daily = false;
            Monday = false;
            Tuesday = false;
            Wednesday = false;
            Thursday = false;
            Friday = false;
            Saturday = false;
            Sunday = false;
            OfflineAlarmSound = "";
            SnoozeInterval = 5;
            MuteOnLightSense = false;
            Enabled = true;

        }
    }
}
