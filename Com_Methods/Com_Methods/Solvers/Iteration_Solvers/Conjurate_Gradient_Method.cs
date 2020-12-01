using System;

namespace Com_Methods
{
    class Conjurate_Gradient_Method : IIteration_Solver
    {
        public int Max_Iter { get; set; }
        public double Eps { get; set; }
        public int Iter { get; set; }
        public Preconditioner Preconditioner { get; set; }

        public Conjurate_Gradient_Method(int MAX_ITER, double EPS)
        {
            Max_Iter = MAX_ITER;
            Eps = EPS;
            Iter = 0;
        }

        public Vector Start_Solver(CSlR_Matrix A, Vector F, Preconditioner.Type_Preconditioner PREC)
        {
            switch (PREC)
            {
                case Preconditioner.Type_Preconditioner.Diagonal_Preconditioner:
                    {
                        Preconditioner = new Diagonal_Preconditioner(A);
                        break;
                    }
                case Preconditioner.Type_Preconditioner.LU_Decomposition:
                    {
                        Preconditioner = new LU_Preconditioner(A);
                        break;
                    }
            }

            int n = A.N;

            Vector RES = new Vector(n);
            for (int i = 0; i < n; i++)
                RES.Elem[i] = 0.0;

            Vector r = new Vector(n);
            Vector p = new Vector(n);
            Vector vec = new Vector(n);

            double alpha, beta, sc1, sc2;
            bool Flag = true;

            double Norma_r = 0;

            A.Mult_MV(RES, vec);
            for (int i = 0; i < n; i++)
                r.Elem[i] = F.Elem[i] - vec.Elem[i];
            Preconditioner.Start_Preconditioner(r, p);

            while (Flag && Iter < Max_Iter)
            {
                sc1 = p * r;
                A.Mult_MV(p, vec);
                sc2 = vec * p;
                alpha = sc1 / sc2;

                for (int i = 0; i < n; i++)
                {
                    RES.Elem[i] += alpha * p.Elem[i];
                    r.Elem[i] -= alpha * vec.Elem[i];
                }

                Preconditioner.Start_Preconditioner(r, vec);

                sc2 = vec * r;
                beta = sc2 / sc1;
                Norma_r = r.Norma();
                if (Norma_r < Eps) Flag = false;

                if (Flag)
                    for (int i = 0; i < n; i++)
                        p.Elem[i] = vec.Elem[i] + beta * p.Elem[i];

                Iter++;
                Console.WriteLine("{0,-20}    {1,-20}", Iter, Norma_r.ToString("E"));
            }

            return RES;
        }
    }
}
