using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;
using System.Threading;

namespace dipl.controller
{
    class Query
    {
        OleDbConnection connection;
        OleDbCommand command;
        OleDbDataAdapter dataAdapter;
        DataTable bufferTable;


        public Query(string Conn)
        {
            connection = new OleDbConnection(Conn);
            bufferTable = new DataTable();
        }//переменные для коннекта к бд
        public DataTable UpdateP(string FROM, DataTable buf)
        {
            connection.Open();
            dataAdapter = new OleDbDataAdapter($"{FROM}", connection);
            buf.Clear();
            dataAdapter.Fill(buf);
            connection.Close();
            return buf;
        }//Запрос к бд возвращает dataTable(копиия бд) но с обнулением DataTable Fixed
        public DataTable UpdateP(string FROM)
        {
            connection.Open();
            dataAdapter = new OleDbDataAdapter($"{FROM}", connection);
            bufferTable.Clear();
            dataAdapter.Fill(bufferTable);
            connection.Close();
            return bufferTable;
        }// Запрос к бд возвращает dataTable(копиия бд) 
        public string[] Lfile()
        {
            string[] dish = new string[100];
            //Loaded log
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.ShowDialog();
            string filename = openFileDialog1.FileName;
            //Write file in array
            dish = System.IO.File.ReadAllLines(filename);
            MessageBox.Show("Файл открыт");
            return dish;
           
        }//Загрузка logo
        public List<string> DishName(string[] mass)
        {
            // разделяем массив на 2 по (-) получаем список блюд
            List<string> stone = new List<string>();
            List<string> kol = new List<string>();
            List<string> NameDish = new List<string>();
            string[] dish = new string[100];
            string[] st1 = new string[200];
            dish = mass;
            string st3;
            for (int i = 0; i < dish.Length; i++)
            {
                st3 = dish[i];
                st1 = st3.Split('-');
                stone.Add(st1[0]);
                stone.Add(st1[1]);

            }
            for (int i = 0; i < dish.Length * 2; i++)
            {
                if (i % 2 == 0)
                {

                    NameDish.Add(stone[i]);

                }
                else
                {

                    kol.Add(stone[i]);

                }
            }

            return NameDish;

        }//разделяем массив на 2 по (-) получаем список блюд
        public List<int> Kol(string[] mass, List<string> Dname)
        {   // разделяем массив на 2 по (-) получаем колличество блюд
            List<string> stone = new List<string>();
            List<int> kol = new List<int>();
            List<string> NameDish = new List<string>();
            string[] dish = new string[100];
            string[] st1 = new string[200];
            dish = mass;
            
            bool[] s = new bool [100];
            // разделяем массив на 2 по -
            string st3;
            for (int i = 0; i < dish.Length; i++)
            {
                st3 = dish[i];
                st1 = st3.Split('-');
                stone.Add(st1[0]);
                stone.Add(st1[1]);

            }


            for (int i = 0; i < dish.Length * 2; i++)
            {
                if (i % 2 == 0)
                {

                    NameDish.Add(stone[i]);

                }
                else
                {
                    kol.Add((Convert.ToInt32(stone[i])));
                }

            }
            for (int i = 0; i < dish.Length; i++)
            {
                for (int k = 0; k < dish.Length; k++)
                {
                     if (NameDish[i]==Dname[k])
                        {
                            s[i] = true;
                        }
                }

            }

            List<int> kol1 = new List<int>();
            for (int i = 0; i < kol.Count; i++)
            {
                if (s[i])
                {
                    kol1.Add(kol[i]);
                }
            }

            return kol1;
        }// разделяем массив на 2 по (-) получаем колличество блюд Fixed
        public List<string> BackMassstring(string FROM)
        {   // получаем из бд массив по выброному столбцу
            List<string> prod = new List<string>();
            DataTable Fdata = new DataTable();
            Fdata.Clear();
            prod.Clear();
            Fdata = UpdateP(FROM, Fdata);
            //load Array from BD
            int i = 0;
            foreach (DataRow item in Fdata.Rows)
            {
                var cells = item.ItemArray;
                foreach (var cell in cells)
                {
                    prod.Add(Convert.ToString(cell));
                    i++;
                }
            }
            
            return prod;
            
        }// получаем из бд массив по выброному столбцу string
        public List<int> BackMassint(string FROM)
        {   // получаем из бд массив по выброному столбцу
            List<int> prod = new List<int>();
            DataTable Fdata = new DataTable();
            Fdata.Clear();
            prod.Clear();
            Fdata = UpdateP(FROM, Fdata);
            //load Array from BD
            int i = 0;


            foreach (DataRow item in Fdata.Rows)
            {
                var cells = item.ItemArray;
                foreach (var cell in cells)
                {
                    prod.Add(Convert.ToInt32(cell));
                    i++;
                }
            }
            Fdata.Clear();
            return prod;
        }// получаем из бд массив по выброному столбцу int
        public void BackMass(string From)
        {
            List<String> vs = new List<string>();
            DataTable Fdata = new DataTable();
            
            Fdata = UpdateP(From);

            DataTableReader dataTableReader = Fdata.CreateDataReader();
            while (dataTableReader.Read())
            {
                for (int i = 0; i < dataTableReader.FieldCount; i++)
                {
                    vs.Add(dataTableReader.GetValue(i).ToString().Trim());
                }


            }
            Fdata.Clear();

        }// альторнатива двум другим методам выше (вдруг понадобится)
        public List<string> FindIDCheck(List<string>products, List<string>Dname)
        {
            List<string> ID = new List<string>();

            for (int i = 0; i < Dname.Count; i++)
            {
                if (1 <= 1 + products.IndexOf(Dname[i]))
                {
                    ID.Add($"ID{Convert.ToString(1 + products.IndexOf(Dname[i]))} name {Dname[i]} ");
                }

            }
            return ID;
        }//Поиск блюда в бд и возвращение ID проверка
        public List<int> FindID(List<string> products, List<string> Dname)
        {
            List<int> ID = new List<int>();

            for (int i = 0; i < Dname.Count; i++)
            {
                if (1 <= 1 + products.IndexOf(Dname[i]))
                {
                    ID.Add(1 + products.IndexOf(Dname[i]));
                }

            }
            return ID;
        }//Поиск блюда в бд и возвращение ID
        public int[,] RecipeTable (string From, int stolb)
        {
            // string From1 = "SELECT DName, Products, Unit, Weight FROM Recipe";
            List<int> Recipe_Dname = BackMassint(From);
            int[,] vs = new int[Recipe_Dname.Count/stolb, stolb];
            int j = 0;
            for (int i = 0; i < Recipe_Dname.Count / stolb; i++)
            {
                for (int k = 0; k <= stolb-1; k++)
                {

                    vs[i, k] = Recipe_Dname[j];
                    j++;
                }
            }
            return vs;
                ;
        }// Возвращает Многомерный массив из Recipe Fixed
        public bool[] Find(int[,] Recipe, int ID, int stolb)
        {
            bool[] s = new bool[Recipe.Length/stolb];
            for (int i = 0; i < s.Length; i++)
            {
                if (Recipe[i, 0] == ID)
                {
                    s[i] = true;
                }
            }
            return s;
        }//Создаем таблицу для поика
        public int[,] Writ(List<int> ID, int[,] Recipe, List<int> kol,int stolb)
        {
            var Recipe1 = Recipe;
            for (int k = 0; k < ID.Count; k++)
            {
                
                bool[] s = Find(Recipe, ID[k], stolb);
                List<int> buf = new List<int>();
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i])
                    {
                        buf.Add(Recipe[i, 3]);
                    }
                }
                for (int i = 0; i < buf.Count; i++)
                {
                    buf[i] = buf[i] * kol[k];

                }
                int j = 0;
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i])
                    {

                        Recipe1[i, 3] = buf[j];
                        j++;

                    }
                }
            }
            return Recipe1;
        }// модификация Recipe по логу
        public void Update(int weight, int ID)
        {
            connection.Open();
            command = new OleDbCommand($"UPDATE Store SET weight = {weight} WHERE Код LIKE '{ID+1}' ", connection);
            command.Parameters.AddWithValue("weight", weight);
            command.ExecuteNonQuery();
            connection.Close();
        }//Обнавление ячеек
        public void AddWeight (string s, string s1)
        {
            connection.Open();
            command = new OleDbCommand($"UPDATE Store SET weight = weight+{s1} WHERE Products LIKE '{s}' ", connection);
            command.Parameters.AddWithValue($"weight+{s1}", s1);
            command.ExecuteNonQuery();
            connection.Close();

        }
        public void NewTable(string dt)
        {
            connection.Open();
            command = new OleDbCommand($"CREATE TABLE [Store_{dt}](Код AUTOINCREMENT Primary Key,products String,unit Int,weight Int)", connection);
            command.ExecuteNonQuery();
            connection.Close();
        }//Копировнаие таблицы
        public void Copy(string dt)
        {
            connection.Open();
            command = new OleDbCommand($"INSERT INTO Store_{dt} SELECT * FROM Store", connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void Insert(string prod, string weight)
        {
            int gramm = 1;//тупо лень 
            connection.Open();
            command = new OleDbCommand($"INSERT INTO Store (products, weight, unit) VALUES ({prod},{weight},{gramm})", connection);
            command.Parameters.AddWithValue("products", prod);
            command.Parameters.AddWithValue("weight", weight);
            command.Parameters.AddWithValue("unit", gramm);
            command.ExecuteNonQuery();
            connection.Close();
        }
        public DataTable FindTable(string dt, DataTable buf)
        {
            connection.Open();
            dataAdapter = new OleDbDataAdapter($"SELECT products, weight FROM Store_{dt}", connection);
            buf.Clear();
            dataAdapter.Fill(buf);
            connection.Close();
            return buf;
        }
    }   


    

    
     
}
