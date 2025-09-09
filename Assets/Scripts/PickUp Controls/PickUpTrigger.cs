using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpTrigger : MonoBehaviour
{
    
    public delegate void ScoreMultiplierPickUpAction();
    public static event ScoreMultiplierPickUpAction OnScoreMultiplierPickUp;
    
    public delegate void SpeedMultiplierPickUpAction();
    public static event SpeedMultiplierPickUpAction OnSpeedMultiplierPickUp;
    
    public delegate void ImmunityPickUpAction();
    public static event ImmunityPickUpAction OnImmunityPickedUp;
    
    public delegate void BossPickUpAction();
    public static event BossPickUpAction OnBossPickUp;
    
    private void OnTriggerEnter(Collider other)
    {
        
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (gameObject.CompareTag("ScoreMultiplier"))
        {
            OnScoreMultiplierPickUp?.Invoke();
            
        }
        
        if (gameObject.CompareTag("SpeedMultiplier"))
        {
            OnSpeedMultiplierPickUp?.Invoke();
            
        }
        
        if (gameObject.CompareTag("ImmunityPickup"))
        {
            OnImmunityPickedUp?.Invoke();
            
        }
        
        if (gameObject.CompareTag("BossPickup"))
        {
            OnBossPickUp?.Invoke();
            Debug.Log("Boss Pickup Picked up");
        }
        
        Destroy(gameObject);
    }
}
