using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
namespace TrustedAuthorityServer
{
    class TAServiceOperation:TAServices.TAOPERATIONS,TAServices.group_gen
    {
        List<int> Zprimes = new List<int>();
        List<Point> coordinates = new List<Point>();
        List<Point> group_1 = new List<Point>();
        List<Point> generator_T = new List<Point>();
        SqlConnection con = new SqlConnection();
        int weilpairing = 0;

        database.databaseoperations dbop;
        database.public_masterKey dbpk;
        lambda_pointAddition.lam_Point_add point_obj1;

        ChannelFactory<database.databaseoperations> channel1 = new ChannelFactory<database.databaseoperations>("random_oracle");
        ChannelFactory<database.public_masterKey> channel2 = new ChannelFactory<database.public_masterKey>("publickey");
        ChannelFactory<lambda_pointAddition.lam_Point_add> channel3 = new ChannelFactory<lambda_pointAddition.lam_Point_add>("pointadd");
   
        public SqlConnection connection()
        {
            //OperationContext.Current.Channel.Close();
            try
            {
                con.ConnectionString = "Data Source = DHAVAL;Initial Catalog = TADatabase; Integrated Security=True;MultipleActiveResultSets=true";
                con.Open();
                Console.WriteLine("succesful");
            }
            catch (Exception e)
            {
                Console.WriteLine("unsuccesful");
                Console.WriteLine(e.Message);
            }
            return con;
        }

