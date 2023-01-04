using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Snake : MonoBehaviour
{
    //Initial state of the snake ---------------------------------------------------------------------
    private Vector2 direction = Vector2.right;
    private List<Transform> segments = new List<Transform>();
    public Transform bodySegment;
    public Transform food;
    public int initialSize = 3;

    //Genetic Algorithm Variables --------------------------------------------------------------------------
    public NeuralNetwork brain;
    private float[] inputs;
    private float[] outputs;

    //Number of Nodes and Mutation Rate --------------------------------------------------------------------
    private int inputNodes = 24;
    private int hiddenNodes = 16;
    private int outputNodes = 4;
    private float mutationRate = 0.33f;

    //Snake Information ------------------------------------------------------------------------------------
    private int movesDone = 0;
    public int score = 0;
    public long fitness = 0;
    private long fitnessScore = 0;
    private Vector3 previousPos;
    private int movesLeft = 200;
    public bool alive = true;

    //Bounds of the Walls (This is the farthest values Snake can go ~ Inclusive)
    private int leftWall;
    private int rightWall;
    private int topWall;
    private int bottomWall;

    //Variable that chooses whether a player will be playing or an Nachine Learning Algorithm
    private bool playMode = false;
    public Transform check;
    private List<Transform> checkObject = new List<Transform>();

    //On Awake, create the food, Neural Network and the Bounds for Snake
    private void Awake() {
        food = Instantiate(food);

        brain = new NeuralNetwork(inputNodes, hiddenNodes, outputNodes, 0.0f, mutationRate);
        inputs = new float[inputNodes];
        outputs = new float[outputNodes];

        leftWall = (int)Mathf.Round(food.GetComponent<Food>().gridArea.bounds.min.x);
        rightWall = (int)Mathf.Round(food.GetComponent<Food>().gridArea.bounds.max.x);
        topWall = (int)Mathf.Round(food.GetComponent<Food>().gridArea.bounds.max.y);
        bottomWall = (int)Mathf.Round(food.GetComponent<Food>().gridArea.bounds.min.y);

        resetState();
    }

    //Keep checking for user input if in playMode
    private void Update()
    {
        if (playMode) {
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && direction != Vector2.down) {
                direction = Vector2.up;
                movesLeft--;
                movesDone++;
            }
            else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) && direction != Vector2.right)) {
                direction = Vector2.left;
                movesLeft--;
                movesDone++;
            }
            else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && direction != Vector2.up) {
                direction = Vector2.down;
                movesLeft--;
                movesDone++;
            }
            else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && direction != Vector2.left) {
                direction = Vector2.right;
                movesLeft--;
                movesDone++;
            }
        }
    }

    //Loop that runs regardless of user frames per second
    private void FixedUpdate() {

        //If not in playmode, decide the next move snake is going to do
        if (!playMode) {
            // for (int i = 0; i < checkObject.Count; i++) {
            //     Destroy(checkObject[i].gameObject);
            // }
            // checkObject = new List<Transform>();
            decide();
            // Debug.Break();
        }

        //Decrement the moves left and increment the moves done
        movesDone += 1;
        movesLeft -= 1;
        fitnessScore -= 1;

        //Set the variable for snake prev position
        previousPos = transform.position;

        //Set the position of all the segments
        for (int i = segments.Count - 1; i > 0; i--) {
            segments[i].position = segments[i-1].position;
        }

        //Round the position of the snakes to give the pixel snake feeling
        transform.position = new Vector3(
            Mathf.Round(transform.position.x) + direction.x,
            Mathf.Round(transform.position.y) + direction.y,
            0.0f
        );

        //If getting closer to the food particle, then increase fitness, otherwise decrease fitness
        if (Mathf.Abs((food.position - transform.position).magnitude) < Mathf.Abs((food.position - previousPos).magnitude)) {
            fitnessScore += 2;
        }
        else {
            fitnessScore -= 1;
        }
    }

    //Grow the Snake if Snake ate food
    private void grow() {
        Transform segment = Instantiate(bodySegment);
        segment.position = segments[segments.Count - 1].position;
        segments.Add(segment);
        movesLeft += 100;
        fitnessScore += 100;
        score++;
    }

    //Reset the State of the snake by destroying it's segments, and clearing the segments from scene
    private void resetState() {
        for (int i = 1; i < segments.Count; i++) {
            Destroy(segments[i].gameObject);
        }
        segments.Clear();
        segments.Add(transform);

        for (int i = 1; i < initialSize; i++) {
            segments.Add(Instantiate(bodySegment));
        }
        transform.position = Vector3.zero;
    }

    //If Snake collides with another object
    private void OnTriggerEnter2D(Collider2D other) {
        
        //If the collided object is a food particle, grow the snake and randomize food again
        if (other == food.GetComponent<Collider2D>()) {
            food.GetComponent<Food>().randomizePosition();
            grow();
        }
        
        //If collided object is an obstacle or Wall, resetState if in playMode, otherwise kill the Snake
        if (other.tag == "Obstacle" || other.tag == "Wall") {
            if (playMode)
                resetState();
            else {
                if (other.tag == "Wall") {
                    //Debug.Log("HIt the Wall");
                    fitnessScore -= 75;
                    Died();
                }
                else if (segments.Contains(other.transform)){
                    fitnessScore -=75;
                    //movesDone -= 5;
                    //Debug.Log("Hit my Tail");
                    Died();
                }
            }
        }
    }

    //Genetic Algorithm Functions----------------------------------------------------------------------
    private void mutate() {
        brain.mutate();
    }

    //Decide the next move for snake
    private void decide() {
        //Fills the Input Array
        look();

        //Gets the outputs
        outputs = brain.predict(inputs);

        //Calculate which decision to take
        float max = 0;
        int maxIndex = 0;
        for(int i = 0; i < outputs.Length; i++) {
            if (outputs[i] > max) {
                max = outputs[i];
                maxIndex = i;
            }
        }

        //Which direction to move depending on the decision
        if (maxIndex == 0) {
            // Debug.Log("Moving up");
            if (direction != Vector2.up && direction != Vector2.down)
                move(Vector2.up);
        }
        else if (maxIndex == 1) {
            // Debug.Log("Moving left");
            if (direction != Vector2.left && direction != Vector2.right)
              move(Vector2.left);
        }
        else if (maxIndex == 2) {
            // Debug.Log("Moving down");
            if (direction != Vector2.down && direction != Vector2.up)
                move(Vector2.down);
        }
        else if (maxIndex == 3) {
            // Debug.Log("Moving right");
            if (direction != Vector2.right && direction != Vector2.left)
              move(Vector2.right);
        }
    }

    //Observes the surroundings from the Snake head
    private void look() {
        // Debug.Log("Checking Left");
        float[] temp = lookInDirection(Vector2.left);
        inputs[0] = temp[0];
        inputs[1] = temp[1];
        inputs[2] = temp[2];
        // Debug.Log("Checking Top Left");
        temp = lookInDirection(Vector2.left + Vector2.up);
        inputs[3] = temp[0];
        inputs[4] = temp[1];
        inputs[5] = temp[2];
        // Debug.Log("Checking Up");
        temp = lookInDirection(Vector2.up);
        inputs[6] = temp[0];
        inputs[7] = temp[1];
        inputs[8] = temp[2];
        // Debug.Log("Checking Top Right");
        temp = lookInDirection(Vector2.up + Vector2.right);
        inputs[9] = temp[0];
        inputs[10] = temp[1];
        inputs[11] = temp[2];
        // Debug.Log("Checking Right");
        temp = lookInDirection(Vector2.right);
        inputs[12] = temp[0];
        inputs[13] = temp[1];
        inputs[14] = temp[2];
        // Debug.Log("Checking Down Right");
        temp = lookInDirection(Vector2.right + Vector2.down);
        inputs[15] = temp[0];
        inputs[16] = temp[1];
        inputs[17] = temp[2];
        // Debug.Log("Checking Down");
        temp = lookInDirection(Vector2.down);
        inputs[18] = temp[0];
        inputs[19] = temp[1];
        inputs[20] = temp[2];
        // Debug.Log("Checking Down Left");
        temp = lookInDirection(Vector2.down + Vector2.left);
        inputs[21] = temp[0];
        inputs[22] = temp[1];
        inputs[23] = temp[2];

        // string temp1 = "";
        // for (int i = 0; i < inputs.Length; i++) {
        //     temp1 += (inputs[i] + "(" + i + ") | ");
        // }
        // Debug.Log(temp1);
    }

    //Look in the specified direction from the Snake Head
    private float[] lookInDirection(Vector2 direction) {

        //Create an array to store observations
        float[] visionInDirection = new float[3];
        Vector2 positionLooking = new Vector2(transform.position.x, transform.position.y);
        bool foodFound = false;
        bool tailFound = false;
        float distance = 0;

        // checkObject.Add(Instantiate(check, positionLooking, Quaternion.identity));

        positionLooking += direction;
        distance += 1;

        while (!(positionLooking.x < leftWall || positionLooking.y < bottomWall || positionLooking.x > rightWall || positionLooking.y > topWall)) {
            // checkObject.Add(Instantiate(check, positionLooking, Quaternion.identity));

            //If food found
            if (!foodFound && positionLooking.x == food.position.x && positionLooking.y == food.position.y) {
                visionInDirection[0] = 1;
                foodFound = true;
                // Debug.Log("Food Found");
            }

            //If tail found
            if (!tailFound && isTail(positionLooking.x, positionLooking.y)) {
                visionInDirection[1] = 1/distance;
                tailFound = true;
                // Debug.Log("Tail Found");
            }

            //Distance until Wall
            positionLooking += direction;
            distance += 1;
        }

        visionInDirection[2] = 1/distance;
        // Debug.Log("Distance: " + distance);

        return visionInDirection;
    }

    //Function to check if a given position is tail
    private bool isTail(float x, float y) {
        for (int i = 1; i < segments.Count; i++) {
            if (segments[i].position.x == x && segments[i].position.y == y) {
                return true;
            }
        }
        return false;
    }

    //Move the snake in specified direction
    private void move(Vector2 direction) {
        if (movesLeft < 0) {
            //movesDone -= 10;
            Debug.Log("Ran out of Moves!");
            Died();
        }

        this.direction = direction;
    }

    //Kill the snake and Calculate the fitness of the snake
    private void Died() {
        alive = false;
        Destroy(food.gameObject);
        calculateFitness();
        for(int i = segments.Count-1; i >= 0; i--) {
            segments[i].gameObject.SetActive(false);
        }
    }

    //Delete all segments of Snake
    public void del() {
        for(int i = segments.Count-1; i >= 0; i--) {
            Destroy(segments[i].gameObject);
        }
    }

    //Calculate the fitness of the snake
    private void calculateFitness() {

        //If the fitness score of the snake is negative make it 0
        if (fitnessScore < 0) {
            fitnessScore = 0;
        }

        //Calculate the fitness
        fitness = fitnessScore*fitnessScore + movesDone * movesDone/4;

        // if (score < 10) {
        //     if (fitnessScore < 0) {
        //         fitnessScore = 0;
        //     }
        //     fitness = Mathf.FloorToInt((Mathf.Pow(score + 1, 3.25f)) + movesDone * fitnessScore);
        // }
        // else {
        //     mutationRate = 0.1f;
        //     fitness = Mathf.FloorToInt(Mathf.Pow(score * 2, 2) * Mathf.Pow(movesDone, 1.5f));
        // }
        // if (segments.Count < 10) {
        //     fitness = Mathf.FloorToInt(movesDone * movesDone * Mathf.Pow(2, Mathf.Floor(segments.Count)));
        // }
        // else {
        //     fitness = movesDone * movesDone;
        //     fitness *= Mathf.FloorToInt(Mathf.Pow(2, 10));
        //     fitness *= (segments.Count-9);
        // }
    }

    //Crossover the Snake with another snake to create a child Snake
    public NeuralNetwork crossover(Snake partner) {
        NeuralNetwork childBrain = new NeuralNetwork(brain);

        childBrain = brain.crossover(partner.brain);
        return childBrain;
    }

    //Set the brain of this snake
    public void setBrain(NeuralNetwork brain) {
        this.brain = brain;
    }
}
