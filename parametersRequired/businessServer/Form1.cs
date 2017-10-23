using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
namespace businessServer
{
    public partial class Form1 : Form
    {
        lambda_pointAddition.lam_Point_add point_obj1;
       
        List<int> Zprimes = new List<int>();
        List<Point> coordinates = new List<Point>();
        List<Point> group_1 = new List<Point>();
        List<Point> generator_T = new List<Point>();

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
            
         host  = new ServiceHost(typeof(businessServer.lambdaAndPointAddition));
            host.Open();
            label1.Text = "server ready";
            label2.Text = "";
        


            




        }

        private void button2_Click(object sender, EventArgs e)
        {
          host.Close();
          label2.Text = "Server Closed";
          label1.Text = "";
        }

        
            }
}
