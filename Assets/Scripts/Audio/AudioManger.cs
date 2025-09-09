using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManger : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;
    public static AudioManger Instance { get; private set; }

    private bool isPlayerAlive = true;
 
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    

    private void OnEnable()
    {
        ObstacleCollision.OnPlayerLifeLost += OnPlayerLifeLostHandler;
        Platform.OnLevel1ModeTriggered += OnLevelModeTriggeredHandler;
        Platform.OnLevel2ModeTriggered += OnLevel2ModeTriggeredHandler;
        ObstacleCollision.OnPlayerDied += OnPlayerDiedHandler;
        PlayerController.OnPlayerJump += OnPlayerJumpHandler;
        PickUpTrigger.OnSpeedMultiplierPickUp += OnPickUpHanlder;
        PickUpTrigger.OnScoreMultiplierPickUp += OnPickUpHanlder;
        PickUpTrigger.OnImmunityPickedUp += OnPickUpHanlder;
        Platform.OnBossModeTriggered += OnBossModeTriggeredHanlder;
        Platform.OnBoss2ModeTriggered += OnBoss2ModeTriggeredHanlder;
        PickUpTrigger.OnBossPickUp += OnBossPickUpHandler;
        Level1Boss.OnBossDefeated += OnBossDefeatedHanlder;
        Level2Boss.OnBoss2Defeated += OnBoss2DefeatedHanlder;
        Level2Boss.OnBoss2Shoots += OnBoss2ShootsHandler;
        Level2Boss.OnBoss2Calls += OnBoss2CallsHandler;
    }

    private void OnDisable()
    {
        ObstacleCollision.OnPlayerLifeLost -= OnPlayerLifeLostHandler;
        Platform.OnLevel1ModeTriggered -= OnLevelModeTriggeredHandler;
        Platform.OnLevel2ModeTriggered -= OnLevel2ModeTriggeredHandler;
        ObstacleCollision.OnPlayerDied -= OnPlayerDiedHandler;
        PlayerController.OnPlayerJump -= OnPlayerJumpHandler;
        PickUpTrigger.OnSpeedMultiplierPickUp -= OnPickUpHanlder;
        PickUpTrigger.OnScoreMultiplierPickUp -= OnPickUpHanlder;
        PickUpTrigger.OnImmunityPickedUp -= OnPickUpHanlder;
        Platform.OnBossModeTriggered -= OnBossModeTriggeredHanlder;
        Platform.OnBoss2ModeTriggered -= OnBoss2ModeTriggeredHanlder;
        PickUpTrigger.OnBossPickUp -= OnBossPickUpHandler;
        Level1Boss.OnBossDefeated -= OnBossDefeatedHanlder;
        Level2Boss.OnBoss2Defeated -= OnBoss2DefeatedHanlder;
        Level2Boss.OnBoss2Shoots -= OnBoss2ShootsHandler;
        Level2Boss.OnBoss2Calls -= OnBoss2CallsHandler;
    }
    

    //----------Private Custom Methods----------//
    private void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        
        s.source.Play();
        Debug.Log("Playing Audio '" + name + "'");
    }
    private void StopSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        
        s.source.Stop();
        Debug.Log("Stopping Audio '" + name + "'");
    }
    
    public void PlayHoverSFX()
    {
        Sound s = Array.Find(sounds, sound => sound.name == "UI Hover");
        s.source.Play();
        Debug.Log("Playing Audio '" + name + "'");
    }
    
    public void PlayClickSFX()
    {
        Sound s = Array.Find(sounds, sound => sound.name == "UI Click");
        s.source.Play();
        Debug.Log("Playing Audio '" + name + "'");
    }

    
    
    //----------Event Handlers----------//
    
    private void OnPlayerLifeLostHandler(int lifeNum)
    {
        if (isPlayerAlive)
        {
            PlaySound("Hurt"); 
        }
        
    }
    private void OnLevelModeTriggeredHandler()
    {
        PlaySound("Level One Background Music");
    }
    private void OnLevel2ModeTriggeredHandler()
    {
        PlaySound("Level Two Background Music");
    }

    private void OnPlayerJumpHandler()
    {
        PlaySound("Jump");
    }
    
    private void OnPickUpHanlder()
    {
        PlaySound("Pickup");
    }
    private void OnBossPickUpHandler()
    {
        StartCoroutine(Boss1TakesDamage());
    }
    
    private void OnBossDefeatedHanlder()
    {
        StartCoroutine(Boss1DefeatedSequence());
    }
    private void OnBoss2DefeatedHanlder()
    {
        StartCoroutine(Boss2DefeatedSequence());
    }
    
    private void OnBossModeTriggeredHanlder()
    {
        StopSound("Level One Background Music");
        PlaySound("Boss 1 Background Music");
    }
    private void OnBoss2ModeTriggeredHanlder()
    {
        StopSound("Level Two Background Music");
        PlaySound("Boss 2 Background Music");
    }
    
    private void OnPlayerDiedHandler()
    {
        StartCoroutine(DeathSoundSequence());
        isPlayerAlive = false;
    }
    
    private void OnBoss2ShootsHandler()
    {
        PlaySound("Boss 2 Shooting");
    }

    private void OnBoss2CallsHandler()
    {
        PlaySound("Boss 2 Calling");
    }

    
    
    //----------Coroutines----------//

    private IEnumerator DeathSoundSequence()
    {
        StopSound("Level One Background Music");
        StopSound("Level Two Background Music");
        PlaySound("Collision");
        yield return new WaitForSeconds(1.5f);
        PlaySound("GameOver");
    }
    
    private IEnumerator Boss1DefeatedSequence()
    {
        StopSound("Boss 1 Background Music");
        PlaySound("Boss 1 Defeated");
        yield return new WaitForSeconds(1.3f);
        PlaySound("Kaleidoscope");
        
    }
    
    private IEnumerator Boss2DefeatedSequence()
    {
        StopSound("Boss 2 Background Music");
        PlaySound("Boss 2 Deafeted");
        yield return new WaitForSeconds(1f);
        PlaySound("Kaleidoscope");
        
    }
    private IEnumerator Boss1TakesDamage()
    {
        PlaySound("Boss 1 Pickup");
        yield return new WaitForSeconds(.75f);
        PlaySound("Boss 1 Takes Damage");
        
    }

    
    
   
}
