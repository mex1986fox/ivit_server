using System;
using System.Collections.Generic;
using Npgsql;
using System.Windows.Forms;
using System.IO;
using System.Net;
using NLog;
using System.Threading;
using WinSCP;
namespace IvitRooms
{
    // класс измеренных показаний ивитов и пасов
    public class DbMetering
    {

        protected NpgsqlConnection db;
        public DbMetering()
        {
            // подключаемся к базе данных
            this.db = new NpgsqlConnection("Server=127.0.0.1;Port=5432;User Id=ivit;Password=ivit;Database=metering;");
            db.Open();
            //writeTemps("{'readings':[{'par':'12','id':'1'}]}");
        }
        
        //пишет в базу показания
        //[
        //  {temp:"", id:""},
        //  {temp:"", id:""},
        //]
        public void writeTemps(Readings readings)
        {
            foreach( Reading reading in readings.pars)
            {
                // проверить в базе есть или нет таблица
                NpgsqlCommand npgSqlCommand = new NpgsqlCommand(" SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'temps_"+reading.id+"';", this.db);
                NpgsqlDataReader npgSqlDataReader = npgSqlCommand.ExecuteReader();
               
                if (npgSqlDataReader.HasRows)
                {
                    npgSqlDataReader.Close();
                    //если есть тогда заполняем
                    NpgsqlCommand createCommand = new NpgsqlCommand("INSERT INTO temps_" + reading.id + " (par) VALUES ('"+reading.par+"');", this.db);
                    createCommand.ExecuteNonQuery();
                }
                else
                {
                    npgSqlDataReader.Close();
                    //если нет тогда создаем таблицу
                    NpgsqlCommand createCommand = new NpgsqlCommand("CREATE TABLE temps_"+reading.id+" (id bigserial, par text, date_create timestamp default current_timestamp, PRIMARY KEY(id));", this.db);
                    createCommand.ExecuteNonQuery();
                }
            }
        }
        

        //читает из базы покащзания   
        //{
        //    startTime:"",
        //    stopTime:"",
        //    idSensors:[]
        //}
        public void showTemps(string startTime, string stopTime, string[] idSensors)
        {

        }
        public class Reading
        {
            public string par { get; set; }
            public string id { get; set; }
        }

        public class Readings
        {
            public List<Reading> pars { get; set; }
        }
    }

    public class FTP
    {   protected string pubPath = Environment.CurrentDirectory;
        protected string imgsPath = Environment.CurrentDirectory + "\\..\\..\\public\\img";
        protected string configPath = Environment.CurrentDirectory + "\\..\\..\\public";
        protected string indicationsPath = Environment.CurrentDirectory + "\\..\\..\\data\\indications";

