using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
namespace GUI_db
{
    public partial class Form1 : Form
    {
        database.userCredintials dbcrendentials;
        TAServices.TAOPERATIONS t;
        private string check = "";
        public Form1()
        {
            InitializeComponent();
            ChannelFactory<database.userCredintials> channel1 = new ChannelFactory<database.userCredintials>("credentials");
            dbcrendentials = channel1.CreateChannel();
            ChannelFactory<TAServices.TAOPERATIONS> channel = new ChannelFactory<TAServices.TAOPERATIONS>("ta");
            t = channel.CreateChannel();
        }

        private void Form1_Load(object sender, EventArgs e)
        {



            groupBox1.Visible = false;
            groupBox2.Visible = false;
            //MessageBox.Show(db_reg.same_user("vinay")+"");
        }

        private void new_user_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            groupBox2.Visible = true;
            label5.Text = "New User Form";
        }

        private void login_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
            groupBox1.Visible = true;
            label2.Text = "Login";
        }

        private void register_Click(object sender, EventArgs e)
        {
            if (checkBox13.Checked == false && checkBox14.Checked == false && checkBox15.Checked == false && checkBox16.Checked == false && checkBox17.Checked == false && checkBox18.Checked == false && checkBox19.Checked == false && checkBox20.Checked == false && checkBox21.Checked == false && checkBox22.Checked == false)
            {
                MessageBox.Show("please select  check box");

            }

            else
            {
                bool flag = false;
                flag = dbcrendentials.same_user(textBox3.Text.Trim());
                if (flag)
                {
                    MessageBox.Show("same user_id exist not allowed");
                }
                else
                {

                    textBox1.Text = "";
                    textBox2.Text = "";
                    new_user.Visible = false;
                    groupBox2.Visible = false;
                    groupBox1.Visible = true;
                    check = checkbox_check();
                    //  check = check;// +textBox6.Text + "," + textBox7.Text + "," + textBox8.Text;
                    dbcrendentials.insert_reg(textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text, check);
                    MessageBox.Show("account created");
                }
            }
            t.generateSecretKey(textBox3.Text, check);


        }

        private void submit_Click(object sender, EventArgs e)
        {
            Boolean flag = false;
            flag = dbcrendentials.check_login(textBox1.Text, textBox2.Text);
            if (!flag)
            {
                MessageBox.Show("could not login...no user id and password match");
            }
            else
            {
                MessageBox.Show("login successfully");
                Form2 form2 = new Form2(textBox1.Text);
                form2.FormClosed += new FormClosedEventHandler(exit);
                form2.Show();
                this.Hide();
            }
        }
        void exit(Object Sender, FormClosedEventArgs e)
        {
            this.Close();
        }
        public string checkbox_check()
        {
            string check = "";

            if (checkBox13.Checked == true)
            {
                check = check + checkBox13.Text + ",";
            }
            if (checkBox14.Checked == true)
            {
                check = check + checkBox14.Text + ",";
            }
            if (checkBox15.Checked == true)
            {
                check = check + checkBox15.Text + ",";
            }
            if (checkBox16.Checked == true)
            {
                check = check + checkBox16.Text + ",";
            }
            if (checkBox17.Checked == true)
            {
                check = check + checkBox17.Text + ",";
            }
            if (checkBox18.Checked == true)
            {
                check = check + checkBox18.Text + ",";
            }
            if (checkBox19.Checked == true)
            {
                check = check + checkBox19.Text + ",";
            }
            if (checkBox20.Checked == true)
            {
                check = check + checkBox20.Text + ",";
            }
            if (checkBox21.Checked == true)
            {
                check = check + checkBox21.Text + ",";
            }
            if (checkBox22.Checked == true)
            {
                check = check + checkBox22.Text + ",";
            }
            if (checkBox23.Checked == true)
            {
                check = check + checkBox23.Text + ",";
            }
            if (checkBox24.Checked == true)
            {
                check = check + checkBox24.Text + ",";
            }
            check = check.Trim();
            return check;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }



    }
}
