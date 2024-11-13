using System.Collections;
using System.Collections.Generic;

using System.Security;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

//usados para el canvas de GameOver
using UnityEngine.SceneManagement;
using UnityEngine.UI;  

public class Clock : MonoBehaviour
{

    public float gameDuration = 120f;  // Duración del juego (5 minutos)
    private float startTime; //determina cuando empezó el juego

    private float elapsedTime; //variable para manejar el paso del tiempo

    public GameObject GameOverUI;


    // Start is called before the first frame update
    void Start()

    {
        startTime = Time.time; 
        GameOverUI.SetActive(false); //se asegura de que el mensaje esté oculto
    }

    void Update()

    {
        elapsedTime = Time.time - startTime;

        if (elapsedTime >= gameDuration) //verifica si ha pasado el tiempo determinado
        {
            EndGame();
        }

    }

    void EndGame()
    {
        Debug.Log("El tiempo ha terminado. Fin del juego.");
        GameOverUI.SetActive(true);  // Mostrar la pantalla de fin de juego
        Time.timeScale = 0;
    }

}
