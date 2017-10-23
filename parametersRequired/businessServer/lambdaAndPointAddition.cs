using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Drawing;
namespace businessServer
{
    class lambdaAndPointAddition:lambda_pointAddition.lam_Point_add
    {





      

        

        public int lam(Point p, Point q, int primeNumber, out int denomenator)
        {
        //    calu = cal1.CreateChannel();
        //    MessageBox.Show(""+calu.hash_point("EMPLOYEE"));

            //k = 1;
            int lamda = 0;
            int numerator = 0;
            denomenator = 0;
            if (p.X == q.X && p.Y == q.Y)
            {
                numerator = (int)((Math.Pow(p.X, 2) * 3 + 1)) % primeNumber;
                denomenator = (2 * p.Y) % primeNumber;
                lamda = lambda(denomenator, numerator, primeNumber, lamda);
                //k++;
            }
            else
            {
                numerator = (q.Y - p.Y) % primeNumber;
                denomenator = (q.X - p.X) % primeNumber;
                lamda = lambda(denomenator, numerator, primeNumber, lamda);
                //k++;
            }


            //calu = cal1.CreateChannel();
            //MessageBox.Show(""+calu.getcon());
            return lamda;
           

        }
        public int lambda(int denomenator, int numerator, int primeNumber, int lamda)
        {
            
            if (((denomenator >= 1 && numerator > 0) || (denomenator < 0 && numerator < 0)) && denomenator != 0)
            {
                for (int i = 0; i < primeNumber; i++)
                {
                    if (((denomenator * i) - numerator) % primeNumber == 0)
                    {
                        lamda = i;
                    }
                }

            }
            else if (((denomenator <= 1 && numerator > 0) || (denomenator > 0 && numerator < 0)) && denomenator != 0)
            {
                for (int i = 0; i < primeNumber; i++)
                {
                    if (((denomenator * i) - numerator) % primeNumber == 0)
                    {
                        lamda = i;
                    }
                }
            }
            return lamda;
        }


       
        // this function is for addition of two points

