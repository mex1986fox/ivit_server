using System;
using System.Collections.Generic;

using System.Drawing;

using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using Newtonsoft.Json;

using Modbus.Device;
using System.Net.Sockets;

using System.Runtime.Serialization.Formatters.Binary;

using NLog;
using System.Threading;
using System.Timers;

namespace IvitRooms
{
    public partial class Form1 : Form
    {
        static System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        public Root _newRoot;
        //protected DbMetering _db = new DbMetering();
        protected FTP _ftp = new FTP(); 
        public static Form1 _form;
        public bool flagStartPooling=false;
        protected WriterIndication _writerIndicaton = new WriterIndication();
        private System.Threading.Timer timerSynch;
        protected Thread aNewThread;
        //public async void startUploadIndications()
        //{
        //    await Task.Run(() =>
        //    {
        //        timerUploadIndication = new System.Threading.Timer(UploadIndications, "", 0, 60000);
        //    });
        //}

        //public void stopUploadIndications(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        timerUploadIndication.Change(System.Threading.Timeout.Infinite, 0);
        //        timerUploadIndication.Dispose();
        //    }
        //    catch (Exception exc)
        //    {

        //        Mylog.writeExc("класс: " + GetType().Name + ". метод: " + System.Reflection.MethodBase.GetCurrentMethod().Name + exc.Message);
        //        return;
        //    }
        public void startUploadApp()
        {
            aNewThread = new Thread(
                () => UplodadApp()
            );
            aNewThread.Start();
        }
        public void stopUploadApp()
        {
            aNewThread.Abort();
        }
        public void UplodadApp()
        {
            Thread.Sleep(3600*1000);
            //Thread.Sleep(30000);
            Application.Restart();
        }
        public Form1()
        {


            InitializeComponent();
            //_ftp.loadJConfig();
            //startUploadIndications();
            //timerUploadIndication = new System.Windows.Forms.Timer(UploadIndications, "", 0, 20000);


            button1.Show();
            button3.Show();
            button2.Hide();
            //var assemb = Assembly.GetExecutingAssembly();
            string pubPath = Environment.CurrentDirectory;
            string imgsPath = pubPath + "\\..\\..\\public\\img";
            string configPath = pubPath + "\\..\\..\\public";

            string jConfig = System.IO.File.ReadAllText(@configPath + "\\JConfig.json", Encoding.UTF8).Replace("\n", " ");
            
            Form1._form = this;
            Room.form = this;
            Ivit.form = this;
            Pas.form = this;
            this._newRoot = JsonConvert.DeserializeObject<Root>(jConfig);
            //richTextBox1.Text = jConfig;
          

            tabControl1.AutoSize = true;
            this.Controls.Add(tabControl1);
            this.startUploadApp();
            this.Start();
        }



        public class Room
        {

            public int id { get; set; }
            public string name { get; set; }

            private string _fon;
            public string fon
            {
                get { return this._fon; }
                set
                {
                   // Form1._form._ftp.loadIMG(value);
                    this._fon = value;
                    //загружает картинку для фона вкладки
                    string pubPath = Environment.CurrentDirectory;
                    string imgsPath = pubPath + "\\..\\..\\public\\img";
                    Bitmap image;
                    using (var bmpTemp = new Bitmap(imgsPath + "\\" + value))
                    {
                        image = new Bitmap(bmpTemp);
                    }
                    PictureBox pictImage = new PictureBox();
                    pictImage.Name = "fon_" + this.id;
                    pictImage.Image = image;
                    pictImage.Width = image.Width;
                    pictImage.Height = image.Height;
                    TabPage newTabPage = new TabPage();
                    newTabPage.AutoSize = true;
                    newTabPage.BackColor = Color.FromArgb(255, 255, 255);
                    newTabPage.Name = "rom_" + this.id;
                    newTabPage.Text = this.name;
                    newTabPage.AutoScroll = true;
                    newTabPage.Controls.Add(pictImage);
                    form.tabControl1.TabPages.Add(newTabPage);
                }
            }
            [JsonIgnore]
            public static Form1 form { get; set; }

        }

        public class Cooler
        {
            protected int _id;
            public int id {
                get { return this._id; }
                set { this._id = value; }
            }

            protected string _status;
            public string status {
                get { return this._status; }
                set { this._status = value; }
            }
            protected int _roomID;
            public int roomID
            {
                get { return this._roomID; }
                set { this._roomID = value; this.init(); }
            }
            public string name { get; set; }

