using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace IvitRooms
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void Form3_Load(object sender, EventArgs e)
        {
            //сформировать список
            int listI = 0;
            foreach (Form1.Room room in Form1._form._newRoot.rooms)
            {
                listBox1.Items.Insert(listI, room.name);
                listI++;
            };
            maskedTextBox1.Text = "010.020.002.";
            maskedTextBox2.Text = "502";
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Проверить заполнение эементов формы
            if (maskedTextBox1.Text == "" 
                || maskedTextBox2.Text == "" 
                || maskedTextBox3.Text == ""
                || textBox5.Text ==""
                || listBox1.SelectedIndex==-1)
            {
                MessageBox.Show(
                    "Заполните все элементы формы",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
            }
            else
            {
                int id = 1;
                // выкрутить id для формы
                foreach (Form1.Ivit ivit in Form1._form._newRoot.ivits)
                {
                    if (id <= ivit.id)
                    {
                        id = ivit.id + 1;
                    }
                }
                int roomId = 1;
                foreach(Form1.Room room in Form1._form._newRoot.rooms)
                {
                    if (listBox1.Items[listBox1.SelectedIndex].ToString() == room.name)
                    {
                        roomId = room.id;
                    }
                }
                // заполнить объект и создать
                Form1.Ivit newIvit = new Form1.Ivit();
                newIvit.id = id;
                newIvit.status = "";
                newIvit.roomID = roomId;
                newIvit.ip = Regex.Replace(maskedTextBox1.Text.Replace(",", "."), "0*([0-9]+)", "${1}");
                newIvit.port = Convert.ToInt32( maskedTextBox2.Text);
                newIvit.sirialID = Convert.ToInt32(maskedTextBox3.Text);
                newIvit.name = textBox5.Text;
                newIvit.temp = "0";
                newIvit.wet = "0";
                Form1._form._newRoot.ivits.Add(newIvit);
                Form1._form.saveJConfig();
                this.Hide();
            }
        }
    }
}
