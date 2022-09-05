using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public BoxCollider2D gridArea;

    private void Awake() {
        gridArea = GameObject.Find("GridArea").GetComponent<BoxCollider2D>();
    }

    private void Start() {
        randomizePosition();
    }

    public void randomizePosition() {
        Bounds bound = gridArea.bounds;

        float x  = Mathf.Round(Random.Range(bound.min.x, bound.max.x));
        float y = Mathf.Round(Random.Range(bound.min.y, bound.max.y));

        // x = -10;
        // y = 10;

        transform.position = new Vector3(x, y, 0f);
    }
}
