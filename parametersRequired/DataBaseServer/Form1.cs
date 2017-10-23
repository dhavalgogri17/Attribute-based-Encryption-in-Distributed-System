using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
namespace DataBaseServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ServiceHost host, host1, host3;
        private void Form1_Load(object sender, EventArgs e)
        {
            
            


            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                host = new ServiceHost(typeof(DATABASE));
                host.Open();
                host3 = new ServiceHost(typeof(database_reg));
                host3.Open();
                host1 = new ServiceHost(typeof(publicKey));
                host1.Open();
                label1.Text = "database server ready";
                label2.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server is already running");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            host1.Close();
            host.Close();
            host3.Close();
            label2.Text="database server closed";
            label1.Text = "";
        }
    }
}
