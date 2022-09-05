// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class NeuralNet
// {
//     int iNodes;//No. of input nodes
//     int hNodes;//No. of hidden nodes
//     int oNodes;//No. of output nodes
//     float learningRate;
//     float mr;

//     Matrix whi;//matrix containing weights between the input nodes and the hidden nodes
//     Matrix whh;//matrix containing weights between the hidden nodes and the second layer hidden nodes
//     Matrix woh;//matrix containing weights between the second hidden layer nodes and the output nodes

//     //---------------------------------------------------------------------------------------------------------------------------------------------------------  

//     //constructor
//     public NeuralNet(int inputs, int hiddenNo, int outputNo, float learningRate, float mutationRate)
//     {

//         //set dimensions from parameters
//         iNodes = inputs;
//         oNodes = outputNo;
//         hNodes = hiddenNo;
//         this.learningRate = learningRate;
//         mr = mutationRate;

//         //create first layer weights 
//         //included bias weight
//         whi = new Matrix(hNodes, iNodes + 1);

//         //create second layer weights
//         //include bias weight
//         whh = new Matrix(hNodes, hNodes + 1);

//         //create second layer weights
//         //include bias weight
//         woh = new Matrix(oNodes, hNodes + 1);

//         //set the matricies to random values
//         whi.randomize();
//         whh.randomize();
//         woh.randomize();
//     }
//     //---------------------------------------------------------------------------------------------------------------------------------------------------------  

//     //mutation function for genetic algorithm
//     public void mutate()
//     {
//         //mutates each weight matrix
//         whi.mutate(mr);
//         whh.mutate(mr);
//         woh.mutate(mr);
//     }

//     //---------------------------------------------------------------------------------------------------------------------------------------------------------  
//     //calculate the output values by feeding forward through the neural network
//     public float[] output(float[] inputsArr)
//     {

//         //convert array to matrix
//         //Note woh has nothing to do with it its just a function in the Matrix class
//         Matrix inputs = woh.singleColumnMatrixFromArray(inputsArr);

//         //add bias 
//         Matrix inputsBias = inputs.addBias();


//         //-----------------------calculate the guessed output

//         //apply layer one weights to the inputs
//         Matrix hiddenInputs = whi.dot(inputsBias);

//         //pass through activation function(sigmoid)
//         Matrix hiddenOutputs = hiddenInputs.activate();

//         //add bias
//         Matrix hiddenOutputsBias = hiddenOutputs.addBias();

//         //apply layer two weights
//         Matrix hiddenInputs2 = whh.dot(hiddenOutputsBias);
//         Matrix hiddenOutputs2 = hiddenInputs2.activate();
//         Matrix hiddenOutputsBias2 = hiddenOutputs2.addBias();

//         //apply level three weights
//         Matrix outputInputs = woh.dot(hiddenOutputsBias2);
//         //pass through activation function(sigmoid)
//         Matrix outputs = outputInputs.activate();

//         //convert to an array and return
//         return outputs.toArray();
//     }
//     //---------------------------------------------------------------------------------------------------------------------------------------------------------  
//     //crossover function for genetic algorithm
//     public NeuralNet crossover(NeuralNet partner)
//     {

//         //creates a new child with layer matrices from both parents
//         NeuralNet child = new NeuralNet(iNodes, hNodes, oNodes, learningRate, mr);
//         child.whi = whi.crossover(partner.whi);
//         child.whh = whh.crossover(partner.whh);
//         child.woh = woh.crossover(partner.woh);
//         return child;
//     }
//     //---------------------------------------------------------------------------------------------------------------------------------------------------------  
//     //return a neural net which is a clone of this Neural net
//     public NeuralNet clone()
//     {
//         NeuralNet clone = new NeuralNet(iNodes, hNodes, oNodes, learningRate, mr);
//         clone.whi = whi.clone();
//         clone.whh = whh.clone();
//         clone.woh = woh.clone();

//         return clone;
//     }
// }
