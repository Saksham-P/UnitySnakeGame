using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NeuralNetwork
{
    //Integers storing the number of input, hidden, and output nodes
    int In_Nodes;
    int Hid_Nodes;
    int Out_Nodes;

    //Matrices storing the weights and biases of nodes
    public Matrix weights_In_Hid;
    public Matrix weights_Hid_Out;
    public Matrix weights_Hid_Hid;
    public Matrix bias_Hid;
    public Matrix bias_Hid2;
    public Matrix bias_Out;

    float learningRate; //Not used in NeuroEvolution (Genetic Algorithms), used for Gradient Descent in Neural Networks

    //Mutatin Rate variable used to mutate certain weights of matrices by a certain amount
    float mutationRate; // Used for NeuroEvolution not for Gradient Decent

    //NeuralNetwork Constructor to create 2 Hidden Layers, and 1 Output Layer Neural Network
    public NeuralNetwork(int In_Nodes, int Hid_Nodes, int Out_Nodes, float learnRate, float mutationRate) {
        
        //Sets the integer variables for the number of nodes
        this.In_Nodes = In_Nodes;
        this.Hid_Nodes = Hid_Nodes;
        this.Out_Nodes = Out_Nodes;

        //Creating matrices for the edges conecting the layers
        weights_In_Hid = new Matrix(Hid_Nodes, In_Nodes + 1);
        weights_Hid_Hid = new Matrix(Hid_Nodes, Hid_Nodes + 1);
        weights_Hid_Out = new Matrix(Out_Nodes, Hid_Nodes + 1);

        //Randomize the matrices
        weights_In_Hid.randomize();
        weights_Hid_Hid.randomize();
        weights_Hid_Out.randomize();

        //Set the learning rate and mutation rate
        this.learningRate = learnRate;
        this.mutationRate = mutationRate;
    }

    //NeuralNetwork Constructor using another NeuralNetwork object
    public NeuralNetwork(NeuralNetwork nn) {
        // this.In_Nodes = nn.In_Nodes;
        // this.Hid_Nodes = nn.Hid_Nodes;
        // this.Out_Nodes = nn.Out_Nodes;

        // this.weights_In_Hid = nn.weights_In_Hid.copy();
        // this.weights_Hid_Hid = nn.weights_Hid_Hid.copy();
        // this.weights_Hid_Out = nn.weights_Hid_Out.copy();

        // this.bias_Hid = nn.bias_Hid.copy();
        // this.bias_Hid2 = nn.bias_Hid2.copy();
        // this.bias_Out = nn.bias_Out.copy();

        this.learningRate = nn.learningRate;
        this.mutationRate = nn.mutationRate;
    }

    //Feedforward Algorithm
    public float[] predict(float[] inputArray) {
        Matrix inputs = new Matrix(inputArray);
        Matrix inputsBias = inputs.addBias();

        Matrix hiddenInputs = Matrix.multiply(weights_In_Hid, inputsBias);
        hiddenInputs.applySigmoid();
        Matrix hiddenOutputsBias = hiddenInputs.addBias();

        Matrix hiddenInputs2 = Matrix.multiply(weights_Hid_Hid, hiddenOutputsBias);
        hiddenInputs2.applySigmoid();
        Matrix hiddenOutputsBias2 = hiddenInputs2.addBias();

        Matrix outputInputs = Matrix.multiply(weights_Hid_Out, hiddenOutputsBias2);
        outputInputs.applySigmoid();

        return outputInputs.toArray();
    }

    //Return a copy of this neural network
    public NeuralNetwork copy() {
        return new NeuralNetwork(this);
    }

    //Mutate the matrices using the mutationRate
    public void mutate() {
        weights_In_Hid.mutate(mutationRate);
        weights_Hid_Hid.mutate(mutationRate);
        weights_Hid_Out.mutate(mutationRate);

        //  weights_In_Hid.mutate(mutationRate);
        //  weights_Hid_Out.mutate(mutationRate);
        //  bias_Hid.mutate(mutationRate);
        //  bias_Out.mutate(mutationRate);
    }

    //Crossover the parent NeuralNetworks into a new child Neural Network
    public NeuralNetwork crossover(NeuralNetwork partner) {
        NeuralNetwork child  = new NeuralNetwork(In_Nodes, Hid_Nodes, Out_Nodes, learningRate, mutationRate);
        child.weights_In_Hid = weights_In_Hid.crossover(partner.weights_In_Hid);
        child.weights_Hid_Hid = weights_Hid_Hid.crossover(partner.weights_Hid_Hid);
        child.weights_Hid_Out = weights_Hid_Out.crossover(partner.weights_Hid_Out);
        return child;
    }

    //Print out the data for this Neural Network
    public void print() {
        string data = "Input Nodes: " + In_Nodes + "\n";
        data += "Hidden Nodes: " + Hid_Nodes + "\n";
        data += "Output Nodes: " + Out_Nodes + "\n";
        data += weights_In_Hid.print();
        data += "\n";
        data += weights_Hid_Out.print();
        data += "\n";
        data += bias_Hid.print();
        data += "\n";
        data += bias_Out.print();
        data += "\n";
        Debug.Log(data);
    }
}