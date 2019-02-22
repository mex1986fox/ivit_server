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
    public partial class FormListsPas : Form
    {
        public FormListsPas()
        {
            InitializeComponent();
        }

        private void FormListsPas_Load(object sender, EventArgs e)
        {

            DataTable workTable = new DataTable("TableLists");
            workTable.Columns.Add("ID", typeof(Int32));
            workTable.Columns.Add("NameRoom", typeof(String));
            workTable.Columns.Add("Name", typeof(String));
            workTable.Columns.Add("Port", typeof(String));
            workTable.Columns.Add("IP", typeof(String));
            workTable.Columns.Add("SerialID", typeof(String));
            workTable.Columns.Add("PressurePK", typeof(Int32));

            DataRow workRow;
            foreach (Form1.Pas pas in Form1._form._newRoot.pases)
            {
                workRow = workTable.NewRow();
                Form1.Room room = Form1._form._newRoot.rooms.Where(ro => ro.id == pas.roomID).ToList()[0];
                workRow[0] = pas.id;
                workRow[1] = room.name;
                workRow[2] = pas.name;
                workRow[3] = pas.port;
                workRow[4] = pas.ip;
                workRow[5] = pas.sirialID;
                workRow[6] = pas.pressurePK;
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
            this.dataGridView1.Columns["PressurePK"].Width = 60;
            this.dataGridView1.Columns["PressurePK"].ReadOnly = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                if (row.Cells["ID"].Value != null)
                {

                    Form1.Pas pas = Form1._form._newRoot.pases.Where(iv => iv.id == Convert.ToInt32(row.Cells["ID"].Value.ToString())).ToList()[0];
                    pas.name = row.Cells["Name"].Value.ToString();
                    pas.port = Convert.ToInt32(row.Cells["Port"].Value.ToString());
                    pas.ip = row.Cells["IP"].Value.ToString();
                    pas.sirialID = Convert.ToInt32(row.Cells["SerialID"].Value.ToString());
                    pas.pressurePK = Convert.ToInt32(row.Cells["PressurePK"].Value.ToString());
                }


            }
            Form1._form.saveJConfig();
        }
    }
}
