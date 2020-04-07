using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dipl.controller;

namespace dipl
{
    public partial class Form2 : Form
    {
        Query controller;

        public Form2()
        {
            InitializeComponent();
            controller = new Query(ConnectionString.ConnStr);
            timer1.Interval = 3000;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Start();
        }

        private void fileSystemWatcher1_Changed(object sender, System.IO.FileSystemEventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            DataTable buf = new DataTable();
            dataGridView1.DataSource = controller.UpdateP("Select products, weight FROM Store", buf);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> prod = controller.BackMassstring("SELECT products FROM Store");
            string s = textBox1.Text;
            int index = s.LastIndexOf(" ");
            string[] s1 = new string[10];
            if (index!=-1)
            {
                s1[0] = s.Substring(0, index);
                s1[1] = s.Substring(index + 1);
            }

            if (radioButton2.Checked)
            {
                int si = 0;
                for (int i = 0; i <= prod.Count; i++)
                {
                    si++;
                        if (s1[0] == prod[i])
                        {
                            controller.AddWeight(s1[0], s1[1]);
                            break;
                        }
                        if (si == (prod.Count/2)+2)//можно было с I сравнить ?
                        {
                            if (s == "")
                            {
                                MessageBox.Show("Введите название продукта");
                                break;
                            }
                            MessageBox.Show($"Продукт ({s1[0]}) не найден. Проверьте написание или добавьте его в базу.");
                            break;
                        }

                    
                }
            }//пока не работает с масивом
            if (radioButton1.Checked)
            {
                int si = 0;
                for (int i = 0; i < prod.Count; i++)
                {
                    si++;
                    if (s1[0] == prod[i])
                    {
                        MessageBox.Show("Такой продукт существует.\nВоспользуйтесь функцией \"Редактировать существующую позицию\" ");
                        break;
                    }
                    if (si == prod.Count)
                    {
                        controller.Insert(s1[0], s1[1]);
                    }
                }

            }//пока не работает с масивом
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Form2_Load(null, null);
        }
    }
}
