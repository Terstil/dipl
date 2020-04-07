using dipl.controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Threading;

namespace dipl
{
    public partial class Form1 : Form
    {
        Query controller;
        string[] log;
        
        public Form1()
        {
            InitializeComponent();
            controller = new Query(ConnectionString.ConnStr);

        }


        private void Form1_Load(object sender, EventArgs e)
        {
           
        }//возможно нужно заменить на кнопку обнавить

        private void button1_Click_1(object sender, EventArgs e)
        {
            progressBar1.Maximum = 4;
            progressBar1.Minimum = 0;
            progressBar1.Value = 0;
            listBox2.Items.Clear();
            if (log!=null)
            {
                //создание новой таблицы, копирование из Store in Store_+(data)
                DateTime dt = DateTime.Now;
                string curDate = dt.ToShortDateString();
                curDate = curDate.Replace(".", "_");
                controller.NewTable(curDate);
                progressBar1.Value = 1;
                controller.Copy(curDate);
                //Берем название продукты из бд
                List<string> products = controller.BackMassstring("SELECT DName FROM dishName");
                // Загружаем лог и делем на массивы kol and Dname
                List<string> Dname = controller.DishName(log);
                // реалезую поиск id продукта 
                List<string> IDCheck = controller.FindIDCheck(products, Dname);
                List<int> kol = controller.Kol(log, products);
                List<int> ID = controller.FindID(products, Dname);
                // Забрали все блюда из рецепта
                int[,] Recipe = controller.RecipeTable("SELECT DName, Products, Unit, Weight FROM Recipe", 4);
                //Ищу блюдо по ID 
                // модификация Recipe Привращаю его в табличку 4х4
                Recipe = controller.Writ(ID, Recipe, kol, 4);
                //Copy column from Store weight in Array
                progressBar1.Value = 2;
                List<int> Store_weight = controller.BackMassint("SELECT  weight FROM Store");
                // Просчитываю кол-во потраченного weigh(Store) = weigh(Store) - weight(Recipe)*kol(log)
                for (int i = 0; i < Recipe.Length / 4; i++)
                {
                    Store_weight[Recipe[i, 1] - 1] = Store_weight[Recipe[i, 1] - 1] - Recipe[i, 3];
                }

                for (int i = 0; i < Store_weight.Count; i++)
                {
                    controller.Update(Store_weight[i], i);
                }
                progressBar1.Value = 3;
                // Остановися на создание даты решил что буду копировать Таблюцу Store
            }
            else
            {
                MessageBox.Show("Загрузите log");
            }
            DateTime dt2 = DateTime.Now;
            string dt1 = dt2.ToShortDateString();
            dt1 = dt1.Replace('.', '_');
            List<int> data = controller.BackMassint("SELECT weight FROM Store");
            List<string> name = controller.BackMassstring("SELECT products FROM Store");
            List<int> fdata = controller.BackMassint($"SELECT weight FROM Store_{dt1}");
            List<int> res = new List<int>();
            for (int i = 0; i < data.Count; i++)
            {
                res.Add(fdata[i] - data[i]);
            }
            for (int i = 0; i < data.Count; i++)
            {
                listBox2.Items.Add(name[i] + " " + Convert.ToString(res[i]));
            }
            dataGridView1.DataSource = controller.UpdateP("SELECT products, weight FROM Store");
            progressBar1.Value = 4;
        }//основыне расчеты 

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            //загружаем log в массив
            log = controller.Lfile();
            foreach (var item in log)
            {
                listBox1.Items.Add(item);
            }
        }//Загрузка лог

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 newForm2 = new Form2();
            newForm2.Show();
        }//открытие формы

        private void Form1_Load_1(object sender, EventArgs e)
        {
            DataTable buf = new DataTable();
            dataGridView1.DataSource = controller.UpdateP("Select products, weight FROM Store", buf);
        }//Загружает бд в грид


        private void button4_Click(object sender, EventArgs e)
        {
            string curDate = textBox1.Text;
            curDate = curDate.Replace(".", "_");
            string[] s1 = curDate.Split(' ');
            DataTable buf = new DataTable();
            dataGridView2.DataSource = controller.FindTable(curDate, buf);
            //if (curdate != "")
            //{
            //    string[] s1 = curdate.split(' ');
            //    datetime dt1 = datetime.parse(s1[0]);
            //    datetime dt2 = datetime.parse(s1[1]);

            //    while (dt1 != dt2)
            //    {
            //        dt1 = dt1.adddays(1);

            //    }
            //}




        }// что то хз что делать..ИСТОРИЮ

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable buf = new DataTable();
            dataGridView1.DataSource = controller.UpdateP("Select products, weight FROM Store", buf);
            
        }//Обновление грида


    }
}
