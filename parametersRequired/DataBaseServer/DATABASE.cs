using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace DataBaseServer
{
    class DATABASE:database.databaseoperations
    {
        publicKey pk = new publicKey();
        
        List<Point> group_1 = new List<Point>();
        List<Point> group1 = new List<Point>();
        SqlConnection con;
        bool flag = false;
        public SqlConnection getcon()
        {
            con = new SqlConnection();
            string mess = "";
            try
            {
                con.ConnectionString = "Data Source = DHAVAL;Initial Catalog = random_oracle; Integrated Security=True;MultipleActiveResultSets=true";
                con.Open();
                mess = "succesful";
            }
            catch (Exception e)
            {
                mess = "un    " + e.Message;
            }
            return con;
        }




        //retreives the hash of the attribute

        public Point hash_point(string input)
        {
            int x = 0, y = 0;
            //Point hash = new Point(0,0);
            con = getcon();
            SqlCommand cmd = new SqlCommand("select * from hash_attributes where attribute = @attr", con);
            cmd.Parameters.Add(new SqlParameter("@attr", input));
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                x = (int)(dr[1]);
                y = (int)(dr[2]);
            }
            //hash.X = x;
            //hash.Y = y;
            Point hash = new Point(x, y);
            dr.Close();
            return hash; 
        }

        public int  Random_R(string input)
        {
            int x = 0;
            //Point hash = new Point(0,0);
            con = getcon();
            SqlCommand cmd = new SqlCommand("select * from hash_attributes where attribute = @attr", con);
            cmd.Parameters.Add(new SqlParameter("@attr", input));
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                x = (int)(dr[3]);
                
            }

            dr.Close();
            return x;
        }

        public List<int> getRandom()
        {
            List<int> Zp = new List<int>();
            for (int i = 1; i <= 190; i++)
            {
                Zp.Add(i);
            }
            return Zp;
        }
        public void groupCreate()
        {
            group_1.Clear();
          
            string grp = pk.retreive_public_key();
            string[] pksub = grp.Split('@');
            string[] groupsub = pksub[0].Split(' ');
            string group = "";

            foreach (string p in groupsub)
            {
                string[] point = p.Split(',');
                int x = Convert.ToInt32(point[0]);
                int y = Convert.ToInt32(point[1]);
                group_1.Add(new Point(x, y));

            }
            group1 = group_1.ToList();

        }
        public List<string> retrieve_attribute()//retriving only attributes...called from form1
        {
            con = getcon();
            List<string> retrieve_attribute = new List<string>();
            string retrieve = "select attribute from hash_attributes";
            SqlCommand cmd = new SqlCommand(retrieve, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                retrieve_attribute.Add((string)dr[0]);
            }
            dr.Close();
            return retrieve_attribute;
        }

        public void Update_attr_points(string attribute, Point attr_point,int r)//updating values of attributes and points
        {
            con = getcon();
            string update = "update hash_attributes set point_X=@X , point_Y=@Y,random_r=@r where attribute= @attr";
            SqlCommand cmd = new SqlCommand(update, con);
            cmd.Parameters.Add(new SqlParameter("@attr", attribute));
            cmd.Parameters.Add(new SqlParameter("@X", attr_point.X));
            cmd.Parameters.Add(new SqlParameter("@Y", attr_point.Y));
            cmd.Parameters.Add(new SqlParameter("@r", r));
            cmd.ExecuteNonQuery();
        }
        public Boolean check_Point(string Attribute)
        {
            con = getcon();
            Point p = new Point();
            string retrieve = "select * from hash_attributes";
            SqlCommand cmd1 = new SqlCommand(retrieve, con);
            SqlDataReader dr = cmd1.ExecuteReader();
            while (dr.Read())
            {
                if (Attribute.ToUpper() == dr[0].ToString().ToUpper())//cheching attributes if already present in db or not
                {
                    // MessageBox.Show("same attributes  and points are " + dr[1] + "  " + dr[2]);
                    flag = true;
                    break;
                }
                else
                {
                    flag = false;
                }
            }
            return flag;
        }

        

        List<Point> points = new List<Point>();


        public void insert(string ip, Point w,int random)//inserting attributes and points
        {
            points.Clear();
            groupCreate();
            List<int> ZP = getRandom();
            Random r = new Random();
            points = retreivepoints();
            List<int> Zp1 = retreiveRandom();
          //  MessageBox.Show("after" + group_1.Count+"integers before  "+Zp1.Count);
            foreach (Point p in points)
            {
                group_1.Remove(p);
            }
            foreach (int i in Zp1)
            {
                ZP.Remove(i);
            }
           // MessageBox.Show("after" + group_1.Count + "integers after " + Zp1.Count);
            bool flag = false;
           random = 0;
            flag = check_Point(ip);
            if (!flag)
            {
                int index = r.Next(0, group_1.Count);
                w = group_1[index];
                int index2 = r.Next(0, ZP.Count);
                random = ZP[index2];
                SqlCommand cmd = new SqlCommand("insert into hash_attributes values(@input,@x,@y,@Zp)", con);
                cmd.Parameters.Add(new SqlParameter("@input", ip));
                cmd.Parameters.Add(new SqlParameter("@x", w.X));
                cmd.Parameters.Add(new SqlParameter("@y", w.Y));
                        cmd.Parameters.Add(new SqlParameter("@Zp",random));
                cmd.ExecuteNonQuery();
            }
        }

        public List<Point> retreivepoints()
        {
            con = getcon();
            List<string> retrieve_attribute = new List<string>();
            string retrieve = "select * from hash_attributes";
            SqlCommand cmd = new SqlCommand(retrieve, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                points.Add(new Point((int)dr[1], (int)dr[2]));
            }
            dr.Close();
            return points;
        }
        public List<int> retreiveRandom()
        {
            con = getcon();
            List<int> retrieve_attribute = new List<int>();
            string retrieve = "select * from hash_attributes";
            SqlCommand cmd = new SqlCommand(retrieve, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                retrieve_attribute.Add(Convert.ToInt16(dr[3]));
            }
            dr.Close();
            return retrieve_attribute;
        }
        public void creation()
        {
            groupCreate();
            

            Point hash = new Point();
            int random = 0;
            List<string> attr = retrieve_attribute();
            List<int> Zp = retreiveRandom();
            foreach (string att in attr)
            {

                Random r = new Random();
                int index = r.Next(0, group_1.Count);
                hash = group_1[index];
                random = r.Next(0, Zp.Count);
               // MessageBox.Show("" + hash);
                Update_attr_points(att.ToString().ToUpper(), hash,random);
                group_1.Remove(hash);

            }

        }









       
    }
}
