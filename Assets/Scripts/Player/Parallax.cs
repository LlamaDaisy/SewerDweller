using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Parallax : MonoBehaviour
{
    private float startPos, length; //stores the initial position and length of the background image.
    public GameObject cam; //gameObject for the camera
    public float parallaxEffect; //sets the amount of parallax to apply to the sprite

    void Start()
    {
        //stores the inital x postion
        startPos = transform.position.x;
        //gets the width on x of the sprite by accessing the sprite renderer using GetComponent
        length = GetComponent<Tilemap>().size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Calculates the distance the camera has moved multiplied by the parallax effect amount.
        float distance = cam.transform.position.x * parallaxEffect;
        //Calculates the movement of the background based on camera movement and parallax effect anount. 
        float movement = cam.transform.position.x * (1 * parallaxEffect);

        //Moves the background horizontally based on the parallax effect amount.
        //Adjusts the background position relative to the camera's movement.
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
    }
}
