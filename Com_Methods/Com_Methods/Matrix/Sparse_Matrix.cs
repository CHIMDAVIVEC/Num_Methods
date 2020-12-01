using System;
using System.IO;
using System.Globalization;

namespace Com_Methods
{
    public interface ISparse_Matrix
    {
        int N { set; get; }
        void Mult_MV(Vector X, Vector Y);
        void Mult_MtV(Vector X, Vector Y);
    }
    class CSlR_Matrix : ISparse_Matrix
    {
        public int N { set; get; }
        public double[] di { set; get; }
        public double[] altr { set; get; }
        public double[] autr { set; get; }
        public int[] jptr { set; get; }
        public int[] iptr { set; get; }

        public CSlR_Matrix() { }

        public CSlR_Matrix(string PATH)
        {
            char[] Separator = new char[] { ' ' };

            using (var Reader = new StreamReader(File.Open(PATH + "Size.txt", FileMode.Open)))
            {
                N = Convert.ToInt32(Reader.ReadLine());
                iptr = new int[N + 1];
                di = new double[N];
            }

            using (var Reader = new StreamReader(File.Open(PATH + "di.txt", FileMode.Open)))
                for (int i = 0; i < N; i++)
                    di[i] = Convert.ToDouble(Reader.ReadLine().Split(Separator, StringSplitOptions.RemoveEmptyEntries)[0], CultureInfo.InvariantCulture);

            using (var Reader = new StreamReader(File.Open(PATH + "iptr.txt", FileMode.Open)))
                for (int i = 0; i <= N; i++)
                    iptr[i] = Convert.ToInt32(Reader.ReadLine().Split(Separator, StringSplitOptions.RemoveEmptyEntries)[0], CultureInfo.InvariantCulture);

            int Size = iptr[N] - 1;
            jptr = new int[Size];
            altr = new double[Size];
            autr = new double[Size];

            var Reader1 = new StreamReader(File.Open(PATH + "jptr.txt", FileMode.Open));
            var Reader2 = new StreamReader(File.Open(PATH + "altr.txt", FileMode.Open));
            var Reader3 = new StreamReader(File.Open(PATH + "autr.txt", FileMode.Open));

            for (int i = 0; i < Size; i++)
            {
                jptr[i] = Convert.ToInt32(Reader1.ReadLine().Split(Separator, StringSplitOptions.RemoveEmptyEntries)[0], CultureInfo.InvariantCulture);
                altr[i] = Convert.ToDouble(Reader2.ReadLine().Split(Separator, StringSplitOptions.RemoveEmptyEntries)[0], CultureInfo.InvariantCulture);
                autr[i] = Convert.ToDouble(Reader3.ReadLine().Split(Separator, StringSplitOptions.RemoveEmptyEntries)[0], CultureInfo.InvariantCulture);
            }

            Reader1.Close(); Reader2.Close(); Reader3.Close();
        }

        public void Mult_MV(Vector X, Vector Y)
        {
            for (int i = 0; i < N; i++) 
                Y.Elem[i] = X.Elem[i] * di[i];

            for (int i = 0; i < N; i++)
                for (int j = iptr[i] - 1; j < iptr[i + 1] - 1; j++)
                {
                    Y.Elem[i] += X.Elem[jptr[j] - 1] * altr[j];
                    Y.Elem[jptr[j] - 1] += X.Elem[i] * autr[j];
                }
        }

        public void Mult_MtV(Vector X, Vector Y)
        {
            for (int i = 0; i < N; i++) Y.Elem[i] = X.Elem[i] * di[i];

            for (int i = 0; i < N; i++)
                for (int j = iptr[i] - 1; j < iptr[i + 1] - 1; j++)
                {
                    Y.Elem[i] += X.Elem[jptr[j] - 1] * autr[j];
                    Y.Elem[jptr[j] - 1] += X.Elem[i] * altr[j];
                }
        }
    }
}