        public List<Object> retreiveMk()
        {
            List<Object> MK=new List<Object>();
            con = connection();
            string query = "select * from MK";
            int beta = 0, alpha = 0;
            Point gen_alpha=new Point();
            string gen = "";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                beta = (int)dr[0];
                gen = (string)dr[1];
                alpha = (int)dr[2];

            }
            string[] geni = gen.Split(',');
            gen_alpha.X = Convert.ToInt32(geni[0]);
            gen_alpha.Y = Convert.ToInt32(geni[1]);
            MK.Add(beta);
            MK.Add(gen_alpha);
            MK.Add(alpha);
            return MK;
        }
        public void insert_into_database_MK(int beta,int alpha, Point g_alpha)
        {
            con = connection();
            string g = "";
            int f = 0; ;
            int alph = alpha;
            try
            {
                string delete = "delete from MK";
                SqlCommand cmd1 = new SqlCommand(delete, con);
                cmd1.ExecuteNonQuery();
               // MessageBox.Show("deleted mk");
                f = beta;
                g = g_alpha.X + "," + g_alpha.Y;

                string inst = "insert into MK values(@beta,@g_alpha,@alpha)";
                SqlCommand cm1 = new SqlCommand(inst, con);
                cm1.Parameters.Add(new SqlParameter("@beta", f));
                cm1.Parameters.Add(new SqlParameter("@g_alpha", g));
                cm1.Parameters.Add(new SqlParameter("@alpha", alph));
                cm1.ExecuteNonQuery();
               // MessageBox.Show("deleted and inserted mk");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public Point AnonymousKeyGeneration(string dataOwnerId)
        {
            dbop = channel1.CreateChannel();
            point_obj1 = channel3.CreateChannel();
            
            int alpha = 0;
            int beta = 0;
            Point p = new Point();
            List<Object> mk = retreiveMk();
            beta = (int)mk[0];

            Point hash = dbop.hash_point(dataOwnerId);
            Point hash_beta = point_obj1.milersPoint(hash, beta, 191);
            return hash_beta;
        }

        public void SystemSetup()
        {
            dbpk = channel2.CreateChannel();
            dbop = channel1.CreateChannel();
            point_obj1 = channel3.CreateChannel();
            int primeNumber = 191;
            int alpha = 130;
            int beta = 160;
            int lambda=0;
            Point generator=new Point(0,190);
            for (int i = 1; i < primeNumber; i++)
            {
                Zprimes.Add(i);
            }
            coordinates = generate_mulgroup(primeNumber, Zprimes);

            group_1 = GroupGen(coordinates, primeNumber, lambda, group_1,generator);
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

            //MessageBox.Show("\n\n\n group 1 count : " + group_1.Count + "\n\n GT : " + generator_T.Count + "\n\n ecc count : " + coordinates.Count + "\n\n\nprime " + primeNumber + "   \n \nzprime " + s1 + "\n \n ecccordinat" + s3 + "\n \n \n\ngroup 1 " + s);
            //MessageBox.Show("generator    " + generator);
            //MessageBox.Show("h......" + H);
            //MessageBox.Show("weil      ....  " + weilpairing);


            foreach (Point i in generator_T)
            {
                s2 = s2 + "  {" + i + "}   ";
            }
            //MessageBox.Show("generator_T : " + s2);
          //  dbpk.insert_into_database_PK(group_1, generator, H, weilpairing);
            insert_into_database_MK(beta, alpha, gen_alpha);
            dbpk.insert_into_database_GT(group_1);

        }

        public List<Point> generate_mulgroup(int primeNumber, List<int> Group_1)
        {
            
            List<Point> cor = new List<Point>();
            int x, y1, y2;
            for (int l = 0; l < primeNumber; l++)
            {
                x = (int)(Math.Pow(l, 3) + 0 + 1) % primeNumber;
                for (int k = 0; k < Group_1.Count; k++)
                {
                    y1 = (int)(Math.Pow(Group_1[k], 2) % primeNumber);
                    if (x == y1)
                    {
                        cor.Add(new Point(l, Group_1[k]));
                        y2 = primeNumber - Group_1[k];
                        cor.Add(new Point(l, y2));
                    }
                }
            }
            cor = cor.Distinct().ToList();
            return cor;
        }

        public List<Point> group_t(List<Point> group_1, int primeNumber)
        {
            int[] a = new int[4];
            int q = 0, p = 0;
            string s4 = "";
            List<Point> newg = new List<Point>();
            for (int i = 0; i < group_1.Count - 1; i++)
            {

                for (int j = 0; j < group_1.Count - 1; j++)
                {

                    a[0] = ((group_1[i].X) * (group_1[j].X)) % primeNumber;
                    a[1] = ((group_1[i].X) * (group_1[j].Y)) % primeNumber;
                    a[2] = ((group_1[i].Y) * (group_1[j].X)) % primeNumber;
                    a[3] = ((group_1[i].Y) * (group_1[j].Y)) % primeNumber;
                    // MessageBox.Show("" + a[0] + "    " + a[1] + "    " + a[2] + "    " + a[3]);

                    for (p = 0; p < 4; p++)
                    {

                        newg.Add(new Point(a[p], a[q]));
                        q++;
                        newg.Add(new Point(a[p], a[q]));
                        q++;
                        newg.Add(new Point(a[p], a[q]));
                        q++;
                        newg.Add(new Point(a[p], a[q]));
                        q = 0;
                    }
                }
            }

            newg = newg.Distinct().ToList();
            return newg;
        }

        public List<Point> GroupGen(List<Point> ecc_coordinates, int primeNumber, int lamda, List<Point> group_1, Point P)
        {

            int denomenator = 0;
            int flag = 1;

            int n = 2;
            int z = 2;

            Point p = P;
            Point q = P;


            for (int i = 0; i < ecc_coordinates.Count; i++)
            {
                // flag = 1;
                for (int j = 0; j < ecc_coordinates.Count; j++)
                {
                    List<Point> h = new List<Point>();
                    lamda = point_obj1.lam(p, q, primeNumber, out denomenator);
                    if (denomenator == 0)
                    {
                        flag = 0;
                        break;
                    }
                    Point r = new Point(0, 0);
                    r = point_obj1.generate_group(p, q, primeNumber, lamda);
                    //   MessageBox.Show("denomenator  " + denomenator + " lambda  >..............." + lamda+"    third point   "+r);

                    group_1.Add(r);
                    q = r;


                }

                if (flag == 0)
                {
                    break;
                }

            }
            group_1.Add(new Point(0, 0));
            return group_1;
        }


        public void encryptSecretKey(System.IO.FileStream fs)
        {
            throw new NotImplementedException();
        }

       // public void stroeFILE(string loginid,string attr,)
        public void generateSecretKey(string loginID, string attributeSets)
        {
            dbpk = channel2.CreateChannel();
            dbop = channel1.CreateChannel();
            point_obj1 = channel3.CreateChannel();
            List<string> secretkey = new List<string>();
            int ri = 0;

            Random ran = new Random();
            ri= ran.Next(1, 190);
            MessageBox.Show("" + ri);


            List<Object> mk = retreiveMk();
            int alpha = (int)mk[2];
            int beta = (int)mk[0];
            int exponent = alpha + ri;

            string gen= dbpk.retreive_public_key();
            string[] gen_sub = gen.Split('@');
            string []gen__sub=gen_sub[1].Split(',');
            Point generator = new Point(0, 190);//new Point(Convert.ToInt16(gen__sub[0]), Convert.ToInt16(gen__sub[1]));
            int r = 0, Rp = 0;
            Random rnd = new Random();

            int random = 0;
            r= rnd.Next(1, 191 - 1);

            secretkey.Add(""+ri);
            //secret keys first part

            Point D = point_obj1.milersPoint(generator, exponent, 191);

          // secretkey.Add(loginID);

            secretkey.Add("" + D.X + "," + D.Y);
            string[] attributes= attributeSets.Split(',');




            for(int attr=0;attr<attributes.Length;attr++)
            {
                if (attributes[attr] != "")
                {
                    MessageBox.Show(attributes[attr]);
                    int denomenator = 0;
                    Point hash = new Point();
                    int random_r = 0;
                    bool flag = dbop.check_Point(attributes[attr]);

                    if (flag)
                    {
                        hash = dbop.hash_point(attributes[attr].ToUpper());
                        random_r = dbop.Random_R(attributes[attr].ToUpper());
                    }
                    else
                    {
                        dbop.insert(attributes[attr].ToUpper(), hash, random_r);
                        hash = dbop.hash_point(attributes[attr].ToUpper());
                        random_r = dbop.Random_R(attributes[attr].ToUpper());
                    }


                    Point hash_Rj = point_obj1.milersPoint(hash, random_r, 191);
                    Point gen_r = point_obj1.milersPoint(generator, ri, 191);

                    int lamda = point_obj1.lam(gen_r, hash_Rj, 191, out denomenator);
                    Point Dj = point_obj1.generate_group(gen_r, hash_Rj, 191, lamda);

                    Point Dj_rj = point_obj1.milersPoint(generator, random_r, 191);

                    Point Dj_n = point_obj1.milersPoint(hash, beta, 191);

                    string s = /*gen_r.X + "," + gen_r.Y + " "+*/Dj.X + "," + Dj.Y + " " + Dj_rj.X + "," + Dj_rj.Y + " " + Dj_n.X + "," + Dj_n.Y;
                    secretkey.Add(s);
                }
            }










            //Dictionary<int, string> Attributes_numbers = new Dictionary<int, string>();

           

            //for (int i = 0; i < attributes.Length; i++)
            //{
            //    int rn = rnd.Next(1, 190);
            //    foreach (KeyValuePair<int, string> p in Attributes_numbers)
            //    {
            //        if (p.Key == rn)
            //        {
            //        l: int nd = rnd.Next(1,190);
            //            if (nd != rn)
            //            {
            //                rn = nd;
            //            }
            //            else if (nd == rn)
            //            {
            //                goto l;
            //            }
            //        }

            //    }
            //    Attributes_numbers.Add(rn, attributes[i].ToUpper());
            //}
           
            //foreach (KeyValuePair<int, string> p in Attributes_numbers)
            ////{
            //    bool flag = dbop.check_Point(p.Value.ToString().ToUpper());
            //    if (flag)
            //    {
            //        hash = dbop.hash_point(p.Value.ToUpper());
            //    }
            //    else
            //    {
            //        dbop.insert(p.Value.ToUpper(),hash);
            //        hash = dbop.hash_point(p.Value.ToUpper());
            //    }
            //    Point hash_Rj = point_obj1.milersPoint(hash, p.Key, 191);
            //    Point gen_r = point_obj1.milersPoint(generator, r, 191);

            //    int lamda = point_obj1.lam(gen_r, hash_Rj, 191, out denomenator);
            //    Point Dj = point_obj1.generate_group(gen_r, hash_Rj, 191, lamda);

            //    Point Dj_rj = point_obj1.milersPoint(generator, p.Key, 191);

            //    Point Dj_n = point_obj1.milersPoint(hash, beta, 191);

            //    string s = Dj.X + "," + Dj.Y + " " + Dj_rj.X + "," + Dj_rj.Y + " " + Dj_n.X + "," + Dj_n.Y;
            //    secretkey.Add(s);

            //}
            string filename = "d:\\taFiles\\" + loginID + ".txt";
            FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            foreach (string s in secretkey)
            {
                MessageBox.Show("secret kay = " + s);
                sw.WriteLine(s);
            }

            sw.Close();
            fs.Close();

            byte[] filedata = readFile(filename);


            con = connection();
            string insertquery = "insert into secretKeyFile values(@loginid,@attributesets,@filedata)";
            SqlCommand cmd = new SqlCommand(insertquery, con);
            cmd.Parameters.Add(new SqlParameter("@loginid", loginID));
            cmd.Parameters.Add(new SqlParameter("@attributesets", attributeSets));
            cmd.Parameters.Add(new SqlParameter("@filedata", (object)filedata));


            cmd.ExecuteNonQuery();



            string sp = "";
            foreach (string din in secretkey)
            {
                sp = sp + din + "\n";
            }


            MessageBox.Show(sp);
        }

       


        public Point PseudonymGen(int t, string dataOwnerId)
        {
            int dataowner_t = t;
            Point p = new Point();
             Point hash=new Point();
            dbpk = channel2.CreateChannel();
            dbop = channel1.CreateChannel();
            point_obj1 = channel3.CreateChannel();
            bool flag = false;
            int random = 0;
            flag = dbop.check_Point(dataOwnerId);
            if (flag)
            {
                hash = dbop.hash_point(dataOwnerId);
            }
            else
            {
                hash = new Point();
                dbop.insert(dataOwnerId,hash,random);
                hash = dbop.hash_point(dataOwnerId);
            }
            Point hash_beta = point_obj1.milersPoint(hash, dataowner_t, 191);
            return hash_beta;
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


        public Point hash_beta(Point gen, int prime)
        {
            point_obj1 = channel3.CreateChannel();
            List<object> mk = retreiveMk();
            int beta = (int)mk[0];
            Point hash = point_obj1.milersPoint(gen, beta, prime);

            return hash;
        }
        public Point hash_beta_t(Point gen,int t, int prime)
        {
            point_obj1 = channel3.CreateChannel();
            List<object> mk = retreiveMk();
            //int beta = (int)mk[0];
            int beta_t = t;
            Point hash = point_obj1.milersPoint(gen, beta_t, prime);

            return hash;
        }


        public void request(string id)
        {
            MessageBox.Show("request came from " + id);
            try
            {
                con = connection();
                string query = "insert into diffiehellman values(@id,0,0,0,0)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.Add(new SqlParameter("id",id));
                cmd.ExecuteNonQuery();





            }
            catch (Exception ex)
            {
                MessageBox.Show("request being sent again ");
            }









        }


        public string check(string id)
        {
            string userserver = "";
            con = connection();
            string select = "select * from diffiehellman where loginId=@id";
            bool flag1 = false,flag2=false;
            SqlCommand cmd = new SqlCommand(select, con);
            cmd.Parameters.Add(new SqlParameter("@id", id));
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (Convert.ToInt16(dr[1]) == 0 && Convert.ToInt16(dr[2]) == 0)
                {
                    flag1 = true;
                    break;
                }
                else if (Convert.ToInt16(dr[3]) == 0 && Convert.ToInt16(dr[4]) == 0)
                {
                    flag2= true;
                    break;
                }

            }
            if (flag1)
            {
                userserver = "user is still sending";


            }
           

            if (flag2)
            {
                userserver = "server is still sending";
            }


            return userserver;

        }

        public void storeValue(string id, Point Z)
        {
            con = connection();
            


            string query = "update diffiehellman set userX=@X,userY=@y where loginId=@id";




            SqlCommand cmd1 = new SqlCommand(query, con);

            cmd1.Parameters.Add(new SqlParameter("@X", Z.X));
            cmd1.Parameters.Add(new SqlParameter("@Y", Z.Y));
            cmd1.Parameters.Add(new SqlParameter("@id", id));
            cmd1.ExecuteNonQuery();
            MessageBox.Show("user value " + Z);
          


        }
        public void updateValues(string id, Point Z)
        {
            con = connection();
            


            string query = "update diffiehellman set serverX=@X,serverY=@y where loginId=@id";

           
         

                SqlCommand cmd1 = new SqlCommand(query, con);

                cmd1.Parameters.Add(new SqlParameter("@X",Z.X));
                cmd1.Parameters.Add(new SqlParameter("@Y",Z.Y));
                cmd1.Parameters.Add(new SqlParameter("@id",id));
                cmd1.ExecuteNonQuery();

                MessageBox.Show("server value " + Z);



        }


        public string retreiveServer(string id)
        {
            string abcd = "";
            
                con = connection();
            string select = "select * from diffiehellman where loginId=@id";
         
            SqlCommand cmd = new SqlCommand(select, con);
            cmd.Parameters.Add(new SqlParameter("@id", id));
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {

            if (Convert.ToInt16(dr[3]) == 0 && Convert.ToInt16(dr[4]) == 0)
            {
                abcd = "server still calculating";
            }
            else
            {
               
                    abcd = dr[3].ToString() + "," + dr[4].ToString();
                }
            }
            return abcd;
        }

        public string retreiveUser(string id)
        {
            string abcd = "";

            con = connection();
            string select = "select * from diffiehellman where loginId=@id";

            SqlCommand cmd = new SqlCommand(select, con);
            cmd.Parameters.Add(new SqlParameter("@id", id));
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
            if (Convert.ToInt16(dr[1]) == 0 && Convert.ToInt16(dr[2]) == 0)
            {
                abcd = "user still calculating";
            }
            else
            {
               

                    abcd = dr[1].ToString() + "," + dr[2].ToString();
                }
            }
            return abcd;
        }


        public string retreivBoth(string id)
        {
            string abcd = "";
            con = connection();
            string select = "select * from diffiehellman where loginId=@id";

            SqlCommand cmd = new SqlCommand(select, con);
            cmd.Parameters.Add(new SqlParameter("@id", id));
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                abcd = dr[1].ToString() + "," + dr[2].ToString() + " " + dr[3].ToString() + "," + dr[4].ToString();
            }
            return abcd;
        }


