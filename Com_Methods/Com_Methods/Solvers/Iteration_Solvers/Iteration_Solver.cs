namespace Com_Methods
{
    public interface IIteration_Solver
    {
        int Max_Iter { set; get; }
        double Eps { set; get; }
        int Iter { set; get; }
    }
}
