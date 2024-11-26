using System;
using System.Collections.Generic;
using System.Security;
using TMPro;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class MapGenerator :MonoBehaviour 
{
    public int ancho = -1;  // Number of tiles
    public int largo = -1;  // Number of tiles
    public float scale = -1f; // nivel de escalas, para dar detalle
    public int map_seed = 0;

    public GameObject agua;
    public GameObject tierra;
    public GameObject landing_zone_water;
    public GameObject landing_zone_land;
    public GameObject BG;
    public GameObject route_particles;
    public GameObject weight_label;
    public GameObject node_label;
    

    void Start()
    {
        ancho = GameManagement.Instance.ancho;
        largo = GameManagement.Instance.largo;
        scale = GameManagement.Instance.scale;

        // Generates background.



        GenerateMap();
        Debug.Log("adjacency_list_nodes.Count = " + GameManagement.Instance.LZs.adjacency_list_nodes.Count);
        Debug.Log("Enters GenerateRoutes()");
        GenerateRoutes();
        Debug.Log("Exits GenerateRoutes()");
        Debug.Log(GameManagement.Instance.LZs.ShowGraphString("Graph: ") + "\n   Edge amount: " + GameManagement.Instance.LZs.adjacency_list_edges.Count);
        Vector3 scene_center_copy = new(GameManagement.Instance.scene_center.x, GameManagement.Instance.scene_center.y, 5); 
        GameObject instantiated_bg = Instantiate(BG, scene_center_copy, Quaternion.identity);
        instantiated_bg.transform.localScale = new Vector3(ancho*1.2f, largo*1.2f, 1);
    }

    void GenerateMap()  // Generates the graphic and logical world maps. 
    {
        int LZ_generation_rate = 5*(int)Mathf.Sqrt(ancho*largo);

        int threshold = ancho*largo * 10 /27;
        int threshold_i = 0;

        for (int x = 0; x < ancho; x++)
        {
            for (int y=0; y < largo; y++)
            {
                float x_proportion = (float)x / ancho;
                float y_proportion = (float)y / largo;
                float x_perlin = (float)x / 10 * scale;
                float y_perlin = (float)y /10 * scale;

                //Generar un valor con Perlin Noise entre 0 y 1
                float perlinValue = Mathf.PerlinNoise(x_perlin + map_seed*scale, y_perlin);

                //Modifica la curva de ruido para que hayan grandes extensiones
                perlinValue = Mathf.Pow(perlinValue, 3f);

                //Decides whether or not to create a landing zone before deciding the tile type.
                bool create_LZ = false;
                // Forces at least three nodes to appear.
                if (UnityEngine.Random.Range(0, LZ_generation_rate) == 1 && x_proportion > 0.15f && x_proportion < 0.85  &&  y_proportion > 0.15f && y_proportion < 0.85)
                {
                    create_LZ = true;
                    Debug.Log("create_LZ = true");
                }
                else
                {
                    threshold_i ++;
                }
                if (threshold_i >= threshold)
                {
                    create_LZ = true;
                    Debug.Log("create_LZ = true. Threshold reached.");
                    threshold_i = 0;
                }
                // else
                // {
                //     Debug.Log("LZ_generation_rate = " + LZ_generation_rate + "; x_proportion = " + x_proportion + "; y_proportion = " + y_proportion);
                // }
                //Se decide si es tierra o mar dependiendo del umbral
                bool water = false;
                Vector3 spawnPosition = new Vector3(x, y, 0);
                if (perlinValue > 0.15f) 
                {
                    // false is the default value of the map matrix. Only water tiles need to be set.
                    Instantiate(tierra, spawnPosition, Quaternion.identity);
                }

                else
                {
                    water = true;
                    GameManagement.Instance.map.SetAt(true, x, y);  // Sets true on the map for water tiles.
                    Instantiate(agua, spawnPosition, Quaternion.identity);
                }
                if (create_LZ)
                {
                    GameManagement.Instance.LZs.AddNode(new Vector2(x,y), water);   // Adds a landing zone in the LZs graph.
                    spawnPosition.z -= 0.5f;
                    if (water)
                    {
                        Instantiate(landing_zone_water, spawnPosition, Quaternion.identity);    
                    }
                    else
                    {
                        Instantiate(landing_zone_land, spawnPosition, Quaternion.identity);
                    }
                    spawnPosition.z = -1;
                    GameObject instantiated_label = Instantiate(node_label, spawnPosition, Quaternion.identity);
                    TextMeshProUGUI text_box = instantiated_label.GetComponentInChildren<TextMeshProUGUI>();
                    char node_label_ascii = (char)(GameManagement.Instance.LZs.adjacency_list_nodes.Count+64);
                    Debug.Log("node_label_ascii = " + node_label_ascii);
                    text_box.text = node_label_ascii.ToString();
                }
            }
        }
    }

    // Generates the edges of the landing zones graph LZs.
    // Calculates the weight taking into account the terrain below (water/land)
    // Instantiates graphic edges as particles.
    public void GenerateRoutes()
    {
        // The maximum amount of edges given an n number of vertices is n(n-1)/2
        // edge_amount is 3/4 of maximum number of edges
        int node_amount = GameManagement.Instance.LZs.adjacency_list_nodes.Count;
        int edge_amount =  node_amount * (node_amount - 1) * 3 / 8;

        for (int i = 0; i < node_amount; i++)
        {
            // Creates a list of nodes to conncect the current node to.
            List<int> nodes_to_connect_indexes = new();
            // nodes_to_connect_indexes is populated with numbers greater than the current index.
            for (int j = i+1; j < node_amount; j++)
            {
                nodes_to_connect_indexes.Add(j);
            }
            // 1/4 of the numbers from nodes_to_connect_indexes is removed.
            for (int j = 0; j < (node_amount-i-1)/4+1; j++)
            {
                if (nodes_to_connect_indexes.Count > 1) // List can't be empty.
                {
                    nodes_to_connect_indexes.RemoveAt(UnityEngine.Random.Range(0,nodes_to_connect_indexes.Count));
                }
            }
            // If no edges were made it 
            // Conncects the current node i to the nodes from nodes_to_connect_indexes. Calculates the weight of the edge for each destination based on terrain type.
            foreach (int node_index in nodes_to_connect_indexes)
            {
                // Gets the nodes coords.
                Vector2 origin_node_coords = GameManagement.Instance.LZs.adjacency_list_nodes[i].coords;
                Vector2 end_node_coords = GameManagement.Instance.LZs.adjacency_list_nodes[node_index].coords;
                // Calculates edge weight and Vector2 midpoint between the nodes.
                (int weight, Vector2 midpoint) weight_and_midpoint = CalculateWeightOfEdge(origin_node_coords, end_node_coords);
                // Creates the logical edge.
                GameManagement.Instance.LZs.AddEdge(i, node_index, weight_and_midpoint.weight);
                // Instantiates the graphic edge as a Particle System. Makes it draw a line between the two points.
                GameObject instantiated_edge = Instantiate(route_particles, new Vector3(weight_and_midpoint.midpoint.x, weight_and_midpoint.midpoint.y, -0.25f), Quaternion.identity);
                ParticleSystem instantiated_PS = instantiated_edge.GetComponent<ParticleSystem>();
                
                instantiated_PS.transform.Rotate(0, 0, Mathf.Rad2Deg*Mathf.Atan2(end_node_coords.y - origin_node_coords.y, end_node_coords.x - origin_node_coords.x));
                instantiated_PS.transform.Rotate(180, 0, 0);
                
                // instantiated_PS.shape.scale = new Vector3(1,2,3);    // Can't directly modify shape bacause .shape returns a struct, and is not the variable.
                
                float distance = Vector2.Distance(origin_node_coords, end_node_coords);
                ParticleSystem.ShapeModule shape = instantiated_PS.shape;
                shape.scale = new Vector3(distance, 0.2f, 0);
                
                ParticleSystem.EmissionModule emission = instantiated_PS.emission;
                emission.rateOverTime = 3*distance;

                ParticleSystem.MainModule main_module = instantiated_PS.main;
                Color color = Color.HSVToRGB(1 - 0.67f*Mathf.Exp(-0.001f*weight_and_midpoint.weight), 0.8f, 0.8f);
                main_module.startColor = new ParticleSystem.MinMaxGradient(color);

                // Instantiates the weight label of the edge.
                GameObject instantiated_label = Instantiate(weight_label, new Vector3(weight_and_midpoint.midpoint.x, weight_and_midpoint.midpoint.y, -1f), Quaternion.identity);
                // if (instantiated_label == null)
                // {
                //     Debug.Log("instantiated_label IS NULL!!!!!!!!");
                // }
                // else
                // {
                //     Debug.Log("instantiated_label is not null.");
                // }
                TextMeshProUGUI text_box = instantiated_label.GetComponentInChildren<TextMeshProUGUI>();

                // if (text_box == null)
                // {
                //     Debug.Log("text_box IS NULL!!!!!!!!");
                // }
                // else
                // {
                //     Debug.Log("text_box is not null.");
                // }
                text_box.text = weight_and_midpoint.weight.ToString();
                instantiated_label.transform.Rotate(0, 0, Mathf.Rad2Deg*Mathf.Atan2(end_node_coords.y - origin_node_coords.y, end_node_coords.x - origin_node_coords.x));
                // instantiated_label.transform.Rotate(0, -180, 0);


            }

        }
    

    }
    // Calculates the approximate number of water tiles using a linear function of y in terms of x
    // that represents the line between two connecting nodes. Returns the square distance + 2*water tiles.
    public (int, Vector2) CalculateWeightOfEdge(Vector2 origin, Vector2 end)
    {
        float dist_x = (end.x - origin.x);
        float dist_y = (end.y - origin.y);
        
        int step = 1;
        if (dist_x < 0)
        {
            step = -1;
        }

        float m = dist_y/dist_x;
        float x0 = origin.x;
        float y0 = origin.y;

        int x = (int)x0;
        int x_end = (int)end.x;
        int water_tiles_passed = 0;
        while (x != x_end)
        {
            float y = m*(x-x0) + y0;
            if (GameManagement.Instance.map.FindAt(x, (int)Math.Round(y)))
            {
                water_tiles_passed ++;
            }

            x += step;
        }

        Vector2 midpoint = new(origin.x + dist_x/2, origin.y + dist_y/2);

        return ((int)(dist_x*dist_x) + (int)(dist_y*dist_y) + 16*water_tiles_passed , midpoint);
    }






}
