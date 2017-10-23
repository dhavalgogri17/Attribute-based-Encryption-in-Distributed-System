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
using database;

namespace GUI_db
{
    public partial class Form2 : Form
    {
        string final_ans = "";
        DataTable dt = new DataTable("userFileEncrypted");

        DataSet ds = new DataSet();
        FileStream fs;
        bool flag = false;
        database.public_masterKey dbparam;
        database.userCredintials du;
        database.databaseoperations dbop;
        TAServices.TAOPERATIONS ta1;
        SqlConnection con;
        Point h = new Point();
        Point C = new Point();
        Point cy_pt = new Point(0, 0);
        string cy = "";
        string cy_dash = "";
        string userid = "";
        lambda_pointAddition.lam_Point_add lapoint;
        int n = 0, rite = 0, left = 0, sec_rite = 0, sec_left = 0;
        int count = 0, event_count = 0;
        string access_tree = "", scrambled_access_tree = "";
        string left_tree = "", scrambled_left_tree = "";
        string rite_tree = "", scrambled_rite_tree;
        char a = ' ';
        public Form2(string userid)
        {
            InitializeComponent();
            this.userid = userid;
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

        private void Form2_Load(object sender, EventArgs e)
        {
            ds = du.retreiveFile();



            button4.Visible = false;
            textBox2.Text = userid;
            string PK = dbparam.retreive_public_key();
            string[] pksub = PK.Split('@');
            string[] groupsub = pksub[0].Split(' ');
            string group = "";
            for (int i = 0; i < 10; i++)
            {
                string g = "{" + groupsub[i] + "}";
                group = group + g + " ";
            }
            label25.Text = "group: " + group + " generator: " + "{" + pksub[1] + "}" + " H: " + "{" + pksub[2] + "}" + " w: " + "{" + pksub[3] + "}";

            groupBox2.Visible = false;
            groupBox4.Visible = false;
            groupBox7.Visible = false;
            groupBox6.Visible = false;
            groupBox5.Visible = false;
            groupBox8.Visible = false;


        }

        private void file_upload_Click(object sender, EventArgs e)
        {
            groupBox7.Visible = false;
            groupBox6.Visible = false;
            groupBox2.Visible = true;
            groupBox5.Visible = false;
            groupBox4.Visible = true;
            groupBox8.Visible = false;
        }

        private void query_generate_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            ds = du.retreiveFile();
            int rows = ds.Tables["userFileEncrypted"].Rows.Count;
            List<string> psuedo = new List<string>();
            comboBox1.Items.Clear();
            psuedo.Clear();
            for (int i = 0; i < rows; i++)
            {


                string id = ds.Tables["userFileEncrypted"].Rows[i]["login_id"].ToString();

                //string filename = "d:\\encryptedfile\\temporary1.txt";

                //fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
                //fs.Write(filedata, 0, filedata.Length);
                //fs.Close();

                //List<string> fileread = File.ReadAllLines(filename).ToList();
                psuedo.Add(id);
            }

            psuedo.Distinct().ToList();

            comboBox1.Items.Clear();
            foreach (string s in psuedo.Distinct().ToList())
            {
                comboBox1.Items.Add(s);
            }
            groupBox7.Visible = true;
            groupBox6.Visible = false;
            groupBox2.Visible = false;
            groupBox4.Visible = false;
            groupBox5.Visible = false;
            groupBox8.Visible = false;
        }

        private void send_request_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
            groupBox4.Visible = false;
            groupBox7.Visible = false;
            groupBox5.Visible = false;
            groupBox6.Visible = true;
            groupBox8.Visible = false;
        }



