using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{

    [Header("Self destruct time delay")]
    [SerializeField] float timeDelay = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroySelf", timeDelay); 
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }





}
