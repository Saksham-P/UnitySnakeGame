using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Matrix
{
    // //local variables
    // int rows;
    // int cols;
    // float[,] matrix;

    // //---------------------------------------------------------------------------------------------------------------------------------------------------------  
    // //constructor
    // public Matrix(int r, int c)
    // {
    //     rows = r;
    //     cols = c;
    //     matrix = new float[rows, cols];
    // }

    // //---------------------------------------------------------------------------------------------------------------------------------------------------------  
    // //constructor from 2D array
    // public Matrix(float[,] m)
    // {
    //     matrix = m;
    //     cols = m.GetLength(0);
    //     rows = m.GetLength(1);
    // }

    // //---------------------------------------------------------------------------------------------------------------------------------------------------------  
    // //print matrix
    // public void output()
    // {
    //     for (int i = 0; i < rows; i++)
    //     {
    //         for (int j = 0; j < cols; j++)
    //         {
    //             Debug.Log(matrix[i,j] + "  ");
    //         }
    //         Debug.Log("\n");
    //     }
    //     Debug.Log("\n");
    // }
    // //---------------------------------------------------------------------------------------------------------------------------------------------------------  

    // //multiply by scalar
    // public void multiply(float n)
    // {

    //     for (int i = 0; i < rows; i++)
    //     {
    //         for (int j = 0; j < cols; j++)
    //         {
    //             matrix[i,j] *= n;
    //         }
    //     }
    // }

    // //---------------------------------------------------------------------------------------------------------------------------------------------------------  
    // //return a matrix which is this matrix dot product parameter matrix 
    // public Matrix dot(Matrix n)
    // {
    //     Matrix result = new Matrix(rows, n.cols);

    //     if (cols == n.rows)
    //     {
    //         //for each spot in the new matrix
    //         for (int i = 0; i < rows; i++)
    //         {
    //             for (int j = 0; j < n.cols; j++)
    //             {
    //                 float sum = 0;
    //                 for (int k = 0; k < cols; k++)
    //                 {
    //                     sum += matrix[i,k] * n.matrix[k,j];
    //                 }
    //                 result.matrix[i,j] = sum;
    //             }
    //         }
    //     }

    //     return result;
    // }
    // //---------------------------------------------------------------------------------------------------------------------------------------------------------  
    // //set the matrix to random ints between -1 and 1
    // public void randomize()
    // {
    //     for (int i = 0; i < rows; i++)
    //     {
    //         for (int j = 0; j < cols; j++)
    //         {
    //             matrix[i,j] = Random.Range(-1.0f, 1.0f);
    //         }
    //     }
    // }

    // //---------------------------------------------------------------------------------------------------------------------------------------------------------  
    // //add a scalar to the matrix
    // public void Add(float n)
    // {
    //     for (int i = 0; i < rows; i++)
    //     {
    //         for (int j = 0; j < cols; j++)
    //         {
    //             matrix[i,j] += n;
    //         }
    //     }
    // }
    // //---------------------------------------------------------------------------------------------------------------------------------------------------------  
    // ///return a matrix which is this matrix + parameter matrix
    // public Matrix add(Matrix n)
    // {
    //     Matrix newMatrix = new Matrix(rows, cols);
    //     if (cols == n.cols && rows == n.rows)
    //     {
    //         for (int i = 0; i < rows; i++)
    //         {
    //             for (int j = 0; j < cols; j++)
    //             {
    //                 newMatrix.matrix[i,j] = matrix[i,j] + n.matrix[i,j];
    //             }
    //         }
    //     }
    //     return newMatrix;
    // }
    // //---------------------------------------------------------------------------------------------------------------------------------------------------------  
    // //return a matrix which is this matrix - parameter matrix
    // public Matrix subtract(Matrix n)
    // {
    //     Matrix newMatrix = new Matrix(cols, rows);
    //     if (cols == n.cols && rows == n.rows)
    //     {
    //         for (int i = 0; i < rows; i++)
    //         {
    //             for (int j = 0; j < cols; j++)
    //             {
    //                 newMatrix.matrix[i,j] = matrix[i,j] - n.matrix[i,j];
    //             }
    //         }
    //     }
    //     return newMatrix;
    // }
    // //---------------------------------------------------------------------------------------------------------------------------------------------------------  
    // //return a matrix which is this matrix * parameter matrix (element wise multiplication)
    // public Matrix multiply(Matrix n)
    // {
    //     Matrix newMatrix = new Matrix(rows, cols);
    //     if (cols == n.cols && rows == n.rows)
    //     {
    //         for (int i = 0; i < rows; i++)
    //         {
    //             for (int j = 0; j < cols; j++)
    //             {
    //                 newMatrix.matrix[i,j] = matrix[i,j] * n.matrix[i,j];
    //             }
    //         }
    //     }
    //     return newMatrix;
    // }
    // //---------------------------------------------------------------------------------------------------------------------------------------------------------  
    // //return a matrix which is the transpose of this matrix
    // public Matrix transpose()
    // {
    //     Matrix n = new Matrix(cols, rows);
    //     for (int i = 0; i < rows; i++)
    //     {
    //         for (int j = 0; j < cols; j++)
    //         {
    //             n.matrix[j,i] = matrix[i,j];
    //         }
    //     }
    //     return n;
    // }
    // //---------------------------------------------------------------------------------------------------------------------------------------------------------  
    // //Creates a single column array from the parameter array
    // public Matrix singleColumnMatrixFromArray(float[] arr)
    // {
    //     Matrix n = new Matrix(arr.Length, 1);
    //     for (int i = 0; i < arr.Length; i++)
    //     {
    //         n.matrix[i,0] = arr[i];
    //     }
    //     return n;
    // }
    // //---------------------------------------------------------------------------------------------------------------------------------------------------------  
    // //sets this matrix from an array
    // public void fromArray(float[] arr)
    // {
    //     for (int i = 0; i < rows; i++)
    //     {
    //         for (int j = 0; j < cols; j++)
    //         {
    //             matrix[i,j] = arr[j + i * cols];
    //         }
    //     }
    // }
    // //---------------------------------------------------------------------------------------------------------------------------------------------------------    
    // //returns an array which represents this matrix
    // public float[] toArray()
    // {
    //     float[] arr = new float[rows * cols];
    //     for (int i = 0; i < rows; i++)
    //     {
    //         for (int j = 0; j < cols; j++)
    //         {
    //             arr[j + i * cols] = matrix[i,j];
    //         }
    //     }
    //     return arr;
    // }

    // //---------------------------------------------------------------------------------------------------------------------------------------------------------  
    // //for ix1 matrixes adds one to the bottom
    // public Matrix addBias()
    // {
    //     Matrix n = new Matrix(rows + 1, 1);
    //     for (int i = 0; i < rows; i++)
    //     {
    //         n.matrix[i,0] = matrix[i,0];
    //     }
    //     n.matrix[rows,0] = 1;
    //     return n;
    // }
    // //---------------------------------------------------------------------------------------------------------------------------------------------------------  
    // //applies the activation function(sigmoid) to each element of the matrix
    // public Matrix activate()
    // {
    //     Matrix n = new Matrix(rows, cols);
    //     for (int i = 0; i < rows; i++)
    //     {
    //         for (int j = 0; j < cols; j++)
    //         {
    //             n.matrix[i,j] = sigmoid(matrix[i,j]);
    //         }
    //     }
    //     return n;
    // }

    // //---------------------------------------------------------------------------------------------------------------------------------------------------------    
    // //sigmoid activation function
    // public float sigmoid(float x)
    // {
    //     float y = 1 / (1 + (float)Mathf.Exp(-x));
    //     return y;
    // }
    // //returns the matrix that is the derived sigmoid function of the current matrix
    // public Matrix sigmoidDerived()
    // {
    //     Matrix n = new Matrix(rows, cols);
    //     for (int i = 0; i < rows; i++)
    //     {
    //         for (int j = 0; j < cols; j++)
    //         {
    //             n.matrix[i,j] = (matrix[i,j] * (1 - matrix[i,j]));
    //         }
    //     }
    //     return n;
    // }

    // //---------------------------------------------------------------------------------------------------------------------------------------------------------  
    // //returns the matrix which is this matrix with the bottom layer removed
    // public Matrix removeBottomLayer()
    // {
    //     Matrix n = new Matrix(rows - 1, cols);
    //     for (int i = 0; i < n.rows; i++)
    //     {
    //         for (int j = 0; j < cols; j++)
    //         {
    //             n.matrix[i,j] = matrix[i,j];
    //         }
    //     }
    //     return n;
    // }
    // //---------------------------------------------------------------------------------------------------------------------------------------------------------  
    // //Mutation function for genetic algorithm 

    // public void mutate(float mutationRate)
    // {

    //     //for each element in the matrix
    //     for (int i = 0; i < rows; i++)
    //     {
    //         for (int j = 0; j < cols; j++)
    //         {
    //             float rand = Random.Range(0.0f, 1.0f);
    //             if (rand < mutationRate)
    //             {//if chosen to be mutated
    //                 matrix[i,j] += randomGaussian(0, 1) / 5;//add a random value to it(can be negative)

    //                 //set the boundaries to 1 and -1
    //                 if (matrix[i,j] > 1)
    //                 {
    //                     matrix[i,j] = 1;
    //                 }
    //                 if (matrix[i,j] < -1)
    //                 {
    //                     matrix[i,j] = -1;
    //                 }
    //             }
    //         }
    //     }
    // }

    //Gaussian Method
    private float randomGaussian(float mean, float stddev)
        {
            // The method requires sampling from a uniform random of (0,1]
            // but Random.NextDouble() returns a sample of [0,1).
            float x1 = Random.Range(0.01f, 1);
            float x2 = Random.Range(0.01f, 1);

            float y1 = Mathf.Sqrt(-2.0f * Mathf.Log(x1)) * Mathf.Cos(2.0f * Mathf.PI * x2);
            return y1 * stddev + mean;
        }
    // //---------------------------------------------------------------------------------------------------------------------------------------------------------  
    // //returns a matrix which has a random number of values from this matrix and the rest from the parameter matrix
    // public Matrix crossover(Matrix partner)
    // {
    //     Matrix child = new Matrix(rows, cols);

    //     //pick a random point in the matrix
    //     int randC = Mathf.FloorToInt(Random.Range(0.0f, cols));
    //     int randR = Mathf.FloorToInt(Random.Range(0.0f, rows));
    //     for (int i = 0; i < rows; i++)
    //     {
    //         for (int j = 0; j < cols; j++)
    //         {

    //             if ((i < randR) || (i == randR && j <= randC))
    //             { //if before the random point then copy from this matric
    //                 child.matrix[i,j] = matrix[i,j];
    //             }
    //             else
    //             { //if after the random point then copy from the parameter array
    //                 child.matrix[i,j] = partner.matrix[i,j];
    //             }
    //         }
    //     }
    //     return child;
    // }
    // //---------------------------------------------------------------------------------------------------------------------------------------------------------  
    // //return a copy of this matrix
    // public Matrix clone()
    // {
    //     Matrix clone = new Matrix(rows, cols);
    //     for (int i = 0; i < rows; i++)
    //     {
    //         for (int j = 0; j < cols; j++)
    //         {
    //             clone.matrix[i,j] = matrix[i,j];
    //         }
    //     }
    //     return clone;
    // }
    public float[][] mat;
    int rows;
    int cols;

    public Matrix(int rows, int cols) {
        this.rows = rows;
        this.cols = cols;

        mat = createMatrix(rows, cols);
    }

    public Matrix(float[] arr) {
        rows = arr.Length;
        cols = 1;
        mat = createMatrix(rows, cols);
        for (int i = 0; i < rows; i++) {
            mat[i][0] = arr[i];
        }
    }

    public float[][] createMatrix(int rows, int cols) {
        float[][] result = new float[rows][];
        for (int i = 0; i < rows; ++i)
            result[i] = new float[cols];
        return result;
    }

    public void randomize() {
        for (int i = 0; i < rows; i++) {
            for (int j = 0 ; j < cols; j++) {
                mat[i][j] = Random.Range(-1.0f, 1.0f);
            }
        }
    }

    public void mutate(float mutationRate) {
        for (int i = 0; i < rows; i++) {
            for (int j = 0 ; j < cols; j++) {
                float ran = Random.Range(0.0f, 1.0f);
                if (ran < mutationRate) {
                    float rand = Random.Range(-0.20f, 0.20f);
                    mat[i][j] += rand;
                }
                if (mat[i][j] > 1) {
                    mat[i][j] = 1;
                }
                else if (mat[i][j] < -1) {
                    mat[i][j] = -1;
                }
            }
        }
    }

    public Matrix crossover(Matrix partner) {
        Matrix child = new Matrix(rows, cols);

        int randomRow = Mathf.RoundToInt(Random.Range(0, rows));
        int randomCol = Mathf.RoundToInt(Random.Range(0, cols));

        for (int i = 0; i < rows; i++) {
            for (int j = 0 ; j < cols; j++) {
                if ((i < randomRow) || (i == randomRow && j <=randomCol)) {
                    child.mat[i][j] = mat[i][j];
                }
                else {
                    child.mat[i][j] = partner.mat[i][j];
                }
            }
        }
        return child;
    }

    // private float bellCurve(float x) {
    //     return (1/(0.1f * Mathf.Sqrt(2 * Mathf.PI))) * Mathf.Exp((-1/2) * Mathf.Pow(((x - 0.5f)/0.11f),2));
    // }

    public Matrix copy() {
        Matrix temp = new Matrix(rows, cols);
         for (int i = 0; i < rows; i++) {
            for (int j = 0 ; j < cols; j++) {
                temp.mat[i][j] = mat[i][j];
            }
        }
        return temp;
    }

    public void add(float n) {
        for (int i = 0; i < rows; i++) {
            for (int j = 0 ; j < cols; j++) {
                mat[i][j] += n;
            }
        }
    }

    public Matrix add(Matrix n) {
        if (rows != n.rows || cols != n.cols) {
            Debug.Log("Matrices are not compatible to be added together");
        }
        Matrix newMatrix = new Matrix(rows, cols);
        for (int i = 0; i < rows; i++) {
            for (int j = 0 ; j < cols; j++) {
                newMatrix.mat[i][j] = mat[i][j] + n.mat[i][j];
            }
        }
        return newMatrix;
    }

    public void multiply(float n) {
        for (int i = 0; i < rows; i++) {
            for (int j = 0 ; j < cols; j++) {
                mat[i][j] *= n;
            }
        }
    }

    public static Matrix multiply(Matrix a, Matrix b) {
        int aRows = a.rows;
        int aCols = a.cols;
        int bRows = b.rows;
        int bCols = b.cols;

        if (aCols != bRows) {
            Debug.Log("Error! Matrices not compatible to Multiply");
            return null;
        }

        Matrix temp = new Matrix(aRows, bCols);

        for (int i = 0; i < aRows; ++i) // each row of A
            for (int j = 0; j < bCols; ++j) // each col of B
                for (int k = 0; k < aCols; ++k) // could use k < bRows
                    temp.mat[i][j] += a.mat[i][k] * b.mat[k][j];

        return temp;
    }

    public void applySigmoid() {
        for (int i = 0; i < rows; i++) {
            for (int j = 0 ; j < cols; j++) {
                //Sigmoid Function
                mat[i][j] =  1 / (1 + Mathf.Exp(mat[i][j] * -1));
            }
        }

    }

    public Matrix addBias() {
        Matrix newMat = new Matrix(rows+1, cols);
        for (int i = 0; i < rows; i++) {
            newMat.mat[i][0] = mat[i][0];
        }
        newMat.mat[rows][0] = 1;
        return newMat;
    }

    public float[] toArray() {
        int pro = cols * rows;
        float[] temp = new float[pro];
        int count = 0;
        for (int i = 0; i < rows; i++) {
            for (int j = 0 ; j < cols; j++) {
                temp[count] = mat[i][j];
                count++;
            }
        }
        return temp;
    }

    public string print() {
        string temp = "";
        for (int i = 0; i < rows; i++) {
            if (i != 0)
                temp = temp + "\n";
            for (int j = 0 ; j < cols; j++) {
                if (j != cols-1)
                    temp = temp + (mat[i][j]).ToString() + ", ";
                else 
                    temp = temp + (mat[i][j]).ToString();
            }
        }
        return temp;
    }
}

