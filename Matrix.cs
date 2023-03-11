


// Simple matrix library by Pawel Duplaga
// Its mediocre but its mine

/*=============================================================================
**
**
** Purpose: Matrix algebra ( especially for neural networks  )
**
**
=============================================================================*/

namespace MatrixAlgebraForDoubles
{
    public class Matrix
    {
        double[,] data;
        public int rows; //rows
        public int columns; //columns


        public Matrix(int rows, int cols) {

            if (rows <= 0 || cols <= 0)
                throw new ArgumentException("Matrix dimenstions must be positive");

            data = new double[rows, cols];
            this.rows = rows;
            this.columns = cols;
        }

        public Matrix(double[,] data)
        {
            this.data = data;
            this.rows = data.GetLength(0);
            this.columns = data.GetLength(1);
        }

        public double this[int i, int k]
        {
            get => data[i, k];
            set => data[i, k] = value;
        }


        #region OPERATORS 

        public static Matrix operator *(Matrix left, Matrix right)
        {
            if (left.columns != right.rows)
                throw new ArgumentException("Number of columns of the first matrix must be the same as number of rows of the second matrix");

            Matrix result = new Matrix(left.rows, right.columns);

            for (int i = 0; i < left.rows; i++)
            {
                for (int k = 0; k < right.columns; k++)
                {
                    double result_cell_value = default(double);
                    for (int j = 0; j < left.columns; j++)
                    {
                        result_cell_value += left[i, j] * right[j, k];
                    }
                    result[i, k] = result_cell_value;
                }
            }

            return result;
        }

        public static Matrix operator *(Matrix matrix, dynamic value)
        {
            Matrix result = new Matrix(matrix.rows, matrix.columns);

            for (int i = 0; i < result.rows; i++)
            {
                for (int k = 0; k < result.columns; k++)
                {
                    result[i, k] = matrix[i, k] * value;
                }
            }
            return result;
        }

        public static Matrix operator *(dynamic value, Matrix matrix)
        {
            Matrix result = new Matrix(matrix.rows, matrix.columns);

            for (int i = 0; i < result.rows; i++)
            {
                for (int k = 0; k < result.columns; k++)
                {
                    result[i, k] = matrix[i, k] * value;
                }
            }
            return result;
        }

        public static Matrix operator +(Matrix left, Matrix right)
        {
            if (left.columns != right.columns || left.rows != right.rows)
                throw new ArgumentException("Numbers of columns and rows must be the same in both matrixes");

            Matrix result = new Matrix(left.rows, left.columns);

            for (int i = 0; i < left.columns; i++)
            {
                for (int k = 0; k < left.rows; k++)
                {
                    result[i, k] = left[i, k] + right[i, k];
                }
            }

            return result;
        }

        public static Matrix operator +(Matrix left, dynamic value)
        {

            Matrix result = new Matrix(left.rows, left.columns);

            for (int i = 0; i < left.columns; i++)
            {
                for (int k = 0; k < left.rows; k++)
                {
                    result[i, k] = left[i, k] + value;
                }
            }

            return result;
        }

        public static Matrix operator -(Matrix left, Matrix right)
        {
            if (left.columns != right.columns || left.rows != right.rows)
                throw new ArgumentException("Numbers of columns and rows must be the same in both matrixes");

            Matrix result = new Matrix(left.rows, left.columns);

            for (int i = 0; i < left.columns; i++)
            {
                for (int k = 0; k < left.rows; k++)
                {
                    result[i, k] = left[i, k] - right[i, k];
                }
            }

            return result;
        }

        public static Matrix operator -(Matrix left, dynamic value)
        {
            Matrix result = new Matrix(left.rows, left.columns);

            for (int i = 0; i < left.columns; i++)
            {
                for (int k = 0; k < left.rows; k++)
                {
                    result = left[i, k] - value;
                }
            }

            return result;

        }



        #endregion

        #region FUNCTIONS

        public Matrix transpose()
        {
            Matrix result = new Matrix(this.rows, this.columns);

            for (int i = 0; i < result.columns; i++)
            {
                for (int k = 0; k < result.rows; k++)
                {
                    result[i, k] = this[this.rows - i - 1, this.columns - k - 1];
                }
            }

            return result;
        }

