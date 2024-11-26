using UnityEngine;
using DataStructures;
using System.Drawing;
using DataStructures.Lists;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using System;
using TMPro;
using System.Collections.Generic;
// using myUnityScripts;

public class GameManagement : MonoBehaviour
{
    public static GameManagement Instance { get; private set; }
    public DataStructures.Lists.MatrixLinkedList<bool> map;
    public Graph LZs;
    public int ancho = 72;  // Number of tiles x
    public int largo = 35;  // Number of tiles y
    public float scale = 0.5f; // nivel de escalas, para dar detalle
    public GameObject main_camera;
    public float fov_constant = 3487;
    public Vector3 scene_center;
    public int plane_speed = 1;
    public GameObject plane;
    // public float plane_base_z = -2;
    private int spawn_plane_timer = 180;   // This controls first plane spawn.
    public int points = 0;
    public GameObject points_UI;
    private void Awake()
    {
        Debug.Log("HAS ENTERED AWAKE FUNCTION IN THE GAME MANAGER.");
        // Implement the singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps this object when loading new scenes
        }
        else
        {
            Destroy(gameObject); // Ensures there's only one instance
        }

        // Main program starts here.
        // Creates map ??? 

        map = new(ancho, largo, false); // Boolean matrix representing the tiles of the map. false = water; true = land.
        LZs = new();    // Adjacency list graph storing the landing zones. false = aircraft carrier (water); true = airport (land).

        // Scene center.
        scene_center = new((ancho-1)/2, (largo-1)/2, 0);







        //Sets max fps to 60.
        Application.targetFrameRate = 60;
        Debug.Log("Exits Awake function.");
    }


    void Start()
    {
        
        // Repositions the main camera.
        scene_center.z = -1*MathF.Sqrt(largo*ancho) * 5;
        main_camera.transform.position = scene_center;
        main_camera.GetComponent<Camera>().fieldOfView = Math.Abs(fov_constant/scene_center.z);

    }    

    void Update()
    {
        // Spawn plane.
        if (spawn_plane_timer < Time.frameCount && UnityEngine.Random.Range(0,60) == 1)
        {
            SpawnPlane();
            Debug.Log("Plane spawns");
            spawn_plane_timer = Time.frameCount + 300; //Adjust this to a natural plane spawn rate.
        }
        // Show Points
        TextMeshProUGUI points_text_box = points_UI.GetComponentInChildren<TextMeshProUGUI>();
        points_text_box.text = "Points: " + points.ToString();

    }

    public void SpawnPlane()
    {
        if (LZs.adjacency_list_nodes.Count <= 1)
        {
            Debug.Log("NOT ENOUGH NODES.");
            return;
        }

        int origin_node_index = UnityEngine.Random.Range(0,LZs.adjacency_list_nodes.Count);
        int end_node_index = UnityEngine.Random.Range(0,LZs.adjacency_list_nodes.Count);
        while(end_node_index == origin_node_index)
        {
            end_node_index = UnityEngine.Random.Range(0,LZs.adjacency_list_nodes.Count);
        }

        (List<int> path, int weight) dijkstra = LZs.Dijkstra(origin_node_index, end_node_index);
        List<Vector2> flight_plan_nodes = new();
        // Gets a List<Vector2>flight_plan_nodes of coords from the List<int>path of indexes.
        int node_amount = dijkstra.path.Count;
        for (int i = 0; i < node_amount; i++)
        {
            flight_plan_nodes.Add(LZs.adjacency_list_nodes[dijkstra.path[i]].coords);
        }
        

        Vector2 start_coords = LZs.adjacency_list_nodes[origin_node_index].coords;
        // z value is irrelevant as it will be updated on Start() by the plane.
        Debug.Log("Plane is stantiated at" + start_coords + " at SpawnPlane");
        GameObject instantiated_plane = Instantiate(plane, start_coords, Quaternion.identity);
        instantiated_plane.GetComponent<PlaneBehaviour>().flight_plan_nodes = flight_plan_nodes;

    }






}

