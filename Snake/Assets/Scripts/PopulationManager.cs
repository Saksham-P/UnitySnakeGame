using System.Collections;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PopulationManager : MonoBehaviour
{
    public GameObject snake;
    private Snake[] snakes;
    private int generation = 1; //Current Generation
    private int populationSize = 100; //The number of Snakes in a single population
    private int globalBest = 0; //The best score achieved over all Generations
    private long globalBestFitness = 0; //The best fitness achieved over all Generations
    private Snake bestSnake;
    private bool canSave = true;
    public Text text;

    private void Awake() {
        snakes = new Snake[populationSize];
        for (int i = 0; i < populationSize; i++) {
            snakes[i] = Instantiate(snake).GetComponent<Snake>();
        }
    }

    public void timeScale(float value) {
        Time.timeScale = value;
    }

    private void Update() {
        getInfo();

        if (everyoneDead()) {
            nextGen();
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            if (canSave) {
                canSave = false;
                saveFile();
            }
        }
    }

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

    private bool everyoneDead() {
        for(int i = 0; i < snakes.Length; i++) {
            if (snakes[i].alive) {
                return false;
            }
        }
        return true;
    }

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

    private void nextGen() {
        Snake[] nextGenSnakes = new Snake[populationSize];

        for (int i = 0; i < nextGenSnakes.Length; i++) {
            Snake parent1 = pickSnake();
            Snake parent2 = pickSnake();

            nextGenSnakes[i] = Instantiate(snake).GetComponent<Snake>();
            NeuralNetwork childBrain = parent1.crossover(parent2);
            nextGenSnakes[i].setBrain(childBrain);
            nextGenSnakes[i].brain.mutate();
        }
        for (int i = 0; i < snakes.Length; i++) {
            snakes[i].del();
        }
        snakes = new Snake[populationSize];
        nextGenSnakes.CopyTo(snakes, 0);

        generation++;
    }

    private Snake pickSnake() {
        long fitnessSum = 0;
        long maxFitness = 0;
        Snake temp = null;
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
        
        long randomNum = Mathf.FloorToInt(UnityEngine.Random.Range(0, fitnessSum));

        long runningSum = 0;

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