        public Matrix GetInverseMatrix()
        {
            if (this.rows != this.columns)
                throw new ArgumentException("This Matrix is not a square matrix and its impossible to get Inverse matrix of it");


            int n = this.rows;

            double det = this.Determinant();
;
            if (det == 0)
            {
                throw new ArgumentException("The matrix is singular and does not have an inverse.");
            }

            Matrix inverse = new Matrix(n,n);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Matrix submatrix = Helpers.GetSubmatrix(this, n, i, j);
                    double subdet = submatrix.Determinant();
                    double inverseValue = Math.Pow(-1, i + j) * subdet / det;
                    if (inverseValue == -0) inverseValue = 0;

                    inverse[j, i] = inverseValue;
                }
            }

            return inverse;
        }



        public double Determinant()
        {
            int size1 = this.rows;
            int size2 = this.columns;

            if (size1 == 1 && size2 == 1)
            {
                return this[0, 0];
            }
            else if (size1 == 2 && size2 == 2)
            {
                return Helpers.Determinant2x2(this);
            }
            else
            {
                double det = 0;

                for (int j = 0; j < size1; j++)
                {
                    double[,] submatrix = new double[size1 - 1, size2 - 1];

                    for (int i = 1; i < size1; i++)
                    {
                        for (int k = 0; k < size1; k++)
                        {
                            if (k < j)
                            {
                                submatrix[i - 1, k] = this[i, k];
                            }
                            else if (k > j)
                            {
                                submatrix[i - 1, k - 1] = this[i, k];
                            }
                        }
                    }

                    Matrix subMatrixObj = new Matrix(submatrix);
                    det += Math.Pow(-1, j) * this[0, j] * subMatrixObj.Determinant();
                }

                return det;
            }
        }

        public void FillRandomInRange(dynamic min, dynamic max)
        {
            Random rand = new Random();

            for (int i = 0; i < this.rows; i++)
            {
                for (int k = 0; k < this.columns; k++) { 
                
                    dynamic randomValue = default(double);
                    randomValue = Helpers.DoubleRandom((double)min, (double)max, rand);
                }
            }
        }

        public void FillWithValue(dynamic value)
        {
            for (int i = 0; i < this.rows; i++)
            {
                for (int k = 0; k < this.columns; k++)
                {
                    this[i, k] = (double)value;
                }
            }
        }


        public void PrintMatrix()
        {

            // Find the length of the longest element
            int maxLength = 0;
            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.columns; j++)
                {
                    int length = this[i, j].ToString().Length;
                    if (length > maxLength)
                    {
                        maxLength = length;
                    }
                }
            }

            // Print the array with even spacing
            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.columns; j++)
                {
                    string element = this[i, j].ToString();
                    Console.Write(element.PadLeft(maxLength) + " ");
                }
                Console.WriteLine();
            }
        }

        public Matrix Clone()
        {
            return (Matrix)this.MemberwiseClone();
        }


        #endregion

        #region HELPERS FUNCTIONS
        private class Helpers
        {
            public static double DoubleRandom(double min, double max, Random rand)
            {
                return rand.NextDouble() * (max - min) + min;
            }

            public static float FloatRandom(float min, float max, Random rand)
            {
                return (float)rand.NextDouble() * (max - min) + min;
            }

            public static double Determinant2x2(Matrix matrix)
            {
                if (matrix.columns != 2 && matrix.rows != 2)
                    throw new ArgumentException("this method is for 2x2 matrixes only");

                double a = matrix[0, 0];
                double b = matrix[0, 1];
                double c = matrix[1, 0];
                double d = matrix[1, 1];

                dynamic det = a * d - b * c;

                return det;
            }

            public static Matrix GetSubmatrix(Matrix matrix, int n, int row, int col)
            {
                Matrix submatrix = new Matrix(n - 1, n - 1);

                int subi = 0;
                for (int i = 0; i < n; i++)
                {
                    if (i == row)
                    {
                        continue;
                    }

                    int subj = 0;
                    for (int j = 0; j < n; j++)
                    {
                        if (j == col)
                        {
                            continue;
                        }

                        submatrix[subi, subj] = matrix[i, j];
                        subj++;
                    }

                    subi++;
                }

                return submatrix;
            }

        }
        #endregion
    }
}