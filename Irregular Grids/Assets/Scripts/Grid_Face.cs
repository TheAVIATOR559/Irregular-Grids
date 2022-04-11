using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Face : MonoBehaviour
{
    public List<Dec_Point> points = new List<Dec_Point>();

    private Vector3 origPosition;
    [SerializeField] private Vector3 rotation2D;
    [SerializeField] private Vector3 rotation3D;
    [SerializeField] private Vector3 position3D;

    private void Start()
    {
        origPosition = transform.position;
        rotation2D = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.identity;
    }

    public void CreatePoint(Vector3 pos, bool isModifiable = true, List<Dec_Point> conns = null)
    {
        GameObject newObj = new GameObject("Point " + (pos + this.transform.position));
        newObj.transform.SetParent(this.transform);
        Dec_Point newPoint = newObj.AddComponent<Dec_Point>();
        newPoint.Position = pos + this.transform.position;
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

    public void CreateSubPoint(Vector3 pos, bool isModifiable = true, List<Dec_Point> conns = null)
    {
        GameObject newObj = new GameObject("SubPoint " + pos);
        newObj.transform.SetParent(this.transform);
        Dec_Point newPoint = newObj.AddComponent<Dec_Point>();
        newPoint.Position = pos;
        newPoint.IsModifiable = isModifiable;

        if (conns != null)
        {
            foreach (Dec_Point point in conns)
            {
                newPoint.AddConnection(point);
            }
        }

        points.Add(newPoint);
    }

    public void ApplyFlatRotation()
    {
        transform.rotation = Quaternion.Euler(rotation2D);
    }

    public void Apply3DRotation()
    {
        transform.rotation = Quaternion.Euler(rotation3D);
        transform.position = position3D;
    }

    public void Reset()
    {
        Clear();
        transform.position = origPosition;
        transform.rotation = Quaternion.identity;
    }

    private void Clear()
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
