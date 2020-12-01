using System;

namespace Com_Methods
{
    class BiConjurate_Gradient_Method : IIteration_Solver
    {
        public int Max_Iter { get; set; }
        public double Eps { get; set; }
        public int Iter { get; set; }
        public Preconditioner Preconditioner { get; set; }

        public BiConjurate_Gradient_Method(int MAX_ITER, double EPS)
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
            {
                RES.Elem[i] = 0.0;
            }

            Vector r = new Vector(n);
            Vector rs = new Vector(n);
            Vector p = new Vector(n);
            Vector ps = new Vector(n);
            Vector vec = new Vector(n);
            Vector vec2 = new Vector(n);

            double alpha, beta, sc1, sc2;
            bool Flag = true;

            A.Mult_MV(RES, vec);
            for (int i = 0; i < n; i++)
                r.Elem[i] = F.Elem[i] - vec.Elem[i];

            Preconditioner.Start_Preconditioner(r, ps);

            for (int i = 0; i < n; i++)
            {
                r.Elem[i] = ps.Elem[i];
                rs.Elem[i] = ps.Elem[i];
                p.Elem[i] = ps.Elem[i];
            }

            while (Flag && Iter < Max_Iter)
            {
                sc1 = r * rs;

                A.Mult_MV(p, vec);
                Preconditioner.Start_Preconditioner(vec, vec2);
                sc2 = vec2 * ps;

                alpha = sc1 / sc2;

                for (int i = 0; i < n; i++)
                {
                    RES.Elem[i] += alpha * p.Elem[i];
                    r.Elem[i] -= alpha * vec2.Elem[i];
                }

                Preconditioner.Start_Tr_Preconditioner(ps, vec);
                A.Mult_MtV(vec, vec2);
                for (int i = 0; i < n; i++)
                    rs.Elem[i] -= alpha * vec2.Elem[i];
                sc2 = r * rs;

                beta = sc2 / sc1;

                double Norma_r = r.Norma();
                if (Norma_r < Eps) Flag = false;

                if (Math.Abs(beta) < Double.Epsilon) 
                    throw new Exception("BiConjurate_Gradient_Method: diverges given ~r(0)");

                for (int i = 0; i < n; i++)
                {
                    p.Elem[i] = r.Elem[i] + beta * p.Elem[i];
                    ps.Elem[i] = rs.Elem[i] + beta * ps.Elem[i];
                }

                Iter++;
                Console.WriteLine("{0,-20}    {1,-20}", Iter, Norma_r);
            }

            return RES;
        }
    }
}