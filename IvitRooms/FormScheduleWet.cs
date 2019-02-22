using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System.Data;

namespace IvitRooms
{
    public partial class FormScheduleWet : Form
    {

        private string activeDate = DateTime.Now.Year.ToString() +
           "-" + DateTime.Now.Month.ToString() +
           "-" + DateTime.Now.Day.ToString();

        protected List<Schedule> wetSchedule = new List<Schedule>();

        protected UserJConfig userJconfig = UserJConfig.Inst();
        public FormScheduleWet()
        {
            InitializeComponent();
        }

        private void FormScheduleWet_Load(object sender, EventArgs e)
        {
            progressBar1.Visible = false;
            checkBox1.Checked = false;
            dateTimePicker1.Enabled = false;
            dateTimePicker2.Enabled = false;
            this.treeView1.Enabled = true;

            chart2.Titles.Add(new System.Windows.Forms.DataVisualization.Charting.Title("Влажность"));
            this.SetTreeVuew1();
            if (checkBox1.Checked == true)
            {
                this.SetWetsSchedulePeriod();
            }
            else
            {
                this.SetWetsSchedule();
            }
            this.SetListViev1();
            this.SetChart2();


        }
        private void SetTreeVuew1()
        {
            string pubPath = Environment.CurrentDirectory;
            DirectoryInfo dirYear = new DirectoryInfo(pubPath + "\\..\\..\\data\\indications");
            foreach (var item in dirYear.GetDirectories())
            {
                TreeNode yearNode = new TreeNode(item.Name);
                DirectoryInfo dirDate = new DirectoryInfo(pubPath + "\\..\\..\\data\\indications\\" + item.Name);
                foreach (var item2 in dirDate.GetDirectories())
                {
                    TreeNode dateNode = new TreeNode(item2.Name);
                    yearNode.Nodes.Add(dateNode);
                }
                treeView1.Nodes.Add(yearNode);
            }
        }
        private void SetWetsSchedule()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string pubPath = Environment.CurrentDirectory;

            string dataPath = pubPath + "\\..\\..\\data\\indications\\" +
                activeDate.Substring(0, 4) +
                "\\" + activeDate +
                "\\wets";

