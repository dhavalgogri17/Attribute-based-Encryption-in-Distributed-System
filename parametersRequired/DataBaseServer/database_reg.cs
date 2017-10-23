using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace DataBaseServer
{
    class database_reg:database.userCredintials
    {
        SqlConnection con = new SqlConnection();
        DataSet ds;
        SqlDataAdapter da;
        public SqlConnection connection()
        {
            try
            {
                con.ConnectionString = "Data Source = DHAVAL;Initial Catalog = Login_Register; Integrated Security=True;MultipleActiveResultSets=true";
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
        public void insert_reg(string user_id, string password, string city, string pin_code, string email_id, string mobile, string check)
        {

            con = connection();
            string insert = "insert into Register values(@user,@password,@city,@mobile,@email_id,@pin_code,@attribute)";
            SqlCommand cmd = new SqlCommand(insert, con);
            cmd.Parameters.Add(new SqlParameter("@user", user_id.Trim()));
            cmd.Parameters.Add(new SqlParameter("@password", password.Trim()));
            cmd.Parameters.Add(new SqlParameter("@city", city.Trim()));
            cmd.Parameters.Add(new SqlParameter("@mobile", mobile.Trim()));
            cmd.Parameters.Add(new SqlParameter("@email_id", email_id.Trim()));
            cmd.Parameters.Add(new SqlParameter("@pin_code", pin_code.Trim()));
            cmd.Parameters.Add(new SqlParameter("@attribute", check.Trim()));
            cmd.ExecuteNonQuery();
        }
        public Boolean same_user(string user_id)
        {
            bool flag = false;
            con = connection();
            string retrieve = "select user_id from Register";
            SqlCommand cmd = new SqlCommand(retrieve,con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (user_id==dr[0].ToString().Trim())
                {
                    MessageBox.Show("Hello");
                    flag = true;
                    break;
                }
                else
                {
                    flag = false;
                   // break;
                }
            }
            return flag;

        }
        public Boolean check_login(string user_id, string password)
        {
            bool flag = false;
            con = connection();
            string retrieve = "select user_id,password from Register";
            SqlCommand cmd = new SqlCommand(retrieve, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (user_id == dr[0].ToString().Trim())
                {
                    if (password ==(string)dr[1].ToString().Trim())
                    {
                        //MessageBox.Show("hello");
                        flag = true;
                        break;
                    }
                }
                else
                {
                    flag = false;
                }
            }
            return flag;
        }

        public string attributeRetreive(string id)
        {
            string query = "select attributes from Register where user_id=@id";
            con = connection();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(new SqlParameter("@id", id));

            SqlDataReader dr = cmd.ExecuteReader();

            string attr = "";
            while (dr.Read())
            {
                attr = dr[0].ToString();
            }



            return attr;



        }


        public void storeEncryptedFile(byte[] filedata, string dataownerid, string filePath, string filename)
        {
            try
            {


                string query = "insert into userFileEncrypted values(@dataownerid,@filename,@filepath,@filedata)";
                con = connection();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.Add(new SqlParameter("@dataownerid", dataownerid));
                cmd.Parameters.Add(new SqlParameter("@filepath", filePath));
                cmd.Parameters.Add(new SqlParameter("@filename", filename));
                cmd.Parameters.Add(new SqlParameter("@filedata",(object)(filedata)));
              
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show("server busy" +ex.Message);

            }
            finally
            {
                con.Close();
            }
        }

        public DataSet retreiveFile()
        {
            con = connection();
            ds = new DataSet();
            da = new SqlDataAdapter("select * from userFileEncrypted", con);
            da.Fill(ds, "userFileEncrypted");
            return ds;
        }
    }
}
