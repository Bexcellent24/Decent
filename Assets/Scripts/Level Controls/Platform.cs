using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Platform : MonoBehaviour
{
    //----------Events----------//
    public delegate void PlatformDespawnedAction();
    public static event PlatformDespawnedAction PlatformDespawnedTriggered;
    
    public delegate void BossModeAction();
    public static event BossModeAction OnBossModeTriggered;
    public delegate void Boss2ModeAction();
    public static event Boss2ModeAction OnBoss2ModeTriggered;
    
    public delegate void Level1ModeAction();
    public static event Level1ModeAction OnLevel1ModeTriggered;
    
    public delegate void Level2ModeAction();
    public static event Level2ModeAction OnLevel2ModeTriggered;
    
    
    //----------Variables----------//
    [SerializeField] private Transform connector;
    public Vector3 ConnectPosition => connector.position;

    
    //----------MonoBehaviour Methods----------//
    private void OnTriggerExit(Collider other)
    {
        
        if (!other.CompareTag("Player"))
        {
            return;
        }
        
        PlatformDespawnedTriggered?.Invoke();
        Destroy(gameObject, 2);
        Debug.Log(gameObject + " Destroyed");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        
        if (gameObject.CompareTag("BossPlatform"))
        {
            OnBossModeTriggered?.Invoke();
            Debug.Log("Boss Mode Triggered");
        }
        
        if (gameObject.CompareTag("Boss2EntryPlatform"))
        {
            OnBoss2ModeTriggered?.Invoke();
            Debug.Log("Boss Mode Triggered");
        }
        
        if (gameObject.CompareTag("StartLevelPlatform"))
        {
            OnLevel1ModeTriggered?.Invoke();
            Debug.Log("Level 1 Triggered");
        }
        
        if (gameObject.CompareTag("Level2StartPlatform"))
        {
            OnLevel2ModeTriggered?.Invoke();
            Debug.Log("Level 2 Triggered");
        }
        
    }
    
}
