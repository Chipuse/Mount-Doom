using NotificationSamples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PushManager : MonoBehaviour
{
    public static PushManager _instance;
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
            var channel = new GameNotificationChannel("ChannelId", "Default Game Channel", "Generic notifications");
            manager.Initialize(channel);
        }
        else
        {
            Destroy(this);
        }
    }

    public GameNotificationsManager manager;
    public GameNotificationsManager errorManager;
    // Start is called before the first frame update
    void Start()
    {
        if(_instance == this)
        {
            //init notificationmanager
            

            //int delay = 20;

            //IGameNotification notification = manager.CreateNotification();
            //notification.Title = "Test Title";
            //notification.LargeIcon = "helia";
            //notification.SmallIcon = "smol";
            //notification.Body = "Test body "+ delay.ToString();
            //notification.DeliveryTime = DateTime.Now.AddSeconds(delay);
            //manager.ScheduleNotification(notification);

            //ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay);
        }
    }

    public static void Test()
    {
        int delay = 1;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
        ScheduleNotification("Note" + delay.ToString(), "Body: Delay = " + delay.ToString(), delay); delay += 5;
    }

    public static void ScheduleNotification(string _title, string _body, DateTime _deliveryTime, string icon = "random")
    {
        IGameNotification notification = _instance.manager.CreateNotification();
        if (notification == null)
            return;
        notification.Title = _title;
        icon = StringForIcon(icon);
        if (icon == "random" && DatabaseManager._instance.defaultHeroData != null && DatabaseManager._instance.defaultHeroData.defaultHeroList.Length > 0)
            icon = StringForIcon(DatabaseManager._instance.defaultHeroData.defaultHeroList[UnityEngine.Random.Range(0, DatabaseManager._instance.defaultHeroData.defaultHeroList.Length)].heroId);
        notification.LargeIcon = icon;
        notification.SmallIcon = "smol";
        notification.Body = _body;
        notification.DeliveryTime = _deliveryTime;
        _instance.manager.ScheduleNotification(notification); //.Reschedule = true;
    }

    public static void ScheduleNotification(string _title, string _body, int _delay, string icon = "random")
    {
        IGameNotification notification = _instance.manager.CreateNotification();
        if (notification == null)
            return;
        notification.Title = _title;
        notification.BadgeNumber = _delay;
        icon = StringForIcon(icon);
        if (icon == "random" && DatabaseManager._instance.defaultHeroData != null && DatabaseManager._instance.defaultHeroData.defaultHeroList.Length > 0)
            icon = StringForIcon(DatabaseManager._instance.defaultHeroData.defaultHeroList[UnityEngine.Random.Range(0, DatabaseManager._instance.defaultHeroData.defaultHeroList.Length)].heroId);
        //ScheduleError(icon);
        notification.LargeIcon = icon;
        notification.SmallIcon = "smol";
        notification.Body = _body + icon;
        notification.DeliveryTime = DateTime.Now.AddSeconds(_delay);
        _instance.manager.ScheduleNotification(notification);  //.Reschedule = true;
    }

    public static void ScheduleError(string _body, string _title = "error")
    {
        IGameNotification notification = _instance.manager.CreateNotification();
        if (notification == null)
            return;
        notification.Title = _title;
        notification.LargeIcon = "error";
        notification.SmallIcon = "smol";
        notification.Body = _body;
        notification.DeliveryTime = DateTime.Now;
        _instance.manager.ScheduleNotification(notification).Reschedule = true;
    }

    static string StringForIcon(string _input)
    {
        string output = _input.ToLower();
        output = output.Replace(" ", "");
        output = output.Replace("-", "");
        return output;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
