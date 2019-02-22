using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace IvitRooms
{
    //график 
    [Serializable]
    public class Schedule
    {
        public List<Indication> indications = new List<Indication>();
        public string name;
        public int id;
    }
    //значение для графика
    [Serializable]
    public class Indication
    {
        public DateTime datetime { get; set; }
        public float value { get; set; }
    }
    //сохраняет графики в файл
    

    public class WriterIndication
    {
        private System.Threading.Timer timer;
        private System.Threading.Timer timer2;
        public void StartWriter()
        {
            timer = new System.Threading.Timer(WritingIvit, "", 0, 30000);
            timer2 = new System.Threading.Timer(WritingPas, "", 0, 5000);
        }
        public void StopWriter()
        {
            timer.Change(System.Threading.Timeout.Infinite, 0);
            timer.Dispose();
            timer2.Change(System.Threading.Timeout.Infinite, 0);
            timer2.Dispose();
        }
        public void WritingPas(object objectInfo)
        {
            foreach (Form1.Pas pas in Form1._form._newRoot.pases)
            {
                string pubPath = Environment.CurrentDirectory;
                string dataPath = pubPath + "\\..\\..\\data\\indications\\" +
                    DateTime.Now.Year.ToString() +
                    "\\" + DateTime.Now.Year.ToString() +
                    "-" + DateTime.Now.Month.ToString() +
                    "-" + DateTime.Now.Day.ToString() +
                    "\\pressure";
                System.IO.Directory.CreateDirectory(dataPath);

                BinaryFormatter formatter = new BinaryFormatter();
                try
                {
                    using (FileStream fs = new FileStream(dataPath + "\\pas_" + pas.id + ".dat", FileMode.Append, FileAccess.Write, FileShare.Read))
                    {
                        using (BinaryWriter writer = new BinaryWriter(fs))
                        {
                            writer.Write(DateTime.Now.ToString());
                            writer.Write(pas.pressure);
                        }
                        fs.Close();
                    }
                }
                catch (Exception e)
                {
                    Mylog.writeExc("класс: " + GetType().Name + ". метод: " + System.Reflection.MethodBase.GetCurrentMethod().Name + e.Message);
                    return;
                }

            }
        }

        public void WritingIvit(object objectInfo)
        {
            foreach (Form1.Ivit ivit in Form1._form._newRoot.ivits)
            {
                string pubPath = Environment.CurrentDirectory;
                string dataPath = pubPath + "\\..\\..\\data\\indications\\" +
                    DateTime.Now.Year.ToString() +
                    "\\" + DateTime.Now.Year.ToString() +
                    "-" + DateTime.Now.Month.ToString() +
                    "-" + DateTime.Now.Day.ToString() +
                    "\\temps";
                System.IO.Directory.CreateDirectory(dataPath);

                BinaryFormatter formatter = new BinaryFormatter();
                try
                {
                    using (FileStream fs = new FileStream(dataPath + "\\ivit_" + ivit.id + ".dat", FileMode.Append, FileAccess.Write, FileShare.Read))
                    {
                        using (BinaryWriter writer = new BinaryWriter(fs))
                        {
                            writer.Write(DateTime.Now.ToString());
                            writer.Write(ivit.temp);
                        }
                        fs.Close();
                    }
                }
                catch (Exception e)
                {
                    Mylog.writeExc("класс: " + GetType().Name + ". метод: " + System.Reflection.MethodBase.GetCurrentMethod().Name + e.Message);
                    return;
                }

            }
            foreach (Form1.Ivit ivit in Form1._form._newRoot.ivits)
            {
                string pubPath = Environment.CurrentDirectory;
                string dataPath = pubPath + "\\..\\..\\data\\indications\\" +
                    DateTime.Now.Year.ToString() +
                    "\\" + DateTime.Now.Year.ToString() +
                    "-" + DateTime.Now.Month.ToString() +
                    "-" + DateTime.Now.Day.ToString() +
                    "\\wets";
                System.IO.Directory.CreateDirectory(dataPath);

                BinaryFormatter formatter = new BinaryFormatter();
                try
                {
                    using (FileStream fs = new FileStream(dataPath + "\\ivit_" + ivit.id + ".dat", FileMode.Append, FileAccess.Write, FileShare.Read))
                    {
                        using (BinaryWriter writer = new BinaryWriter(fs))
                        {
                            writer.Write(DateTime.Now.ToString());
                            writer.Write(ivit.wet);
                        }
                        fs.Close();
                    }
                }
                catch (Exception e)
                {
                    Mylog.writeExc("класс: " + GetType().Name + ". метод: " + System.Reflection.MethodBase.GetCurrentMethod().Name + e.Message);
                    return;
                }


            }
            
        }
    }
    public class FormScheduleTemps
    {
        public List<int> checkedIvits { get; set; }
    }

    public class FormScheduleWets
    {
        public List<int> checkedIvits { get; set; }
    }

    public class FormSchedulePressures
    {
        public List<int> checkedPases { get; set; }
    }

    public class UserJConfig:JConfigur
    {
        public FormScheduleTemps formScheduleTemps { get; set; }
        public FormScheduleWets formScheduleWets { get; set; }
        public FormSchedulePressures formSchedulePressures { get; set; }

        protected static UserJConfig userJConfig = null;
        protected static string path =  Environment.CurrentDirectory + "\\..\\..\\public\\userJConfig.json";
        private UserJConfig()
        {

        }
        public static UserJConfig Inst()
        {
            if (userJConfig == null)
            {
                userJConfig = new UserJConfig();
                userJConfig.FillFromFile(UserJConfig.path);
            }
         
            return userJConfig;
        }
        public void Save()
        {
            this.SaveInFile(UserJConfig.path);
        }
    }

    public class JConfigur
    {

        //заполнит объект из файла JSON
        protected void FillFromFile(string path)
        {
            string jConfig = System.IO.File.ReadAllText(@path, Encoding.UTF8).Replace("\n", " ");
            JsonConvert.PopulateObject(jConfig, this);
            
        }

        //сохранит объект в файл
        protected void SaveInFile(string path)
        {
            string serialized = JsonConvert.SerializeObject(this, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine(serialized);
            sw.Close();
        }
    }
    public class Synchroniz
    {
        FTP _ftp = new FTP();
        List<Task> tasks = new List<Task>();
        private System.Threading.Timer timerSynchronization;
        public Synchroniz()
        {
            UploadFiles();
        }


        public void UploadFiles()
        {
            var tasks = new List<Task>();
            string date = DateTime.Now.Year.ToString() +
           "-" + DateTime.Now.Month.ToString() +
           "-" + DateTime.Now.Day.ToString();
            string dateMDay = DateTime.Now.AddDays(-1).Year.ToString() +
           "-" + DateTime.Now.AddDays(-1).Month.ToString() +
           "-" + DateTime.Now.AddDays(-1).Day.ToString();
            tasks.Add(UploadFile("\\" + DateTime.Now.AddDays(-1).Year.ToString() + "\\" + dateMDay));
            tasks.Add(UploadFile("\\" + DateTime.Now.Year.ToString() + "\\" + date));
            Task.WhenAll(tasks);


        }

        public async Task UploadFile(string path)
        {

            await Task.Run(() =>
            {
                try
                {
                    _ftp.uploadIndications(path + "\\wets");
                    _ftp.uploadIndications(path + "\\temps");
                    _ftp.uploadIndications(path + "\\pressure");

                }
                catch (Exception exc)
                {
                    Mylog.writeExc("Ошибка класс: " + GetType().Name + ". метод: " + System.Reflection.MethodBase.GetCurrentMethod().Name + " Mes: " + exc.Message);
                    return;
                }
            });


        }
    }
}
