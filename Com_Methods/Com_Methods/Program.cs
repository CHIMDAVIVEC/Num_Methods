using System;
using System.Diagnostics;
using System.Threading;

namespace Com_Methods
{
    class CONST
    {
        //точность решения
        public static double EPS = 1e-20;
    }

    class Tools
    {
        //замер времени
        public static string Measurement_Time(Thread thread)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            thread.Start();
            while (thread.IsAlive) ;
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            return ("RunTime: " + elapsedTime);
        }

        //относительная погрешность
        public static double Relative_Error(Vector X, Vector x)
        {
            double s = 0.0;
            for (int i = 0; i < X.N; i++)
            {
                s += Math.Pow(X.Elem[i] - x.Elem[i], 2);
            }
            return Math.Sqrt(s) / x.Norma();
        }

        //относительная невязка
        public static double Relative_Discrepancy(Matrix A, Vector X, Vector F)
        {
            var x = A * X;
            for (int i = 0; i < X.N; i++) x.Elem[i] -= F.Elem[i];
            return x.Norma() / F.Norma();
        }
    }

    class Program
    {
        static void Main()
        {
            try
            {
                //прямые методы: Гаусс, LU-разложение, QR-разложение
                var T1 = new Thread(() =>
                {
                    //int N = 10;
                    //var A = new Matrix(N, N);
                    //var X_true = new Vector(N);

                    //заполнение СЛАУ
                    //for (int i = 0; i < N; i++)
                    //{
                    //    for (int j = 0; j < N; j++)
                    //    {
                    //        A.Elem[i][j] = 1.0 / (i + j + 1.0);
                    //    }
                    //    X_true.Elem[i] = 1;
                    //}

    //                for (int i = 0; i < N; i++)
    //                {
    //                    for (int j = 0; j < N; j++)
    //                    {
    //                        //A.Elem[i][j] = 1.0 / (i + j + 1.0);
    //                    }
    //                    X_true.Elem[i] = 1;
    //                }

    //                var ElemMatrix = new double[][]
    //                {
    //                    //new double[]{-2,-2,-1},
    //                    //new double[]{1,0,-2},
    //                    //new double[]{0,1,2}
    //                    new double[]{5,1,9},
    //                    new double[]{1,4,1},
    //                    new double[]{5,1,3}

    //                };
    //                A.Elem = ElemMatrix;

    //                //правая часть
    //                var F = A * X_true;

    //                //решатель
    //                //var Solver = new Gauss_Method();
    //                //var Solver = new LU_Decomposition(A);
    //                var Solver = new QR_Decomposition(A, QR_Decomposition.QR_Algorithm.Householder);

    //                var X = Solver.Start_Solver(F);
    //                //X.Console_Write_Vector();
    //                Console.WriteLine("\nError: {0}\n", Tools.Relative_Error(X, X_true));

                });

                //Итерационные методы
                var T2 = new Thread(() =>
                {
                });

                //итерационные методы: Якоби и SOR
                var T3 = new Thread(() =>
                {
                    int SIZE_BLOCK = 450;
                    double m = 1e+4;
                    double z = 1.2;
                    for (int k = 0; k < 1; k++)
                    {
                        //var Solver = new Jacobi_Method(30000, 1e-12);
                        var Solver = new SOR_Method(30000, 1e-4);


                        //Вычисление точного решения

                        //var A_g = new Matrix("Data\\System3\\", m);
                        //var F_g = new Vector("Data\\System3\\");
                        //var GSolver = new Gauss_Method();

                        //var sw = new Stopwatch();
                        //sw.Start();

                        //var X_true = GSolver.Start_Solver(A_g, F_g);

                        //sw.Stop();
                        //Console.WriteLine($"\nTime: {sw.Elapsed}");


                        //Блочная или обычная матрица/вектор

                        //var A = new Block_Matrix("Data\\System3\\", SIZE_BLOCK, m);
                        //var F = new Block_Vector("Data\\System3\\", SIZE_BLOCK);
                        //var A = new Matrix("Data\\System3\\", m);
                        //var F = new Vector("Data\\System3\\");
                        var A = new Matrix(3, 3);
                        var F = new Vector(3);
                        F.Elem[0] = 8;
                        F.Elem[1] = 8;
                        F.Elem[2] = 11;
                        var ElemMatrix = new double[][]
                        {
                                            //new double[]{-2,-2,-1},
                                            //new double[]{1,0,-2},
                                            //new double[]{0,1,2}
                                            new double[]{10,1,0},
                                            new double[]{1,10,0},
                                            new double[]{0,0,10}

                        };
                        A.Elem = ElemMatrix;
                        var sw = new Stopwatch();
                        sw.Start();


                        //var X = Solver.Start_Solver(A, F);
                        var X = Solver.Start_Solver(A, F, 1);
                        //var X = Solver.Start_Solver(A, F, z);


                        sw.Stop();
                        Console.WriteLine($"\nTime: {sw.Elapsed}");



                        //var X_true_Norm = X_true.Norma();

                        //ДЛЯ ОБЫЧНОЙ МАТРИЦЫ

                        //for (int j = 0; j < 900; j++)
                        //{
                        //    X_true.Elem[j] = X.Elem[j] - X_true.Elem[j];
                        //}


                        //ДЛЯ БЛОЧНОЙ МАТРИЦЫ


                        //int q = 0;
                        //for (int i = 0; i < X.N; i++)
                        //{
                        //    for (int j = 0; j < SIZE_BLOCK; j++)
                        //    {
                        //        X_true.Elem[q] = X.Block[i].Elem[j] - X_true.Elem[q];
                        //        q++;
                        //    }
                        //}

                        //Console.WriteLine("delta = " + (X_true.Norma() / X_true_Norm));
                        //Console.WriteLine("Cond(A) = " + A.Cond_InfinityNorm() + "\n");
                        //Console.WriteLine("m = " + m);
                        //Console.WriteLine("z = " + z);
                        Console.WriteLine("--------------------------");

                        //m /= 100;
                        //z += 0.1;
                    }
                });

                var T4 = new Thread(() =>
                {
                    var A = new CSlR_Matrix("Data\\Sparse_Format\\Systems2\\nonSPD\\");
                    var X_true = new Vector(A.N);
                    var F = new Vector(A.N);

                    for (int i = 0; i < A.N; i++)
                        X_true.Elem[i] = 1.0;
                    A.Mult_MV(X_true, F);

                    var Solver = new Conjurate_Gradient_Method(30000, 1e-12);
                    //var Solver = new BiConjurate_Gradient_Method(30000, 1e-12);

                    var sw = new Stopwatch();
                    sw.Start();

                    var X = Solver.Start_Solver(A, F, Preconditioner.Type_Preconditioner.LU_Decomposition);

                    sw.Stop();
                    Console.WriteLine($"\nTime: {sw.Elapsed}");

                    Console.WriteLine("Relative_Error: {0}", Tools.Relative_Error(X, X_true));
                    Console.ReadLine();
                    for (int i = 0; i < X.N; i++)
                        Console.WriteLine("X[{0}] = {1}", i + 1, X.Elem[i]);
                });
                //время решения
                Console.WriteLine(Tools.Measurement_Time(T4));

            }
            catch (Exception E)
            {
                Console.WriteLine("\n*** Error! ***");
                Console.WriteLine("Method:  {0}", E.TargetSite);
                Console.WriteLine("Message: {0}\n", E.Message);
            }
            Console.ReadLine();
        }
    }
}