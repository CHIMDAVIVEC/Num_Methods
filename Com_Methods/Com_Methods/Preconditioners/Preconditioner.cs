namespace Com_Methods
{
    abstract class Preconditioner
    {
        abstract public void Start_Preconditioner(Vector X, Vector RES);
        abstract public void Start_Tr_Preconditioner(Vector X, Vector RES);
        public enum Type_Preconditioner
        {
            Diagonal_Preconditioner = 1,
            LU_Decomposition
        }
    }
}