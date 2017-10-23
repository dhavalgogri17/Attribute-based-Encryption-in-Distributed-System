using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ServiceModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace DataBaseServer
{
    class publicKey:database.public_masterKey
    {
        SqlConnection con=new SqlConnection();

        
        public SqlConnection connection()
        {
            try
            {
                con.ConnectionString = "Data Source = DHAVAL;Initial Catalog = random_oracle; Integrated Security=True;MultipleActiveResultSets=true";
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

        public void insert_into_database_GT(List<Point> gt)
        {
            //OperationContext.Current.Channel.Close();
            string e = "";
            foreach (Point i in gt)
            {
                e = e + i.X + "," + i.Y + " "; ;
            }
            try
            {
                con = connection();
                string delete = "delete from GT";
                SqlCommand cmd1 = new SqlCommand(delete, con);
                cmd1.ExecuteNonQuery();
              //  MessageBox.Show("deleted gt");

                string ins = "insert into GT values(@gt)";
                SqlCommand cm = new SqlCommand(ins, con);
                cm.Parameters.Add(new SqlParameter("@gt", e));
                cm.ExecuteNonQuery();
               // MessageBox.Show("inserted gt");
                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        

        public void insert_into_database_PK(List<Point> lp, Point gen, Point h, int weil)
        {
            try
            {
                //OperationContext.Current.Channel.Close();
                con = connection();
                string delete = "delete from Public_Key";
                SqlCommand cmd1 = new SqlCommand(delete, con);
                cmd1.ExecuteNonQuery();
              //  MessageBox.Show("deleted pk");
                string a = "", b = "", c = "";
                int d = 0;
                foreach (Point i in lp)
                {
                    a = a + i.X + "," + i.Y + " "; ;
                }
                b = gen.X + "," + gen.Y;
                c = h.X + "," + h.Y;
                d = weil;

                string insert = "insert into Public_Key values(@G,@gen,@h,@weil)";
                SqlCommand cmd = new SqlCommand(insert, con);
                cmd.Parameters.Add(new SqlParameter("@G", a.Trim()));
                cmd.Parameters.Add(new SqlParameter("@gen", b.Trim()));
                cmd.Parameters.Add(new SqlParameter("@h", c.Trim()));
                cmd.Parameters.Add(new SqlParameter("@weil", d));
                cmd.ExecuteNonQuery();
               // MessageBox.Show("inserted pk");
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public string retreive_public_key()
        {
            //OperationContext.Current.Channel.Close();
            con = connection();
            string query = "select * from Public_Key";
            string group="",gen="",H="",w="";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                group = dr[0].ToString();
                gen = dr[1].ToString();
                H = dr[2].ToString();
                w = dr[3].ToString();

            }
            string combine = group + "@" + gen + "@" + H + "@" + w;
            con.Close();
            return combine;
        
        }
    }
}
