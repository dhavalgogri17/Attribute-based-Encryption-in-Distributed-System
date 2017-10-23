using System;
using System.IO;
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
    public partial class Form3 : Form
    {
        database.public_masterKey dbparam;
        database.userCredintials du;
        database.databaseoperations dbop;
        TAServices.TAOPERATIONS ta1;
        lambda_pointAddition.lam_Point_add lapoint;
        public Form3(List<string> data)
        {
            InitializeComponent();
            this.data = data;
            ChannelFactory<database.public_masterKey> channel1 = new ChannelFactory<database.public_masterKey>("myclient3");
            ChannelFactory<TAServices.TAOPERATIONS> channel2 = new ChannelFactory<TAServices.TAOPERATIONS>("ta");
            ChannelFactory<lambda_pointAddition.lam_Point_add> channel3 = new ChannelFactory<lambda_pointAddition.lam_Point_add>("lamdapoint");
            ChannelFactory<database.databaseoperations> channel4 = new ChannelFactory<database.databaseoperations>("operations");
            ChannelFactory<database.userCredintials> channel = new ChannelFactory<database.userCredintials>("credentials");
            dbparam = channel1.CreateChannel();
            ta1 = channel2.CreateChannel();
            lapoint = channel3.CreateChannel();
            dbop = channel4.CreateChannel();
            du = channel.CreateChannel();

        }
        List<string> data = new List<string>();
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            string s = "";
            for (int i = 5; i < data.Count; i++)
            {
                s = s + data[i];
            }
            richTextBox1.Text = s;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<string> secretKeyFile = File.ReadAllLines(textBox1.Text).ToList();
            List<Point> secPoints = new List<Point>();
            string abc = "";
            for (int i = 2; i < secretKeyFile.Count; i++)
            {
                if (secretKeyFile[i] != "")
                {
                    // MessageBox.Show(secretKeyFile[i]);
                    string[] subsec = secretKeyFile[i].Split(' ');
                    string[] subkeypoint = subsec[2].Split(',');
                    int x = Convert.ToInt16(subkeypoint[0]);
                    int y = Convert.ToInt16(subkeypoint[1]);
                    secPoints.Add(new Point(x, y));

                }
            }
            string[] dataownerid = data[2].ToString().Split(',');
            int t = 4;
            Point Anonymous_key = new Point(Convert.ToInt16(dataownerid[0]), Convert.ToInt16(dataownerid[1])); //ta1.AnonymousKeyGeneration(dataownerid);
            Anonymous_key = ta1.hash_beta_t(Anonymous_key, t, 191);
            Point hash_pt = new Point(0, 0);
            List<Point> grp_1 = new List<Point>();


            //    string abc = "";
            int main = 0;
            Point r = new Point(0, 0);
            //                dr.Close();
            for (int i = 0; i < secPoints.Count; i++)
            {


                main = lapoint.div(Anonymous_key, secPoints[i], grp_1, 191);
                abc = abc + main + "@";

            }
            //  MessageBox.Show(abc + "\n" + data[0] + "\n" + data[1] + "\n" + data[2] + "\n" + data[3] + "\n" + data[4] + "\n" + data[5] + "\n");

            bool flag = lapoint.match_key(data[0].ToString(), abc);
            if (flag == false)
            {
                MessageBox.Show("You are wrong user");
                richTextBox2.Text = "";
            }
            else
            {
                MessageBox.Show("User Verified");



                string final_ans = "";
                string sec = richTextBox1.Text;
                string[] secretKey = sec.Split('\n');
                string Key_SK = Convert.ToString(127, 2).PadLeft(8, '0');
                string result = "";
                string SK = "";
                foreach (string sk in secretKey)
                {
                    //MessageBox.Show("" + sk);

                    char[] skey = sk.ToCharArray();
                    foreach (char a in skey)
                    {
                        //  MessageBox.Show(" char " + a);
                        string m_st = Convert.ToString((char)a, 2).PadLeft(8, '0');
                        result = XOR(Key_SK, m_st);//performs XOR function
                        //  MessageBox.Show("result    " + result);
                        long abc1 = Convert.ToInt64(result, 2);
                        char c = (char)abc1;
                        final_ans = final_ans + c;
                        // MessageBox.Show("final ans      " + final_ans);
                    }
                    SK = SK + final_ans;// + "\n";
                    final_ans = "";
                    result = "";

                }
                richTextBox2.Text = SK;



            }


        }
        public string XOR(string w_st, string m_st)
        {
            string result = "";
            char[] w = w_st.ToCharArray();
            char[] m = m_st.ToCharArray();
            for (int i = 0; i < w.Length; i++)
            {
                if (w[i].Equals(m[i]))
                {
                    result = result + '0';
                }
                else if (!(w[i].Equals(m[i])))
                {
                    result = result + '1';
                }
            }
            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string[] abc = richTextBox2.Text.Split('\n');
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {

                    FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);

                    StreamWriter sw = new StreamWriter(fs);
                    foreach (string a in abc)
                    {
                        sw.WriteLine(a);
                    }

                    sw.Close();
                    fs.Close();







                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error in saving");
            }
        }
    }
}