        public Point generate_group(Point p, Point q, int primenumber, int lambda_value)
        {

            
            Point pabc = new Point(0, 0);
            int x3 = 0, y3 = 0;
            int x2 = 0, y2 = 0;
            int lamda = 0;
            int x1 = p.X;
            int y1 = p.Y;

            Point r = new Point(0, 0);

            List<Point> generate = new List<Point>();

            
            lamda = lambda_value;
            if (p.X == 0 && p.Y == 0)
            {
                r.X = p.X + q.X;
                r.Y = p.Y + q.Y;
            }
            else
            {
                r.X = ((int)(Math.Pow(lamda, 2)) - p.X - q.X) % primenumber;
                r.Y = ((lamda * (p.X - r.X)) - p.Y) % primenumber;
            }
            if (r.X < 0)
            {
                r.X = primenumber + r.X;
            }
            if (r.Y < 0)
            {
                r.Y = primenumber + r.Y;
            }
            pabc = new Point(r.X, r.Y);
            generate.Add(pabc);



            return pabc;
        }
        public Point milersPoint(Point p, int n, int primeNumber)
        {
            int lamda = 0;
            int den=0;
            Point Q = p;
            Q = p;
            Point R = new Point(0, 0);//infinity point
            while (n > 0)
            {
                //MessageBox.Show("n= " + n + "lam   odd after    " + lamda + "    " + "     Q is        " +Q + "    \nR....   " + R);
                if (n % 2 == 1)
                {
                    lamda = lam(R, Q, primeNumber, out den);
                    R = generate_group(R, Q, primeNumber, lamda);
                }
                lamda = lam(Q, Q, primeNumber, out den);
                Q =generate_group(Q, Q, primeNumber, lamda);

                n = n / 2;
                ///R = Q;
            }
            return R;
        }
        public Boolean match_key(string line, string key1)
        {
            string tree = "";
            Boolean flag1 = false, flag2 = false, flag3 = false, flag4 = false, flag5 = false;
            bool[] a = new bool[5];
            int j = 0, l = 0, m = 0;
            string[] scrambled_tree = line.ToString().Split(' ');
            string[] abc1 = key1.ToString().Split('@');
            string flg1 = "";
            string flg2 = "";
            for (int i = 0; i < scrambled_tree.Length - 1; i++)
            {
                if (scrambled_tree[i] == "@")
                {
                    tree = tree + scrambled_tree[i - 1] + " ";
                }
                if (scrambled_tree[i] == "|")
                {
                    tree = tree + scrambled_tree[i] + " ";
                }
                if (scrambled_tree[i] == "&")
                {
                    tree = tree + scrambled_tree[i] + " ";
                }
            }
            tree = tree.Trim();
            string[] tree1 = tree.ToString().Split(' ');
            //if (abc == key1)
            // {
            for (int i = 0; i < ((tree1.Length - 1) / 2); i++)
            {
                if (tree1[i] == "|")
                {
                    for (int k = 0; k <= abc1.Length - 1; k++)
                    {
                        if (tree1[i - 1] == abc1[k] || tree1[i + 1] == abc1[k])
                        {
                            a[j++] = true;
                            break;
                        }
                    }
                }
                else if (tree1[i] == "&")
                {
                    for (int k = 0; k <= abc1.Length - 1; k++)
                    {
                        if (tree1[i - 1] == abc1[k])
                        {
                            flag3 = flag1 = true;
                            tree1[i - 1] = "";
                            l = k;
                            flg1 = abc1[k];
                            abc1[k] = "";
                        }
                        if (tree1[i + 1] == abc1[k])
                        {
                            flag4 = flag2 = true;
                            tree1[i + 1] = "";
                            m = k;
                            flg2 = abc1[k];
                            abc1[k] = "";
                        }
                    }
                    if (flag1 && flag2)
                    {
                        a[j++] = true;
                        break;
                    }
                }
            }
            flag1 = false;
            flag2 = false;
            for (int i = tree1.Length - 3; i < tree1.Length - 1; i++)
            {
                if (tree1[i] == "|")
                {
                    if (flag3 == true && flag4 == false)
                    {
                        abc1[l] = flg1;
                    }
                    else if (flag3 == false && flag4 == true)
                    {
                        abc1[m] = flg2;
                    }
                    else if (flag3 == true && flag4 == true)
                    {
                        abc1[l] = flg1;
                        abc1[m] = flg2;
                    }
                    for (int k = 0; k <= abc1.Length - 1; k++)
                    {
                        if (tree1[i - 1] == abc1[k] || tree1[i + 1] == abc1[k])
                        {
                            a[j++] = true;
                            break;
                        }
                    }
                }
                if (tree1[i] == "&")
                {
                    /*if (flag3 == true && flag4 == false)
                    {
                        abc1[l] = flg1;
                    }
                    else if (flag3 == false && flag4 == true)
                    {
                        abc1[m] = flg2;
                    }
                    else if (flag3 == true && flag4 == true)
                    {
                        abc1[l] = flg1;
                        abc1[m] = flg2;
                    }*/
                    for (int k = 0; k <= abc1.Length - 1; k++)
                    {
                        if (tree1[i - 1] == abc1[k])
                        {
                            flag1 = true;
                            tree1[i - 1] = "";
                            abc1[k] = "";
                        }
                        if (tree1[i + 1] == abc1[k])
                        {
                            flag2 = true;
                            tree1[i + 1] = "";
                            abc1[k] = "";
                        }
                    }
                    if (flag1 && flag2)
                    {
                        a[j++] = true;
                        break;
                    }
                }
            }
            if (tree1[((tree1.Length - 1) / 2)] == "|")
            {
                if (a[0] || a[1])
                {
                    flag5 = true;
                    return flag5;
                }
                else
                {
                    flag5 = false;
                    return flag5;
                }
            }
            else if (tree1[((tree1.Length - 1) / 2)] == "&")
            {
                if (a[0] && a[1])
                {
                    flag5 = true;
                    return flag5;
                }
                else
                {
                    flag5 = false;
                    return flag5;
                }
            }
            return flag5;
            //}
            //else
            //{
            //  MessageBox.Show("both key are not same.......");
            //}
        }


