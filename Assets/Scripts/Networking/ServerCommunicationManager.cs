using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public class ServerCommunicationManager : MonoBehaviour
{
    public static ServerCommunicationManager _instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            if (CommVis == null && transform.childCount >= 1)
                CommVis = transform.GetChild(0).gameObject;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this);
    }
    private string _URL = "https://script.google.com/macros/s/AKfycbxmcdlCpQ2CiN-4dmtM9iFHaQXyIVne-wGqieddb9eEKR7WkGh3ELBJh5E5VuJDh12Q/exec";

    private List<WebRequestInstance> webRequestQueue = new List<WebRequestInstance>();
    private UnityWebRequest _webRequest;
    private bool Imported = true;
    private Request lastGetInfo;
    public Request LastGetInfo { get { return lastGetInfo; } }
    private string lastMessage;
    public string LastMessage { get { return lastMessage; } }
    private bool Exported = true;

    public GameObject CommVis;
    public TMP_Text errorInfoText; 

    float waitTimer = 0.0f;
    float waitTime = 30.0f;

    // Update is called once per frame
    void Update()
    {
        if (_webRequest != null && _webRequest.isDone && !Imported)
            CheckForImportRequestEnd();
        if (_webRequest != null && _webRequest.isDone && !Exported)
        {
            _webRequest = null;
            Exported = true;
        }
        if(_webRequest == null && webRequestQueue.Count > 0)
        {
            PopRequestQueue();
        }
        waitTimer += Time.deltaTime;
        if(waitTimer >= waitTime && _webRequest != null && !Imported && webRequestQueue.Count > 0)
        {
            //retry last Message
            //Debug.LogWarning("Retry sending message. TimeOut");
            //PopRequestQueue();
        }
    }

    private void CheckForImportRequestEnd()
    {
        if (_webRequest != null && _webRequest.isDone)
        {
            if (errorInfoText != null)
                errorInfoText.text = "";
            if (_webRequest.result != UnityWebRequest.Result.Success)
            {
                if (errorInfoText != null)
                    errorInfoText.text = _webRequest.error;
                Debug.LogWarning(_webRequest.error);
                switch (_webRequest.result)
                {
                    case UnityWebRequest.Result.InProgress:
                        break;
                    case UnityWebRequest.Result.Success:
                        break;
                    case UnityWebRequest.Result.ConnectionError:
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        break;
                    case UnityWebRequest.Result.DataProcessingError:
                        break;
                    default:
                        break;
                }
                if (_webRequest.error == "Request timeout")
                {
                    //Retry operation!
                    _webRequest = null;
                    Imported = true;
                    WebRequestInstance _temp = webRequestQueue[0];
                    string parameters = "?data=" + _temp.jsonText;
                    WebRequestInstance _newTemp = new WebRequestInstance { jsonText = _temp.jsonText, request = UnityWebRequest.Get(_URL + parameters), requestType = _temp.requestType, waitForAnswer = _temp.waitForAnswer, simpleEvent = _temp.simpleEvent, messageEvent = _temp.messageEvent };
                    webRequestQueue[0] = _newTemp;
                    if (webRequestQueue.Count >= 1)
                        CommVis.SetActive(true);
                    else
                        CommVis.SetActive(false);
                }
                else if (_webRequest.error == "Cannot resolve destination host")
                {
                    //No Internet Connection!
                    //Just Retry until its working lol
                    _webRequest = null;
                    Imported = true;
                    WebRequestInstance _temp = webRequestQueue[0];
                    string parameters = "?data=" + _temp.jsonText;
                    WebRequestInstance _newTemp = new WebRequestInstance { jsonText = _temp.jsonText, request = UnityWebRequest.Get(_URL + parameters), requestType = _temp.requestType, waitForAnswer = _temp.waitForAnswer, simpleEvent = _temp.simpleEvent, messageEvent = _temp.messageEvent };
                    webRequestQueue[0] = _newTemp;
                    if (webRequestQueue.Count >= 1)
                        CommVis.SetActive(true);
                    else
                        CommVis.SetActive(false);
                }
            }
            else
            {
                ExecuteImport();
            }
            
        }
    }

    private void ExecuteImport()
    {
        if (_webRequest == null || !_webRequest.isDone)
            return;
        string message = _webRequest.downloadHandler.text;
        //ToDO! more length for the requestmarker!!! maybe check where the first non numeric symbol is ('{' maybe?)
        string requestMarker = message.Split('{')[0];
        //string requestMarker = message.Substring(0, 1);
        lastMessage = message.Remove(0, requestMarker.Length);
        int requestTypeInt;
        if (!Int32.TryParse(requestMarker, out requestTypeInt))
            return;
        Request requestType = (Request)Int32.Parse(requestMarker);
        _webRequest = null;
        Imported = true;
        //Debug.Log(lastMessage);
        switch (requestType)
        {
            case Request.Error:
                ErrorHandling(lastMessage);
                break;
            case Request.SignUp:
                //start game
                //create new userprofil with data given
                //tutorials + first hero
                //DatabaseManager._instance.UpdateActivePlayerFromServer(lastMessage);
                break;
            case Request.SignIn:
                //get user profil
                //if time stamp on online profile is newer than local discard local data and reapply online data                
                break;
            case Request.GetPlayerData:
                // generically download a specified user profile for diverse use cases
                break;
            case Request.DownloadHeroList:
                //download the default hero list and replace local copy
                DatabaseManager._instance.UpdateDefaultHeroListFromServer(lastMessage);
                break;
            case Request.DownloadEventData:
                //download the default hero list and replace local copy
                DatabaseManager._instance.UpdateEventDataFromServer(lastMessage);
                if(DeleventSystem.eventDataDownloaded != null)
                    DeleventSystem.eventDataDownloaded();
                break;
            case Request.PushPlayerData:
                break;
            case Request.PushDungeonData:
                break;
            case Request.DownloadDungeonData:
                DatabaseManager._instance.UpdateDungeonDataFromServer(LastMessage);
                break;
            case Request.PushInventory:
                break;
            case Request.PushBlacklist:
                break;
            case Request.PullRewardTable:
                DatabaseManager._instance.UpdateRewardTableFromServer(lastMessage);
                break;
            case Request.UploadOffer:
                break;
            case Request.UpdateOffer:
                break;
            case Request.PullTradeOffers:
                DatabaseManager._instance.UpdateTradeDataFromServer(lastMessage);
                break;
            case Request.DeleteOffers:
                //ToDo
                break;
            case Request.PullGlobalData:
                DatabaseManager._instance.UpdateGlobalDataFromServer(lastMessage);
                break;
            case Request.pushDexEntries:
                //DatabaseManager._instance.UpdateGlobalDataFromServer(lastMessage);
                break;
            case Request.getPlotData:
                if(ClusterAnalysis._instance != null)
                {
                    ClusterAnalysis._instance.plotData = JsonUtility.FromJson<PlotData>(lastMessage);
                }
                break;
            default:
                break;
        }
        WebRequestInstance _temp = webRequestQueue[0];
        webRequestQueue.RemoveAt(0); //in case one of the functions throw an error i want to ´still remove the message
        if (_temp.simpleEvent != null)
        {
            _temp.simpleEvent();
        }
        if (_temp.messageEvent != null)
        {
            _temp.messageEvent(lastMessage);
        }

        if (webRequestQueue.Count >= 1)
            CommVis.SetActive(true);
        else
            CommVis.SetActive(false);
        if (errorInfoText != null)
            errorInfoText.text = "Communicating... Do not close app";
    }

    private void ErrorHandling(string _message)
    {
        Debug.LogWarning(_message);
        switch (LastGetInfo)
        {
            case Request.Error:
                break;
            case Request.SignUp:
                lastMessage = JsonUtility.ToJson(new PlayerData { playerId = "Error", password = _message });
                break;
            case Request.SignIn:
                lastMessage = JsonUtility.ToJson(new PlayerData { playerId = "Error", password = _message });
                break;
            case Request.GetPlayerData:
                break;
            case Request.DownloadHeroList:
                break;
            case Request.DownloadEventData:
                break;
            case Request.PushPlayerData:
                break;
            case Request.PushDungeonData:
                break;
            case Request.DownloadDungeonData:
                break;
            case Request.PushInventory:
                break;
            case Request.PushBlacklist:
                break;
            case Request.PullRewardTable:
                break;
            case Request.UploadOffer:
                break;
            case Request.UpdateOffer:
                break;
            case Request.PullTradeOffers:
                break;
            case Request.DeleteOffers:
                //ToDo
                break;
            case Request.PullGlobalData:
                break;
            case Request.pushDexEntries:
                break;
            case Request.getPlotData:
                break;
            default:
                break;
        }
    }

    public void PopRequestQueue()
    {
        WebRequestInstance _temp = webRequestQueue[0];
        errorInfoText.text = _temp.requestType.ToString();
        _webRequest = _temp.request;
        _webRequest.timeout = (int)waitTime;
        _webRequest.SendWebRequest();
        Debug.Log(_temp.requestType);
        if (_temp.waitForAnswer)
        {
            lastGetInfo = _temp.requestType;
            Imported = false;
            waitTimer = 0.0f;
        }
        else
        {
            Exported = false;
            webRequestQueue.RemoveAt(0);
        }

        if (webRequestQueue.Count >= 1)
            CommVis.SetActive(true);
        else
            CommVis.SetActive(false);
    }

    public void GetInfo(Request _request, string _message = "", DeleventSystem.SimpleEvent _simpleEvent = null, DeleventSystem.MessageEvent _messageEvent = null)
    {
        if (_webRequest != null)
        {
            //return;
        }
        ServerRequest newRequest = new ServerRequest { request = _request, jsonData = _message};
        var JsonPackage = JsonUtility.ToJson(newRequest);
        string parameters = "?data=" + JsonPackage;
        //Debug.Log(_request.ToString() + " length: " + JsonPackage.Length.ToString());
        if(parameters.Length >= 3000)
        {
            Debug.LogError("Attention. URL Might be too long!!! " + _request.ToString() + " length: " + parameters.Length.ToString());
        }
        //check for message length?
        WebRequestInstance _temp = new WebRequestInstance{jsonText = JsonPackage, request = UnityWebRequest.Get(_URL + parameters), requestType = _request, waitForAnswer = true, simpleEvent = _simpleEvent, messageEvent = _messageEvent};
        webRequestQueue.Add(_temp);        
    }

    public void PostInfo(Request _request, string _message = "")
    {
        if(_webRequest != null)
        {
            return;
        }
        ServerRequest newRequest = new ServerRequest { request = Request.DownloadHeroList, jsonData = _message };
        var JsonPackage = JsonUtility.ToJson(newRequest);
        //check for message length?
        WWWForm form = new WWWForm();
        form.AddField("data", JsonPackage);
        WebRequestInstance _temp = new WebRequestInstance { jsonText = JsonPackage, request = UnityWebRequest.Post(_URL, form), requestType = _request, waitForAnswer = false };
        webRequestQueue.Add(_temp);
    }

    public void DoServerRequest(Request _request, DeleventSystem.SimpleEvent _simpleEvent = null, DeleventSystem.MessageEvent _messageEvent = null)
    {
        switch (_request)
        {
            case Request.Error:
                break;
            case Request.SignUp:
                //CAn not be called from here siince the new username and pw are likely not in the database
                ServerCommunicationManager._instance.GetInfo(Request.SignUp, JsonUtility.ToJson(new LoginInfo { playerId = DatabaseManager._instance.activePlayerData.playerId, password = DatabaseManager._instance.activePlayerData.password }), _simpleEvent, _messageEvent);
                break;
            case Request.SignIn:
                //Might be not called from here if there is no pw and username in the database
                ServerCommunicationManager._instance.GetInfo(Request.SignIn, JsonUtility.ToJson(new LoginInfo { playerId = DatabaseManager._instance.activePlayerData.playerId, password = DatabaseManager._instance.activePlayerData.password }), _simpleEvent, _messageEvent);
                break;
            case Request.GetPlayerData:
                //ToDo implement da shit
                break;
            case Request.DownloadHeroList:
                ServerCommunicationManager._instance.GetInfo(Request.DownloadHeroList, "", _simpleEvent, _messageEvent);
                break;
            case Request.DownloadEventData:
                ServerCommunicationManager._instance.GetInfo(Request.DownloadEventData, "", _simpleEvent, _messageEvent);
                break;
            case Request.PushPlayerData:
                //ToDO: Decouple PlayerData from Inventory
                DoServerRequest(Request.PushInventory);
                DatabaseManager._instance.activePlayerData.lastUpdate = DateTime.Now.ToUniversalTime().ToString("u");
                DatabaseManager._instance.SaveGameDataLocally();
                ServerCommunicationManager._instance.GetInfo(Request.PushPlayerData, JsonUtility.ToJson(new UploadPlayerData( DatabaseManager._instance.activePlayerData)), _simpleEvent, _messageEvent);
                break;
            case Request.PushDungeonData:
                UploadDungeonData dataDungeon = new UploadDungeonData { dungeonData = DatabaseManager._instance.dungeonData, playerInfo = new LoginInfo { playerId = DatabaseManager._instance.activePlayerData.playerId, password = DatabaseManager._instance.activePlayerData.password } };
                ServerCommunicationManager._instance.GetInfo(Request.PushDungeonData, JsonUtility.ToJson(dataDungeon), _simpleEvent, _messageEvent);
                break;
            case Request.DownloadDungeonData:
                ServerCommunicationManager._instance.GetInfo(Request.DownloadDungeonData, JsonUtility.ToJson(new LoginInfo { playerId = DatabaseManager._instance.activePlayerData.playerId, password = DatabaseManager._instance.activePlayerData.password }), _simpleEvent, _messageEvent);
                break;
            case Request.PushInventory:
                //ToDo: Splitted into multiple Requests
                List<UploadInventoryEntry> entries = new List<UploadInventoryEntry>();
                for (int i = 0; i < DatabaseManager._instance.activePlayerData.inventory.Count; i++)
                {
                    entries.Add(new UploadInventoryEntry { index = i, entry = DatabaseManager._instance.activePlayerData.inventory[i] });
                }
                UploadInventory upload = new UploadInventory();
                upload.loginInfo = new LoginInfo { playerId = DatabaseManager._instance.activePlayerData.playerId, password = DatabaseManager._instance.activePlayerData.password };
                while (entries.Count > 0)
                {
                    upload.inventorySegment.Add(entries[0]);
                    entries.RemoveAt(0);
                    if(upload.inventorySegment.Count >= 10 && entries.Count > 0)
                    {
                        ServerCommunicationManager._instance.GetInfo(Request.PushInventory, JsonUtility.ToJson(upload));
                        upload = new UploadInventory();
                        upload.loginInfo = new LoginInfo { playerId = DatabaseManager._instance.activePlayerData.playerId, password = DatabaseManager._instance.activePlayerData.password };
                    }
                }
                //Last message also invokes events
                ServerCommunicationManager._instance.GetInfo(Request.PushInventory, JsonUtility.ToJson(upload), _simpleEvent, _messageEvent);
                break;
            case Request.PushBlacklist:
                //ToDo: Splitted into multiple Requests
                break;
            case Request.PullRewardTable:
                ServerCommunicationManager._instance.GetInfo(Request.PullRewardTable, "", _simpleEvent, _messageEvent);
                //ToDo: Splitted into multiple Requests
                break;
            case Request.UploadOffer:
                //ToDo
                break;
            case Request.UpdateOffer:
                //ToDo
                break;
            case Request.PullTradeOffers:
                //ToDo
                ServerCommunicationManager._instance.GetInfo(Request.PullTradeOffers, "", _simpleEvent, _messageEvent);
                break;
            case Request.DeleteOffers:
                //ToDo
                break;
            case Request.PullGlobalData:
                ServerCommunicationManager._instance.GetInfo(Request.PullGlobalData, "", _simpleEvent, _messageEvent);
                break;
            case Request.pushDexEntries:
                //ServerCommunicationManager._instance.GetInfo(Request.PullGlobalData, "", _simpleEvent, _messageEvent);
                break;
            case Request.getPlotData:
                ServerCommunicationManager._instance.GetInfo(Request.getPlotData, "", _simpleEvent, _messageEvent);
                break;
            default:
                break;
        }
    }
}

[System.Serializable]
public class ServerRequest
{
    public Request request;
    public string jsonData;
}

public enum Request
{
    Error,
    SignUp,
    SignIn,
    GetPlayerData,
    DownloadHeroList,
    DownloadEventData,
    PushPlayerData,
    PushDungeonData,
    DownloadDungeonData,
    PushInventory,
    PushBlacklist,
    PullRewardTable,
    UploadOffer,
    UpdateOffer,
    PullTradeOffers,
    DeleteOffers,
    PullGlobalData,
    pushDexEntries,
    getPlotData
}

//for the request queue:
public struct WebRequestInstance
{
    public UnityWebRequest request;
    public string jsonText;
    public Request requestType;
    public bool waitForAnswer;
    public DeleventSystem.SimpleEvent simpleEvent;
    public DeleventSystem.MessageEvent messageEvent;
}

[System.Serializable]
public class TextMessage
{
    public string text;
}
