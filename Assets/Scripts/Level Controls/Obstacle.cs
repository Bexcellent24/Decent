using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    
    //----------Events----------//
    public delegate void ObstacleCollidedAction();
    public static event ObstacleCollidedAction OnObstacleCollided;
    
    public delegate void PlatformSideCollidedAction();
    public static event PlatformSideCollidedAction OnPlatformSideCollided;
    
    public delegate void LaserCollidedAction();
    public static event LaserCollidedAction OnLaserCollided;
    
    
    //----------Variables----------//

    private bool immunityPickupActive = false;
    private Coroutine immunityPickup;
    
    //----------MonoBehaviour Methods----------//

    private void OnEnable()
    {
        PickUpTrigger.OnImmunityPickedUp += OnImmunityPickedUpHandler;
    }
    
    private void OnDisable()
    {
        PickUpTrigger.OnImmunityPickedUp -= OnImmunityPickedUpHandler;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (!other.CompareTag("Player"))
        {
            return;
        }
        
        if (immunityPickupActive && !gameObject.CompareTag("Lava"))
        {
            GetComponent<Collider>().enabled = false;
            return;
        }

        if (gameObject.CompareTag("PlatformMissed"))
        {
            OnPlatformSideCollided?.Invoke();
            return;
        }
        
        if (gameObject.CompareTag("Laser"))
        {
            OnLaserCollided?.Invoke();
            return;
        }
        
        
        OnObstacleCollided?.Invoke();
     
    }
    
    //----------Event Handlers----------//

    private void OnImmunityPickedUpHandler()
    {
        if (immunityPickupActive)
        {
            StopCoroutine(immunityPickup);
        }
        immunityPickup = StartCoroutine(ImmunityPickUpTimer());
    }
    
    //----------Coroutines----------//

    IEnumerator ImmunityPickUpTimer()
    {
        immunityPickupActive = true;
        yield return new WaitForSeconds(10);
        immunityPickupActive = false;
    }
}
