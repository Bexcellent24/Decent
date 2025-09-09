using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public class DatabaseManger : MonoBehaviour
{

    public delegate void DataFetchCompleteAction(string path, JSONNode data);
    public static event DataFetchCompleteAction OnDataFetchConplete;
    
    [SerializeField] private string baseUrl = "";
    
    public static DatabaseManger Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
    }

    //DatabaseManager.Instance.AddUser(user);
    public void AddPlayerScore(UserObject userObject)
    {
        byte[] body = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(userObject));

        StartCoroutine(PostData("scores", body));
    }
    

    private IEnumerator PostData(string path, byte[] body)
    {
        string URL = baseUrl + path + ".json";

        UnityWebRequest www = new UnityWebRequest(URL, "POST");

        www.uploadHandler = new UploadHandlerRaw(body);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError ||
            www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log("User score added to database");
            Debug.Log(www.downloadHandler.text);
        }
    }

    public void GetData(string path)
    {
        StartCoroutine(FetchData(path));
    }
    
    
    private IEnumerator FetchData(string path)
    {
        string URL = baseUrl + path + ".json";

        UnityWebRequest www = UnityWebRequest.Get(URL);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError ||
            www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log("Data fetched " + www.downloadHandler.text);

            
            var data = JSON.Parse(www.downloadHandler.text);
            var sortedData = SortDataByHighScore(data);

            if (sortedData != null)
            {
                Debug.Log("Data sorted by high score: " + sortedData);
                OnDataFetchConplete?.Invoke(path, sortedData);
            }
            else
            {
                Debug.LogWarning("No valid entries found in the data.");
            }
        }
    }
    
    private JSONNode SortDataByHighScore(JSONNode data)
    {
        List<JSONNode> dataList = new List<JSONNode>();

        foreach (var entry in data.Children)
        {
            dataList.Add(entry);
        }

        dataList.Sort((x, y) => y["score"].AsInt.CompareTo(x["score"].AsInt));

        JSONArray sortedArray = new JSONArray();
        foreach (var entry in dataList)
        {
            sortedArray.Add(entry);
        }

        return sortedArray;
    }
   
}