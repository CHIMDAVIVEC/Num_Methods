namespace Com_Methods
{
    class LU_Preconditioner : Preconditioner
    {
        IIncomplete_LU_Decomposition ILU;

        public LU_Preconditioner(CSlR_Matrix A)
        {
            ILU = new Incomplete_LU_Decomposition_CSlR(A);
        }

        public override void Start_Preconditioner(Vector X, Vector RES)
        {
            ILU.SLAU_L(RES, X);
            ILU.SLAU_U(RES, RES);
        }

        public override void Start_Tr_Preconditioner(Vector X, Vector RES)
        {
            ILU.SLAU_Ut(RES, X);
            ILU.SLAU_Lt(RES, RES);
        }
    }
}