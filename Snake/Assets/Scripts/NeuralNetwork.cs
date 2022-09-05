using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NeuralNetwork
{
    //TODO: Find the correct object type for the following properties
    int In_Nodes;
    int Hid_Nodes;
    int Out_Nodes;

    public Matrix weights_In_Hid;
    public Matrix weights_Hid_Out;
    public Matrix weights_Hid_Hid;
    public Matrix bias_Hid;
    public Matrix bias_Hid2;
    public Matrix bias_Out;

    float learningRate; //Not used in NeuroEvolution (Genetic Algorithms), used for Gradient Descent in Neural Networks
    float mutationRate; // Used for NeuroEvolution not for Gradient Decent

    public NeuralNetwork(int In_Nodes, int Hid_Nodes, int Out_Nodes, float learnRate, float mutationRate) {
        this.In_Nodes = In_Nodes;
        this.Hid_Nodes = Hid_Nodes;
        this.Out_Nodes = Out_Nodes;

        weights_In_Hid = new Matrix(Hid_Nodes, In_Nodes + 1);
        weights_Hid_Hid = new Matrix(Hid_Nodes, Hid_Nodes + 1);
        weights_Hid_Out = new Matrix(Out_Nodes, Hid_Nodes + 1);

        weights_In_Hid.randomize();
        weights_Hid_Hid.randomize();
        weights_Hid_Out.randomize();
        

        // weights_In_Hid = new Matrix(Hid_Nodes, In_Nodes);
        // weights_Hid_Hid = new Matrix(Hid_Nodes, Hid_Nodes);
        // weights_Hid_Out = new Matrix(Out_Nodes, Hid_Nodes);

        // weights_In_Hid.randomize();
        // weights_Hid_Hid.randomize();
        // weights_Hid_Out.randomize();

        // bias_Hid = new Matrix(Hid_Nodes, 1);
        // bias_Hid2 = new Matrix(Hid_Nodes, 1);
        // bias_Out = new Matrix(Out_Nodes, 1);

        // bias_Hid.randomize();
        // bias_Hid2.randomize();
        // bias_Out.randomize();

        this.learningRate = learnRate;
        this.mutationRate = mutationRate;
    }
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

        // Matrix inputs = new Matrix(inputArray);
        // Matrix hidden = Matrix.multiply(weights_In_Hid, inputs);
        // hidden.add(bias_Hid);

        // hidden.applySigmoid();

        // Matrix inputs2 = Matrix.multiply(weights_Hid_Hid, hidden);
        // inputs2.applySigmoid();
        // inputs2.add(bias_Hid2);

        // Matrix output = Matrix.multiply(weights_Hid_Out, inputs2);
        // output.add(bias_Out);
        // output.applySigmoid();

        // return output.toArray();
    }

    public NeuralNetwork copy() {
        return new NeuralNetwork(this);
    }

    public void mutate() {
        weights_In_Hid.mutate(mutationRate);
        weights_Hid_Hid.mutate(mutationRate);
        weights_Hid_Out.mutate(mutationRate);

        //  weights_In_Hid.mutate(mutationRate);
        //  weights_Hid_Out.mutate(mutationRate);
        //  bias_Hid.mutate(mutationRate);
        //  bias_Out.mutate(mutationRate);
    }

    

    public NeuralNetwork crossover(NeuralNetwork partner) {
        NeuralNetwork child  = new NeuralNetwork(In_Nodes, Hid_Nodes, Out_Nodes, learningRate, mutationRate);
        child.weights_In_Hid = weights_In_Hid.crossover(partner.weights_In_Hid);
        child.weights_Hid_Hid = weights_Hid_Hid.crossover(partner.weights_Hid_Hid);
        child.weights_Hid_Out = weights_Hid_Out.crossover(partner.weights_Hid_Out);
        return child;

        // NeuralNetwork child = new NeuralNetwork(In_Nodes, Hid_Nodes, Out_Nodes, learningRate, mutationRate);
        // child.weights_In_Hid = weights_In_Hid.crossover(partner.weights_In_Hid);
        // child.weights_Hid_Hid = weights_Hid_Hid.crossover(partner.weights_Hid_Hid);
        // child.weights_Hid_Out = weights_Hid_Out.crossover(partner.weights_Hid_Out);
        // child.bias_Hid = bias_Hid.crossover(partner.bias_Hid);
        // child.bias_Hid2 = bias_Hid2.crossover(partner.bias_Hid2);
        // child.bias_Out = bias_Out.crossover(partner.bias_Out);
        // return child;
    }

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
