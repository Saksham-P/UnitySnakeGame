using System.Collections;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PopulationManager : MonoBehaviour
{
    //reuires a reference to the Snake GameObject
    public GameObject snake;

    //Array containing all snakes in scene
    private Snake[] snakes;

    //Values to keep count of data of the current generation
    private int generation = 1; //Current Generation
    private int populationSize = 100; //The number of Snakes in a single population
    private int globalBest = 0; //The best score achieved over all Generations
    private long globalBestFitness = 0; //The best fitness achieved over all Generations

    //Variable to store the bestSnake Snake Object
    private Snake bestSnake;

    //variable to make sure multiple instances of save function aren't active
    private bool canSave = true;

    //Text variable to display text on screen
    public Text text;

    //On Program Awake, create *populationSize* snakes and add them to the array and scene
    private void Awake() {
        snakes = new Snake[populationSize];
        for (int i = 0; i < populationSize; i++) {
            snakes[i] = Instantiate(snake).GetComponent<Snake>();
        }
    }

    //TimeScale function to slow down or speed up game using a slider
    public void timeScale(float value) {
        Time.timeScale = value;
    }

    //Update function running each frame
    private void Update() {
        
        //Update the info about current generation
        getInfo();

        //If everyone from this generation is dead, run the next generation
        if (everyoneDead()) {
            nextGen();
        }

        //If F key pressed, and we are allowed to save, save the data into a file
        if (Input.GetKeyDown(KeyCode.F)) {
            if (canSave) {
                canSave = false;
                saveFile();
            }
        }
    }

    //Save File function to save bestSnakeData into a txt file
    private void saveFile() {
        string fileName = "bestSnakeData.txt";
        if (File.Exists(fileName))
            File.Delete(fileName);
        StreamWriter sr = File.CreateText(fileName);

        sr.Write("Input to Hidden:\n" + bestSnake.brain.weights_In_Hid.print() + "\n");
        sr.Write("Hidden to Hidden:\n" + bestSnake.brain.weights_Hid_Hid.print() + "\n");
        sr.Write("Hidden to Output:\n" + bestSnake.brain.weights_Hid_Out.print() + "\n");
        sr.Close();
        Debug.Log("FileSaved");
        canSave = true;
    }

    //check if every snake in the scene died
    private bool everyoneDead() {
        for(int i = 0; i < snakes.Length; i++) {
            if (snakes[i].alive) {
                return false;
            }
        }
        return true;
    }

    //Get the current information about the current generation and update the text
    private void getInfo() {
        int maxScore = 0;
        int snakesAlive = 0;
        for (int i = 0; i < snakes.Length; i++) {
            if (snakes[i].score > maxScore) {
                maxScore = snakes[i].score;
            }
            if (snakes[i].alive) {
                snakesAlive++;
            }
        }
        if (maxScore > globalBest) {
            globalBest = maxScore;
        }
        text.text = "Generation: " + generation + "\t\tCurrent Best: " + maxScore + "\t\tAll Time Best: " + globalBest + 
        "\nAll Time Fitness: " + globalBestFitness + "\t\tSnakes Alive: " + snakesAlive;
    }

    //Run the next generation of snakes using the best 2 snakes from last generation
    private void nextGen() {

        //Create Snake array to store new gen snakes
        Snake[] nextGenSnakes = new Snake[populationSize];

        //Go through the population of previous gen snakes and pick 2 parents for each snake
        for (int i = 0; i < nextGenSnakes.Length; i++) {
            Snake parent1 = pickSnake();
            Snake parent2 = pickSnake();

            nextGenSnakes[i] = Instantiate(snake).GetComponent<Snake>();
            NeuralNetwork childBrain = parent1.crossover(parent2);
            nextGenSnakes[i].setBrain(childBrain);
            nextGenSnakes[i].brain.mutate();
        }

        //delete all the previous gen snakes
        for (int i = 0; i < snakes.Length; i++) {
            snakes[i].del();
        }

        //Copy the new gen snakes into the old array
        snakes = new Snake[populationSize];
        nextGenSnakes.CopyTo(snakes, 0);

        //increment the generation value to next gen
        generation++;
    }

    //Pick a snake based on it's fitness
    private Snake pickSnake() {

        long fitnessSum = 0;
        long maxFitness = 0;
        Snake temp = null;

        //Go through the snakes and find the max fitness and calculate total fitness of all snakes
        for (int i = 0; i < snakes.Length; i++) {
            fitnessSum += snakes[i].fitness;
            if (snakes[i].fitness > maxFitness) {
                maxFitness = snakes[i].fitness;
                temp = snakes[i];
            }
        }
        if(maxFitness > globalBestFitness) {
            globalBestFitness = maxFitness;
            bestSnake = temp;
        }
        
        //choose a random number between 0 and fitness Sum
        long randomNum = Mathf.FloorToInt(UnityEngine.Random.Range(0, fitnessSum));
        long runningSum = 0;

        //Select a snake using the current runningSum and the randomNum
        for (int i = 0; i < snakes.Length; i++) {
            runningSum += snakes[i].fitness;
            if (runningSum > randomNum) {
                //Debug.Log("Fitness Chosen: " + snakes[i].fitness);
                return snakes[i];
            }
        }

        Debug.Log("Something Went Wrong! (Snake couldn't be chosen for next Generation)");
        return null;
    }
}
