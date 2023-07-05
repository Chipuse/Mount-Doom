using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonNodeVisual : MonoBehaviour
{
    DungeonNode dungeonNode;

    //LineRenderer lineRenderer;

    public TMP_Text nodeType;
    public TMP_Text eventName;
    [SerializeField] Image nodeImage;

    //public TMP_Text eventGrowth;
    //public CheapProgressBar pgBar;

    public GameObject pathVisualPrefab;
    public List<Image> pathVisualizer = new List<Image>();

    [SerializeField] Color clearColor;
    //public PathGradient[] pathGradients;
    //Dictionary<string, Gradient> pathGradientDict;

    // Start is called before the first frame update
    void Awake()
    {
        if (dungeonNode == null)
            dungeonNode = gameObject.GetComponent<DungeonNode>();
       // if (lineRenderer == null)
            //lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    private void Start()
    {
        pathVisualizer.Clear();

        foreach (var node in dungeonNode.nextNodes)
        {
            /*GameObject go = Instantiate(pathVisualPrefab, transform.position, transform.rotation);
            LineRenderer lr = go.GetComponent<LineRenderer>();
            Vector3[] positions = new Vector3[]
            {
                dungeonNode.transform.position,
                node.transform.position
            };
            lr.positionCount = 2;
            lr.SetPositions(positions);

            
            pathVisualizer.Add(lr);
            go.transform.SetParent(this.transform);*/


            Vector3 start = dungeonNode.transform.position;
            Vector3 end = node.transform.position;

            var direction = end - start;

            GameObject go = Instantiate(pathVisualPrefab, transform.position, transform.rotation);

            Image image = go.GetComponent<PathPrefabWorkAround>().path;
            
            image.transform.SetPositionAndRotation(dungeonNode.transform.position, Quaternion.FromToRotation(Vector3.up, direction));
            image.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Vector3.Distance(start, end));
            image.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0.2f);

            pathVisualizer.Add(image);
            go.transform.SetParent(this.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        nodeType.text = "Lvl" + dungeonNode.level.ToString() + " " + dungeonNode.nodeType;

        if(IconStruct.IconDictionary.ContainsKey(dungeonNode.nodeType))
        {
            nodeImage.sprite = IconStruct.IconDictionary[dungeonNode.nodeType].sprite;
            nodeImage.color = IconStruct.IconDictionary[dungeonNode.nodeType].color;
        }

        //pgBar.SetVal(dungeonNode.eventHealth, dungeonNode.maxEventHealth);
        
        if (dungeonNode.eventHealth != dungeonNode.maxEventHealth)
        {
            eventName.text = dungeonNode.nodeEvent.eventName;
            //eventGrowth.text = "Growth: " + dungeonNode.currentGrowth.ToString();
        }
        else
        {
            eventName.text = "???";
        }

        for (int i = 0; i < pathVisualizer.Count; i++)
        {
            pathVisualizer[i].color = IconStruct.IconDictionary[dungeonNode.nextPaths[i]].color;
            pathVisualizer[i].sprite = IconStruct.IconDictionary[dungeonNode.nextPaths[i]].sprite;

        }
        if(dungeonNode.chosenPathIndex != -1 && DungeonManager._instance.CheckCalcRun() && DungeonManager._instance.currentCalcRun.currentNode != dungeonNode)
        {
            pathVisualizer[dungeonNode.chosenPathIndex].sprite = IconStruct.IconDictionary[dungeonNode.nextPaths[dungeonNode.chosenPathIndex]].sprite;
            pathVisualizer[dungeonNode.chosenPathIndex].color = clearColor;
        }
    }

    [System.Serializable]
    public struct PathGradient
    {
        public string pathId;
        public Gradient gradient;
    }

    private void OnDisable()
    {
        if(pathVisualizer != null && pathVisualizer.Count != 0)
        { 
            foreach(var image in pathVisualizer)
            {
                if(image != null)
                    image.gameObject.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        if (pathVisualizer != null && pathVisualizer.Count != 0)
        {
            foreach (var image in pathVisualizer)
            {
                if (image != null)
                    image.gameObject.SetActive(true);
            }
        }
    }

    private void OnDestroy()
    {
        pathVisualizer.Clear();
    }
}
