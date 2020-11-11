using System;

namespace Com_Methods
{
    class SOR_Method : Iteration_Solver
    {
        public int Max_Iter { set; get; }
        public double Eps { set; get; }
        public int Iter { set; get; }
        public SOR_Method(int MAX_ITER, double EPS)
        {
            Max_Iter = MAX_ITER;
            Eps = EPS;
            Iter = 0;
        }
        public Vector Start_Solver(Matrix A, Vector F, double w)    //Классический
        {
            double Norm_Xnew_Xold;

            var RES = new Vector(F.N);
            var RES_New = new Vector(F.N);

            for (int k = 0; k < RES.N; k++)
                //RES.Elem[k] = 0.0;
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

                    F_Ax /= A.Elem[i][i];
                    RES_New.Elem[i] = (1.0 - w) * RES.Elem[i] + w * F_Ax;
                    Norm_Xnew_Xold += Math.Pow(RES.Elem[i] - RES_New.Elem[i], 2);
                }
                RES.Copy(RES_New);

                Norm_Xnew_Xold = Math.Sqrt(Norm_Xnew_Xold);
                Iter++;

                Console.WriteLine("Iter {0,-10} {1}", Iter, Norm_Xnew_Xold);
            } while (Norm_Xnew_Xold > Eps && Iter < Max_Iter);
            return RES;
        }

        public Block_Vector Start_Solver(Block_Matrix A, Block_Vector F, double w)  //Блочный
        {
            double Norm_Xnew_Xold;

            var RES = new Block_Vector(A.N, A.Size_Block);

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
                    for (int k = 0; k < RES.Size_Block; k++)
                        F_Ax.Elem[k] = F.Block[i].Elem[k];

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
                        double X_NEW = (1 - w) * RES.Block[i].Elem[k] + w * F_Ax.Elem[k];
                        Norm_Xnew_Xold += Math.Pow(X_NEW - RES.Block[i].Elem[k], 2);
                        RES.Block[i].Elem[k] = X_NEW;
                    }
                }
                Norm_Xnew_Xold = Math.Sqrt(Norm_Xnew_Xold);
                Iter++;

                Console.WriteLine("Iter {0,-10} {1}", Iter, Norm_Xnew_Xold);
            } 
            while (Norm_Xnew_Xold > Eps && Iter < Max_Iter);
            return RES;
        }
    }
}