            DirectoryInfo dir = new DirectoryInfo(@dataPath);
            progressBar1.Visible = true;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = dir.GetFiles().Length;
            progressBar1.Value = 1;
            progressBar1.Step = 10;
            foreach (var item in dir.GetFiles())
            {
                progressBar1.PerformStep();
                Schedule shed = new Schedule();
                shed.id = Convert.ToInt32(item.Name.Substring(0, item.Name.Length - 4).Substring(5));
                shed.name = "noname!";
                Form1._form._newRoot.ivits.ForEach(iv =>
                {
                    if (iv.id == shed.id)
                    {
                        shed.name = iv.name;
                    }

                });
                using (FileStream fs = new FileStream(dataPath + "\\" + item.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {

                    foreach (int id in userJconfig.formScheduleWets.checkedIvits)
                    {
                        if (id == Convert.ToInt32(item.Name.Substring(0, item.Name.Length - 4).Substring(5)))
                        {
                            using (BinaryReader reader = new BinaryReader(fs))
                            {
                                // пока не достигнут конец файла
                                // считываем каждое значение из файла
                                while (reader.PeekChar() > -1)
                                {
                                    Indication ind = new Indication();

                                    DateTime dt = DateTime.Parse(reader.ReadString());
                                    ind.datetime = dt;

                                    ind.value = Convert.ToSingle(reader.ReadString());
                                    shed.indications.Add(ind);
                                }
                            }
                        }
                    }
                    fs.Close();
                }
                wetSchedule.Add(shed);
            }
            progressBar1.Visible = false;
        }
        private void SetWetsSchedulePeriod()
        {

            DateTime startDate = dateTimePicker1.Value;
            DateTime stopDate = dateTimePicker2.Value;
            string pubPath = Environment.CurrentDirectory;
            string dataPath = "";
            progressBar1.Visible = true;
            while (startDate < stopDate)
            {
                dataPath = pubPath + "\\..\\..\\data\\indications\\" +
                startDate.Year.ToString() +
                "\\" + startDate.Year.ToString() + "-" + startDate.Month.ToString() + "-" + startDate.Day.ToString() +
                "\\wets";

                //определить существование дирректории
                if (Directory.Exists(dataPath))
                {
                    DirectoryInfo dir = new DirectoryInfo(@dataPath);
                    progressBar1.Minimum = 1;
                    progressBar1.Value = 1;
                    progressBar1.Step = 10;
                    progressBar1.Maximum = dir.GetFiles().Length;
                    foreach (var item in dir.GetFiles())
                    {
                        progressBar1.PerformStep();
                        Schedule shed = new Schedule();
                        wetSchedule.ForEach(sciv =>
                        {
                            if (sciv.id == Convert.ToInt32(item.Name.Substring(0, item.Name.Length - 4).Substring(5)))
                            {
                                shed = sciv;
                                return;
                            }
                        });
                        if (shed.name == null)
                        {
                            shed.id = Convert.ToInt32(item.Name.Substring(0, item.Name.Length - 4).Substring(5));
                            shed.name = "noname!";
                            Form1._form._newRoot.ivits.ForEach(iv =>
                            {
                                if (iv.id == shed.id)
                                {
                                    shed.name = iv.name;
                                }
                            });
                            wetSchedule.Add(shed);
                        }
                        using (FileStream fs = new FileStream(dataPath + "\\" + item.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            foreach (int id in userJconfig.formScheduleWets.checkedIvits)
                            {
                                if (id == Convert.ToInt32(item.Name.Substring(0, item.Name.Length - 4).Substring(5)))
                                {
                                    using (BinaryReader reader = new BinaryReader(fs))
                                    {
                                        // пока не достигнут конец файла
                                        // считываем каждое значение из файла

                                        while (reader.PeekChar() > -1)
                                        {

                                            Indication ind = new Indication();
                                            DateTime dt = DateTime.Parse(reader.ReadString());
                                            ind.datetime = dt;
                                            ind.value = Convert.ToSingle(reader.ReadString());
                                            shed.indications.Add(ind);

                                        }
                                    }
                                }
                            }
                            fs.Close();
                        }
                    }
                }
                startDate = startDate.AddDays(1);
            }
            progressBar1.Visible = false;
        }
        private void SetChart2()
        {
            //формируем таблицу
            // DataTable workTable = new DataTable("TableLists");
            //  workTable.Columns.Add("Time", typeof(String));
            //   workTable.Columns.Add("Value", typeof(String));
            // Создание заголовков диаграмм. 
            // создаем заголовки и заголовки нижнего колонтитула
            //DataRow workRow;
            DateTime actualDT = DateTime.MinValue;
            chart2.Series.Clear();
            foreach (Legend legend in chart2.Legends)
            {
                legend.Enabled = false;
                legend.MaximumAutoSize = 35;
            }
            chart2.ChartAreas[0].AxisY.Title = "%";
            chart2.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chart2.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chart2.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM/yy - HH:mm:ss";
            foreach (Schedule ivit in wetSchedule)
            {
                Int32 PK = 0;
                Form1._form._newRoot.ivits.ForEach(p =>
                {
                    if (p.id == ivit.id)
                    {
                        PK = p.wetPK;
                    }
                });
                foreach (int id in userJconfig.formScheduleWets.checkedIvits)
                {
                    if (ivit.id == id)
                    {
                        Random rnd = new Random(ivit.id);
                        Series series = new Series();
                        series.Name = ivit.id + " - " + ivit.name;
                        series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
                        series.Color = Color.FromArgb(rnd.Next(5, 200), rnd.Next(5, 200), rnd.Next(5, 200));
                        series.BorderWidth = 3;
                        series.XValueType = ChartValueType.DateTime;
                        chart2.Series.Add(series);

                        foreach (Indication indication in ivit.indications)
                        {
                            if (actualDT < indication.datetime && indication.value != 0)
                            {
                                series.Points.AddXY(indication.datetime, indication.value+PK);

                                //  workRow = workTable.NewRow();
                                //     workRow[0] = indication.datetime;
                                //      workRow[1] = indication.value;
                                //      workTable.Rows.Add(workRow);
                            }

                            actualDT = indication.datetime;
                        }

                    }
                }
            };

            //  this.dataGridView1.DataSource = workTable;
            //  this.dataGridView1.Columns["Time"].Width = 140;
            //   this.dataGridView1.Columns["Value"].Width = 150;

        }
        private void SetListViev1()
        {

            listView1.CheckBoxes = true;
            listView1.View = View.Details;
            listView1.Columns.Add("Name");
            foreach (Schedule ivit in wetSchedule)
            {
                Random rnd = new Random(ivit.id);
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.ForeColor = Color.FromArgb(240, 240, 240);
                listViewItem.BackColor = Color.FromArgb(rnd.Next(5, 200), rnd.Next(5, 200), rnd.Next(5, 200)); ;
                listViewItem.Name = ivit.id.ToString();
                listViewItem.Text = ivit.id + " - " + ivit.name;
                listViewItem.Checked = false;
                foreach (int id in userJconfig.formScheduleWets.checkedIvits)
                {
                    if (id.ToString() == listViewItem.Name)
                    {
                        listViewItem.Checked = true;
                        break;
                    }
                }
                listView1.Items.Add(listViewItem);
            }
            foreach (ColumnHeader column in listView1.Columns)
            {
                column.Width = -2;
            }
        }



        private void treeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            if (e.Node.FirstNode == null)
            {
                this.activeDate = e.Node.Text;

                wetSchedule.Clear();
                listView1.Clear();
                chart2.Series.Clear();
                if (checkBox1.Checked == true)
                {
                    this.SetWetsSchedulePeriod();
                }
                else
                {
                    this.SetWetsSchedule();
                }
                this.SetListViev1();
                this.SetChart2();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            userJconfig.formScheduleWets.checkedIvits.Clear();
            foreach (ListViewItem item in listView1.CheckedItems)
            {
                userJconfig.formScheduleWets.checkedIvits.Add(Convert.ToInt32(item.Name));
            }
            userJconfig.Save();


            wetSchedule.Clear();
            chart2.Series.Clear();
            if (checkBox1.Checked == true)
            {
                this.SetWetsSchedulePeriod();
            }
            else
            {
                this.SetWetsSchedule();
            }
            this.SetChart2();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (Legend legend in chart2.Legends)
            {
                legend.Enabled = true;
            }

            System.Windows.Forms.DataVisualization.Charting.PrintingManager print = chart2.Printing;
            print.PageSetup();
            print.PrintPreview();
            foreach (Legend legend in chart2.Legends)
            {
                legend.Enabled = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked == true)
            {
                this.dateTimePicker1.Enabled = true;
                this.dateTimePicker2.Enabled = true;
                this.treeView1.Enabled = false;
            }
            else
            {
                this.dateTimePicker1.Enabled = false;
                this.dateTimePicker2.Enabled = false;
                this.treeView1.Enabled = true;
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}