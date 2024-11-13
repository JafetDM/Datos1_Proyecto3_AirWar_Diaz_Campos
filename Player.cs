using System.Collections;
using System.Collections.Generic;

using System.Security;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D playerRB;
    private Vector2 moveInput;
    private int speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        moveInput = Vector2.right;

        
    }

    // Update is called once per frame
    void Update()
    {
        // utilizado para mantener al tanque en el eje y
        Vector2 set_position = transform.position;
        set_position.y = 0;
        transform.position = set_position;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Disparar();
        }

    }

    void FixedUpdate()
    {
        //se encarga del movimiento de derecha a izquierda

        if (playerRB.position.x > 33)
        {
            moveInput = Vector2.left;
        }

        if (playerRB.position.x < -33)
        {
            moveInput = Vector2.right;
        }

        Vector2 normalizedInput = moveInput.normalized;
        playerRB.MovePosition(playerRB.position + normalizedInput * speed * Time.fixedDeltaTime);
         
    }

    private void Disparar()
    {
        GameObject bullet = BulletPool.instance.GetBullets();

        if (bullet != null)
        {

            // Posición ligeramente arriba del tanque
            bullet.transform.position = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z); 
            bullet.transform.rotation = Quaternion.Euler(0,0,-90); // Asegurarse de que esté orientada hacia arriba
            bullet.SetActive(true);
        }
    }

}
