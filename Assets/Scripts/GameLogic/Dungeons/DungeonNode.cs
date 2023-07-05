using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DungeonNode : MonoBehaviour
{
    //relevant for creating layoutPrefab
    public DungeonNode[] nextNodes;

    //Relevant for runtime logic
    //gets calculated with daily seed
    public List<string> nextPaths;
    public string nodeType;
    public int level;

    //gets calculated with run seed
    public Event nodeEvent;
    public string eventEnemy;
    //public string statType;
    //public string eventName;
    public int maxEventHealth;
    public int defaultGrowth;

    //on run. Calculated eventually also with run seed
    public int eventHealth;
    public int currentGrowth;
    public int chosenPathIndex = -1;

    bool hovered = false;

    public Vector3 PathPosition(int _pathIndex)
    {
        Vector3 result = Vector3.zero;
        if(nextNodes.Length > _pathIndex && nextPaths.Count > _pathIndex)
        {
            result = transform.position + 0.5f * (nextNodes[_pathIndex].transform.position - transform.position);
        }
        return result;
    }

    public void AffectCurrentGrowth(int _amount)
    {
        currentGrowth += _amount;
        if (currentGrowth < -2 * level)
            currentGrowth = -2 * level;
    }

    private void Update()
    {
        if (CameraMover._instance == null)
            return;
        Vector3 mousePosition = CameraMover._instance.mousePosition;
        mousePosition.z = transform.position.z;
        if ((mousePosition - transform.position).magnitude <= .5f)
        {
            //Debug.Log((mousePosition - transform.position).magnitude);
            //hovers over node
            hovered = true;
            if (Input.GetMouseButtonDown(0))
            {
                //on clicked
                //CameraMover._instance?.SetTargetPos(transform.position);
            }
        }
        else
        {
            hovered = false;
        }
    }

    void OnDrawGizmos()
    {
#if(UNITY_EDITOR)
        // Draw a yellow sphere at the transform's position
        float size = .2f;
        if (hovered)
            size = .5f;

        if (nodeEvent != null)
            Handles.Label(transform.position + Vector3.up * .4f + Vector3.left * .75f, nodeEvent.eventName);
        if (DatabaseManager._instance != null && DatabaseManager._instance.eventData.nodeTypes.Length > 1 && nodeType == DatabaseManager._instance.eventData.nodeTypes[1])
            Gizmos.color = Color.yellow;
        else if (DatabaseManager._instance != null && DatabaseManager._instance.eventData.nodeTypes.Length > 2 && nodeType == DatabaseManager._instance.eventData.nodeTypes[2])
            Gizmos.color = Color.red;
        else if (DatabaseManager._instance != null && DatabaseManager._instance.eventData.nodeTypes.Length > 3 && nodeType == DatabaseManager._instance.eventData.nodeTypes[3])
            Gizmos.color = Color.blue;
        else if (DatabaseManager._instance != null && DatabaseManager._instance.eventData.nodeTypes.Length > 4 && nodeType == DatabaseManager._instance.eventData.nodeTypes[4])
            Gizmos.color = Color.green;
        else if (DatabaseManager._instance != null && DatabaseManager._instance.eventData.nodeTypes.Length > 5 && nodeType == DatabaseManager._instance.eventData.nodeTypes[5])
            Gizmos.color = Color.cyan;
        else if (DatabaseManager._instance != null && DatabaseManager._instance.eventData.nodeTypes.Length > 6 && nodeType == DatabaseManager._instance.eventData.nodeTypes[6])
            Gizmos.color = Color.black;
        else if (DatabaseManager._instance != null && DatabaseManager._instance.eventData.nodeTypes.Length > 7 && nodeType == DatabaseManager._instance.eventData.nodeTypes[7])
            Gizmos.color = Color.magenta;
        else
            Gizmos.color = Color.grey;
        Gizmos.DrawSphere(transform.position, size);
        // Draws a blue line from this transform to the target
        if(nextNodes != null)
        {
            foreach (var item in nextNodes)
            {
                if (item == null)
                    continue;
                if (nextPaths == null || nextPaths.Count <= 0)
                {
                    Gizmos.color = Color.blue;
                }
                else
                {
                    for (int i = 0; i < nextPaths.Count; i++)
                    {
                        if (DatabaseManager._instance != null && DatabaseManager._instance.eventData.pathTypes.Length > 1 && nextPaths[i] == DatabaseManager._instance.eventData.pathTypes[1])
                            Gizmos.color = Color.yellow;
                        else if (DatabaseManager._instance != null && DatabaseManager._instance.eventData.pathTypes.Length > 2 && nextPaths[i] == DatabaseManager._instance.eventData.pathTypes[2])
                            Gizmos.color = Color.red;
                        else if (DatabaseManager._instance != null && DatabaseManager._instance.eventData.pathTypes.Length > 3 && nextPaths[i] == DatabaseManager._instance.eventData.pathTypes[3])
                            Gizmos.color = Color.blue;
                        else if (DatabaseManager._instance != null && DatabaseManager._instance.eventData.pathTypes.Length > 4 && nextPaths[i] == DatabaseManager._instance.eventData.pathTypes[4])
                            Gizmos.color = Color.green;
                        else if (DatabaseManager._instance != null && DatabaseManager._instance.eventData.pathTypes.Length > 5 && nextPaths[i] == DatabaseManager._instance.eventData.pathTypes[5])
                            Gizmos.color = Color.cyan;
                        else if (DatabaseManager._instance != null && DatabaseManager._instance.eventData.pathTypes.Length > 6 && nextPaths[i] == DatabaseManager._instance.eventData.pathTypes[6])
                            Gizmos.color = Color.black;
                        else if (DatabaseManager._instance != null && DatabaseManager._instance.eventData.pathTypes.Length > 7 && nextPaths[i] == DatabaseManager._instance.eventData.pathTypes[7])
                            Gizmos.color = Color.magenta;
                        else
                            Gizmos.color = Color.grey;
                    }
                }
                Gizmos.DrawLine(transform.position, item.transform.position);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (nextNodes != null)
        {
            foreach (var item in nextNodes)
            {
                if (item == null)
                    continue;
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(transform.position + 0.5f * (item.transform.position - transform.position), .4f);
                Gizmos.DrawSphere(transform.position + 0.60f * (item.transform.position - transform.position), .3f);
                Gizmos.DrawSphere(transform.position + 0.65f * (item.transform.position - transform.position), .2f);
                Gizmos.DrawCube(transform.position + 0.90f * (item.transform.position - transform.position), new Vector3(.2f, .2f, .2f));
            }
        }
#endif
    }
}