        private void psuedonym_Click(object sender, EventArgs e)
        {


            int t = 140;

            Point pseudonym = ta1.PseudonymGen(t, textBox2.Text);
            textBox4.Text = pseudonym.X + "," + pseudonym.Y;

            Point Anonym = ta1.AnonymousKeyGeneration(textBox2.Text);

            textBox3.Text = Anonym.X + "," + Anonym.Y;

        }
        string content = "";
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;
                StreamReader sr = new StreamReader(ofd.FileName);
                content = sr.ReadToEnd();
                richTextBox1.Text = content;
            }
            label26.Text = ofd.SafeFileName;
        }

        private void and_button_Click(object sender, EventArgs e)
        {
            char and = '&';
            count++;
            if (count == 1)
            {
                label15.Text = "level 1";
                checkbox_enable();
                access_tree = access_tree + "&";
                tree(access_tree, count, and);
            }
            else if (count == 2)
            {
                label15.Text = "level 2";
                tree(access_tree, count, and);
            }
            else if (count == 3)
            {
                label15.Text = "level 3";
                tree(access_tree, count, and);
            }
        }

        private void or_button_Click(object sender, EventArgs e)
        {
            char or = '|';
            count++;
            if (count == 1)
            {
                label15.Text = "level 1";
                checkbox_enable();
                access_tree = access_tree + "|";
                tree(access_tree, count, or);
            }
            else if (count == 2)
            {
                label15.Text = "level 2";
                tree(access_tree, count, or);
            }
            else if (count == 3)
            {
                label15.Text = "level 3";
                tree(access_tree, count, or);
            }
        }

        private void or_button_Click_1(object sender, EventArgs e)
        {
            char or = '|';
            count++;
            if (count == 1)
            {
                label15.Text = "level 1";
                checkbox_enable();
                access_tree = access_tree + "|";
                tree(access_tree, count, or);
            }
            else if (count == 2)
            {
                label15.Text = "level 2";
                tree(access_tree, count, or);
            }
            else if (count == 3)
            {
                label15.Text = "level 3";
                tree(access_tree, count, or);
            }
        }
        public void tree(string name, int count, char sy)
        {
            Random r = new Random();
            string pubkey = dbparam.retreive_public_key();
            string[] sub = pubkey.Split('@');
            Point generator = new Point();
            string[] gen = sub[1].Split(',');
            generator.X = Convert.ToInt16(gen[0]);
            generator.Y = Convert.ToInt16(gen[1]);

            if (count == 1)
            {
                h = ta1.hash_beta(generator, 191);
                a = sy;
                n = r.Next(100, 190);
                C = lapoint.milersPoint(h, n, 191);
                access_tree = access_tree + " " + n;
                MessageBox.Show(" access tree  " + access_tree);
            }
            if ((count == 2 && sy == '&') || (count == 2 && sy == '|'))
            {
                if (a == '&')
                {
                    left = r.Next(1, n);
                }
                else if (a == '|')
                {
                    left = n;
                }
                if (sy == '&')
                {
                    sec_left = r.Next(1, left);
                    sec_rite = left - sec_left;
                }
                else if (sy == '|')
                {
                    sec_left = left;
                    sec_rite = left;
                }

                left_tree = checkbox_check();

                left_tree = left_tree.Trim();

                string[] left_tree1 = left_tree.Split((' '));
                left_tree = left_tree1[0] + " " + sec_left + " " + ("" + sy + " " + left) + " " + left_tree1[1] + " " + sec_rite;
                MessageBox.Show("left tree   " + left_tree);
                int[] qy = new int[2];
                qy[0] = sec_left;
                qy[1] = sec_rite;






                int random = 0;

                for (int i = 0; i < 2; i++)
                {

                    bool flag = false;
                    flag = dbop.check_Point(left_tree1[i].ToUpper().Trim());
                    if (flag)
                    {
                        cy_pt = dbop.hash_point(left_tree1[i].ToUpper().Trim());
                        random = dbop.Random_R(left_tree1[i].ToUpper().Trim());
                    }
                    else
                    {
                        dbop.insert(left_tree1[i].ToUpper().Trim(), cy_pt, random);
                        cy_pt = dbop.hash_point(left_tree1[i].ToUpper().Trim());
                        random = dbop.Random_R(left_tree1[i].ToUpper().Trim());
                    }

                    cy_pt = lapoint.milersPoint(cy_pt, qy[i], 191);
                    cy_dash = cy_dash + cy_pt.X + "," + cy_pt.Y;
                    cy_pt = lapoint.milersPoint(generator, qy[i], 191);
                    cy = cy + cy_pt.X + "," + cy_pt.Y;
                    cy = cy + "@";
                    cy_dash = cy_dash + "@";

                }
            }

            if ((count == 3 && sy == '&') || (count == 3 && sy == '|'))
            {
                sec_left = 0;
                sec_rite = 0;
                if (a == '&')
                {
                    rite = n - left;
                }
                else if (a == '|')
                {
                    rite = left;
                }
                if (sy == '&')
                {
                    sec_left = r.Next(1, rite);
                    sec_rite = rite - sec_left;
                }
                else if (sy == '|')
                {
                    sec_left = rite;
                    sec_rite = rite;
                }
                rite_tree = checkbox_check();
                rite_tree = rite_tree.Trim();
                string[] rite_tree1 = rite_tree.Split(' ');
                rite_tree = rite_tree1[0] + " " + sec_left + " " + ("" + sy + " " + rite) + " " + rite_tree1[1] + " " + sec_rite;
                MessageBox.Show("rite tree   " + rite_tree);

                int[] qy = new int[2];
                qy[0] = sec_left;
                qy[1] = sec_rite;



                Point cy_pt = new Point(0, 0);
                for (int i = 0; i < 2; i++)
                {
                    bool flag = false;
                    int random = 0;
                    flag = dbop.check_Point(rite_tree1[i].ToUpper().Trim());
                    if (flag)
                    {
                        cy_pt = dbop.hash_point(rite_tree1[i].ToUpper().Trim());

                    }
                    else
                    {
                        dbop.insert(rite_tree1[i].ToUpper().Trim(), cy_pt, random);
                        cy_pt = dbop.hash_point(rite_tree1[i].ToUpper().Trim());
                    }

                    cy_pt = lapoint.milersPoint(cy_pt, qy[i], 191);
                    cy_dash = cy_dash + cy_pt.X + "," + cy_pt.Y;
                    cy_pt = lapoint.milersPoint(generator, qy[i], 191);
                    cy = cy + cy_pt.X + "," + cy_pt.Y;
                    cy = cy + "@";
                    cy_dash = cy_dash + "@";

                }
            }
        }

        public void checkbox_enable()
        {
            checkBox13.Enabled = true;
            checkBox14.Enabled = true;
            checkBox15.Enabled = true;
            checkBox16.Enabled = true;
            checkBox17.Enabled = true;
            checkBox18.Enabled = true;
            checkBox19.Enabled = true;
            checkBox20.Enabled = true;
            checkBox21.Enabled = true;
            checkBox22.Enabled = true;
            checkBox23.Enabled = true;
            checkBox24.Enabled = true;
        }
        public string checkbox_check()
        {
            string tree = "";
            if (checkBox13.Checked == true && checkBox13.Enabled == true)
            {
                tree = tree + checkBox13.Text + " ";
                checkBox13.Enabled = false;
            }
            if (checkBox14.Checked == true && checkBox14.Enabled == true)
            {
                tree = tree + checkBox14.Text + " ";
                checkBox14.Enabled = false;
            }
            if (checkBox15.Checked == true && checkBox15.Enabled == true)
            {
                tree = tree + checkBox15.Text + " ";
                checkBox15.Enabled = false;
            }
            if (checkBox16.Checked == true && checkBox16.Enabled == true)
            {
                tree = tree + checkBox16.Text + " ";
                checkBox16.Enabled = false;
            }
            if (checkBox17.Checked == true && checkBox17.Enabled == true)
            {
                tree = tree + checkBox17.Text + " ";
                checkBox17.Enabled = false;
            }
            if (checkBox18.Checked == true && checkBox18.Enabled == true)
            {
                tree = tree + checkBox18.Text + " ";
                checkBox18.Enabled = false;
            }
            if (checkBox19.Checked == true && checkBox19.Enabled == true)
            {
                tree = tree + checkBox19.Text + " ";
                checkBox19.Enabled = false;
            }
            if (checkBox20.Checked == true && checkBox20.Enabled == true)
            {
                tree = tree + checkBox20.Text + " ";
                checkBox20.Enabled = false;
            }
            if (checkBox21.Checked == true && checkBox21.Enabled == true)
            {
                tree = tree + checkBox21.Text + " ";
                checkBox21.Enabled = false;
            }
            if (checkBox22.Checked == true && checkBox22.Enabled == true)
            {
                tree = tree + checkBox22.Text + " ";
                checkBox22.Enabled = false;
            }
            if (checkBox23.Checked == true && checkBox23.Enabled == true)
            {
                tree = tree + checkBox23.Text + " ";
                checkBox23.Enabled = false;
            }
            if (checkBox24.Checked == true && checkBox24.Enabled == true)
            {
                tree = tree + checkBox24.Text + " ";
                checkBox24.Enabled = false;
            }
            return tree;
        }

        private void send_request_Click_1(object sender, EventArgs e)
        {
            List<string> secretKeyFile = File.ReadAllLines(textBox13.Text).ToList();
            List<Point> secPoints = new List<Point>();

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



            label15.Text = "";
            groupBox2.Visible = false;
            groupBox4.Visible = false;
            int count = 0;




            int t = 4;


            string dataownerid = comboBox1.SelectedItem.ToString();



            Point Anonymous_key = ta1.AnonymousKeyGeneration(dataownerid);// new Point(Convert.ToInt16(dataownerid[0]), Convert.ToInt16(dataownerid[1])); //ta1.AnonymousKeyGeneration(dataownerid);
            Anonymous_key = ta1.hash_beta_t(Anonymous_key, t, 191);
            Point hash_pt = new Point(0, 0);
            List<Point> grp_1 = new List<Point>();


            string abc = "";
            int main = 0;
            Point r = new Point(0, 0);
            //                dr.Close();
            for (int i = 0; i < secPoints.Count; i++)
            {


                main = lapoint.div(Anonymous_key, secPoints[i], grp_1, 191);
                abc = abc + main + "@";

            }
            label15.Text = abc;
            MessageBox.Show("your data query is " + label15.Text);

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
        private void assign_Click(object sender, EventArgs e)
        {
            access_tree = left_tree + "  " + access_tree + "  " + rite_tree;
            MessageBox.Show("access tree  " + access_tree);

            scrambled_access_tree = attribute_scramble(access_tree);
            MessageBox.Show("Scrambled access tree  " + scrambled_access_tree);

            //MessageBox.Show("Cy dash = " + cy_dash);
            assign.Visible = false;
        }
        public string attribute_scramble(string access_tree)
        {
            MessageBox.Show("inside");
            int t = 4;

            Point r = new Point(0, 0);
            int main = 0;
            Point Anonymous_key = ta1.AnonymousKeyGeneration(textBox2.Text);

            //  con =dbparam.connection();
            List<Point> grp_1 = new List<Point>();

            string pub = dbparam.retreive_public_key();

            string[] sub = pub.Split('@');
            string[] suba = sub[0].Split(' ');
            foreach (string sp in suba)
            {

                string[] subarr = sp.Split(',');

                Point grp = new Point(Convert.ToInt16(subarr[0]), Convert.ToInt16(subarr[1]));
                grp_1.Add(grp);
            }


            int i = 0;
            string attri = "";
            Point hash_pt = new Point(0, 0);
            Anonymous_key = ta1.hash_beta_t(Anonymous_key, t, 191);

            try
            {
                int ra = 0;
                char[] access = access_tree.ToCharArray();
                for (i = 0; i < access.Length; i++)
                {
                    if (((int)(access[i]) >= 65 && (int)(access[i]) <= 90) || ((int)(access[i]) >= 97 && (int)(access[i]) <= 122))
                    {

                        attri = attri + access[i];
                    }
                    else if (attri.Length > 0)
                    {
                        bool flag = false;
                        flag = dbop.check_Point(attri.ToUpper().Trim());

                        if (flag)
                        {
                            hash_pt = dbop.hash_point(attri.ToUpper().Trim());
                        }
                        else
                        {
                            hash_pt = new Point();
                            dbop.insert(attri.ToUpper().Trim(), hash_pt, ra);
                            hash_pt = dbop.hash_point(attri.ToUpper().Trim());
                        }
                        hash_pt = ta1.hash_beta(hash_pt, 191);
                        main = lapoint.div(Anonymous_key, hash_pt, grp_1, 191);



                        scrambled_access_tree = scrambled_access_tree + main.ToString() + " " + "@" + access[i];
                        attri = "";


                    }
                    else
                    {
                        scrambled_access_tree = scrambled_access_tree + access[i].ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return scrambled_access_tree;
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

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
        private void button3_Click(object sender, EventArgs e)
        {

            button4.Visible = true;
            int j = 0;

            string w_st = Convert.ToString(127, 2).PadLeft(8, '0');
            string message = richTextBox1.Text;
            char[] mess = message.ToCharArray();
            string result = "";
            foreach (char i in mess)
            {
                string m_st = Convert.ToString((char)i, 2).PadLeft(8, '0');
                result = XOR(w_st, m_st);//performs XOR function  
                long a = Convert.ToInt64(result, 2);
                char c = (char)a;
                final_ans = final_ans + c;
            }

            richTextBox2.Text = scrambled_access_tree + "\n" + C.X + "," + C.Y + "\n" + textBox3.Text + "\n" + cy + "\n" + cy_dash + "\n" + final_ans;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox6.Text == "")
            {
                MessageBox.Show("please enter the filename to save");
            }
            try
            {

                string path = "D:\\encryptedfile\\" + textBox6.Text.Trim() + label9.Text;
                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);

                List<string> writing = new List<string>();
                //writing.Add(richTextBox2.Text);
                writing.Add(scrambled_access_tree);
                writing.Add(C.X + "," + C.Y);
                writing.Add(textBox3.Text);
                writing.Add(cy);
                writing.Add(cy_dash);
                writing.Add(final_ans);
                foreach (string s in writing)
                {
                    sw.WriteLine(s);
                }




                sw.Close();
                fs.Close();
                byte[] filedata = readFile(path);
                du.storeEncryptedFile(filedata, userid, path, (textBox6.Text.Trim() + label9.Text));
                MessageBox.Show("File Uploaded successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void search_query_Click(object sender, EventArgs e)
        {


            textBox17.Text = label15.Text;

            groupBox7.Visible = false;
            groupBox6.Visible = false;
            groupBox2.Visible = false;
            groupBox4.Visible = false;
            groupBox5.Visible = false;
            groupBox6.Visible = true;
            groupBox8.Visible = false;
        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {


        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void submit_key_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.ForeColor = Color.Black;
                DataSet ds = new DataSet();
                ds = du.retreiveFile();
                DataSet ds1 = new DataSet();
                ds1 = du.retreiveFile();
                dt = ds1.Tables["userFileEncrypted"].Clone();
                dt.Columns.Remove("file_data");
                dt.Columns.Remove("file_path");
                dt.Columns.Remove("login_id");



                string abc = textBox17.Text.Trim();


                int no_of_rows = ds.Tables["userFileEncrypted"].Rows.Count;
                for (int i = 0; i < no_of_rows; i++)
                {

                    string name = (string)ds.Tables["userFileEncrypted"].Rows[i]["file_name"];
                    byte[] filedata = (byte[])ds.Tables["userFileEncrypted"].Rows[i]["file_data"];

                    string filename = "d:\\encryptedfile\\temporary.txt";

                    fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
                    fs.Write(filedata, 0, filedata.Length);
                    fs.Close();

                    List<string> fileread = File.ReadAllLines(filename).ToList();
                    //MessageBox.Show("first line      " + fileread[0]);
                    string key = fileread[0].ToString();


                    bool flag1 = lapoint.match_key(key, abc);

                    //MessageBox.Show("flag       " + flag1);
                    if (flag1)
                    {
                        dt.ImportRow(ds.Tables["userFileEncrypted"].Rows[i]);

                    }

                }
                dataGridView1.DataSource = dt;
            }
            catch (DataException ex)
            {
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }


        private void button5_Click(object sender, EventArgs e)
        {



            DataGridViewSelectedCellCollection dgv = this.dataGridView1.SelectedCells;
            int index = Convert.ToInt16(dgv[0].Value);
            string filename = "D:\\filenam.txt";
            FileStream fs;//new FileStream(filename, FileMode.Create, FileAccess.Write);
            List<string> fileread = new List<string>();
            int dsnumber = ds.Tables["userFileEncrypted"].Rows.Count;
            for (int i = 0; i < dsnumber; i++)
            {
                if (index == Convert.ToInt16(ds.Tables["userFileEncrypted"].Rows[i]["file_id"]))
                {
                    byte[] filedata = (byte[])ds.Tables["userFileEncrypted"].Rows[i]["file_data"];
                    fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
                    fs.Write(filedata, 0, filedata.Length);
                    fs.Close();

                    fileread = File.ReadAllLines(filename).ToList();
                    break;
                }
            }

            Form3 fs1 = new Form3(fileread);
            fs1.Show();
            //int nom=




        }

        private void button7_Click(object sender, EventArgs e)
        {
            string mess = ta1.request(userid);

            if (mess == "request being sent again " && !ta1.diffe(userid))
            {
                groupBox7.Visible = false;
                groupBox6.Visible = false;

                groupBox2.Visible = false;
                groupBox4.Visible = false;
                groupBox5.Visible = false;
                groupBox8.Visible = true;
                richTextBox3.Text = "";
                richTextBox4.Text = "";
            }
            else
            {
                button12.Visible = false;
                label11.Visible = false;
                textBox7.Visible = false;
                button8.Visible = false;
                label14.Visible = false;
                textBox10.Visible = false;
                button10.Visible = false;
                label16.Visible = false;
                textBox11.Visible = false;
                button11.Visible = false;
                groupBox7.Visible = false;
                groupBox6.Visible = false;

                groupBox2.Visible = false;
                groupBox4.Visible = false;
                groupBox5.Visible = true;
                groupBox8.Visible = false;
                button7.Visible = false;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            groupBox7.Visible = false;
            groupBox6.Visible = false;
            //groupBox2.Visible = true;
            groupBox4.Visible = false;
            groupBox5.Visible = false;
            groupBox8.Visible = true;
            button7.Visible = true;
        }

        private void button16_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            int a = Convert.ToInt16(textBox9.Text.Trim());
            label11.Visible = true;
            textBox7.Visible = true;
            button8.Visible = true;
            Point gen_a = lapoint.milersPoint(new Point(0, 190), a, 191);
            textBox7.Text = gen_a.X + "," + gen_a.Y;
        }

        private void button8_Click(object sender, EventArgs e)
        {

            string[] abc = textBox7.Text.Trim().Split(',');
            ta1.storeValue(userid, new Point(Convert.ToInt16(abc[0]), Convert.ToInt16(abc[1])));


            MessageBox.Show("your code has been sent to server");
            label14.Visible = true;
            textBox10.Visible = true;
            button10.Visible = true;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox10.Text = ta1.retreiveServer(userid);
            if (textBox10.Text != "server still calculating")
            {
                label16.Visible = true;
                textBox11.Visible = true;
                button11.Visible = true;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string[] server = textBox10.Text.Trim().Split(',');
            string[] user = textBox7.Text.Trim().Split(',');
            Point u = new Point(Convert.ToInt16(user[0]), Convert.ToInt16(user[1]));
            Point s = new Point(Convert.ToInt16(server[0]), Convert.ToInt16(server[1]));
            List<Point> gr = new List<Point>();
            int weil = lapoint.div(u, s, gr, 191);
            textBox11.Text = weil + "";
            button12.Visible = true;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            OpenFileDialog odb = new OpenFileDialog();
            if (odb.ShowDialog() == DialogResult.OK)
            {
                textBox13.Text = odb.FileName.ToString();
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            List<string> secen = ta1.getSecretKey(userid);
            string s = "";
            foreach (string sec in secen)
            {

                s = s + sec + "\n";

            }

            richTextBox3.Text = s;

        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                string final_ans = "";
                string sec = richTextBox3.Text;
                string[] secretKey = sec.Split('\n');
                int key = Convert.ToInt16(textBox12.Text);

                string Key_SK = Convert.ToString(key, 2).PadLeft(8, '0');
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
                        long abc = Convert.ToInt64(result, 2);
                        char c = (char)abc;
                        final_ans = final_ans + c;
                        // MessageBox.Show("final ans      " + final_ans);
                    }
                    SK = SK + final_ans + "\n";
                    final_ans = "";
                    result = "";

                }
                richTextBox4.Text = SK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("error " + ex.Message);
            }


            // int key = (textBox12.Text.Trim());
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                string[] abc = richTextBox4.Text.Split('\n');
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

    }
}
