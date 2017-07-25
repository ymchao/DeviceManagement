using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;  // 为了弹框的设计


using MySql.Data;
using MySql.Data.MySqlClient;



namespace GUET
{
    public partial class MainForm : Form
    {
        public MainForm(string txName)
        {
            InitializeComponent();

            this.skinEngine1.SkinFile = "DiamondBlue.ssk";

            //Sunisoft.IrisSkin.SkinEngine skin = new Sunisoft.IrisSkin.SkinEngine();

            //skin.SkinFile = System.Environment.CurrentDirectory + "\\skins\\" + "DiamondBlue.ssk";

            //skin.SkinAllForm = true; // 这句话是用来设置整个系统下所有窗体都采用这个皮肤

            //skin.Active = true;

            this.toolStripStatusLabel4.Text = txName; // 将登陆用户名传入 toolStripStatusLabel4

            this.textBox8.Text = txName;          // 将登陆用户名传入 入库详情中的textBox8

            this.textBox9.Text = DateTime.Today.ToString("yyyy-MM-dd");
                


        }

        private void timer1_Tick(object sender, EventArgs e)  //  显示时间
        {
            this.toolStripStatusLabel2.Text = DateTime.Now.ToString();
        }

        string constr = "server=172.16.64.73;User Id=everyone;password=123456;Database=repertory_manager;charset=utf8";//学校数据库，MySql

        private void button1_Click(object sender, EventArgs e)
        {

            MySqlConnection sqlConnection = new MySqlConnection(constr);

            sqlConnection.Open();

            if (comboBox1.Text.Trim() == "")
            {
                string sql = "select * from G_device";

                MySqlDataAdapter sda = new MySqlDataAdapter(sql, sqlConnection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "emp");

                dataGridView1.DataSource = ds.Tables[0];

            }
            else
            {
                string sql = "select * from G_device where 名称 = '" + comboBox1.Text.Trim() + "'";

                MySqlCommand comm = new MySqlCommand(sql, sqlConnection);

                MySqlDataReader read = comm.ExecuteReader();


                if (read.HasRows)
                {
                    read.Close();   // 不然报错

                    MySqlDataAdapter sda = new MySqlDataAdapter(sql, sqlConnection);

                    DataSet ds = new DataSet();

                    sda.Fill(ds, "emp");

                    dataGridView1.DataSource = ds.Tables[0];

                }
                else
                    MessageBox.Show("库存中未发现此器件！", "提示");

                sqlConnection.Close();

            }

            int psum = 0;      // 定义总数为0         显示 总库存量和总价值
            double msum = 0;   //定义总价为0

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                psum = psum + Convert.ToInt32(dataGridView1.Rows[i].Cells["库存数量"].Value);
            }
            label25.Text = "器件总数： " + psum.ToString();

            for (int j = 0; j < dataGridView1.Rows.Count; j++)
            {
                msum = msum + Convert.ToDouble(dataGridView1.Rows[j].Cells["总价（元）"].Value);
            }
            label26.Text = "总价值(元)： " + msum.ToString();
            //foreach (DataGridViewColumn col in this.dataGridView1.Columns)//表头文字不居中的问题主要是DataGridView要绘制排序的箭头引起的,可以使下以下方法让他们居中,但可能需要自已写排序代码(即不会自动实现单击列头排序).
            //{
            //    col.SortMode = DataGridViewColumnSortMode.NotSortable;
            //}
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void tableControl_SelectedIndexChanged_1(object sender, EventArgs e)
        {

            if (tableControl.SelectedTab.Name == "tabPage4")
            {
                if (toolStripStatusLabel4.Text != "admin")  // 非admin 账号 不允许访问  账号管理界面
                {
                    MessageBox.Show("您不是管理员，没有权限访问此页！", "提示");
                    tableControl.SelectedTab = tabPage1;
                }
            }
        }

        //public int a = 0;  // 定义一个全局变量

        private void button2_Click_1(object sender, EventArgs e)  // 查看账户表里的账户信息
        {

            MySqlConnection sqlConnection = new MySqlConnection(constr);

            sqlConnection.Open();

            string sql = "select * from G_user";

            MySqlDataAdapter sda = new MySqlDataAdapter(sql, sqlConnection);

            DataSet ds = new DataSet();

            sda.Fill(ds, "emp");

            dataGridView2.DataSource = ds.Tables[0];


            //if(a==0)  //   确保  点击 查询按钮时不会在最后生成多列，而只生成一列
            //{

            //DataGridViewButtonColumn Column1 = new System.Windows.Forms.DataGridViewButtonColumn();  //  实例化一个DataGridViewButtonColumn

            //Column1.ReadOnly = true;

            //Column1.HeaderText = "操作"; //设置列标题和按钮文本

            //Column1.UseColumnTextForButtonValue = true;

            //this.dataGridView2.Columns.Add(Column1); // 添加到dataGridView2

            //a++;
            //}

        }

