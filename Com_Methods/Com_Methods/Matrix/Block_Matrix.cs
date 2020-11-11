using System;
using System.IO;

namespace Com_Methods
{
    class Block_Matrix : IMatrix
    {
        public int M { set; get; }
        public int N { set; get; }
        public int Size_Block { set; get; }
        public Matrix[][] Block { set; get; }

        public Block_Matrix() { }
        public Block_Matrix(string PATH, int SIZE_BLOCK, double Perturbation = 1.0) 
        {
            using (var Reader = new BinaryReader(File.Open(PATH + "Size.bin", FileMode.Open)))
            {
                M = Reader.ReadInt32();
                N = M;
            }

            if (M % SIZE_BLOCK != 0) throw new Exception("Block_Matrix: Block size error");

            M /= SIZE_BLOCK;
            N /= SIZE_BLOCK;
            Size_Block = SIZE_BLOCK;
            Block = new Matrix[M][];

            using (var Reader = new BinaryReader(File.Open(PATH + "Matrix.bin", FileMode.Open)))
            {
                try
                {
                    for (int i = 0; i < M; i++)
                    {
                        Block[i] = new Matrix[N];

                        for (int j = 0; j < N; j++)
                            Block[i][j] = new Matrix(Size_Block, Size_Block);

                        for (int ii = 0; ii < Size_Block; ii++)
                        {
                            for (int j = 0; j < N; j++)
                                for (int k = 0; k < Size_Block; k++)
                                {
                                    Block[i][j].Elem[ii][k] = Reader.ReadDouble();
                                    if (i == j && ii == k) Block[i][j].Elem[ii][k] *= Perturbation;
                                }
                        }
                        var LU_Decomp = new LU_Decomposition(Block[i][i]);
                        Block[i][i] = LU_Decomp.LU;
                    }
                }
                catch
                {
                    throw new Exception("Block_Matrix: Incorrect data file");
                }
            }
        }
    }
}
