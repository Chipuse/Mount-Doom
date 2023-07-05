using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterAnalysis : MonoBehaviour
{
    
    public static ClusterAnalysis _instance;
    // Start is called before the first frame update
    void Awake()
    {
        Application.runInBackground = true;
        if (_instance == null)
        {
            _instance = this;
        }
        else
            Destroy(this);
    }

    void Start()
    {
        if(ServerCommunicationManager._instance != null)
        {
            ServerCommunicationManager._instance.DoServerRequest(Request.getPlotData, Plot);
        }
    }

    void Plot()
    {
        Debug.Log("plotDataArrived");
        foreach (var item in plotData.entries)
        {
            GameObject go = Instantiate(dataObjectPrefab, new Vector3(item.social, item.achievement, item.attachment), transform.rotation, transform);
            ClusterItem tempItem = go.GetComponent<ClusterItem>();
            tempItem.pair = item;
            items.Add(tempItem);
        }
        StartCoroutine(RePlot(3, 2));
    }

    IEnumerator RePlot(int numCentroids = 3, float steps = 100)
    {
        foreach (var item in centroidObj)
        {
            Destroy(item.gameObject);
        }
        centroidObj.Clear();
        {
            int counter = 0;
            //Perform the k-means
            centroids = new List<Centroid>();
            currentOptimum = new Config();
            Config currentTest = new Config();
            //Try config foreach potential combination of centroids:
            currentTest.centroids = new List<Centroid>();
            for (int i = 0; i < numCentroids; i++)
            {
                currentTest.centroids.Add(new Centroid { id = i, pos = Vector3.zero });
                if (i == 0)
                    currentTest.centroids[i].col = Color.red;
                if (i == 1)
                    currentTest.centroids[i].col = Color.blue;
                if (i == 2)
                    currentTest.centroids[i].col = Color.green;
                currentOptimum.centroids.Add(new Centroid { id = i, pos = Vector3.zero });
                if (i == 0)
                    currentOptimum.centroids[i].col = Color.red;
                if (i == 1)
                    currentOptimum.centroids[i].col = Color.blue;
                if (i == 2)
                    currentOptimum.centroids[i].col = Color.green;
                    

                centroidObj.Add(Instantiate(dataObjectPrefab).GetComponent<ClusterItem>());
                if (i == 0)
                {
                    centroidObj[i].col = Color.magenta;
                    centroidObj[i].ApplyColor();
                }
                if (i == 1)
                {
                    centroidObj[i].col = Color.cyan;
                    centroidObj[i].ApplyColor();
                }
                if (i == 2)
                {
                    centroidObj[i].col = Color.yellow;
                    centroidObj[i].ApplyColor();
                }

            }
            //Start is ready. all centroids at 000
            for (int oneX = 0; oneX <= steps; oneX++)
            {
                for (int oneY = 0; oneY <= steps; oneY++)
                {
                    for (int oneZ = 0; oneZ <= steps; oneZ++)
                    {
                        currentTest.centroids[0].pos = new Vector3((1 / steps) * oneX, (1 / steps) * oneY, (1 / steps) * oneZ);
                        if (numCentroids > 1)
                        {                           
                            for (int twoX = 0; twoX <= steps; twoX++)
                            {
                                for (int twoY = 0; twoY <= steps; twoY++)
                                {
                                    for (int twoZ = 0; twoZ <= steps; twoZ++)
                                    {                                        
                                        currentTest.centroids[1].pos = new Vector3((1 / steps) * twoX, (1 / steps) * twoY, (1 / steps) * twoZ);
                                        if (numCentroids > 2)
                                        {
                                            for (int threeX = 0; threeX <= steps; threeX++)
                                            {
                                                for (int threeY = 0; threeY <= steps; threeY++)
                                                {
                                                    for (int threeZ = 0; threeZ <= steps; threeZ++)
                                                    {                                                        
                                                        currentTest.centroids[2].pos = new Vector3((1 / steps) * threeX, (1 / steps) * threeY, (1 / steps) * threeZ);                                                        
                                                        //perform with three centroids                                                        
                                                        if (currentTest.centroids[0].pos != currentTest.centroids[1].pos && currentTest.centroids[0].pos != currentTest.centroids[2].pos && currentTest.centroids[1].pos != currentTest.centroids[2].pos)
                                                        {
                                                            //Skip any config where one or more centroid overlap
                                                            currentTest.CalcDistanceTotalNoChange(items);
                                                            

                                                            if (currentTest.distanceTotal < currentOptimum.distanceTotal || currentOptimum.distanceTotal <= 0)
                                                            {
                                                                //currentOptimum.distanceTotal = currentTest.distanceTotal;
                                                                //update currentOptimum
                                                                for (int i = 0; i < currentOptimum.centroids.Count; i++)
                                                                {
                                                                    currentOptimum.centroids[i].pos = currentTest.centroids[i].pos;
                                                                    if (i == 0)
                                                                    {
                                                                        centroidObj[i].transform.position = currentTest.centroids[i].pos;
                                                                    }
                                                                    if (i == 1)
                                                                    {
                                                                        centroidObj[i].transform.position = currentTest.centroids[i].pos;
                                                                    }
                                                                    if (i == 2)
                                                                    {
                                                                        centroidObj[i].transform.position = currentTest.centroids[i].pos;
                                                                    }
                                                                }
                                                                currentOptimum.CalcDistanceTotal(items);                                                                
                                                            }                                                            
                                                        }                                                        
                                                    }
                                                    
                                                }
                                                
                                            }
                                        }
                                        else
                                        {
                                            //Just perform with two centroid
                                        }
                                        
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Just perform with one centroid
                        }
                        yield return new WaitForEndOfFrame();
                        counter++;
                        Debug.Log(counter);
                    }
                }
            }
            currentOptimum.CalcDistanceTotal(items);
            yield return new WaitForEndOfFrame();
            Debug.Log("finished");
        }
    }

    
    public List<Centroid> centroids;
    public Config currentOptimum;
    public List<ClusterItem> items;
    public GameObject dataObjectPrefab;
    public PlotData plotData;
    List<ClusterItem> centroidObj = new List<ClusterItem>();

    void OnDrawGizmosSelected()
    {
        // Draws a blue line from this transform to the target
        Gizmos.color = Color.black;

        Gizmos.DrawLine(Vector3.zero, new Vector3(0,0,1));
        Gizmos.DrawLine(Vector3.zero, new Vector3(0,1,0));
        Gizmos.DrawLine(Vector3.zero, new Vector3(1,0,0));
    }
}

[System.Serializable]
public class Config
{
    public List<Centroid> centroids = new List<Centroid>();
    public float distanceTotal;
    public void CalcDistanceTotal(List<ClusterItem> _clusterItems)
    {
        distanceTotal = 0;
        float tempDist = 1000;
        float temptempDist = 0;
        foreach (var item in _clusterItems)
        {
            tempDist = 1000;
            //assign _clusteritem to centroid:
            foreach (var centroid in centroids)
            {
                temptempDist = (centroid.pos - item.transform.position).sqrMagnitude;
                if (tempDist > temptempDist)
                {
                    tempDist = temptempDist;
                    item.id = centroid.id;
                    item.col = centroid.col;
                    item.ApplyColor();
                }
                distanceTotal += tempDist;
            }
        }
    }
    public void CalcDistanceTotalNoChange(List<ClusterItem> _clusterItems)
    {
        distanceTotal = 0;
        float tempDist = 1000;
        float temptempDist = 0;
        foreach (var item in _clusterItems)
        {
            tempDist = 1000;
            //assign _clusteritem to centroid:
            foreach (var centroid in centroids)
            {
                temptempDist = (centroid.pos - item.transform.position).sqrMagnitude;
                if (tempDist > temptempDist)
                {
                    tempDist = temptempDist;
                }
                distanceTotal += tempDist;
            }
        }
    }
}

public class Centroid
{
    public int id;
    public Vector3 pos;
    public Color col;
}

[System.Serializable]
public class PlotData
{
    public List<ValuePair> entries;
}

[System.Serializable]
public class ValuePair
{
    public string name;
    public float neuroticism;
    public float extraversion;
    public float openness;
    public float agreeableness;
    public float conscientiousness;
    public float social;
    public float achievement;
    public float attachment;
}