            protected bool _rotation = false;
            public bool rotation {
                get { return this._rotation; }
                set {
                    this._rotation = value;
                    if (value == true)
                    {
                        this.startFlip();
                    }
                    if (value == false)
                    {
                        this.stopFlip();
                    }
                }
            }
            private int _positionX;
            public int positionX
            {
                get { return this._positionX; }
                set
                {
                    this._positionX = value;
                    this.setPositionX();
                }
            }

            private int _positionY;
            public int positionY
            {
                get { return this._positionY; }
                set
                {
                    this._positionY = value;
                    this.setPositionY();
                }
            }

            [JsonIgnore]
            public Panel _panel;
            [JsonIgnore]
            public PictureBox _picImg;
            [JsonIgnore]
            public Bitmap _img;
            [JsonIgnore]
            System.Threading.Timer timer;
            protected void init()
            {
                //загружает картинку
                string pubPath = Environment.CurrentDirectory;
                string imgsPath = pubPath + "\\..\\..\\public\\img";
                Bitmap image = (Bitmap)Bitmap.FromFile(imgsPath + "\\cooler.png");
                this._img = image;
                Panel panel = new Panel();
                this._panel = panel;
                panel.Name = "panel_" + id;
                panel.Width = 86;
                panel.Height = 57;
                //panel.BackColor = Color.FromArgb(0, 10, 100);
                panel.BackColor = Color.Transparent;

                PictureBox pictImage = new PictureBox();
                this._picImg = pictImage;
                pictImage.Name = "img_" + id;
                pictImage.Image = image;
                pictImage.BackColor = Color.Transparent;
                panel.Controls.Add(pictImage);
                Form1._form.tabControl1.TabPages["rom_" + roomID].Controls.Find("fon_" + roomID, true)[0].Controls.Add(panel);
                pictImage.MouseDown += mDown;
                panel.MouseDown += mDown;
                Form1._form.tabControl1.TabPages["rom_" + roomID].Controls.Find("fon_" + roomID, true)[0].MouseDown += mUp;

            }
            public void mDown(object sender, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left)
                {

                    Form1._form.tabControl1.TabPages["rom_" + roomID].Controls.Find("fon_" + roomID, true)[0].MouseMove += setPositionIvit;
                }
            }
            public void mUp(object sender, MouseEventArgs e)
            {

                if (e.Button == MouseButtons.Right)
                {
                    Form1._form.tabControl1.TabPages["rom_" + roomID].Controls.Find("fon_" + roomID, true)[0].MouseMove -= setPositionIvit;
                }

            }

            public void setPositionIvit(object sender, MouseEventArgs e)
            {
                //  form.richTextBox1.Text = "OK Cursor";

                this.positionX = Cursor.Position.X
                    - Form1._form.Location.X - Form1._form.tabControl1.Location.X
                    - Form1._form.tabControl1.TabPages["rom_" + roomID].Location.X
                    - Form1._form.tabControl1.TabPages["rom_" + roomID].Controls.Find("fon_" + roomID, true)[0].Location.X;
                this.positionY = Cursor.Position.Y
                    - Form1._form.Location.Y - Form1._form.tabControl1.Location.Y
                    - Form1._form.tabControl1.TabPages["rom_" + roomID].Location.Y
                    - Form1._form.tabControl1.TabPages["rom_" + roomID].Controls.Find("fon_" + roomID, true)[0].Location.Y - 25;
                this._panel.Location = new Point(this.positionX, this.positionY);

                //Form1._form.richTextBox1.Text = this.positionX + ":" + this.positionY;


            }
            public void setPositionX()
            {
                this._panel.Location = new Point(this.positionX, this.positionY);
            }
            public void setPositionY()
            {
                this._panel.Location = new Point(this.positionX, this.positionY);
            }
            public void startFlip()
            {

                string pubPath = Environment.CurrentDirectory;
                string imgsPath = pubPath + "\\..\\..\\public\\img";
                this._img = (Bitmap)Bitmap.FromFile(imgsPath + "\\cooler.gif");
                this._picImg.Image = this._img;
            }

            public void stopFlip()
            {
                string pubPath = Environment.CurrentDirectory;
                string imgsPath = pubPath + "\\..\\..\\public\\img";
                this._img = (Bitmap)Bitmap.FromFile(imgsPath + "\\cooler.png");
                this._picImg.Image = this._img;

            }

        }
        public class Ivit
        {

