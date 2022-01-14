using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    //global variables
    public Vector2 startPosition = new Vector2(0, 0);
    public float playerSpeed = 10.0f;
    private Vector3 currentPos;
    private Vector3 boundPos;
    private Vector3 prevPos;

    private Transform playerTransform;
    private int isMoving;

    int NOMOVE = 0;
    int UPMOVE = 1;
    int DOWNMOVE = -1;
    int LEFTMOVE = 2;
    int RIGHTMOVE = -2;

    void spawnPlayer(Vector3 endPoint) { //sets player's location and speed
        playerTransform.Translate(Vector3.zero);

        playerTransform.position = endPoint;
        currentPos = endPoint;
        boundPos = endPoint;
        prevPos = endPoint;

        playerTransform.Translate(Vector3.zero);
        isMoving = NOMOVE;
    }//end spawnPlayer

    void Start() { //runs once at the beginning
        playerTransform = this.transform;
        spawnPlayer(new Vector3(startPosition.x, 1, startPosition.y));
    }//end Start

    void Update() //runs every frame
    {

        if (playerTransform.position.y < -2) spawnPlayer(new Vector3(startPosition.x, 1, startPosition.y));

        currentPos = playerTransform.position;
        if (isMoving == NOMOVE)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log("Up pressed");
                boundPos = currentPos + new Vector3(0, 0, 1);
                isMoving = UPMOVE;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log("Down pressed");
                boundPos = currentPos + new Vector3(0, 0, -1);
                isMoving = DOWNMOVE;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                Debug.Log("Right pressed");
                boundPos = currentPos + new Vector3(1, 0, 0);
                isMoving = RIGHTMOVE;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("Left pressed");
                boundPos = currentPos + new Vector3(-1, 0, 0);
                isMoving = LEFTMOVE;
            }
        }
        else
        {

            if (((isMoving == UPMOVE) &&((currentPos.z - prevPos.z) > 1))||
                ((isMoving == DOWNMOVE) &&((currentPos.z - prevPos.z) < -1))||
                ((isMoving == LEFTMOVE) &&((currentPos.x - prevPos.x) < -1))||
                ((isMoving == RIGHTMOVE) &&((currentPos.x - prevPos.x) > 1)))
            {
                spawnPlayer(boundPos);
            }
            else
            {
                playerTransform.Translate((boundPos - prevPos) * Time.deltaTime * playerSpeed);
            }
        }

    }//end Update

    void OnCollisionEnter(Collision col) {//runs when hitting another object
        if (col.gameObject.tag == "Wall")
        {
            Debug.Log("Hit Wall");
            Vector3 temp = prevPos;
            prevPos = boundPos;
            boundPos = temp;
            isMoving *= -1;
        }
        else if (col.gameObject.tag == "Hole") {
            Debug.Log("Fell in Hole");
            boundPos = new Vector3(0, -1, 0);
        }
    }//end OnCollisionEnter

}
