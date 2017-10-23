using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.ServiceModel;
using System.Drawing;
namespace database
{
    [ServiceContract]
    public interface databaseoperations
    {
        [OperationContract]
        void insert(string ip, Point w, int random_r);
        [OperationContract]
        SqlConnection getcon();
        [OperationContract]
        Point hash_point(string input);
       
        [OperationContract]
        Boolean check_Point(string Attribute);
        [OperationContract]
        int Random_R(string input);
        [OperationContract]
        List<string> retrieve_attribute();
        [OperationContract]
        void Update_attr_points(string attribute, Point attr_point,int r);
        [OperationContract]
        void creation();
       
       
        
    }
    [ServiceContract]
    public interface public_masterKey
    {
        [OperationContract]
        SqlConnection connection();
       
        [OperationContract]
        void insert_into_database_PK(List<Point> lp, Point gen, Point h, int weil);
        [OperationContract]
        void insert_into_database_GT(List<Point>GT);
        [OperationContract]
        string retreive_public_key();

    }

    [ServiceContract]
    public interface userCredintials
    {
        [OperationContract]
        SqlConnection connection();
        [OperationContract]
        void insert_reg(string user_id, string password, string city, string pin_code, string email_id, string mobile, string check);
        [OperationContract]
        Boolean same_user(string user_id);
        [OperationContract]
        Boolean check_login(string user_id, string password);
        [OperationContract]

        string attributeRetreive(string id);
        [OperationContract]
        void storeEncryptedFile(byte[] data, string dataownerid, string filePath, string filename);
        [OperationContract]
        DataSet retreiveFile();

   



    }
}