        protected string host = "10.20.2.93";
        protected string login = "ivit";
        protected string password = "";
        SessionOptions sessionOptions = new SessionOptions
        {
            Protocol = Protocol.Ftp,
            HostName = "10.20.2.93",
            UserName = "ivit",
            Password = "",
            //SshHostKeyFingerprint = "ssh-rsa 2048 xx:xx:xx:xx:xx:xx:xx:xx..."
        };
        public void loadJConfig()
        {
            try
            {
                // Создаем объект FtpWebRequest
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://"+host+"/JConfig.json");
                // устанавливаем метод на загрузку файлов
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                // если требуется логин и пароль, устанавливаем их
                request.Credentials = new NetworkCredential(login, password);
                //request.EnableSsl = true; // если используется ssl

                // получаем ответ от сервера в виде объекта FtpWebResponse
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                // получаем поток ответа
                Stream responseStream = response.GetResponseStream();
                // сохраняем файл в дисковой системе
                // создаем поток для сохранения файла

                FileStream fs = new FileStream(@configPath + "\\JConfig.json", FileMode.Create);

                //Буфер для считываемых данных
                byte[] buffer = new byte[64];
                int size = 0;

                while ((size = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fs.Write(buffer, 0, size);
                }
                fs.Close();
                response.Close();
               
            }catch(Exception exc)
            {
                Mylog.writeExc("класс: " + GetType().Name + ". метод: " + System.Reflection.MethodBase.GetCurrentMethod().Name + exc.Message);

                return;
    
            }
            
        }
        public void loadIMGs()
        {
            foreach (Form1.Room room in Form1._form._newRoot.rooms)
            {
                this.loadIMG(room.fon);
            }
        }

        public void loadIMG(string name)
        {
            try
            {
                // Создаем объект FtpWebRequest
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + host + "/img/"+name);
                // устанавливаем метод на загрузку файлов
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                // если требуется логин и пароль, устанавливаем их
                request.Credentials = new NetworkCredential(login, password);
                //request.EnableSsl = true; // если используется ssl

                // получаем ответ от сервера в виде объекта FtpWebResponse
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                // получаем поток ответа
                Stream responseStream = response.GetResponseStream();
                // сохраняем файл в дисковой системе
                // создаем поток для сохранения файла

                FileStream fs = new FileStream(imgsPath + "\\"+name, FileMode.Create);

                //Буфер для считываемых данных
                byte[] buffer = new byte[64];
                int size = 0;

                while ((size = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fs.Write(buffer, 0, size);
                }
                fs.Close();
                response.Close();
            }
            catch (Exception exc)
            {
                Mylog.writeExc("класс: " + GetType().Name + ". метод: " + System.Reflection.MethodBase.GetCurrentMethod().Name + exc.Message);
                return;

            }

        }
        public void uploadJConfig()
        {
            try
            {
                // Создаем объект FtpWebRequest - он указывает на файл, который будет создан
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + host + "/JConfig.json");
                // устанавливаем метод на загрузку файлов
                request.Method = WebRequestMethods.Ftp.UploadFile;
                // если требуется логин и пароль, устанавливаем их
                request.Credentials = new NetworkCredential(login, password);
                //request.EnableSsl = true; // если используется ssl

                // создаем поток для загрузки файла
                FileStream fs = new FileStream(@configPath + "\\JConfig.json", FileMode.Open);
                byte[] fileContents = new byte[fs.Length];
                fs.Read(fileContents, 0, fileContents.Length);
                fs.Close();
                request.ContentLength = fileContents.Length;

                // пишем считанный в массив байтов файл в выходной поток
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                // получаем ответ от сервера в виде объекта FtpWebResponse
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();
                this.uploadIMGs();
            }
            catch (Exception exc)
            {
                Mylog.writeExc("класс: " + GetType().Name + ". метод: " + System.Reflection.MethodBase.GetCurrentMethod().Name + exc.Message);
                return;

            }
        }
        public void uploadIMGs()
        {
            foreach(Form1.Room room in Form1._form._newRoot.rooms)
            {
                this.uploadIMG(room.fon);
            }
        }
        public void uploadIMG(string name)
        {
            try
            {
                // Создаем объект FtpWebRequest - он указывает на файл, который будет создан
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + host + "/img/"+name);
                // устанавливаем метод на загрузку файлов
                request.Method = WebRequestMethods.Ftp.UploadFile;
                // если требуется логин и пароль, устанавливаем их
                request.Credentials = new NetworkCredential(login, password);
                //request.EnableSsl = true; // если используется ssl

                // создаем поток для загрузки файла
                FileStream fs = new FileStream(imgsPath + "\\"+name, FileMode.Open);
                byte[] fileContents = new byte[fs.Length];
                fs.Read(fileContents, 0, fileContents.Length);
                fs.Close();
                request.ContentLength = fileContents.Length;

                // пишем считанный в массив байтов файл в выходной поток
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                // получаем ответ от сервера в виде объекта FtpWebResponse
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();
            }
            catch (Exception exc)
            {
                Mylog.writeExc("класс: " + GetType().Name + ". метод: " + System.Reflection.MethodBase.GetCurrentMethod().Name + exc.Message);

                return;

            }
        }

