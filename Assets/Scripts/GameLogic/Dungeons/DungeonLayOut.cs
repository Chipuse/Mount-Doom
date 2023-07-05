using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonLayOut : MonoBehaviour
{
    public DungeonNode startNode;
    public DungeonNode endNode;
    public DungeonType dungeonType; 

    //not to fill out
    public List<DungeonNode> nodes;

    public void FocusCamera()
    {
        //ToDo
        if (PlayerParty.visualsActive)
            CameraMover.SetTargetPos(PlayerParty._instance.visuals.transform.position);
        else
            CameraMover.SetTargetPos(startNode.transform.position);
    }

    public void SetupDungeonDailySeed(int _seed, DungeonDifficulty _ddiff = null)
    {
        UnityEngine.Random.InitState(_seed);
        //set dungeon nodes
        nodes = new List<DungeonNode>();
        SetupNodeDailySeed(startNode, _ddiff);
        startNode.nodeType = DatabaseManager._instance.eventData.nodeTypes[1];
        endNode.nodeType = DatabaseManager._instance.eventData.nodeTypes[2];
        DontDestroyOnLoad(this);
        UnityEngine.Random.InitState((int)DateTime.Now.ToUniversalTime().Ticks);
    }

    public void SetupDungeonRunSeed(int _seed)
    {
        UnityEngine.Random.InitState(_seed);
        nodes = new List<DungeonNode>();
        SetupNodeRunSeed(startNode);
        UnityEngine.Random.InitState((int)DateTime.Now.ToUniversalTime().Ticks);
    }

    public void ResetNodes()
    {
        foreach (var item in nodes)
        {
            item.eventHealth = item.maxEventHealth;
            item.currentGrowth = item.defaultGrowth;
            item.chosenPathIndex = -1;
            //maybe i also need to reset hero information in the future
        }
    }

    public void SetupNodeDailySeed(DungeonNode _node, DungeonDifficulty _ddiff = null)
    {
        if (nodes.Contains(_node))
            return;
        nodes.Add(_node);
        _node.nodeType = DatabaseManager._instance.eventData.nodeTypes[UnityEngine.Random.Range(3, DatabaseManager._instance.eventData.nodeTypes.Length)];
        
        if(_ddiff == null)
            _node.level = UnityEngine.Random.Range(1, 8);
        else
        {
            _node.level = UnityEngine.Random.Range(_ddiff.minLvl, _ddiff.maxLvl + 1);
        }

        for (int i = 0; i < _node.nextNodes.Length; i++)
        {
            _node.nextPaths.Add(DatabaseManager._instance.eventData.pathTypes[UnityEngine.Random.Range(1, DatabaseManager._instance.eventData.pathTypes.Length)]);
            SetupNodeDailySeed(_node.nextNodes[i], _ddiff);
        }
    }

    public void SetupNodeRunSeed(DungeonNode _node)
    {
        if (nodes.Contains(_node))
            return;
        nodes.Add(_node);

        if(DatabaseManager._instance.eventData.GetNodeTypeIndex(_node.nodeType) <= 2)
        {
            if (DatabaseManager._instance.eventData.GetNodeTypeIndex(_node.nodeType) == 0)
                _node.nodeEvent = new Event { eventName = "none", endText = "none", startText = "none", statType = "social" };
            else if (DatabaseManager._instance.eventData.GetNodeTypeIndex(_node.nodeType) == 1)
                _node.nodeEvent = new Event { eventName = "Start of the Quest", endText = "The heroes left the warmth of the home and face the cold of the unknown", startText = "Preperations are made", statType = "social" };
            else if (DatabaseManager._instance.eventData.GetNodeTypeIndex(_node.nodeType) == 2)
                _node.nodeEvent = new Event { eventName = "End of the Quest", endText = "The heroic deed was done and our heroes will now continue to walk their paths. What will the future bring?", startText = "Almost there. The grande finale is so close!", statType = "physical" };
            _node.eventEnemy = DatabaseManager._instance.eventData.textFlavours.textsEnemyNames[UnityEngine.Random.Range(0, DatabaseManager._instance.eventData.textFlavours.textsEnemyNames.Length)].name;
        }
        else
        {
            //_node.statType = DatabaseManager._instance.eventData.eventDecks.[UnityEngine.Random.Range(0, DatabaseManager._instance.eventData.nodeTypes.Length)];
            _node.nodeEvent = DatabaseManager._instance.eventData.eventDecks[DatabaseManager._instance.eventData.GetNodeTypeIndex(_node.nodeType)].deck[UnityEngine.Random.Range(0, DatabaseManager._instance.eventData.eventDecks[DatabaseManager._instance.eventData.GetNodeTypeIndex(_node.nodeType)].deck.Length)];
            //ToDo
            _node.eventEnemy = DatabaseManager._instance.eventData.textFlavours.textsEnemyNames[UnityEngine.Random.Range(0, DatabaseManager._instance.eventData.textFlavours.textsEnemyNames.Length)].name;
        }
        //toDo Rika neue function für dungeonHealth
        //_node.maxEventHealth = UnityEngine.Random.Range(100 *_node.level , 150 * _node.level);
        //_node.maxEventHealth = (int)(200 + (100 * Math.Pow(2, _node.level - 1))/(4) ) * 2;
        //ToDo dungeonLevel!!!!
        var dungeonLevel = 4;
        _node.maxEventHealth = (int)(200 * dungeonLevel + (100 * Math.Pow(2, _node.level - 1))/( 8 - dungeonLevel));
        int randomOffset = UnityEngine.Random.Range((-_node.maxEventHealth) / 20, (_node.maxEventHealth) / 10);
        _node.maxEventHealth += randomOffset;
        _node.eventHealth = _node.maxEventHealth;
        _node.defaultGrowth = UnityEngine.Random.Range(1 * _node.level, 2 * _node.level);
        _node.currentGrowth = _node.defaultGrowth;
        for (int i = 0; i < _node.nextNodes.Length; i++)
        {
            SetupNodeRunSeed(_node.nextNodes[i]);
        }
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        
        Gizmos.color = Color.green;
        if(startNode != null)
            Gizmos.DrawSphere(startNode.transform.position, .3f);
        Gizmos.color = Color.red;
        if (endNode != null)
            Gizmos.DrawSphere(endNode.transform.position, .3f);

        // Draws a blue line from this transform to the target

    }
}

//create and recreate a dungeon run:
//seed for randomization
//name of the dungeon layout -> to use the right prefab
//step count -> can be used to recreate any point of time ine this dungeon run

//two runs of dungeon creation + randomisation
//1: choosing different dungeon layouts and applying nodetypes to them (Seed 1)
//2: (needs set party information and starting time) assigns actual events to each nodetype based on event list (also event health), then calculates all steps until end of dungeon. Important: decision makings are relative to seed 2
