using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleventSystem
{
    public delegate void SimpleEvent();
    public static SimpleEvent eventDataDownloaded;
    public static SimpleEvent dungeonRunFinished;

    public static SimpleEvent DungeonStep; //When the dungeon advances by one step
    public static SimpleEvent DungeonStart; //When the dungeon is started
    public static SimpleEvent DungeonEnd; //when the final step of the dungeon is reached
    public static SimpleEvent DungeonEvent; //When an event or similar action in the dungeon is finished
    public static SimpleEvent DungeonEventStart; //When an event or similar action in the dungeon is finished
    public static SimpleEvent DungeonEventEnd; //When an event or similar action in the dungeon is finished
    public static SimpleEvent DungeonLog; //When an event or similar action in the dungeon is finished
    public static SimpleEvent DungeonRewardFinished;
    public static SimpleEvent DungeonCancel;
    public static SimpleEvent RewardHealthChanged;

    public static SimpleEvent TradeStart;
    public static SimpleEvent TradeEnd;
    public static SimpleEvent TradeCancel;
    public static SimpleEvent TradeStep;

    public delegate void MessageEvent(string _message);

    public delegate void PlayerDataEvent(PlayerData _playerData);
    public static PlayerDataEvent trySignUp;
    public static PlayerDataEvent trySignIn;
}
