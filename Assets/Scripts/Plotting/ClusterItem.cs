using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterItem : MonoBehaviour
{
    public int id = -1;
    public Color col;
    public ValuePair pair;
    MeshRenderer rend;
    private void Start()
    {
        if (rend == null)
            rend = GetComponent<MeshRenderer>();
        rend.material.SetColor("_Color", col);
    }
    public void ApplyColor()
    {
        if (rend == null)
            rend = GetComponent<MeshRenderer>();
        rend.material.SetColor("_Color", col);
    }

    void OnDrawGizmosSelected()
    {
        // Draws a blue line from this transform to the target
        Gizmos.color = col;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x,0,transform.position.z));
    }
}
