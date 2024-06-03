using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectPlacement : MonoBehaviour
{
    public Transform player; // Reference to the player object
    public GameObject []objectToPlace; // Object to be placed
    float timer;
    [SerializeField] float beat;



    public float[] timestamps; // Array of timestamps when objects should be placed
    float[] spawnObjectsPoint;
    public float startPosition;
    private float totalDistance = 0.0f; // Total distance traveled by the player
    GameObject priorGameObject;
    [SerializeField] PlayerMovementTutorial playerMovementScript;



    private void Start()
    {
        /*spawnObjectsPoint = new float[timestamps.Length];

        for (int i = 0; i < timestamps.Length; i++)
        {
            spawnObjectsPoint[i] = startPosition + (20 * timestamps[i]);

            PlaceObjectAtDistance(spawnObjectsPoint[i]);
        }*/


        
    }
    private void Update()
    {
        if (GameManager.Instance.startGame)
        {
            if (timer > beat)
            {
                GameObject newObject = Instantiate(objectToPlace[Random.Range(0, objectToPlace.Length)], new Vector3(player.position.x, player.position.y, startPosition + (20 * timer)), Quaternion.identity);
                timer -= beat;
            }

            timer += Time.deltaTime;
        }
    }
    // Function to place object at a given distance
    /*private void PlaceObjectAtDistance(float distance)
    {
        // Calculate position along player's forward direction
        Vector3 position = new Vector3(player.position.x , player.position.y,distance);

        // Instantiate object at calculated position
       GameObject newObject =  Instantiate(objectToPlace[Random.Range(0,objectToPlace.Length)], position, Quaternion.identity);
       
        newObject.GetComponent<ObjectInfo>().MakeGeometricChanges(priorGameObject);



        priorGameObject = newObject;
    }*/
}
