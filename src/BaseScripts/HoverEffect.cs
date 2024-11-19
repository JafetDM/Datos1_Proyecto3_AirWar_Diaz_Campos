using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    // private Transform pos; // Reference to player's Rigidbody.
    public float revolutionsPerSecond = 1;
    public float heightConstant = 2.5f;
    public float hoveringSpeedConstant = 150;
    public float dampening = 1;
    public bool local_rotation = false;
    private int phase;

    // Start is called before the first frame update
    private void Start()
    {
        // pos = GetComponent<Transform>(); // Access the object's Transform Component.
        phase = Time.frameCount;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Hover.
        Vector3 movement = transform.position;
        double hoverMovement = Math.Cos(hoveringSpeedConstant/10000/dampening* Time.frameCount + phase)*heightConstant*hoveringSpeedConstant/10000/dampening;
        movement.y += (float)hoverMovement;
        transform.position = movement;
        //Rotation.
        // This method works:
        // float turn = revolutionsPerSecond/dampening;
        // Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        // transform.rotation *= turnRotation;
        // Simpler method:
        if (local_rotation)
        {
            transform.Rotate(0, revolutionsPerSecond*7.2f/dampening, 0);    
        }
        else
        {
            transform.Rotate(Vector3.up, revolutionsPerSecond * 7.2f/dampening, Space.World);
        }
        
        

    }
}
