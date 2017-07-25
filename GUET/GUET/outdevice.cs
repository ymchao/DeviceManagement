using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUET
{
    public partial class outdevice : Form
    {
        private string select_id;
        private string select_name;
        private string select_spe;
        private string select_enc;
        private string select_pri;
        private string select_num;


        public outdevice(string select_id, string select_name, string select_spe, string select_enc, string select_pri, string select_num)
        {
            // TODO: Complete member initialization
            this.select_id = select_id;
            this.select_name = select_name;
            this.select_spe = select_spe;
            this.select_enc = select_enc;
            this.select_pri = select_pri;
            this.select_num = select_num;


            InitializeComponent();

            this.skinEngine1.SkinFile = "DiamondBlue.ssk";


            //Sunisoft.IrisSkin.SkinEngine skin = new Sunisoft.IrisSkin.SkinEngine();   //加载皮肤

            //skin.SkinFile = System.Environment.CurrentDirectory + "\\skins\\" + "DiamondBlue.ssk";

            //skin.Active = true;

            //skin.SkinAllForm = true; // 这句话是用来设置整个系统下所有窗体都采用这个皮肤

            label1.Text = "器件编号：" + select_id;
            label2.Text = "名称：" + select_name;
            label3.Text = "规格：" + select_spe;
            label4.Text = "封装：" + select_enc;
            label5.Text = "单价：" + select_pri;

        }



        private void button2_Click(object sender, EventArgs e)  // 点击取消 关闭此FROM
        {
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e) 
        {
            if (!(char.IsNumber(e.KeyChar)) && e.KeyChar != (char)8)
            {
                e.Handled = true; //经判断是数字，可以输入
            }
        }

        private string outnum = "";

        public string StrValue
        {
            get { return outnum; }
            set { outnum = value; }
        }

        private string other = "";

        public string StrValue2
        {
            get { return other; }
            set { other = value; }
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("出库数量未输入！", "提示");

            }
            else
            {
                int a = int.Parse( textBox1.Text);  
                int b =int.Parse(select_num);

                if (a > b)
                {
                    MessageBox.Show("出库数量应小于库存数量！", "提示");


                }

                else {
                    outnum = textBox1.Text; //将文本框的出库量的值赋予窗体的属性

                    other = textBox2.Text;   //将文本框的备注值赋予窗体的属性

                    this.DialogResult = DialogResult.OK;

                    this.Close();
                
                }
            }

        }
    }
}