        private void button3_Click_1(object sender, EventArgs e)       //上传账户信息
        {

            MySqlConnection sqlConnection = new MySqlConnection(constr);

            sqlConnection.Open();

            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("请输入完整的账户信息！", "提示");
            }
            else
            {
                string sql1 = "select * from G_user where 用户名 = '" + textBox1.Text.Trim() + "'"; // 验证用户名是否可用

                MySqlCommand comm = new MySqlCommand(sql1, sqlConnection);

                MySqlDataReader read = comm.ExecuteReader();

                read.Close();

                if (read.HasRows)
                {

                    MessageBox.Show("用户名已存在，请重新输入！", "提示");

                    this.textBox1.Text = "";

                }
                else
                { 
                    //上传注册用户信息
                    string sql = " insert into G_user values('" + textBox1.Text.Trim() + "','" + textBox2.Text.Trim() + "','" + textBox3.Text.Trim() + "','" + textBox4.Text.Trim() + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "')";

                    MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("添加账户成功！点击查看刷新！", "提示");

                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";

                    sqlConnection.Close();

                }
            }

        }



        private void button4_Click(object sender, EventArgs e) //删除用户按钮
        {
            MySqlConnection sqlConnection = new MySqlConnection(constr);

            try
            {
                sqlConnection.Open();

                string select_id = dataGridView2.CurrentRow.Cells[0].Value.ToString();//选择的当前行第一列的值，也就是ID用户名

                string delete_by_id = "delete from G_user where 用户名 = '" + select_id + "'";//sql删除语句

                MySqlCommand cmd = new MySqlCommand(delete_by_id, sqlConnection);

                DialogResult YORN = MessageBox.Show(this, "确定要删除【" + select_id + "】的账户吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (YORN == DialogResult.Yes)   //  如果点击“是”  则执行删除语句，否则不执行
                {
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("删除成功！点击查看刷新！", "提示");
                }

            }
            catch
            {
                MessageBox.Show("请正确选择行!");
            }
            finally
            {
                sqlConnection.Dispose();
            }

        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e) // 限定电话输入框textBox3只能输入数字
        {
            if (!(char.IsNumber(e.KeyChar)) && e.KeyChar != (char)8)
            {
                e.Handled = true; //经判断是数字，可以输入
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

            MySqlConnection sqlConnection = new MySqlConnection(constr);

            sqlConnection.Open();

            if (comboBox2.Text.Trim() == "")
            {
                string sql = "select * from G_indevice";

                MySqlDataAdapter sda = new MySqlDataAdapter(sql, sqlConnection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "emp");

                dataGridView3.DataSource = ds.Tables[0];

            }
            else
            {
                string sql = "select * from G_indevice where 名称 = '" + comboBox2.Text.Trim() + "'";

                MySqlCommand comm = new MySqlCommand(sql, sqlConnection);

                MySqlDataReader read = comm.ExecuteReader();


                if (read.HasRows)
                {
                    read.Close();   // 不然报错

                    MySqlDataAdapter sda = new MySqlDataAdapter(sql, sqlConnection);

                    DataSet ds = new DataSet();

                    sda.Fill(ds, "emp");

                    dataGridView3.DataSource = ds.Tables[0];
                }
                else
                    MessageBox.Show("未发现此器件入库情况！", "提示");

                sqlConnection.Close();
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)  //  将单价与数量之积给总价
        {
            try
            {
                double a = Convert.ToDouble(textBox6.Text);

                double b = Convert.ToDouble(textBox7.Text);

                double result = a * b;

                textBox5.Text = result.ToString();
            }
            catch
            {
                MessageBox.Show("输入不正确，请重新输入！", "提示");
            }
        }


        private void textBox7_KeyPress(object sender, KeyPressEventArgs e) // 限定单价输入框textBox7只能输入数字和小数点
        {
            if (!(char.IsNumber(e.KeyChar)) && e.KeyChar != (char)8 && e.KeyChar != '.')
            {
                e.Handled = true; //经判断是数字，可以输入
            }
        }
        private void textBox6_KeyPress(object sender, KeyPressEventArgs e) // 限定数量输入框textBox6只能输入数字和小数点
        {
            if (!(char.IsNumber(e.KeyChar)) && e.KeyChar != (char)8 && e.KeyChar != '.')
            {
                e.Handled = true; //经判断是数字，可以输入
            }
        }

        private void button6_Click_1(object sender, EventArgs e)   // 入库按钮操作
        {
            MySqlConnection sqlConnection = new MySqlConnection(constr);

            sqlConnection.Open();

            if (comboBox3.Text == "")   //  检测 器件名称  是否输入
            {
                MessageBox.Show("请输入器件名称！", "提示");
            }
            else
            {
                if (textBox7.Text == "0")        //  检测 数量 是否输入正确
                {
                    MessageBox.Show("入库数量不能为0！", "提示");
                }
                else
                {
                     string sql = "select * from G_device where 器件编号 = '"+comboBox6.Text.Trim()+"' and 名称 = '"+comboBox3.Text.Trim()+"' and 规格 = '"+comboBox4.Text.Trim()+"' and 封装 = '"+comboBox5.Text.Trim()+"' and 单价（元） = '"+textBox6.Text.Trim()+"'";                  
                     
                     MySqlCommand comm = new MySqlCommand(sql, sqlConnection);

                     MySqlDataReader read = comm.ExecuteReader();

                     string aaa = read.HasRows.ToString();

                     read.Close();

                     if (aaa == "True")  //  当条件都满足时 ， 则说明库存中有此 器件，，，只需要在G_device中加数量，和总价
                            {
                                DialogResult YORN = MessageBox.Show(this, "器件编号：" + comboBox6.Text + "\n" 
                                 + "器件名称：" + comboBox3.Text + "\n"
                                 + "器件规格：" + comboBox4.Text + "\n"
                                 + "器件封装：" + comboBox5.Text + "\n"
                                 + "器件数量：" + textBox7.Text + "\n"
                                 + "器件单价：" + textBox6.Text + "\n"
                                 + "确定入库？"
                                 , "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                                if (YORN == DialogResult.Yes)   //  如果点击“是”  则执行入库语句，上传到G_indevice（入库情况表）,  并且在G_device的表里相应数量列+1否则不执行
                                {
                                    // 先上传到G_indevice（入库情况表
                                    string sql1 = " insert into G_indevice values('" + comboBox6.Text.Trim() + "','" + comboBox3.Text.Trim() + "','" + comboBox4.Text.Trim() + "','" + comboBox5.Text.Trim() + "','" + textBox7.Text.Trim() + "','" + textBox6.Text.Trim() + "','" + textBox5.Text.Trim() + "','" + textBox8.Text.Trim() + "','" + textBox9.Text.Trim() + "','" + textBox10.Text.Trim() + "')";

                                    MySqlCommand cmd = new MySqlCommand(sql1, sqlConnection);

                                    cmd.ExecuteNonQuery();

                                    // 上传到G_device（库存情况表）

                                    string sql5 = "select 库存数量 from G_device where 器件编号 = '" + comboBox6.Text.Trim() + "' and 名称 = '" + comboBox3.Text.Trim() + "' and 规格 = '" + comboBox4.Text.Trim() + "' and 封装 = '" + comboBox5.Text.Trim() + "' and 单价（元） = '" + textBox6.Text.Trim() + "'"; // 获取到G_device 里的原有器件数量，与入库数量相加再上传

                                    MySqlCommand comm5 = new MySqlCommand(sql5, sqlConnection);

                                    MySqlDataReader reader1 = comm5.ExecuteReader();

                                    reader1.Read();

                                    int x = (int)reader1["库存数量"]; 

                                    int y = int.Parse(textBox7.Text);

                                    int total = x + y;

                                    string totalnum = total.ToString();

                                    reader1.Close();

                                    string sql6 = "select 总价（元） from G_device where 器件编号 = '" + comboBox6.Text.Trim() + "' and 名称 = '" + comboBox3.Text.Trim() + "' and 规格 = '" + comboBox4.Text.Trim() + "' and 封装 = '" + comboBox5.Text.Trim() + "' and 单价（元） = '" + textBox6.Text.Trim() + "'"; // 获取到G_device 里的原有器件总价，与入库总价相加再上传

                                    MySqlCommand comm6 = new MySqlCommand(sql6, sqlConnection);

                                    MySqlDataReader reader2 = comm6.ExecuteReader();

                                    reader2.Read();

                                    string m = reader2["总价（元）"].ToString();

                                    double mm = Convert.ToDouble(m); 

                                    double n = double.Parse(textBox5.Text);

                                    double  totalmn = mm + n;

                                    string z = totalmn.ToString();

                                    reader2.Close();

                                    string sql7 = "update G_device set 库存数量 = '" + totalnum + "',总价（元） = '" + z + "',备注 = '" + textBox10.Text.Trim() + " 'where 器件编号 = '" + comboBox6.Text.Trim() + "' and 名称 = '" + comboBox3.Text.Trim() + "' and 规格 = '" + comboBox4.Text.Trim() + "' and 封装 = '" + comboBox5.Text.Trim() + "' and 单价（元） = '" + textBox6.Text.Trim() + "'";  // 将数量上传  更新G_device

                                    MySqlCommand cmd4 = new MySqlCommand(sql7, sqlConnection);

                                    cmd4.ExecuteNonQuery();

                                    MessageBox.Show("入库成功！点击查看刷新！", "提示");

                                    sqlConnection.Close();
                                }                       
                             }

                    // 如果G_device中没有这个器件，则将全部信息都更新到两个表中
                    else{
                        DialogResult YORN2 = MessageBox.Show(this, "器件编号：  " + comboBox6.Text + "\n"
                           + "器件名称：  " + comboBox3.Text + "\n"
                           + "器件规格：  " + comboBox4.Text + "\n"
                           + "器件封装：  " + comboBox5.Text + "\n"
                           + "器件数量：  " + textBox7.Text + "\n"
                           + "器件单价：  " + textBox6.Text + "\n"
                           + "确定新器件入库？  "
                           , "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                        if (YORN2 == DialogResult.Yes)   //  如果点击“是”  则执行入库语句，上传到G_indevice 和 G_device
                        {
                            // 先上传到G_indevice（入库情况表
                            string sql11 = " insert into G_indevice values('" + comboBox6.Text.Trim() + "','" + comboBox3.Text.Trim() + "','" + comboBox4.Text.Trim() + "','" + comboBox5.Text.Trim() + "','" + textBox7.Text.Trim() + "','" + textBox6.Text.Trim() + "','" + textBox5.Text.Trim() + "','" + textBox8.Text.Trim() + "','" + textBox9.Text.Trim() + "','" + textBox10.Text.Trim() + "')";

                            MySqlCommand cmd11 = new MySqlCommand(sql11, sqlConnection);

                            cmd11.ExecuteNonQuery();

                            // 再上传到G_device（入库情况表

                            string sql22 = " insert into G_device values('" + comboBox6.Text.Trim() + "','" + comboBox3.Text.Trim() + "','" + comboBox4.Text.Trim() + "','" + comboBox5.Text.Trim() + "','" + textBox7.Text.Trim() + "','" + textBox6.Text.Trim() + "','" + textBox5.Text.Trim() + "','" + textBox10.Text.Trim() + "')";

                            MySqlCommand cmd22 = new MySqlCommand(sql22, sqlConnection);

                            cmd22.ExecuteNonQuery();

                            MessageBox.Show("新器件入库成功！点击查看刷新！", "提示");

                            sqlConnection.Close();


                        }
                     }
                    
                }

            }
        }

        private void button10_Click(object sender, EventArgs e)   // 查询出库表
        {
            MySqlConnection sqlConnection = new MySqlConnection(constr);

            sqlConnection.Open();

            if (comboBox8.Text.Trim() == "")
            {
                string sql = "select * from G_outdevice";

                MySqlDataAdapter sda = new MySqlDataAdapter(sql, sqlConnection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "emp");

                dataGridView5.DataSource = ds.Tables[0];

            }
            else
            {
                string sql = "select * from G_outdevice where 名称 = '" + comboBox8.Text.Trim() + "'";

                MySqlCommand comm = new MySqlCommand(sql, sqlConnection);

                MySqlDataReader read = comm.ExecuteReader();


                if (read.HasRows)
                {
                    read.Close();   // 不然报错

                    MySqlDataAdapter sda = new MySqlDataAdapter(sql, sqlConnection);

                    DataSet ds = new DataSet();

                    sda.Fill(ds, "emp");

                    dataGridView5.DataSource = ds.Tables[0];
                }
                else
                    MessageBox.Show("未发现此器件出库情况！", "提示");        
            }

            sqlConnection.Close();

            int psum = 0;      // 定义总数为0         显示 总库存量和总价值
            double msum = 0;   //定义总价为0

            for (int i = 0; i < dataGridView5.Rows.Count; i++)
            {
                psum = psum + Convert.ToInt32(dataGridView5.Rows[i].Cells["出库数量"].Value);
            }
            label24.Text = "器件出库总数： " + psum.ToString();

            for (int j = 0; j < dataGridView5.Rows.Count; j++)
            {
                msum = msum + Convert.ToDouble(dataGridView5.Rows[j].Cells["总价（元）"].Value);
            }
            label23.Text = "总价值(元)： " + msum.ToString();
        }

       


        private void button8_Click_1(object sender, EventArgs e)
        {
            MySqlConnection sqlConnection = new MySqlConnection(constr);

            try
            {


                sqlConnection.Open();

                string select_id = dataGridView1.CurrentRow.Cells[0].Value.ToString();//选择的当前行第1列的值，也就是器件编号

                string select_name = dataGridView1.CurrentRow.Cells[1].Value.ToString();//选择的当前行第2列的值，也就是器件名称

                string select_spe = dataGridView1.CurrentRow.Cells[2].Value.ToString();//选择的当前行第3列的值，也就是器件规格

                string select_enc = dataGridView1.CurrentRow.Cells[3].Value.ToString();//选择的当前行第4列的值，也就是器件封装

                string select_num = dataGridView1.CurrentRow.Cells[4].Value.ToString();//选择的当前行第5列的值，也就是器件封装

                string select_pri = dataGridView1.CurrentRow.Cells[5].Value.ToString();//选择的当前行第6列的值，也就是器件单价

                string select_totalpri = dataGridView1.CurrentRow.Cells[6].Value.ToString();//选择的当前行第7列的值，也就是器件单价          

                outdevice frm = new outdevice(select_id, select_name, select_spe, select_enc, select_pri, select_num);

                frm.ShowDialog();

                if (frm.DialogResult == System.Windows.Forms.DialogResult.OK)  //  如果点击确定  则返回出库数量和备注
                {
                    string outnum = frm.StrValue;    //获取弹出窗体的出库数量

                    string other = frm.StrValue2;    //获取弹出窗体的备注

                    double outpri = Convert.ToDouble(outnum); //出库数

                    double b = Convert.ToDouble(select_pri); //单价

                    double outpritotal = outpri * b;  //出库总金额

                    string outpritotal2 = outpritotal.ToString();


                    // 先上传到G_outdevice（出库情况表
                    string insql = " insert into G_outdevice values('" + select_id + "','" + select_name + "','" + select_spe + "','" + select_enc + "','" + outnum + "','" + select_pri + "','" + outpritotal2 + "','" + textBox8.Text.Trim() + "','" + textBox9.Text.Trim() + "','" + other + "')";

                    MySqlCommand cmd = new MySqlCommand(insql, sqlConnection);

                    cmd.ExecuteNonQuery();



                    // 更改G_device（库存情况表）

                    double nownum = Convert.ToDouble(select_num) - Convert.ToDouble(outnum); //出库后的数量

                    if (nownum == 0)  //  如果出库后的数量为0，则继续删除G_device对应的语句
                    {

                        string delsql = "delete from G_device where 器件编号 = '" + select_id + "' and 名称 = '" + select_name + "' and 规格 = '" + select_spe + "' and 封装 = '" + select_enc + "' and 单价（元） = '" + select_pri + "'";

                        MySqlCommand cmd2 = new MySqlCommand(delsql, sqlConnection);

                        cmd2.ExecuteNonQuery();

                    }

                    else
                    {    //  如果出库后的数量不为0，则继续更新G_device


                        double nowpri = nownum * b; //出库后的总价

                        string nownum1 = nownum.ToString();

                        string nowpri1 = nowpri.ToString();

                        string sql = "update G_device set 库存数量 = '" + nownum1 + "',总价（元） = '" + nowpri1 + " 'where 器件编号 = '" + select_id + "' and 名称 = '" + select_name + "' and 规格 = '" + select_spe + "' and 封装 = '" + select_enc + "' and 单价（元） = '" + select_pri + "'";  // 将数量上传  更新G_device

                        MySqlCommand cmd4 = new MySqlCommand(sql, sqlConnection);

                        cmd4.ExecuteNonQuery();
                    }


                    MessageBox.Show("出库成功！点击查看刷新！", "提示");

                    sqlConnection.Close();
                }

            }
            catch
            {
                MessageBox.Show("请正确选择出库器件!");
            }
            finally
            {
                sqlConnection.Dispose();
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();   //  关闭所有进程
        }


    }
}