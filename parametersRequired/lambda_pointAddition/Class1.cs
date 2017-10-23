using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Drawing;
namespace lambda_pointAddition
{

   [ServiceContract]
    public interface lam_Point_add
    {
       [OperationContract]
       int getPrime();
       [OperationContract]
       Boolean match_key(string line, string key1);
       [OperationContract]
       int getAlphaOrBeta(int primeNumber);
       [OperationContract]
       int lambda(int denomenator, int numerator, int primeNumber, int lamda);
       [OperationContract]
       int lam(Point p, Point q, int primeNumber, out int denomenator);
       [OperationContract]
       Point generate_group(Point p, Point q, int primenumber, int lambda_value);
       [OperationContract]
       Point milersPoint(Point p, int n, int primeNumber);
       [OperationContract]
       int WeilPairing(Point p, Point q, int primeNumber, Point S, Point T, Point R);
       [OperationContract]
      int div(Point p, Point q, List<Point> group_1, int primeNumber);
       [OperationContract]
       void polynomial(Point p, int primeNumber, out int a, out int c);
    }
   


}