        public bool checkvalue(string id, int i, int j)
        {
            string abcd = "";
            con = connection();
            string select = "select * from diffiehellman where loginId=@id";
            bool flag = false;
            SqlCommand cmd = new SqlCommand(select, con);
            cmd.Parameters.Add(new SqlParameter("@id", id));
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (Convert.ToInt16(dr[i]) == 0 && Convert.ToInt16(dr[j]) == 0)
                {
                    flag = true;
                    break;
                }
                else if (Convert.ToInt16(dr[i]) != 0 && Convert.ToInt16(dr[j]) != 0)
                {
                    flag = false;
                    break;
                }
               

            }
            return flag;
        }


      public  List<string> getSecretKey(string id)
        {
            con = connection();
            List<string> encrypted = new List<string>();
            try
            {
                string query = "select * from enccryptedSk where loginid=@id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.Add(new SqlParameter("@id",id));
                SqlDataReader dr = cmd.ExecuteReader();
                string filename="d:\\encryptsecretkey.txt";
                FileStream fs=new FileStream(filename,FileMode.Create,FileAccess.Write);
                while (dr.Read())
                {
                    byte[] data = (byte[])(dr[1]);
                    fs.Write(data, 0, data.Length);
                    fs.Close();

                }

                encrypted = File.ReadAllLines(filename).ToList();

            }
            catch (Exception ex)
            {
                MessageBox.Show("no data exist " + ex.Message);
            }
            return encrypted;
        }
    }
}