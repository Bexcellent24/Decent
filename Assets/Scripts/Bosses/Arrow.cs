using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    
    void Start()
    {
        Destroy(gameObject, 3);
        Debug.Log("Arrow Destroy");
    }
    
}
