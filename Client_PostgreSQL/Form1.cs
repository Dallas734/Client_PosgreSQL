using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace Client_PostgreSQL
{
    public partial class Form1 : Form
    {
        private String connectionString;
        private NpgsqlConnection conn;
        private String sql;
        private NpgsqlCommand cmd;
        private DataTable dataTable;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Обеспечиваем доступ к БД
            connectionString = "Server=localhost;Port=5432;Username=postgres;Password=111;Database=Polyclinic";
            conn = new NpgsqlConnection(connectionString);
        }

        private void SelectSql(String id)
        {
            try
            {
                // Подключаемся к БД
                conn.Open();
                if (id == "")
                {
                    sql = "select * from \"DOCTOR\"";
                }
                else
                {
                    // Создаем запрос
                    sql = String.Format("SELECT \"VISIT_DATE\", \"DOCTOR_FIO\", \"PATIENT_FIO\" " +
                    "FROM \"VISIT\"INNER JOIN \"DOCTOR\" " +
                    "ON \"VISIT\".\"DOCTOR_ID\" = \"DOCTOR\".\"DOCTOR_ID\" " +
                    "INNER JOIN \"PATIENT\"" +
                    "ON \"VISIT\".\"PATIENT_ID\" = \"PATIENT\".\"PATIENT_ID\" " +
                    " WHERE \"DOCTOR\".\"DOCTOR_ID\" = {0}", id);
                }
                // На основе запроса создаем команду
                cmd = new NpgsqlCommand(sql, conn);
                dataTable = new DataTable();
                // Выполняем команду и результат добавляем в таблицу
                dataTable.Load(cmd.ExecuteReader()); 
                // Отключаемся от БД
                conn.Close();
                // Выводим результат выполнения команды в dataGridView
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dataTable;  
            }
            catch (Exception exception)
            {
                conn.Close();
                MessageBox.Show("Error: " + exception.Message);
            }
        }

        private void DeleteSql(String id)
        {
            try
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Вы не ввели ID!");
                }
                else
                {
                    conn.Open();
                    sql = String.Format("DELETE FROM \"DOCTOR\"" +
                    "WHERE \"DOCTOR_ID\" = {0}", textBox1.Text);
                    cmd = new NpgsqlCommand(sql, conn);
                    // Выполняем команду
                    cmd.ExecuteScalar();
                    conn.Close();
                    MessageBox.Show("Удаление прошло успешно!");
                    SelectSql("");
                }

            }
            catch (Exception exception)
            {
                conn.Close();
                MessageBox.Show("Error: " + exception.Message);
            }
        }

        private void InsertSql(String FIO, String spec, String cat)
        {
            try
            {
                conn.Open();
                sql = "INSERT INTO \"DOCTOR\" (\"DOCTOR_FIO\", \"DOCTOR_SPECIALIZATION\", \"DOCTOR_CATEGORY\")" +
                    "VALUES (:_FIO, :_SPEC, :_CAT)";
                cmd = new NpgsqlCommand(sql, conn);
                // на место ключевых слов добавляем параметры из textBox'ов
                cmd.Parameters.AddWithValue("_FIO", FIO);
                cmd.Parameters.AddWithValue("_SPEC", spec);
                cmd.Parameters.AddWithValue("_CAT", cat);
                cmd.ExecuteScalar();
                conn.Close();
                MessageBox.Show("Добавление прошло успешно!");
                SelectSql("");

            }
            catch (Exception exception)
            {
                conn.Close();
                MessageBox.Show("Error: " + exception.Message);
            }
        }

        // Обработчики событий кнопок
        private void button1_Click(object sender, EventArgs e)
        {
            SelectSql(textBox1.Text);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DeleteSql(textBox1.Text);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            InsertSql(textBox5.Text, textBox2.Text, textBox4.Text);
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

    }
}
