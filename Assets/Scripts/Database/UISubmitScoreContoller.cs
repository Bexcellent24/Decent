using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISubmitScoreContoller : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputName;
    [SerializeField] private Button submitButton;

    private int runningScore;
    private void Start()
    {
        submitButton.onClick.AddListener(SubmitForm);
        
    }
    
    private void SubmitForm()
    {
        submitButton.interactable = false;
        UserObject user = new UserObject()
        {
            name = inputName.text,
            score = GameManager.Instance.score,
            levelsCompleted = GameManager.Instance.levelsCompleted,
        };
        DatabaseManger.Instance.AddPlayerScore(user);
    }
}