            protected int _id;
            private System.Threading.Timer timer;
            protected Modbus.Device.ModbusIpMaster _modbusIpMaster;
            public int id {
                get { return this._id; }
                set {
                    this._id = value;
                    tempSchedule.id = value;
                    wetSchedule.id = value;
                }
            }
            protected string _status;
            public string status
            {
                get { return this._status; }
                set { this._status = value; this.initStatus(); }
            }
            protected int _roomID;
            public int roomID {
                get { return this._roomID; }
                set { this._roomID = value; this.init(); }
            }
            public string ip { get; set; }
            public int port { get; set; }
            public int sirialID { get; set; }

            protected string _name;
            public string name {
                get { return _name; }

                set {
                    _name = value;
                    tempSchedule.name = value;
                    wetSchedule.name = value;
                }
            }

            protected string _temp;
            public string temp
            {
                get { return this._temp; }

                set {
                    this._temp = value;
                    this.initTemp();
                }
            }

            protected string _wet;

            public string wet {
                get { return this._wet; }
                set {
                    this._wet = value;
                    this.initWet();
                }
            }

            protected Int32 _tempPK = 0;
            public Int32 tempPK
            {
                get { return this._tempPK; }

                set
                {
                    this._tempPK = value;
                }
            }

            protected Int32 _wetPK = 0;
            public Int32 wetPK
            {
                get { return this._wetPK; }

                set
                {
                    this._wetPK = value;
                }
            }

            private int _positionX;
            public int positionX
            {
                get { return this._positionX; }
                set {
                    this._positionX = value;
                    this.setPositionX();
                }
            }

            private int _positionY;
            public int positionY
            {
                get { return this._positionY; }
                set
                {
                    this._positionY = value;
                    this.setPositionY();
                }
            }
            protected void init()
            {
                //загружает картинку для ивита
                string pubPath = Environment.CurrentDirectory;
                string imgsPath = pubPath + "\\..\\..\\public\\img";
                Image image = Image.FromFile(imgsPath + "\\ivit.png");
                
                Panel panel = new Panel();
                this._panel = panel;
                panel.Name = "panel_" + id;
                panel.Width = 86;
                panel.Height = 57;
                //panel.BackColor = Color.FromArgb(0, 10, 100);
                panel.BackColor = Color.Transparent;

                Label lable = new Label();
                lable.Name = "lable_" + id;
                lable.Height = 36;
                lable.Width = 64;
                lable.Text = this.temp;
                lable.Padding = new Padding(0, 2, 0, 0);
                lable.Font = new Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
                lable.ForeColor = Color.FromArgb(0, 204, 0);
                lable.BackColor = Color.FromArgb(0, 0, 0);
                lable.Location = new Point(21, 19);
                panel.Controls.Add(lable);

     
                PictureBox pictImage = new PictureBox();
                this._img = pictImage;
                pictImage.Name = "img_" + id;
                pictImage.Image = image;
                pictImage.BackColor = Color.Transparent;
                panel.Controls.Add(pictImage);
                form.tabControl1.TabPages["rom_" + roomID].Controls.Find("fon_" + roomID, true)[0].Controls.Add(panel);
                
                pictImage.MouseDown += mDown;
                panel.MouseDown += mDown;
                lable.MouseDown += mDown;
                lable.MouseHover += mGotCapture;
                form.tabControl1.TabPages["rom_" + roomID].Controls.Find("fon_" + roomID, true)[0].MouseDown += mUp;

            }
            protected void initTemp()
            {
                if (temp != null && wet != null)
                {
                    this._panel.Controls.Find("lable_" + id, true)[0].Invoke(new Action(() => {
                        string t = (Convert.ToDouble(temp) + tempPK).ToString();
                        string w = (Convert.ToDouble(wet) + wetPK).ToString();
                        this._panel.Controls.Find("lable_" + id, true)[0].Text = t + " C\n" + w + " %";
                    }));
                    this.status = "";
                }
            }
            protected void initWet()
            {
                if (temp != null && wet != null)
                {
                    this._panel.Controls.Find("lable_" + id, true)[0].Invoke(new Action(() => {
                        string t = (Convert.ToDouble(temp) + tempPK).ToString();
                        string w = (Convert.ToDouble(wet) + wetPK).ToString();
                        this._panel.Controls.Find("lable_" + id, true)[0].Text = t + " C\n" + w + " %";
                    }));
                    this.status = "";
                }

            }
            protected void initStatus()
            {
                if (status!="" && temp != null && wet != null)
                {
                    this._panel.Controls.Find("lable_" + id, true)[0].Invoke(new Action(() => {
                        this._panel.Controls.Find("lable_" + id, true)[0].Text = status;
                        this._panel.Controls.Find("lable_" + id, true)[0].ForeColor = Color.FromArgb(255, 0, 0);
                    }));
                    if(status=="no connect!")
                    {
                        this._img.Invoke(new Action(() => {
                            string pubPath = Environment.CurrentDirectory;
                            string imgsPath = pubPath + "\\..\\..\\public\\img";
                            Image image = Image.FromFile(imgsPath + "\\ivit_err.png");
                            this._img.Image = image;
                            
                        }));
                    }
                }
                if (status == "" && temp != null && wet != null)
                {
                    this._img.Invoke(new Action(() => {
                        string pubPath = Environment.CurrentDirectory;
                        string imgsPath = pubPath + "\\..\\..\\public\\img";
                        Image image = Image.FromFile(imgsPath + "\\ivit.png");
                        this._img.Image = image;
                        this._panel.Controls.Find("lable_" + id, true)[0].ForeColor = Color.FromArgb(0, 204, 0);

                    }));
                }
            }
            [JsonIgnore]
            public Panel _panel;
            [JsonIgnore]
            public PictureBox _img;
            [JsonIgnore]
            public Schedule tempSchedule = new Schedule();
            [JsonIgnore]
            public Schedule wetSchedule = new Schedule();

