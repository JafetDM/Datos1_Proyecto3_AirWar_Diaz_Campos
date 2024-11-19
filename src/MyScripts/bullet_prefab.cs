using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_prefab : MonoBehaviour

{
    public float speed = 10f;

    private void Update()
    {
        // Mover la bala hacia arriba en el eje Y
        transform.Translate(Vector3.left * speed * Time.deltaTime); //se pone left porque el modelo de la bala se giro -90 en y

        // Desactivar la bala si sale de los límites de la pantalla
        if (transform.position.y > 50) // Ajustar el límite según tu escena
        {
            gameObject.SetActive(false);
        }
    }
}
