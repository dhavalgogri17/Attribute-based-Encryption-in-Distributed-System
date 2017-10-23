using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
namespace SystemSetup
{
    public partial class Form1 : Form
    {
        lambda_pointAddition.lam_Point_add point_obj1;
       // lambda_pointAddition.group_gen group_obj1;
        database.public_masterKey dbobj;
        List<int> Zprimes = new List<int>();
        List<Point> coordinates = new List<Point>();
        List<Point>group_1 = new List<Point>();
        List<Point> generator_T = new List<Point>();
        int weilpairing = 0;
        
        public Form1()
        {
            InitializeComponent();
            ChannelFactory<lambda_pointAddition.lam_Point_add> channel1 = new ChannelFactory<lambda_pointAddition.lam_Point_add>("MyClient1");
            point_obj1 = channel1.CreateChannel();

            //ChannelFactory<lambda_pointAddition.group_gen> channel2 = new ChannelFactory<lambda_pointAddition.group_gen>("MyClient2");
            //group_obj1 = channel2.CreateChannel();

            ChannelFactory<database.public_masterKey> channel3 = new ChannelFactory<database.public_masterKey>("myclient3");
            dbobj = channel3.CreateChannel();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int primeNumber = 191;
            int alpha = 130;
            int beta = 160;
            int lambda=0;
            Point generator=new Point(0,190);
            for (int i = 1; i < primeNumber; i++)
            {
                Zprimes.Add(i);
            }
           // coordinates = group_obj1.generate_mulgroup(primeNumber, Zprimes);

          //  group_1 = group_obj1.GroupGen(coordinates, primeNumber, lambda, group_1,generator);
           // generator_T= group_obj1.group_t(group_1, primeNumber);
            //dbobj.insert_into_database_GT(group_1);
            Point H = point_obj1.milersPoint(generator, beta, primeNumber);
          
            Point gen_alpha = point_obj1.milersPoint(generator, alpha, primeNumber);
           weilpairing = point_obj1.div(gen_alpha, gen_alpha, group_1, primeNumber);

            string s1 = "", s2 = "", s3 = "";
            foreach (int i in Zprimes)
            {
                s1 = s1 + ".." + i;
            }

            foreach (Point i in coordinates)
            {
                s3 = s3 + "------" + i;
            }

            string s = "";
            //Gt = Gt[][0].Distinct().ToList();
            foreach (Point i in group_1)
            {
                s = s + "  {" + i + "}   ";
            }

            MessageBox.Show("\n\n\n group 1 count : " + group_1.Count + "\n\n GT : " + generator_T.Count + "\n\n ecc count : " + coordinates.Count + "\n\n\nprime " + primeNumber + "   \n \nzprime " + s1 + "\n \n ecccordinat" + s3 + "\n \n \n\ngroup 1 " + s);
            MessageBox.Show("generator    " + generator);
            MessageBox.Show("h......" + H);
            MessageBox.Show("weil      ....  " + weilpairing);


            foreach (Point i in generator_T)
            {
                s2 = s2 + "  {" + i + "}   ";
            }
            MessageBox.Show("generator_T : " + s2);
           
            dbobj.insert_into_database_PK(group_1, generator, H, weilpairing);
           // dbobj.insert_into_database_MK(beta, gen_alpha);
            dbobj.insert_into_database_GT(group_1);



        }
    }
}
