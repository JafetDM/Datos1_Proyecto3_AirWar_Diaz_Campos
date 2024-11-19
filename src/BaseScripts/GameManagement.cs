using UnityEngine;
using DataStructures;
using System.Drawing;
using DataStructures.Lists;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using System;
using TMPro;
// using myUnityScripts;

public class GameManagement : MonoBehaviour
{
    public static GameManagement Instance { get; private set; }
    public DataStructures.Lists.MatrixLinkedList<bool> map;
    public Graph LZs;
    public int ancho = 72;  // Number of tiles
    public int largo = 35;  // Number of tiles
    public float scale = 0.5f; // nivel de escalas, para dar detalle
    public GameObject main_camera;
    public float fov_constant = 3487;
    public Vector3 scene_center;
    
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

    }








}

