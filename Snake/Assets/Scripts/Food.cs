using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    //Requires a reference to the grid where the snake roams around in
    public BoxCollider2D gridArea;

    //When the progam first starts, find the reference to the gridArea box Collider 2D Object
    private void Awake() {
        gridArea = GameObject.Find("GridArea").GetComponent<BoxCollider2D>();
    }

    //Call the randomizePosition function once the game starts
    private void Start() {
        randomizePosition();
    }

    //Randomize the position of the food
    public void randomizePosition() {

        //Get the bounds of the grid area
        Bounds bound = gridArea.bounds;

        //generate a random x value between the horizontal bounds of gridArea
        float x  = Mathf.Round(Random.Range(bound.min.x, bound.max.x));

        //generate a random y value between the vertical bounds of the gridArea
        float y = Mathf.Round(Random.Range(bound.min.y, bound.max.y));

        //Set the position of the foood
        transform.position = new Vector3(x, y, 0f);
    }
}
