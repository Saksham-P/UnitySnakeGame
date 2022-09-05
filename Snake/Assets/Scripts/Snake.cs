using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 direction = Vector2.right;
    private List<Transform> segments = new List<Transform>();
    public Transform bodySegment;
    public Transform food;
    public int initialSize = 3;

    //Genetic Algorithm Stuff --------------------------------------------------------------------------
    public NeuralNetwork brain;
    private float[] inputs;
    private float[] outputs;

    private int inputNodes = 24;
    private int hiddenNodes = 16;
    private int outputNodes = 4;
    private float mutationRate = 0.33f;

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

    private bool playMode = false;
    public Transform check;
    private List<Transform> checkObject = new List<Transform>();

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
        else {
            
        }  
    }

    private void FixedUpdate() {
        if (!playMode) {
            // for (int i = 0; i < checkObject.Count; i++) {
            //     Destroy(checkObject[i].gameObject);
            // }
            // checkObject = new List<Transform>();
            decide();
            // Debug.Break();
        }

        movesDone += 1;
        movesLeft -= 1;
        fitnessScore -= 1;

        previousPos = transform.position;

        for (int i = segments.Count - 1; i > 0; i--) {
            segments[i].position = segments[i-1].position;
        }

        transform.position = new Vector3(
            Mathf.Round(transform.position.x) + direction.x,
            Mathf.Round(transform.position.y) + direction.y,
            0.0f
        );
        if (Mathf.Abs((food.position - transform.position).magnitude) < Mathf.Abs((food.position - previousPos).magnitude)) {
            fitnessScore += 2;
        }
        else {
            fitnessScore -= 1;
        }
    }

    private void grow() {
        Transform segment = Instantiate(bodySegment);
        segment.position = segments[segments.Count - 1].position;
        segments.Add(segment);
        movesLeft += 100;
        fitnessScore += 100;
        score++;
    }

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

    private void OnTriggerEnter2D(Collider2D other) {
        if (other == food.GetComponent<Collider2D>()) {
            food.GetComponent<Food>().randomizePosition();
            grow();
        }
        if (other.tag == "Obstacle" || other.tag == "Wall") {
            if (playMode)
                resetState();
            else {
                if (other.tag == "Wall") {
                    //Debug.Log("HIt the Wall");
                    fitnessScore -= 50;
                    Died();
                }
                else if (segments.Contains(other.transform)){
                    fitnessScore -=50;
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

    private void look() {
        //Look Left
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

    private float[] lookInDirection(Vector2 direction) {
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
            if (!foodFound && positionLooking.x == food.position.x && positionLooking.y == food.position.y) {
                visionInDirection[0] = 1;
                foodFound = true;
                // Debug.Log("Food Found");
            }
            if (!tailFound && isTail(positionLooking.x, positionLooking.y)) {
                visionInDirection[1] = 1/distance;
                tailFound = true;
                // Debug.Log("Tail Found");
            }
            positionLooking += direction;
            distance += 1;
        }

        visionInDirection[2] = 1/distance;
        // Debug.Log("Distance: " + distance);

        return visionInDirection;
    }

    private bool isTail(float x, float y) {
        for (int i = 1; i < segments.Count; i++) {
            if (segments[i].position.x == x && segments[i].position.y == y) {
                return true;
            }
        }
        return false;
    }

    private void move(Vector2 direction) {
        if (movesLeft < 0) {
            //movesDone -= 10;
            Debug.Log("Ran out of Moves!");
            Died();
        }

        this.direction = direction;
    }

    private void Died() {
        alive = false;
        Destroy(food.gameObject);
        calculateFitness();
        for(int i = segments.Count-1; i >= 0; i--) {
            segments[i].gameObject.SetActive(false);
        }
    }

    public void del() {
        for(int i = segments.Count-1; i >= 0; i--) {
            Destroy(segments[i].gameObject);
        }
    }

    private void calculateFitness() {
        if (fitnessScore < 0) {
            fitnessScore = 0;
        }
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

    public NeuralNetwork crossover(Snake partner) {
        NeuralNetwork childBrain = new NeuralNetwork(brain);

        childBrain = brain.crossover(partner.brain);
        return childBrain;
    }

    public void setBrain(NeuralNetwork brain) {
        this.brain = brain;
    }
}
