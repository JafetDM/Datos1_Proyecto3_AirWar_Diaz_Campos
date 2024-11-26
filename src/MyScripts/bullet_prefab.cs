using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_prefab : MonoBehaviour

{
    public float speed = 10f;

    private void Start()
    {
        transform.Rotate(0,270,0);
    }
    private void FixedUpdate()
    {
        // Mover la bala hacia arriba en el eje Y
        transform.Translate(Vector3.up * speed * Time.deltaTime); //se pone left porque el modelo de la bala se giro -90 en y
        // transform.Rotate(0,6,0);
        // Desactivar la bala si sale de los límites de la pantalla
        if (transform.position.y > GameManagement.Instance.largo) // Ajustar el límite según tu escena
        {
            // gameObject.SetActive(false);
            Destroy(gameObject);
            Debug.Log("Bullet destroyed.");
            return;
        }
    }
}
