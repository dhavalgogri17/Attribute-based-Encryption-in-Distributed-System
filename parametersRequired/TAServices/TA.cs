using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.ServiceModel;
using System.IO;
namespace TAServices
{
    [ServiceContract]
    public interface TAOPERATIONS
    {

        [OperationContract]
        void insert_into_database_MK(int beta, int alpha, Point g_alpha);
        [OperationContract]
        List<string> getSecretKey(string id);
        [OperationContract]
        Point AnonymousKeyGeneration(string dataOwnerId);
        [OperationContract]
     void generateSecretKey(string loginID,string attributeSets);
        
       
        [OperationContract]
        void encryptSecretKey(FileStream fs);
        [OperationContract]
        Point PseudonymGen(int t, string dataOwnerId);

        [OperationContract]
        Point hash_beta(Point gen, int prime);
        [OperationContract]
        Point hash_beta_t(Point gen, int t,int prime);

        [OperationContract]
        void request(string id);
        [OperationContract]
        void storeValue(string id, Point X);
        [OperationContract]
        string check(string id);
        [OperationContract]
        string retreiveServer(string id);
        [OperationContract]
        string retreivBoth(string id);
        [OperationContract]
        bool checkvalue(string id, int i, int j);
    }
    [ServiceContract]
    public interface group_gen
    {
        [OperationContract]
        List<Point> group_t(List<Point> group_1, int primeNumber);
        [OperationContract]
        List<Point> GroupGen(List<Point> ecc_coordinates, int primeNumber, int lamda, List<Point> group_1, Point P);
        [OperationContract]
        List<Point> generate_mulgroup(int primeNumber, List<int> Group_1);
    }

   
}
