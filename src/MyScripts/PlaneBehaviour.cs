using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneBehaviour : MonoBehaviour
{
    // public List<int> path = new();
    // private Vector2 start_coords;
    public float base_z = -2f;
    public List<Vector2> flight_plan_nodes = new();
    private List<float> flight_plan_schedule =  new();
    private int i = 0;
    private float target_time;
    private FollowCoords followCoords;
    // Start is called before the first frame update
    void Start()
    {
        // Gets the FollowCoords script;
        followCoords = GetComponent<FollowCoords>();

        // Repositions the plane's z coordinate to base_z.
        Vector3 move = transform.position;
        move.z = base_z;
        transform.position = move;

        Vector2 start_coords = transform.position;
        
        // flight_plan_nodes should be filled on plane instantiation by GameManager.
        // flight_plan_nodes.Insert(0, start_coords);
        int node_amount = flight_plan_nodes.Count;
        float current_time = Time.time;

        // Creates the flight_plan_schedule as a list of times for the plane to arrive at the nodes at constant speed.
        float speed = followCoords.speed_multiplier;
        flight_plan_schedule.Add(current_time + Vector2.Distance(start_coords, flight_plan_nodes[i])/speed);
        for (int i = 1; i < node_amount; i++)
        {
            // Adds distance to the current time. This implies speed is 1 m/s.
            flight_plan_schedule.Add(Vector2.Distance(flight_plan_nodes[i-1], flight_plan_nodes[i])/speed + flight_plan_schedule[i-1]);
        }
        target_time = flight_plan_schedule[0];

        // Debug
        for (int i = 0; i < flight_plan_schedule.Count; i++)
        {
            Debug.Log("flight_plan_schedule[i] = " + flight_plan_schedule[i]);
        }
        for (int i = 0; i < flight_plan_nodes.Count; i++)
        {
            Debug.Log("flight_plan_nodes[i] = " + flight_plan_nodes[i]);
        }

        Debug.Log("AT PLANE START flight_plan_schedule.Count = " + flight_plan_schedule.Count);
        Debug.Log("AT PLANE START flight_plan_nodes.Count = " + flight_plan_nodes.Count);

    }

    // Update is called once per frame.
    void FixedUpdate()
    {
        // Vector2 current_position = transform.position;

        Vector2 current_coords_to_follow = flight_plan_nodes[i];
        followCoords.Follow(current_coords_to_follow.x, current_coords_to_follow.y, base_z);


        if (target_time <= Time.time)
        {
            i++;
            // flight_plan_nodes.Count == flight_plan_schedule.Count is always true.
            if (i >= flight_plan_nodes.Count)   // Plane has reached the end of the path.
            {
                Destroy(gameObject);
                Debug.Log("PLANE DESTROYED. i = " + i);
                return;
            }
            Debug.Log("i = " + i);
            target_time = flight_plan_schedule[i];
            Debug.Log("i UPDATED TO: " + i);
            Debug.Log("flight_plan_nodes.Count: " + flight_plan_nodes.Count);
        }   
    }
}
