using System;
using System.ServiceModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;

namespace TrustedAuthorityServer
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
           
        }
        ServiceHost host;

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                host = new ServiceHost(typeof(TAServiceOperation));
                host.Open();
                MessageBox.Show("server ready");
                TAServiceOperation ta = new TAServiceOperation();
                ta.SystemSetup();
                label1.Text = "Trusted Authorithy Server Ready";
                label3.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server is already running");
            }

          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            diffieHellman dh = new diffieHellman();
            dh.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            host.Close();
            label3.Text = "Trusted Authorithy Server Closed";
            label1.Text = "";
        }
    }
}