        public void loadIndications(string path)
        {

            try
            {
                //загрузить файл если размер разный
                using (Session session = new Session())
                {
                    session.Open(sessionOptions);
                    var directory = session.ListDirectory("/indications" + path.Replace("\\", "/"));
                    foreach (RemoteFileInfo fileInfo in directory.Files)
                    {

                        if (!File.Exists(indicationsPath + path + "\\" + fileInfo.Name))
                        {
                            session.GetFiles("/indications" + path.Replace("\\", "/") + "/" + fileInfo.Name, indicationsPath + path + "\\" + fileInfo.Name);
                        }
                        else
                        {
                            //если существует файл, проверяем размеры
                            FileInfo file = new FileInfo(indicationsPath + path + "\\" + fileInfo.Name);
                            
                            if (file.Length < fileInfo.Length)
                            {MessageBox.Show(file.Length + " < " + fileInfo.Length);
                                session.GetFiles("/indications" + path.Replace("\\", "/") + "/" + fileInfo.Name, indicationsPath + path + "\\" + fileInfo.Name);
                            }
                        }
                    }
                    session.Close();
                }

            }
            catch (Exception exc)
            {
                Mylog.writeExc("класс: " + GetType().Name + ". метод: " + System.Reflection.MethodBase.GetCurrentMethod().Name + exc.Message);
                if (exc.HResult.ToString() == "-2146233088" || exc.HResult.ToString() == "2146233088")
                {
                    loadIndications(path);
                }
                return;

            }

        }
        public void uploadIndications(string path)
        {

        
                //загрузить файл если размер разный
                using (Session session = new Session())
                {

                    try
                    {
                        session.Open(sessionOptions);

                        DirectoryInfo directory = new DirectoryInfo(@indicationsPath + path);
                        foreach (var fileInfo in directory.GetFiles())
                        {

                            if (!session.FileExists("/indications" + path.Replace("\\", "/") + "/" + fileInfo.Name))
                            {
                                session.PutFiles(indicationsPath + path + "\\" + fileInfo.Name, "/indications" + path.Replace("\\", "/") + "/" + fileInfo.Name);
                            }
                            else
                            {
                                //если существует файл, проверяем размеры
                                long fileLength = session.GetFileInfo("/indications" + path.Replace("\\", "/") + "/" + fileInfo.Name).Length;
                                //MessageBox.Show(fileLength.ToString() + " < " + fileInfo.Length.ToString());
                                if (fileLength < fileInfo.Length)
                                {
                                    session.PutFiles(indicationsPath + path + "\\" + fileInfo.Name, "/indications" + path.Replace("\\", "/") + "/" + fileInfo.Name);
                                }
                            }
                        }
                        session.Close();
                    }catch(Exception exc)
                    {
                        Mylog.writeExc("класс: " + GetType().Name + ". метод: " + System.Reflection.MethodBase.GetCurrentMethod().Name + exc.Message);
                        return;
                    }
                }


        }
        public void createIndicationPaths()
        {

            if (!Directory.Exists(indicationsPath))
            {
                Directory.CreateDirectory(indicationsPath);
            }

            using (Session session = new Session())
            {

                session.Open(sessionOptions);
                RemoteDirectoryInfo directory = session.ListDirectory("/indications");
                foreach (RemoteFileInfo fileInfo in directory.Files)
                {
                    if (!Directory.Exists(indicationsPath + '\\' + fileInfo.Name))
                    {
                        Directory.CreateDirectory(indicationsPath + '\\' + fileInfo.Name);
                    }
                }
                DirectoryInfo dirYear = new DirectoryInfo(indicationsPath);
                foreach (var item in dirYear.GetDirectories())
                {
                    RemoteDirectoryInfo directory2 = session.ListDirectory("/indications/" + item);
                    foreach (RemoteFileInfo fileInfo2 in directory2.Files)
                    {
                        if (!Directory.Exists(indicationsPath + '\\' + item + "\\" + fileInfo2.Name))
                        {
                            Directory.CreateDirectory(indicationsPath + '\\' + item + "\\" + fileInfo2.Name + "\\wets");
                            Directory.CreateDirectory(indicationsPath + '\\' + item + "\\" + fileInfo2.Name + "\\temps");
                            Directory.CreateDirectory(indicationsPath + '\\' + item + "\\" + fileInfo2.Name + "\\pressure");
                        }
                    }
                }
                session.Close();



            }

        }
    } 

    public class Mylog
    {
        protected static Logger _logger;

        private Mylog()
        {
        }

        protected static Logger Logger
        {
            get {
                    if (Mylog._logger == null) {
                        Mylog._logger = LogManager.GetCurrentClassLogger();
                    }
                    return Mylog._logger; 
                }
            set {}
        }
        public static void writeExc(string message)
        {
            Mylog.Logger.Debug(message);
        }
    }
    
}
