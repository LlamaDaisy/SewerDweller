using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //this tells it to follow the player - you need to add this transform in Unity
    public Transform player;

    //we are calling velocity back to 0 so it stops moving when the player stops (vector3.zero = x - 0, y - 0, z - 0. and is just shorthand for 0,0,0)
    Vector3 velocity = Vector3.zero;

    //offsets the camera to the selected object
    public Vector3 offset;

    //sets the amount/speed of camera movement this is in seconds
    public float damping;

    private void FixedUpdate()
    {
        Vector3 moveTo = player.position + offset;

        //this is telling it we want it to move it one vector 3 to another SMOOTHLY
        transform.position = Vector3.SmoothDamp(transform.position, moveTo, ref velocity, damping);
    }
}