        public List<Point> GroupGen(List<Point> ecc_coordinates, int primeNumber, int lamda, List<Point> group_1, out int indexvalue)
        {
            int denomenator = 0;
            int flag = 1;
            Random randomCoordinate = new Random();
            indexvalue = randomCoordinate.Next(0, ecc_coordinates.Count);
            int n = 2;
            int z = 2;
            Point p = ecc_coordinates[indexvalue];

            Point q = ecc_coordinates[indexvalue];


            for (int i = 0; i < ecc_coordinates.Count; i++)
            {
                // flag = 1;
                for (int j = 0; j < ecc_coordinates.Count; j++)
                {
                    List<Point> h = new List<Point>();
                    lamda = lam(p, q, primeNumber, out denomenator);

                    Point r = new Point(0, 0);
                    r = generate_group(p, q, primeNumber, lamda);
                    //   MessageBox.Show("denomenator  " + denomenator + " lambda  >..............." + lamda+"    third point   "+r);
                    if (denomenator == 0)
                    {
                        flag = 0;
                        break;
                    }
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






        public int getAlphaOrBeta(int primeNumber)
        {
            Random r = new Random();
            int alpha_beta = r.Next(1, primeNumber - 1);
            return alpha_beta;
        }

        


        public int getPrime()
        {
            List<int> primeNumber = new List<int>() { 191 };
            Random r = new Random();
            int index = r.Next(0, primeNumber.Count);
            int prime = primeNumber[index];
            return prime;
        }



        public int div(Point p, Point q, List<Point> group_1, int primeNumber)
        {
            int main_answer = 0;
            Point r = new Point();
            int lamda = 0;
            int denomenator = 0;
            r = new Point(18, 165);
            Point S = p;
            Point T = q;
            Point R = r;
           
            lamda = lam(T, R, primeNumber, out denomenator);
            Point R1 = new Point(0, 0);
            R1 =generate_group(T, R, primeNumber, lamda);
            main_answer = WeilPairing(p,q,primeNumber, S, R, R1);
            return main_answer;
        }

        //CALCULATION OF WEIL PAIRING



        public int WeilPairing(Point p,Point q,int primeNumber, Point S, Point R, Point R1)
        {
            int a = 0, b = 0, cons = 0;
            int first_anser = 0;
            int second_answer = 0;
            int third_answer = 0;
            int fourth_answer = 0;
            int main_answer = 0;


            //Point R1 = new Point(0, 0);

            polynomial(S, primeNumber, out a, out cons);
           
            first_anser = a * q.X + (q.Y) + cons;
            first_anser = first_anser % primeNumber;

            polynomial(R, primeNumber, out a, out cons);

          
            second_answer = a * p.X + (p.Y) + cons;
           
            
            polynomial(R1, primeNumber, out a, out cons);
           
            third_answer = a * p.X +(p.Y) + cons;
            
            fourth_answer =lambda(second_answer, third_answer, primeNumber, fourth_answer);
           
            main_answer = lambda(fourth_answer, first_anser, primeNumber, main_answer);

            return main_answer;
        }
       

        public void polynomial(Point p, int primeNumber, out int a, out int c)
        {
            a = 0;
            c = 0;
            int lamda = 0, denomenator = 0;
            int[] abc = new int[3];
            lamda = lam(p, p, primeNumber, out denomenator);

            a = lamda;
            c = (lamda * p.X) + p.Y;

        }



    }
}
