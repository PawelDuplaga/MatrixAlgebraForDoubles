


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
            
            data= new double[rows, cols];
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

        public static Matrix operator - (Matrix left, dynamic value)
        {
            Matrix result = new Matrix(left.rows, left.columns);

            for(int i=0; i< left.columns; i++)
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


        #endregion



    }
}