using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Matrix
{
    //Jagged Array that stores the values of the matrix
    public float[][] mat;

    //rows of the matrix and the columns of the matrix
    int rows;
    int cols;

    //Matrix constructor with rows and columns as parameters
    public Matrix(int rows, int cols) {

        //initializing the variables
        this.rows = rows;
        this.cols = cols;

        mat = createMatrix(rows, cols);
    }

    //Matrix constructor with an array as the parameter
    public Matrix(float[] arr) {

        //the amaount of rows is equal to the length of the array with 1 column
        rows = arr.Length;
        cols = 1;

        //create the matrix 
        mat = createMatrix(rows, cols);

        //fill in the matrix
        for (int i = 0; i < rows; i++) {
            mat[i][0] = arr[i];
        }
    }

    //create the skeleton of matrix by returning Jagged Array
    public float[][] createMatrix(int rows, int cols) {
        float[][] result = new float[rows][];
        for (int i = 0; i < rows; ++i)
            result[i] = new float[cols];
        return result;
    }

    //Randomize the values of the matrix
    public void randomize() {
        for (int i = 0; i < rows; i++) {
            for (int j = 0 ; j < cols; j++) {
                mat[i][j] = Random.Range(-1.0f, 1.0f);
            }
        }
    }

    //Mutate the current Matrix by changing only mutationRate percentage of values in the array by a random number between -0.20 - 0.20
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

    //crossover the two parent matrices to create a new child matrix with values from both parents
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

    //return a copy of this matrix
    public Matrix copy() {
        Matrix temp = new Matrix(rows, cols);
         for (int i = 0; i < rows; i++) {
            for (int j = 0 ; j < cols; j++) {
                temp.mat[i][j] = mat[i][j];
            }
        }
        return temp;
    }

    //Scalar matrix addition
    public void add(float n) {
        for (int i = 0; i < rows; i++) {
            for (int j = 0 ; j < cols; j++) {
                mat[i][j] += n;
            }
        }
    }

    //Matrix addition with another matrix
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

    //Scalar matrix multiplication
    public void multiply(float n) {
        for (int i = 0; i < rows; i++) {
            for (int j = 0 ; j < cols; j++) {
                mat[i][j] *= n;
            }
        }
    }

    //Matrix multiplication with another matrix
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

    //Apply sigmoid function on matrix for normalization
    public void applySigmoid() {
        for (int i = 0; i < rows; i++) {
            for (int j = 0 ; j < cols; j++) {
                //Sigmoid Function
                mat[i][j] =  1 / (1 + Mathf.Exp(mat[i][j] * -1));
            }
        }

    }

    //Create a new row in the matrix to add the biases into the matrix
    public Matrix addBias() {
        Matrix newMat = new Matrix(rows+1, cols);
        for (int i = 0; i < rows; i++) {
            newMat.mat[i][0] = mat[i][0];
        }
        newMat.mat[rows][0] = 1;
        return newMat;
    }

    //Converting the matrix to an array
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

    //Printing out the matrix
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