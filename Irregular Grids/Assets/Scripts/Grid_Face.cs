using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Face : MonoBehaviour
{
    public List<Dec_Point> points = new List<Dec_Point>();

    public void CreatePoint(Vector3 pos, bool isModifiable = true, List<Dec_Point> conns = null)
    {
        GameObject newObj = new GameObject("Point " + pos);
        newObj.transform.SetParent(this.transform);
        Dec_Point newPoint = newObj.AddComponent<Dec_Point>();
        newPoint.Position = pos;
        newPoint.IsModifiable = isModifiable;
        
        if(conns != null)
        {
            foreach (Dec_Point point in conns)
            {
                newPoint.AddConnection(point);
            }
        }

        points.Add(newPoint);
    }

    public void Clear()
    {
        points.Clear();
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (Dec_Point point in points)
        {
            foreach (Dec_Point connection in point.Connections)
            {
                Gizmos.DrawLine(point.Position, connection.Position);
            }
        }

        foreach (Dec_Point point in points)
        {
            if (point.IsModifiable)
            {
                Gizmos.color = Color.blue;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawSphere(point.Position, 0.1f);
        }
    }
}
