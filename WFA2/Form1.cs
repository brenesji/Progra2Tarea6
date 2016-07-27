using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFA2
{
    public partial class Form1 : Form
    {
        //definicion del objeto
        Persona person1 = new Persona();
        Persona person2 = new Persona(2, "Cesar", "Duran");
        PersonaAmiga personA = new PersonaAmiga();
        PersonaHeredada personH = new PersonaHeredada();
        int i = 0;

        public Form1()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Down)
            {
                button1.Location = new Point(i + 20, i + 20);                
                i += 20;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            //person1.Id = 1;
            //person1.Name = "Ana";
            //person1.LastName = "Bolaños";
            //person1.SetSize(size);


            //conexion DB ... Mostrar objetos

            string sqlquery;
            string conexion = "Data Source=(local); " +
                              "Initial Catalog=bd_tarea;" +
                              "Integrated Security=True;";
            //'User ID=UserName;Password=Password;
            DataTable dt = new DataTable();
            sqlquery = "select Cedula as 'Cédula', Nombre, Apellido, Direccion from Personas";
            SqlConnection sqlconn = new SqlConnection(conexion);
            sqlconn.Open();
            SqlDataAdapter sqlda = new SqlDataAdapter(sqlquery, sqlconn);
            sqlda.Fill(dt);
            sqlconn.Close();
            dataGridView1.DataSource = dt;

            person1.Initdata();


        }

        private void button1_Click(object sender, EventArgs e)
        {

            //personH.NewCompleteName()
            label4.Text = personH.CompleteName(); 
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool resul;
            string message = "";
            var result = MessageBox.Show("Desea guardar la información", "Aviso",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (textBox1.Text != "")
                {
                    person1.Id = Convert.ToInt16(textBox1.Text);
                }                
                person1.Name = textBox2.Text;
                person1.LastName = textBox3.Text;

                //aqui meter bases de datos

                resul = person1.NewData(person1);
                if (resul == true)
                {

                    string conexion = "Data Source=(local);" + // en vez de Data Source=LAB210-01 uso (local)
                  "Initial Catalog=bd_tarea;" +
                  "Integrated Security=True;";
                    string sqlquery;
                    //'User ID=UserName;Password=Password;
                    SqlConnection sqlconn = new SqlConnection(conexion);
                    sqlconn.Open();
                    SqlCommand sqlcomm = new SqlCommand();

                    DataTable dt = new DataTable();

                    sqlquery = "insert into Personas (" +
                        "Cedula," +
                        "Nombre," +
                        "Apellido," +
                        "Direccion" +
                        ") values (" +
                            "'" + textBox1.Text + "'," +
                            "'" + textBox2.Text + "'," +
                            "'" + textBox3.Text + "'," +
                            "'" + textBox5.Text + "')";
                    sqlcomm.Connection = sqlconn;
                    sqlcomm.CommandText = sqlquery;
                    sqlcomm.CommandType = CommandType.Text;
                    sqlcomm.ExecuteNonQuery();
                    sqlconn.Close();
                    sqlquery = "select Cedula, Nombre,  Apellido, Direccion from Personas";
                    sqlconn.Open();
                    SqlDataAdapter sqlda = new SqlDataAdapter(sqlquery, sqlconn);
                    sqlda.Fill(dt);
                    sqlconn.Close();
                    dataGridView1.DataSource = dt;

                    //fin de la coneccion
                    message = "Información guardada";
                }
                else
                {
                    message = "No se guardó la información, los registros están llenos";
                }
                result = MessageBox.Show(message, "Aviso",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Question);
            }

            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox5.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
            {
                person1.SetSize(Convert.ToInt16(textBox4.Text));
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox5.Enabled = true;
                textBox4.Enabled = false;
            }
            
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.Show();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            bool resul = false;
            string message = "";
            var result = MessageBox.Show("Desea modificar la información", "Aviso",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (textBox1.Text != "")
                {
                    person1.Name = textBox2.Text;
                    person1.LastName = textBox3.Text;
                    resul = person1.ModifyData(person1, Convert.ToInt16(textBox1.Text));
                } 
                               
                if (resul == true)
                {
                    message = "Información modificada";
                }
                else
                {
                    message = "No se modificó la información, por favor digite la cédula correctamente";
                }
                result = MessageBox.Show(message, "Aviso",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Question);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            bool resul = false;
            string message = "";
            var result = MessageBox.Show("Desea eliminar la información", "Aviso",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (textBox1.Text != "")
                {
                    resul = person1.DeleteData(Convert.ToInt16(textBox1.Text));
                }
                
                if (resul == true)
                {
                    message = "Información eliminada";
                    textBox2.Text = "";
                    textBox3.Text = "";
                }
                else
                {
                    message = "No se eliminó la información, por favor digite la cédula correctamente";
                }
                result = MessageBox.Show(message, "Aviso",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Question);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            bool resul;
            string message = "";
            Persona person1 = new Persona();
            resul = person1.SeekData(Convert.ToInt16(textBox1.Text));
            if (resul == true)
            {
                textBox1.Text = Convert.ToString(person1.Id);
                textBox2.Text = person1.Name;
                textBox3.Text = person1.LastName;
            }
            else
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
            }
            if (resul == true)
            {
                message = "Información encontrada";
            }
            else
            {
                message = "No se encontró información, por favor digite la cédula a buscar";
            }
            var result = MessageBox.Show(message, "Aviso",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Question);
        }

        private void BtnEliminarBD_Click(object sender, EventArgs e)
        {
            string conexion = "Data Source=(local);" +
                  "Initial Catalog=bd_tarea;" +
                  "Integrated Security=True;";
            string sqlquery;
            //'User ID=UserName;Password=Password;
            SqlConnection sqlconn = new SqlConnection(conexion);
            sqlconn.Open();
            SqlCommand sqlcomm = new SqlCommand();
            DataTable dt = new DataTable();
            sqlquery =
            "DELETE FROM PERSONAS WHERE " +
            "Cedula = " + textBox1.Text + ";";

            sqlcomm.Connection = sqlconn;
            sqlcomm.CommandText = sqlquery;
            sqlcomm.CommandType = CommandType.Text;
            sqlcomm.ExecuteNonQuery();
            sqlconn.Close();
            sqlquery = "select Cedula, Nombre, Apellido, Direccion from Personas";
            sqlconn.Open();
            SqlDataAdapter sqlda = new SqlDataAdapter(sqlquery, sqlconn);
            sqlda.Fill(dt);
            sqlconn.Close();
            dataGridView1.DataSource = dt;

        }



        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                button1.Location = new Point(i+20, i+40);;
                i += 20;
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Down)
            {
                button1.Location = new Point(i + 20, i + 40); ;
                i += 20;
            }
        }

    }
}
