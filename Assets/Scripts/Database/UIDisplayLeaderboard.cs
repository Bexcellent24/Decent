using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleJSON;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIDisplayLeaderboard : MonoBehaviour
{
    [SerializeField] private GameObject entryTemplate;
    [SerializeField] private GameObject verticalLayoutGameObject;
    private TextMeshProUGUI[] userInfo;
    private int userPos;
    //[SerializeField] private TextMeshProUGUI userScore;
    

    private void Start()
    {
        entryTemplate.GameObject().SetActive(false);
        DatabaseManger.Instance.GetData("scores");
    }
    private void OnEnable()
    {
        DatabaseManger.OnDataFetchConplete += OnDataFetchCompleteHandler;
    }
    private void OnDisable()
    {
        DatabaseManger.OnDataFetchConplete -= OnDataFetchCompleteHandler;
    }

    private void OnDataFetchCompleteHandler(string path, JSONNode data)
    {
        if (path != "scores")
        {
            return;
        }
        
        foreach (JSONNode user in data)
        {
            userPos++;
            CreateEntry(user);
        }
        
    }

    private void CreateEntry(JSONNode userData)
    {
        string name = userData["name"];
        string score = userData["score"];
        string levelsCompleted = userData["levelsCompleted"];
        
        GameObject userEntry = Instantiate(entryTemplate, verticalLayoutGameObject.transform);
        
        userInfo = userEntry.GetComponentsInChildren<TextMeshProUGUI>();

        
        if (userInfo.Length <= 4)
        {
            userInfo[0].text = userPos.ToString();
            userInfo[1].text = name;
            userInfo[2].text = levelsCompleted;
            userInfo[3].text = score;
        }
        else
        {
            Debug.LogError("Not enough TextMeshPro components found!");
        }
        userEntry.GameObject().SetActive(true);
    }
    
   
}