            public static Form1 form { get; set; }
            public void mDown(object sender, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left)
                {

                    form.tabControl1.TabPages["rom_" + roomID].Controls.Find("fon_" + roomID, true)[0].MouseMove += setPositionIvit;
                }
            }
            public void mUp(object sender, MouseEventArgs e)
            {

                if (e.Button == MouseButtons.Right)
                {
                    form.tabControl1.TabPages["rom_" + roomID].Controls.Find("fon_" + roomID, true)[0].MouseMove -= setPositionIvit;
                }

            }
            public void mGotCapture(object sender, System.EventArgs e)
            {
                ToolTip toolTip1 = new ToolTip();

                // Set up the delays for the ToolTip.
                toolTip1.AutoPopDelay = 5000;
                toolTip1.InitialDelay = 1000;
                toolTip1.ReshowDelay = 500;
                // Force the ToolTip text to be displayed whether or not the form is active.
                toolTip1.ShowAlways = true;
                var lable = (Label)sender;
                Ivit infoIvit = Form1._form._newRoot.ivits[Convert.ToInt32( lable.Name.Replace("lable_", ""))-1];
                string info =
                    infoIvit.name + "\r\n"+
                    "ID: " +infoIvit.id.ToString()+"\r\n"+
                    "IP: " + infoIvit.ip.ToString() + "\r\n" +
                    "SerNumber: " + infoIvit.sirialID.ToString() + "\r\n";
                // Set up the ToolTip text for the Button and Checkbox.
                toolTip1.SetToolTip(lable, info);
      
            }
            public void setPositionIvit(object sender, MouseEventArgs e)
            {
                //  form.richTextBox1.Text = "OK Cursor";

                this.positionX = Cursor.Position.X
                    - form.Location.X - form.tabControl1.Location.X
                    - form.tabControl1.TabPages["rom_" + roomID].Location.X
                    - form.tabControl1.TabPages["rom_" + roomID].Controls.Find("fon_" + roomID, true)[0].Location.X;
                this.positionY = Cursor.Position.Y
                    - form.Location.Y - form.tabControl1.Location.Y
                    - form.tabControl1.TabPages["rom_" + roomID].Location.Y
                    - form.tabControl1.TabPages["rom_" + roomID].Controls.Find("fon_" + roomID, true)[0].Location.Y - 25;
                this._panel.Location = new Point(this.positionX, this.positionY);

               // form.richTextBox1.Text = this.positionX + ":" + this.positionY;


            }
            public void setPositionX()
            {
                this._panel.Location = new Point(this.positionX, this.positionY);
            }
            public void setPositionY()
            {
                this._panel.Location = new Point(this.positionX, this.positionY);
            }
            //стартует опрос ивита
            public async void startPooling()
            {

               await Task.Run(() =>
               {
                    temp = "0";
                    wet = "0";
                    if (_modbusIpMaster != null)
                    {
                        CreatModbIPMaster();
                    }
                    timer = new System.Threading.Timer(Pool, "", 0, 1000);
                   
                    // Form1._form.richTextBox1.Invoke(new Action(() => Form1._form.richTextBox1.Text = Form1._form.richTextBox1.Text + "\r\n IVIT #" + id + " start"));
                });
                

            }
            //прекращает опрос ивита
            public void stopPooling()
            {
                try
                {
                    timer.Change(System.Threading.Timeout.Infinite, 0);
                    timer.Dispose();
                }catch(Exception exc)
                {

                    Mylog.writeExc("класс: " + GetType().Name + ". метод: " + System.Reflection.MethodBase.GetCurrentMethod().Name + exc.Message);
                    return;
                }
                temp = "0";
                wet = "0";
                // Form1._form.richTextBox1.Text = Form1._form.richTextBox1.Text + "\r\n IVIT #" + id + " stop";
            }
            //создает подключение модбус
            private void CreatModbIPMaster()
            {
               // await Task.Run(() => {
                    try
                    {
                        this._modbusIpMaster = ModbusIpMaster.CreateIp(new TcpClient(ip, port));
                    }
                    catch (Exception exc)
                    {
                    // Form1._form.richTextBox1.Invoke(new Action(() => Form1._form.richTextBox1.Text ="\r\n IVIT #" + id + " ошибка при подключении \r\n"+exc.Message + "\r\n" +Form1._form.richTextBox1.Text));
                        if(exc.HResult.ToString() !="-2147467259"){
                            Mylog.writeExc("класс: "+ GetType().Name+". метод: "+ System.Reflection.MethodBase.GetCurrentMethod().Name + exc.Message);
                        }
                   
                        status = "no connect!";
                        return;
                    }

                //});
            }
            public void Pool(object objectInfo)
            {
                if (_modbusIpMaster != null)
                {
                    try
                    {
                        // получаем температуру влажность 
                        var temps = _modbusIpMaster.ReadInputRegisters(Convert.ToByte(sirialID), 34, 2);
                        var mois = _modbusIpMaster.ReadInputRegisters(Convert.ToByte(sirialID), 22, 2);
                        temp = System.Math.Round(FloatRegToString(temps[0], temps[1]), 2).ToString();
                        wet = System.Math.Round(FloatRegToString(mois[0], mois[1]), 2).ToString();
                        // Form1._form.CreatModbIPMaster("ivit", ivit.id, ivit.ip, ivit.port);

                    }
                    catch (Exception exc)
                    {
                        
                        if (exc.Message == "Read resulted in 0 bytes returned.")
                        {
                            return;
                        }
                        if(exc.Message == "Операция не разрешается на неподключенных сокетах.")
                        {
                            CreatModbIPMaster();
                            return;
                        }

                        Mylog.writeExc("класс: " + GetType().Name + ". метод: " + System.Reflection.MethodBase.GetCurrentMethod().Name + exc.Message);
                        return;
                        // Form1._form.richTextBox1.Invoke(new Action(() => Form1._form.richTextBox1.Text = Form1._form.richTextBox1.Text + "\n" + name + ": - " + exc.Message));
                        //Form1._form.richTextBox1.Text = exc.Message;
                    }
                }
                else
                {
                    CreatModbIPMaster();
                }

            }



}

        public class Pas
        {
            private System.Threading.Timer timer;
            protected Modbus.Device.ModbusIpMaster _modbusIpMaster;
            protected int _id;
            public int id
            {
                get { return this._id; }
                set {
                    this._id = value;
                    this.pressureSchedule.id = value;

                }
            }
            protected string _status;
            public string status
            {
                get { return this._status; }
                set { this._status = value; this.initStatus(); }
            }
            protected int _roomID;
            public int roomID
            {
                get { return this._roomID; }
                set { this._roomID = value; this.init(); }
            }
            public string ip { get; set; }
            public int port { get; set; }
            public int sirialID { get; set; }
            protected string _name;
            public string name
            {
                get { return this._name; }
                set
                {
                    this._name = value;
                    this.pressureSchedule.name = value;
                }
            }

            protected string _pressure;
            public string pressure
            {
                get { return this._pressure; }

                set { this._pressure = value; this.initPressure(); }
            }

            protected Int32 _pressurePK = 0;
            public Int32 pressurePK
            {
                get { return this._pressurePK; }

                set
                {
                    this._pressurePK = value;
                }
            }

            private int _positionX;
            public int positionX
            {
                get { return this._positionX; }
                set
                {
                    this._positionX = value;
                    this.setPositionX();
                }
            }

            private int _positionY;
            public int positionY
            {
                get { return this._positionY; }
                set
                {
                    this._positionY = value;
                    this.setPositionY();
                }
            }
            protected void init()
            {
                //загружает картинку для ивита
                string pubPath = Environment.CurrentDirectory;
                string imgsPath = pubPath + "\\..\\..\\public\\img";
                Image image = Image.FromFile(imgsPath + "\\pas.png");

                Panel panel = new Panel();
                this._panel = panel;
                panel.Name = "panel-pas_" + id;
                panel.Width = 76;
                panel.Height = 61;
                //panel.BackColor = Color.FromArgb(0, 10, 100);
                panel.BackColor = Color.Transparent;

                Label lable = new Label();
                lable.Name = "lable_" + id;
                lable.Height = 17;
                lable.Width = 65;
                lable.Text = this.pressure;
                //lable.Padding = new Padding(0, 2, 0, 0);
                lable.Font = new Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
                lable.ForeColor = Color.FromArgb(0, 204, 0);
                lable.BackColor = Color.FromArgb(0, 0, 0);
                //lable.BackColor = Color.Transparent;
                lable.Location = new Point(5, 26);
                panel.Controls.Add(lable);


                PictureBox pictImage = new PictureBox();
                this._img = pictImage;
                pictImage.Height = 61;
                pictImage.Name = "img_" + id;
                pictImage.Image = image;
                pictImage.BackColor = Color.Transparent;
                panel.Controls.Add(pictImage);
                form.tabControl1.TabPages["rom_" + roomID].Controls.Find("fon_" + roomID, true)[0].Controls.Add(panel);
                pictImage.MouseDown += mDown;
                panel.MouseDown += mDown;
                lable.MouseDown += mDown;
                lable.MouseHover += mGotCapture;
                form.tabControl1.TabPages["rom_" + roomID].Controls.Find("fon_" + roomID, true)[0].MouseDown += mUp;

            }
            protected void initPressure()
            {
                if (pressure != null)
                {
                    this._panel.Controls.Find("lable_" + id, true)[0].Invoke(new Action(() => {
                        this._panel.Controls.Find("lable_" + id, true)[0].Text = (Convert.ToDouble(pressure) + pressurePK).ToString() + " Па";
                    }));
                    this.status = "";
                }
            }

            protected void initStatus()
            {
                if (status != "" && pressure != null)
                {
                    this._panel.Controls.Find("lable_" + id, true)[0].Invoke(new Action(() => {
                        this._panel.Controls.Find("lable_" + id, true)[0].Text = "no con!";
                        this._panel.Controls.Find("lable_" + id, true)[0].ForeColor = Color.FromArgb(255, 0, 0);
                    }));
                    if (status == "no connect!")
                    {
                        this._img.Invoke(new Action(() => {
                            string pubPath = Environment.CurrentDirectory;
                            string imgsPath = pubPath + "\\..\\..\\public\\img";
                            Image image = Image.FromFile(imgsPath + "\\pas_err.png");
                            this._img.Image = image;

                        }));
                    }
                }
                if (status == "" && pressure != null)
                {
                    this._img.Invoke(new Action(() => {
                        string pubPath = Environment.CurrentDirectory;
                        string imgsPath = pubPath + "\\..\\..\\public\\img";
                        Image image = Image.FromFile(imgsPath + "\\pas.png");
                        this._img.Image = image;
                        this._panel.Controls.Find("lable_" + id, true)[0].ForeColor = Color.FromArgb(0, 204, 0);

                    }));
                }
            }
            [JsonIgnore]
            public Panel _panel;
            [JsonIgnore]
            public Schedule pressureSchedule = new Schedule();
            [JsonIgnore]
            public PictureBox _img;
            public static Form1 form { get; set; }
            public void mDown(object sender, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left)
                {

                    form.tabControl1.TabPages["rom_" + roomID].Controls.Find("fon_" + roomID, true)[0].MouseMove += setPositionIvit;
                }
            }
            public void mUp(object sender, MouseEventArgs e)
            {

                if (e.Button == MouseButtons.Right)
                {
                    form.tabControl1.TabPages["rom_" + roomID].Controls.Find("fon_" + roomID, true)[0].MouseMove -= setPositionIvit;
                }

            }
            public void mGotCapture(object sender, System.EventArgs e)
            {
                ToolTip toolTip1 = new ToolTip();

                // Set up the delays for the ToolTip.
                toolTip1.AutoPopDelay = 5000;
                toolTip1.InitialDelay = 1000;
                toolTip1.ReshowDelay = 500;
                // Force the ToolTip text to be displayed whether or not the form is active.
                toolTip1.ShowAlways = true;
                var lable = (Label)sender;
                Pas infoIvit = Form1._form._newRoot.pases[Convert.ToInt32(lable.Name.Replace("lable_", "")) - 1];
                string info =
                    infoIvit.name + "\r\n" +
                    "ID: " + infoIvit.id.ToString() + "\r\n" +
                    "IP: " + infoIvit.ip.ToString() + "\r\n" +
                    "SerNumber: " + infoIvit.sirialID.ToString() + "\r\n";
                // Set up the ToolTip text for the Button and Checkbox.
                toolTip1.SetToolTip(lable, info);

            }
            public void setPositionIvit(object sender, MouseEventArgs e)
            {
                //  form.richTextBox1.Text = "OK Cursor";

                this.positionX = Cursor.Position.X
                    - form.Location.X - form.tabControl1.Location.X
                    - form.tabControl1.TabPages["rom_" + roomID].Location.X
                    - form.tabControl1.TabPages["rom_" + roomID].Controls.Find("fon_" + roomID, true)[0].Location.X;
                this.positionY = Cursor.Position.Y
                    - form.Location.Y - form.tabControl1.Location.Y
                    - form.tabControl1.TabPages["rom_" + roomID].Location.Y
                    - form.tabControl1.TabPages["rom_" + roomID].Controls.Find("fon_" + roomID, true)[0].Location.Y - 25;
                this._panel.Location = new Point(this.positionX, this.positionY);

                //form.richTextBox1.Text = this.positionX + ":" + this.positionY;


            }
            public void setPositionX()
            {
                this._panel.Location = new Point(this.positionX, this.positionY);
            }
            public void setPositionY()
            {
                this._panel.Location = new Point(this.positionX, this.positionY);
            }
            //стартует опрос ивита
            public async void startPooling()
            {

                await Task.Run(() =>
                {
                    pressure = "0";
                    if (_modbusIpMaster != null)
                    {
                        CreatModbIPMaster();
                    }
                    timer = new System.Threading.Timer(Pool, "", 0, 1000);

                    // Form1._form.richTextBox1.Invoke(new Action(() => Form1._form.richTextBox1.Text = Form1._form.richTextBox1.Text + "\r\n IVIT #" + id + " start"));
                });


            }
            //прекращает опрос ивита
            public void stopPooling()
            {
                try
                {
                    timer.Change(System.Threading.Timeout.Infinite, 0);
                    timer.Dispose();
                }
                catch (Exception exc)
                {
                    Mylog.writeExc("класс: " + GetType().Name + ". метод: " + System.Reflection.MethodBase.GetCurrentMethod().Name + exc.Message);
                    return;
                }
                pressure = "0";
                // Form1._form.richTextBox1.Text = Form1._form.richTextBox1.Text + "\r\n IVIT #" + id + " stop";
            }
            //создает подключение модбус
            private void CreatModbIPMaster()
            {
                // await Task.Run(() => {
                try
                {
                    this._modbusIpMaster = ModbusIpMaster.CreateIp(new TcpClient(ip, port));
                }
                catch (Exception exc)
                {
                    //Form1._form.richTextBox1.Invoke(new Action(() => Form1._form.richTextBox1.Text = "\r\n IVIT #" + id + " ошибка при подключении \r\n" + exc.Message + "\r\n" + Form1._form.richTextBox1.Text));
                    if (exc.HResult.ToString() != "-2147467259")
                    {
                        Mylog.writeExc("класс: "+ GetType().Name+". метод: "+ System.Reflection.MethodBase.GetCurrentMethod().Name + exc.Message);
                    }
                    status = "no connect!";
                    return;
                }

                //});
            }
            public void Pool(object objectInfo)
            {
                if (_modbusIpMaster != null)
                {
                    try
                    {
                        // получаем температуру влажность 
                        var temps = _modbusIpMaster.ReadInputRegisters(Convert.ToByte(sirialID), 0, 2);
                        pressure = System.Math.Round(FloatRegToString(temps[0], temps[1]), 2).ToString();
                        // Form1._form.CreatModbIPMaster("ivit", ivit.id, ivit.ip, ivit.port);

                    }
                    catch (Exception exc)
                    {
                        if (exc.Message == "Read resulted in 0 bytes returned.")
                        {
                            return;
                        }
                        if (exc.Message == "Операция не разрешается на неподключенных сокетах.")
                        {
                            CreatModbIPMaster();
                            return;
                        }

                        Mylog.writeExc("класс: " + GetType().Name + ". метод: " + System.Reflection.MethodBase.GetCurrentMethod().Name + exc.Message);
                        return;
                        // Form1._form.richTextBox1.Invoke(new Action(() => Form1._form.richTextBox1.Text = Form1._form.richTextBox1.Text + "\n" + name + ": - " + exc.Message));
                        //Form1._form.richTextBox1.Text = exc.Message;
                    }
                }
                else
                {
                    CreatModbIPMaster();
                }

            }

        }
        public class Root
        {
            public List<Room> rooms { get; set; }
            public List<Ivit> ivits { get; set; }
            public List<Pas> pases { get; set; }
            public List<Cooler> coolers { get; set; }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            saveJConfig();
        }

        public void saveJConfig()
        {
            //сериализуем в строку
      
            string serialized = JsonConvert.SerializeObject(this._newRoot, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
           // richTextBox1.Text = serialized;
              
            // пишем в файл
            string pubPath = Environment.CurrentDirectory;
            string configPath = pubPath + "\\..\\..\\public";
            StreamWriter sw = new StreamWriter(configPath + "\\JConfig.json");
            sw.WriteLine(serialized);
            sw.Close();
            _ftp.uploadJConfig();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

            this.Start();
          
        }
        private void Start()
        {
            this.flagStartPooling = true;
            this.startSynch();
            foreach (Ivit ivit in _newRoot.ivits)
            {
                ivit.startPooling();
            }
            foreach (Pas pas in _newRoot.pases)
            {
                pas.startPooling();
            }

            //foreach (Ivit ivit in _newRoot.ivits)
           // {
          //      CreatModbIPMaster("ivit", ivit.id, ivit.ip, ivit.port);
           // }

          ///  foreach (Pas pas in _newRoot.pases)
          //  {
          //      CreatModbIPMaster("pas", pas.id, pas.ip, pas.port);
         //   }

            foreach (Cooler cooler in _newRoot.coolers)
            {
                cooler.rotation = true;
            }

            // TimerCallback tmCallback = PoolIvits;
            //   this.timer = new System.Threading.Timer(tmCallback, "", 0, 6000);
            _writerIndicaton.StartWriter();
            button1.Hide();
            button3.Hide();
            button2.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Stop();
        }
        private void Stop()
        {
            this.flagStartPooling = true;
            this.stopSynch();
            foreach (Cooler cooler in _newRoot.coolers)
            {
                cooler.rotation = false;
            }
            foreach (Ivit ivit in _newRoot.ivits)
            {
                ivit.stopPooling();
            }
            foreach (Pas pas in _newRoot.pases)
            {
                pas.stopPooling();
            }
             _writerIndicaton.StopWriter();
            button1.Show();
            button3.Show();
            button2.Hide();
        }

        // конвертирует значения регистров в HEX
        // конкантинирует в одно big значение
        // переводит float и отдает его 
        static public float FloatRegToString(params int[] registers)
        {
            string hexT = "";
            foreach (int reg in registers)
            {

                string hex = reg.ToString("X");
                if (hex.Length==0)
                    hexT += "0000";
                if (hex.Length == 1)
                    hexT += "000"+hex;
                if (hex.Length == 2)
                    hexT += "00" + hex;
                if (hex.Length == 3)
                    hexT += "0" + hex;
                if (hex.Length == 4)
                    hexT += hex;

            }
            uint num = uint.Parse(hexT, System.Globalization.NumberStyles.AllowHexSpecifier);
            byte[] floatVals = BitConverter.GetBytes(num);
            float f = BitConverter.ToSingle(floatVals, 0);
            return f;
        }

        private void добавитьПомещениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 newForm = new Form2();
            newForm.Show();
        }

        private void добавитьЭвитToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 newForm = new Form3();
            newForm.Show();
        }

        private void добавитьКуллерToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 newForm = new Form4();
            newForm.Show();
        }

        private void добавтьПАСToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form5 newForm = new Form5();
            newForm.Show();
        }


        private void температура2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormScheduleTemp2 newForm = new FormScheduleTemp2();
            newForm.Show();
        }

        private void ивитыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLists newForm = new FormLists();
            newForm.Show();
        }

        private void пасыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormListsPas newForm = new FormListsPas();
            newForm.Show();
        }

        private void влажностьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormScheduleWet newForm = new FormScheduleWet();
            newForm.Show();
        }

        private void перепадToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSchedulePressure newForm = new FormSchedulePressure();
            newForm.Show();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public async void startSynch()
        {

            await Task.Run(() =>
            {
                timerSynch = new System.Threading.Timer(Synch, "", 0, 15*60*1000);
            });


        }
        //прекращает опрос ивита
        public void stopSynch()
        {
            try
            {
                timerSynch.Change(System.Threading.Timeout.Infinite, 0);
                timerSynch.Dispose();
            }
            catch (Exception exc)
            {

                Mylog.writeExc("класс: " + GetType().Name + ". метод: " + System.Reflection.MethodBase.GetCurrentMethod().Name + exc.Message);
                return;
            }
        }
        public void Synch(object objectInfo)
        {
           new Synchroniz();
        }
        private void Form1_Closing(Object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Stop();
                this.stopUploadApp();
            }

        }
    }

}
