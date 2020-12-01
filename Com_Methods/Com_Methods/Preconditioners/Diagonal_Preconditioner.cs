using System;

namespace Com_Methods
{
    class Diagonal_Preconditioner : Preconditioner
    {
        Vector Diag { get; }

        public Diagonal_Preconditioner(CSlR_Matrix A)
        {
            Diag = new Vector(A.N);
            for (int i = 0; i < A.N; i++)
            {
                if (Math.Abs(A.di[i]) < CONST.EPS)
                    throw new Exception("Diagonal_Preconditioner: " + (i + 1).ToString() + " position on di = " + A.di[i].ToString());
                Diag.Elem[i] = A.di[i];
            }
        }

        public override void Start_Preconditioner(Vector X, Vector RES)
        {
            for (int i = 0; i < Diag.N; i++)
                RES.Elem[i] = X.Elem[i] / Diag.Elem[i];
        }

        public override void Start_Tr_Preconditioner(Vector X, Vector RES)
        {
            for (int i = 0; i < Diag.N; i++)
                RES.Elem[i] = X.Elem[i] / Diag.Elem[i];
        }
    }
}