using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data;

using MySql.Data;
using MySql.Data.MySqlClient;

namespace GUET
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();

            this.skinEngine1.SkinFile = "DiamondBlue.ssk";   //  引用皮肤


            //Sunisoft.IrisSkin.SkinEngine skin = new Sunisoft.IrisSkin.SkinEngine();

            //skin.SkinFile = System.Environment.CurrentDirectory + "\\skins\\" + "DiamondBlue.ssk";

            //skin.Active = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string constr = "server=172.16.64.73;User Id=everyone;password=123456;Database=repertory_manager;charset=utf8";//学校数据库，MySql

            MySqlConnection sqlConnection = new MySqlConnection(constr);

            if (textBox1.Text == "" || textBox2.Text == "")

                MessageBox.Show("请输入用户名和密码!", "提示");
            else
            {
                sqlConnection.Open();

                string sql = "select * from G_user where 用户名='" + textBox1.Text.Trim() + " 'and 密码='" + textBox2.Text.Trim() + "'";

                MySqlCommand comm = new MySqlCommand(sql, sqlConnection);

                MySqlDataReader read = comm.ExecuteReader();

               // read.Read();

                if (read.HasRows)
                {
                    read.Close();

                    string name = textBox1.Text.Trim();

                    MainForm main = new MainForm(name);

                    main.Show();

                   this.Hide();
                }
                else
                    MessageBox.Show("用户名或密码错误，请重新输入！", "提示");

                sqlConnection.Close();


            }
        }

   
        private void login_Load(object sender, EventArgs e)
        {
           
        }

      
       
    }
}
