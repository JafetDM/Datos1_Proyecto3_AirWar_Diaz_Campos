using System.Collections;
using System.Collections.Generic;

using System.Security;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{

    public static BulletPool instance;
    private List<GameObject> pool_bullets = new List<GameObject>();

    public int cantidad = 20;

    [SerializeField] private GameObject bullet_prefab;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i <cantidad; i++)
        {
            GameObject obj = Instantiate(bullet_prefab);
            obj.SetActive(false);
            pool_bullets.Add(obj);
        }

        
    }

    public GameObject GetBullets()
    {
        for (int i = 0; i < pool_bullets.Count; i++)
        {
            if (!pool_bullets[i].activeInHierarchy)
            {
                return pool_bullets[i];
            }
        }
        
        return null;
    }

    

}
