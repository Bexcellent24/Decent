using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFollow : MonoBehaviour
{

    public GameObject platform;
    

    void Update()
    {
        transform.LookAt(platform.transform);
    }
}
