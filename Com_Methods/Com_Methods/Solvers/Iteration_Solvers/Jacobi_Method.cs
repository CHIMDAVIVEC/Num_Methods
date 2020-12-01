using System;

namespace Com_Methods
{
    class Jacobi_Method : IIteration_Solver
    {
        public int Max_Iter { set; get; }
        public double Eps { set; get; }
        public int Iter { set; get; }
        public Jacobi_Method(int MAX_ITER, double EPS)
        {
            Max_Iter = MAX_ITER;
            Eps = EPS;
            Iter = 0;
        }
        public Vector Start_Solver(Matrix A, Vector F) //Классический метод
        {
            double Norm_Xnew_Xold;

            var RES = new Vector(F.N);
            var RES_New = new Vector(F.N);

            //for (int k = 0; k < RES.N; k++)
            //    RES.Elem[k] = 0.0;
            for (int k = 0; k < RES.N; k++)
                RES.Elem[k] = 1.0;
            do
            {
                Norm_Xnew_Xold = 0.0;
                for (int i = 0; i < RES.N; i++)
                {
                    double F_Ax = F.Elem[i];

                    for (int j = 0; j < i; j++)
                        F_Ax -= A.Elem[i][j] * RES.Elem[j];

                    for (int j = i + 1; j < A.N; j++)
                        F_Ax -= A.Elem[i][j] * RES.Elem[j];

                    RES_New.Elem[i] = F_Ax / A.Elem[i][i];
                    Norm_Xnew_Xold += Math.Pow(RES.Elem[i] - RES_New.Elem[i], 2);
                }
                RES.Copy(RES_New);

                Norm_Xnew_Xold = Math.Sqrt(Norm_Xnew_Xold);
                Iter++;

                Console.WriteLine("Iter {0,-10} {1}", Iter, Norm_Xnew_Xold);
            } while (Norm_Xnew_Xold > Eps && Iter < Max_Iter);
            return RES;
        }

        public Block_Vector Start_Solver(Block_Matrix A, Block_Vector F)    //Блочный метод
        {
            double Norm_Xnew_Xold;

            var RES = new Block_Vector(A.N, A.Size_Block);
            var RES_New = new Block_Vector(A.N, A.Size_Block);

            for (int i = 0; i < RES.N; i++)
                for (int k = 0; k < RES.Size_Block; k++)
                    RES.Block[i].Elem[k] = 1.0;

            var LU_Solver = new LU_Decomposition();
            var F_Ax = new Vector(A.Size_Block);

            do
            {
                Norm_Xnew_Xold = 0.0;
                for (int i = 0; i < RES.N; i++)
                {
                    F_Ax.Copy(F.Block[i]);

                    for (int j = 0; j < i; j++)
                    {
                        var Current_Matrix_Block = A.Block[i][j];
                        var Current_Vector_Block = RES.Block[j];

                        for (int Row = 0; Row < Current_Matrix_Block.M; Row++)
                            for (int Col = 0; Col < Current_Matrix_Block.N; Col++)
                                F_Ax.Elem[Row] -= Current_Matrix_Block.Elem[Row][Col] * Current_Vector_Block.Elem[Col];
                    }

                    for (int j = i + 1; j < A.N; j++)
                    {
                        var Current_Matrix_Block = A.Block[i][j];
                        var Current_Vector_Block = RES.Block[j];

                        for (int Row = 0; Row < Current_Matrix_Block.M; Row++)
                            for (int Col = 0; Col < Current_Matrix_Block.N; Col++)
                                F_Ax.Elem[Row] -= Current_Matrix_Block.Elem[Row][Col] * Current_Vector_Block.Elem[Col];
                    }

                    LU_Solver.LU = A.Block[i][i];
                    F_Ax = LU_Solver.Start_Solver(F_Ax);

                    for (int k = 0; k < RES.Size_Block; k++)
                    {
                        RES_New.Block[i].Elem[k] = F_Ax.Elem[k];
                        Norm_Xnew_Xold += Math.Pow(RES_New.Block[i].Elem[k] - RES.Block[i].Elem[k], 2);
                    }
                }
                for (int i = 0; i < RES.N; i++)
                    RES.Block[i].Copy(RES_New.Block[i]);

                Norm_Xnew_Xold = Math.Sqrt(Norm_Xnew_Xold);
                Iter++;

                Console.WriteLine("Iter {0,-10} {1}", Iter, Norm_Xnew_Xold);
            }
            while (Norm_Xnew_Xold > Eps && Iter < Max_Iter);
            return RES;
        }
    }
}
