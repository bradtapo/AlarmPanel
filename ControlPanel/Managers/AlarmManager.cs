using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlPanel.Models;
using System.IO;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace ControlPanel.Managers
{
    public class AlarmManager
    {
        private bool DisableAllAlarms;
        private bool AlarmIsOn;
        List<Alarm> AlarmList;

        public AlarmManager()
        {
            DisableAllAlarms = false;
            AlarmIsOn = false;
            AlarmList = new List<Alarm>();
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            // Load a default alarm if none exist
            LoadSavedAlarms();
        }

        public void LoadSavedAlarms()
        {
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            path = path + "\\Alarms.xml";
            AlarmList = DeSerializeObject<List<Alarm>>(path);
        }

        public void SaveAlarms()
        {
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            path = path + "\\Alarms.xml";
            SerializeObject<List<Alarm>>(AlarmList, path);
        }

        public void AlarmService()
        {
            // Check to see if already in Alarm
            if(!DisableAllAlarms && !AlarmIsOn)
            {
                // Get the current date and time 
                DayOfWeek day = DateTime.Now.DayOfWeek;
                int hour = DateTime.Now.Hour;
                int minute = DateTime.Now.Minute;

                foreach(Alarm a in AlarmList)
                {
                    if(a.Enabled)
                    {
                        //if(DayOfWeek.Monday == day && a.Monday)
                    }
                }

            }
        }

        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject"></param>
        /// <param name="fileName"></param>
        public void SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                //Log exception here
            }
        }


        /// <summary>
        /// Deserializes an xml file into an object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public T DeSerializeObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }

            T objectOut = default(T);

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception ex)
            {
                //Log exception here
            }

            return objectOut;
        }
    }
}
