using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
namespace clientlambdapoint
{
    public partial class Form1 : Form
    {
        lambda_pointAddition.lam_Point_add cal;
        database.databaseoperations cal1;
        public Form1()
        {
            InitializeComponent();
            ChannelFactory<lambda_pointAddition.lam_Point_add> customersFactory = new ChannelFactory<lambda_pointAddition.lam_Point_add>("MyClient");
            cal = customersFactory.CreateChannel();
          
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        int lamda = 0;

        private void button1_Click(object sender, EventArgs e)
        {
           
            
            Point p = new Point(Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text));
            Point q = new Point(Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox4.Text));
            int prime = Convert.ToInt32(textBox5.Text);
            int denomenator = 0;
           label4.Text =""+ cal.lam(p, q, prime, out denomenator);

            //Point p = new Point(Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text));
            //Point q = new Point(Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox4.Text));
            //int prime = Convert.ToInt32(textBox5.Text);
            //int denomenator=0;
            //lamda = cal.lam(p, q, prime, out denomenator);
            //label4.Text = "" + cal.generate_group(p, q, prime, lamda);
            //label1.Text = "" + cal.milersPoint(p, (Convert.ToInt32(textBox6.Text)), prime);

        }
    }
}
