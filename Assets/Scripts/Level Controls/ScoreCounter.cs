using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    //----------Events----------/
    public delegate void ScoreCounterAction();
    public static event ScoreCounterAction OnScoreCounted;
    
    
    //----------MonoBehaviour Methods----------//
    private void OnTriggerEnter(Collider other)
    {
        
        
        if (!other.CompareTag("Player"))
        {
            return;
        }
       
       
        OnScoreCounted?.Invoke();
    }
}
