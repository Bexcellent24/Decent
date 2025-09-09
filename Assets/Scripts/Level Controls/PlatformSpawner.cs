using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlatformSpawner : MonoBehaviour
{

    //----------Variables----------//

    
    [Header("Level 1 Platforms")] 
    [SerializeField] private GameObject levelStartPlatform;
    [SerializeField] private GameObject[] limboPlatformPrefabs;
    
    [Header("Boss 1 Platforms")]
    [SerializeField] private GameObject[] stage1BossPlatformPrefabs;
    [SerializeField] private GameObject[] stage2BossPlatformPrefabs;
    [SerializeField] private GameObject[] stage3BossPlatformPrefabs;
    [SerializeField] private GameObject[] stage4BossPlatformPrefabs;
    [SerializeField] private GameObject limboBossEntryPlatform;
    [SerializeField] private GameObject endingBossPlatform;
    [SerializeField] private GameObject endBossPlatform;
    
    
    [Header("Level 2 Platforms")] 
    [SerializeField] private GameObject level2StartPlatform;
    [SerializeField] private GameObject[] lustPlatformPrefabs;

    [Header("Boss 2 Platforms")] 
    [SerializeField] private GameObject[] lustBossPlatformPrefabs;
    [SerializeField] private GameObject lustBossEntryPlatform;
    [SerializeField] private GameObject endingLustBossPlatform;
    [SerializeField] private GameObject endLustBossPlatform;
    

    private int platformsPassed = 0;
    private int initialPlatformCount = 3;
    private Vector3 spawnPosition = Vector3.zero;
    private int platformCount = 0;
    private bool isBossTime = false;
    private int bossStage = 1;
    private int levelNumber = 0;
    private bool first2LevelsCompete = false;
    
    
    //----------MonoBehaviour Methods----------//
    private void Start()
    {
        SpawnInitialLimboPlatform();
    }
    
    private void OnEnable()
    {
        Platform.PlatformDespawnedTriggered += PlatformDespawnedTriggeredHandler;
        Level1Boss.OnBossHit += OnBossHitHandler;
        Level2Boss.OnBoss2Defeated += OnBoss2DefeatedHanlder;
    }
    private void OnDisable()
    {
        Platform.PlatformDespawnedTriggered -= PlatformDespawnedTriggeredHandler;
        Level1Boss.OnBossHit -= OnBossHitHandler;
        Level2Boss.OnBoss2Defeated -= OnBoss2DefeatedHanlder;
    }
    
    
   
    //----------Event Handlers----------//
    private void PlatformDespawnedTriggeredHandler()
    {
        platformsPassed++;
        platformCount--;
        if (!isBossTime)
        {
            RegularPlatformSpawner();
            return;
        }
        
        if(levelNumber == 0)
        {
            if (platformCount >= 2)
            {
                Debug.Log("Platform Count = " + platformCount);
                return;
            }
            
            switch (bossStage)
            {
                case 1: Stage1BossPlatformSpawner();
                    break;
                case 2: Stage2BossPlatformSpawner();
                    break;
                case 3: Stage3BossPlatformSpawner();
                    break;
                case 4: Stage4BossPlatformSpawner();
                    break;
            }
        }

        if (levelNumber == 1)
        {
            if (platformCount >= 3)
            {
                Debug.Log("Platform Count = " + platformCount);
                return;
            }
            
            LustBossPlatformSpawner();
        }
    }
    private void OnBossHitHandler(int bossLife)
    {
        switch (bossLife)
        {
            case 1: bossStage = 2;
                break;
            case 2: bossStage = 3;
                break;
            case 3: bossStage = 4;
                break;
            case 4: bossStage = 4;
                break;
            case 5: ResetLevel();
                break;
        }
    }
    private void OnBoss2DefeatedHanlder()
    {
        first2LevelsCompete = true;
        ResetLevel();
    }
    
    
    //----------Custom Private Methods----------//
    
    private void RegularPlatformSpawner()
    {
        if (platformsPassed == 3 && levelNumber == 0)
        {
            isBossTime = true;


            GameObject newPlatformGameObject1 = Instantiate(limboBossEntryPlatform, spawnPosition, Quaternion.identity);
            Platform platform1 = newPlatformGameObject1.GetComponent<Platform>();
            spawnPosition = platform1.ConnectPosition;
            platformCount++;
            return;
        }

        if (platformsPassed == 6 && levelNumber == 1)
        {
            isBossTime = true;
            
            GameObject newPlatformGameObject1 = Instantiate(lustBossEntryPlatform, spawnPosition, Quaternion.identity);
            Platform platform1 = newPlatformGameObject1.GetComponent<Platform>();
            spawnPosition = platform1.ConnectPosition;
            platformCount++;
            return;
        }
            
        

        if (levelNumber == 0)
        {
            int index = Random.Range(0, limboPlatformPrefabs.Length);
            GameObject newPlatformGameObject = Instantiate(limboPlatformPrefabs[index], spawnPosition, Quaternion.identity);
            Platform platform = newPlatformGameObject.GetComponent<Platform>();
            spawnPosition = platform.ConnectPosition;
            platformCount++;
            return;
        }
        if (levelNumber == 1)
        {
            int index = Random.Range(0, lustPlatformPrefabs.Length);
            GameObject newPlatformGameObject = Instantiate(lustPlatformPrefabs[index], spawnPosition, Quaternion.identity);
            Platform platform = newPlatformGameObject.GetComponent<Platform>();
            spawnPosition = platform.ConnectPosition;
            platformCount++;
        }
    }
    private void SpawnInitialLimboPlatform()
    {
        GameObject newPlatformGameObject1 = Instantiate(levelStartPlatform, spawnPosition, Quaternion.identity);
        Platform platform1 = newPlatformGameObject1.GetComponent<Platform>();
        spawnPosition = platform1.ConnectPosition;
        platformCount++;
        
        for (int i = 0; i < initialPlatformCount; i++)
        {
            int index = Random.Range(0, limboPlatformPrefabs.Length);
            GameObject newPlatformGameObject = Instantiate(limboPlatformPrefabs[index], spawnPosition, Quaternion.identity);
            Platform platform = newPlatformGameObject.GetComponent<Platform>();
            spawnPosition = platform.ConnectPosition;
            platformCount++;
        }
    }
    private void SpawnInitialLustPlatform()
    {
        GameObject newPlatformGameObject1 = Instantiate(level2StartPlatform, spawnPosition, Quaternion.identity);
        Platform platform1 = newPlatformGameObject1.GetComponent<Platform>();
        spawnPosition = platform1.ConnectPosition;
        platformCount++;
        
        for (int i = 0; i < initialPlatformCount; i++)
        {
            int index = Random.Range(0, lustPlatformPrefabs.Length);
            GameObject newPlatformGameObject = Instantiate(lustPlatformPrefabs[index], spawnPosition, Quaternion.identity);
            Platform platform = newPlatformGameObject.GetComponent<Platform>();
            spawnPosition = platform.ConnectPosition;
            platformCount++;
        }
    }
    private void Stage1BossPlatformSpawner()
    {
        Debug.Log("Stage 1 Boss Platform Spawned");
        int index = Random.Range(0, stage1BossPlatformPrefabs.Length);
        GameObject newPlatformGameObject = Instantiate(stage1BossPlatformPrefabs[index], spawnPosition, Quaternion.identity);
        Platform platform = newPlatformGameObject.GetComponent<Platform>();
        spawnPosition = platform.ConnectPosition;
        platformCount++;
    }
    private void Stage2BossPlatformSpawner()
    {
        Debug.Log("Stage 2 Boss Platform Spawned");
        int index = Random.Range(0, stage2BossPlatformPrefabs.Length);
        GameObject newPlatformGameObject = Instantiate(stage2BossPlatformPrefabs[index], spawnPosition, Quaternion.identity);
        Platform platform = newPlatformGameObject.GetComponent<Platform>();
        spawnPosition = platform.ConnectPosition;
        platformCount++;
    }
    private void Stage3BossPlatformSpawner()
    {
        Debug.Log("Stage 3 Boss Platform Spawned");
        int index = Random.Range(0, stage3BossPlatformPrefabs.Length);
        GameObject newPlatformGameObject = Instantiate(stage3BossPlatformPrefabs[index], spawnPosition, Quaternion.identity);
        Platform platform = newPlatformGameObject.GetComponent<Platform>();
        spawnPosition = platform.ConnectPosition;
        platformCount++;
    }
    private void Stage4BossPlatformSpawner()
    {
        Debug.Log("Stage 4 Boss Platform Spawned");
        int index = Random.Range(0, stage4BossPlatformPrefabs.Length);
        GameObject newPlatformGameObject = Instantiate(stage4BossPlatformPrefabs[index], spawnPosition, Quaternion.identity);
        Platform platform = newPlatformGameObject.GetComponent<Platform>();
        spawnPosition = platform.ConnectPosition;
        platformCount++;
    }

    private void LustBossPlatformSpawner()
    {
        int index = Random.Range(0, lustBossPlatformPrefabs.Length);
        GameObject newPlatformGameObject = Instantiate(lustBossPlatformPrefabs[index], spawnPosition, Quaternion.identity);
        Platform platform = newPlatformGameObject.GetComponent<Platform>();
        spawnPosition = platform.ConnectPosition;
        platformCount++;
        
        first2LevelsCompete = true;
    }
    
    private void ResetLevel()
    {
        isBossTime = false;
        bossStage = 1;
        platformsPassed = -2;

        if (levelNumber == 0)
        {
            GameObject newPlatformGameObject = Instantiate(endingBossPlatform, spawnPosition, Quaternion.identity);
            Platform platform = newPlatformGameObject.GetComponent<Platform>();
            spawnPosition = platform.ConnectPosition;
            platformCount++;
        
            GameObject newPlatformGameObject1 = Instantiate(endingBossPlatform, spawnPosition, Quaternion.identity);
            Platform platform1 = newPlatformGameObject1.GetComponent<Platform>();
            spawnPosition = platform1.ConnectPosition;
            platformCount++;
        
            GameObject newPlatformGameObject2 = Instantiate(endBossPlatform, spawnPosition, Quaternion.identity);
            Platform platform2 = newPlatformGameObject2.GetComponent<Platform>();
            spawnPosition = platform2.ConnectPosition;
            platformCount++;
        }
        else if (levelNumber == 1)
        {
            GameObject newPlatformGameObject2 = Instantiate(endingLustBossPlatform, spawnPosition, Quaternion.identity);
            Platform platform2 = newPlatformGameObject2.GetComponent<Platform>();
            spawnPosition = platform2.ConnectPosition;
            platformCount++;
            
            GameObject newPlatformGameObject1 = Instantiate(endingLustBossPlatform, spawnPosition, Quaternion.identity);
            Platform platform1 = newPlatformGameObject1.GetComponent<Platform>();
            spawnPosition = platform1.ConnectPosition;
            platformCount++;
            
            GameObject newPlatformGameObject = Instantiate(endLustBossPlatform, spawnPosition, Quaternion.identity);
            Platform platform = newPlatformGameObject.GetComponent<Platform>();
            spawnPosition = platform.ConnectPosition;
            platformCount++;
        }
        

        if (!first2LevelsCompete)
        {
            SpawnInitialLustPlatform();
            levelNumber = 1;
        }
        else
        {
            int randomLevel = Random.Range(0, 2);
            Debug.Log("Level Number: " + randomLevel);
            if (randomLevel == 0)
            {
                levelNumber = 0;
                SpawnInitialLimboPlatform();
                Debug.Log("Limbo Level");
            }
            else if (randomLevel == 1)
            {
                levelNumber = 1;
                SpawnInitialLustPlatform();
                Debug.Log("Lust Level");
            }
        }
        
    }

  
}
