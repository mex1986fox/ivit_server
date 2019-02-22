using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IvitRooms
{
    public partial class FormLists : Form
    {
        public FormLists()
        {
            InitializeComponent();
        }

        private void FormLists_Load(object sender, EventArgs e)
        {
            DataTable workTable = new DataTable("TableLists");
            workTable.Columns.Add("ID", typeof(Int32));
            workTable.Columns.Add("NameRoom", typeof(String));
            workTable.Columns.Add("Name", typeof(String));
            workTable.Columns.Add("Port", typeof(String));
            workTable.Columns.Add("IP", typeof(String));
            workTable.Columns.Add("SerialID", typeof(String));
            workTable.Columns.Add("tempPK", typeof(Int32));
            workTable.Columns.Add("wetPK", typeof(Int32));
            DataRow workRow;
            foreach (Form1.Ivit ivit in Form1._form._newRoot.ivits)
            {
                workRow = workTable.NewRow();
                Form1.Room room = Form1._form._newRoot.rooms.Where(ro => ro.id == ivit.roomID).ToList()[0];
                workRow[0] = ivit.id;
                workRow[1] = room.name;
                workRow[2] = ivit.name;
                workRow[3] = ivit.port;
                workRow[4] = ivit.ip;
                workRow[5] = ivit.sirialID;
                workRow[6] = ivit.tempPK;
                workRow[7] = ivit.wetPK;
                workTable.Rows.Add(workRow);
            }

            this.dataGridView1.DataSource = workTable;
            this.dataGridView1.Columns["ID"].Width = 40;
            this.dataGridView1.Columns["ID"].ReadOnly = true;
            this.dataGridView1.Columns["NameRoom"].Width = 150;
            this.dataGridView1.Columns["NameRoom"].ReadOnly = true;
            this.dataGridView1.Columns["Name"].Width = 350;
            this.dataGridView1.Columns["Name"].ReadOnly = false;
            this.dataGridView1.Columns["Port"].Width = 60;
            this.dataGridView1.Columns["Port"].ReadOnly = false;
            this.dataGridView1.Columns["IP"].Width = 150;
            this.dataGridView1.Columns["IP"].ReadOnly = false;
            this.dataGridView1.Columns["SerialID"].Width = 60;
            this.dataGridView1.Columns["SerialID"].ReadOnly = false;
            this.dataGridView1.Columns["tempPK"].Width = 60;
            this.dataGridView1.Columns["tempPK"].ReadOnly = false;
            this.dataGridView1.Columns["wetPK"].Width = 60;
            this.dataGridView1.Columns["wetPK"].ReadOnly = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in this.dataGridView1.Rows)
            {
                if (row.Cells["ID"].Value != null)
                {

                    Form1.Ivit ivit = Form1._form._newRoot.ivits.Where(iv => iv.id == Convert.ToInt32(row.Cells["ID"].Value.ToString())).ToList()[0];
                    ivit.name = row.Cells["Name"].Value.ToString();
                    ivit.port = Convert.ToInt32(row.Cells["Port"].Value.ToString());
                    ivit.ip = row.Cells["IP"].Value.ToString();
                    ivit.sirialID = Convert.ToInt32(row.Cells["SerialID"].Value.ToString());
                    ivit.tempPK = Convert.ToInt32(row.Cells["tempPK"].Value.ToString());
                    ivit.wetPK = Convert.ToInt32(row.Cells["wetPK"].Value.ToString());
                }
               

            }
            Form1._form.saveJConfig();
        }
    }
}
