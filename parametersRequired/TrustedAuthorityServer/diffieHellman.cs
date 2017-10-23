using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;

namespace TrustedAuthorityServer
{
    public partial class diffieHellman : Form
    {
        TAServices.TAOPERATIONS ta1;
        lambda_pointAddition.lam_Point_add lapoint;
        public diffieHellman()
        {
            InitializeComponent();
            ChannelFactory<TAServices.TAOPERATIONS> channel2 = new ChannelFactory<TAServices.TAOPERATIONS>("ta");
            ta1 = channel2.CreateChannel();
            ChannelFactory<lambda_pointAddition.lam_Point_add> channel3 = new ChannelFactory<lambda_pointAddition.lam_Point_add>("pointadd");
            lapoint=channel3.CreateChannel();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt16(textBox9.Text) > 1 && Convert.ToInt16(textBox9.Text) < 100)
            {
                int a = Convert.ToInt16(textBox9.Text.Trim());
                label11.Visible = true;
                textBox7.Visible = true;
                button8.Visible = true;
                Point gen_a = lapoint.milersPoint(new Point(0, 190), a, 191);
                textBox7.Text = gen_a.X + "," + gen_a.Y;
            }
            else
            {
                MessageBox.Show("please enter between 1 and 100!!!!!");
            }
          
        }

        private void button8_Click(object sender, EventArgs e)
        {
            TrustedAuthorityServer.TAServiceOperation ta=new TrustedAuthorityServer.TAServiceOperation();
           
             string []abc=textBox7.Text.Trim().Split(',');
            ta.updateValues(textBox1.Text.Trim(),new Point(Convert.ToInt16(abc[0]),Convert.ToInt16(abc[1])));
          //storeValue(textBox1.Text.Trim(),new Point(Convert.ToInt16(abc[0]),Convert.ToInt16(abc[1])));
           MessageBox.Show("your code has been sent to user");
           label14.Visible = true;
           textBox10.Visible = true;
           button10.Visible = true;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            TrustedAuthorityServer.TAServiceOperation ta = new TrustedAuthorityServer.TAServiceOperation();

            textBox10.Text = ta.retreiveUser(textBox1.Text.Trim());
            label16.Visible = true;
            textBox11.Visible = true;
            button11.Visible = true;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string[] user = textBox10.Text.Trim().Split(',');
            string[] server = textBox7.Text.Trim().Split(',');
            Point u = new Point(Convert.ToInt16(user[0]), Convert.ToInt16(user[1]));
            Point s = new Point(Convert.ToInt16(server[0]), Convert.ToInt16(server[1]));
            List<Point>gr=new List<Point>();
            int weil = lapoint.div(u, s, gr, 191);
            textBox11.Text = weil + "";
            encryptSK.Visible = true;
        }
        byte[] readFile(string path)
        {
            byte[] data = null;
            FileInfo finfo = new FileInfo(path);
            long numbytes = finfo.Length;
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            data = br.ReadBytes((int)numbytes);
            br.Close();
            fs.Close();
            return data;
        }
        private void encrypt_SK_Click(object sender, EventArgs e)
        {
            string id = textBox1.Text;
            int key = Convert.ToInt16(textBox11.Text);
            string Key_SK = Convert.ToString(key, 2).PadLeft(8, '0');
            SqlConnection con = new SqlConnection();
            TAServiceOperation ta = new TAServiceOperation();
            con = ta.connection();
            string query = "select * from secretKeyFile where loginID=@id";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.Add(new SqlParameter("@id",id));
            SqlDataReader dr = cmd.ExecuteReader();
            FileStream fs;
            string filename = "d:\\temporary.txt";
            fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
            List<string> fileread=new List<string>();
            while (dr.Read())
            {
                byte[] filedata = (byte[])(dr[2]);

                MessageBox.Show("abcd");
               
                fs.Write(filedata, 0, filedata.Length);
                 fs.Close();
            fileread.Clear();
            fileread = File.ReadAllLines(filename).ToList();

               
            }
          
            string file_name = "d:\\skfile.txt";
            FileStream fs1=new FileStream(file_name,FileMode.Create,FileAccess.Write);
            StreamWriter sw=new StreamWriter(fs1);
            string final_ans = "";
            string result = "";
            foreach (string sk in fileread)
            {
                MessageBox.Show("read from temporary = " + sk);
               
                char[] skey = sk.ToCharArray();
                foreach(char a in skey)
                {

                    //MessageBox.Show(" char =     \n" + a);
                    string m_st = Convert.ToString((char)a, 2).PadLeft(8, '0');
                    result = XOR(Key_SK, m_st);//performs XOR function

                    //MessageBox.Show("result    " + result);
                    int abc = Convert.ToInt32(result, 2);
                    
                    char c = (char)abc;
                    //MessageBox.Show("the value s c = " + c);
                    final_ans = final_ans + c.ToString();
                   //MessageBox.Show("final ans      " + final_ans);
                }
                sw.WriteLine(final_ans);
                final_ans = "";
                result = "";
            }
            sw.Close();
            fs1.Close();
            con.Close();

            byte[] filedata1 = readFile(file_name);


            con = ta.connection();
            string insertquery = "insert into enccryptedSK values(@loginid,@filedata)";
            SqlCommand cmd1 = new SqlCommand(insertquery, con);
            cmd1.Parameters.Add(new SqlParameter("@loginid", id));
            cmd1.Parameters.Add(new SqlParameter("@filedata", (object)filedata1));


            cmd1.ExecuteNonQuery();

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

        private void diffieHellman_Load(object sender, EventArgs e)
        {
        encryptSK.Visible = false;
            label11.Visible = false;
            textBox7.Visible = false;
            button8.Visible = false;
            label14.Visible = false;
            textBox10.Visible = false;
            button10.Visible = false;
            label16.Visible = false;
            textBox11.Visible = false;
            button11.Visible = false;
        }
    }
}
