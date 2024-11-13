using System.Security;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class MapGenerator :MonoBehaviour 
{
    public int ancho = 72;
    public int largo = 35;
    public float scale = 2f; // nivel de escalas, para dar detalle

    public GameObject agua;
    public GameObject tierra;

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        for (int x = 0; x < ancho; x++)
        {
            for (int y=0; y < largo; y++)
            {
                float xCoord = (float)x / ancho * scale;
                float yCoord = (float)y / largo * scale;

                //Generar un valor con Perlin Noise entre 0 y 1
                float perlinValue = Mathf.PerlinNoise(xCoord, yCoord);

                //Modifica la curva de ruido para que hayan grandes extensiones
                perlinValue = Mathf.Pow(perlinValue, 3f);

                //Se decide si es tierra o mar dependiendo del umbral
                Vector3 spawnPosition = new Vector3(x-35, y+5, 0);
                if (perlinValue > 0.25f) 
                {
                    Instantiate(tierra, spawnPosition, Quaternion.identity);
                }

                else
                {
                    Instantiate(agua, spawnPosition, Quaternion.identity);
                }
            }
        }
    }
}
