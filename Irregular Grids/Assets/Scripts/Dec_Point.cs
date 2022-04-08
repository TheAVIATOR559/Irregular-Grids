using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dec_Point : MonoBehaviour
{
    public static float COMPARISON_TOLERANCE = 0.1f;

    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
        set
        {
            if(IsModifiable)
            {
                transform.position = value;
            }
        }
    }

    public List<Dec_Point> Connections = new List<Dec_Point>();
    public bool IsModifiable = true;

    public void AddConnection(Dec_Point other)
    {
        if (!this.Connections.Contains(other))
        {
            this.Connections.Add(other);
        }
    }

    public void RemoveConnection(Dec_Point other)
    {
        if (this.Connections.Contains(other))
        {
            this.Connections.Remove(other);
        }
        else
        {
            Debug.LogError("invalid connection removal");
        }
    }

    public void RemoveConnectionMutual(Dec_Point other)
    {
        RemoveConnection(other);
        other.RemoveConnection(this);
    }

    public virtual bool Equals(Dec_Point other)
    {
        if (other.Position.x == this.Position.x
            && other.Position.y == this.Position.y
            && other.Position.z == this.Position.z)
        {
            return true;
        }

        return false;
    }

    public bool IsNearEnough(Dec_Point other)
    {

        if (Mathf.Abs(this.Position.x - other.Position.x) <= COMPARISON_TOLERANCE
            && Mathf.Abs(this.Position.y - other.Position.y) <= COMPARISON_TOLERANCE
            && Mathf.Abs(this.Position.z - other.Position.z) <= COMPARISON_TOLERANCE)
        {
            return true;
        }

        return false;
    }

    public bool IsNearEnough(float x, float y, float z)
    {
        //Debug.Log((Mathf.Abs(this.Position.x - x) <= COMPARISON_TOLERANCE) + "::" + (Mathf.Abs(this.Position.y - y) <= COMPARISON_TOLERANCE) + "::" + (Mathf.Abs(this.Position.z - z) <= COMPARISON_TOLERANCE));
        //Debug.Log(this.Position + "::(" + x + ", " + y + ", " + z + ")");

        if (Mathf.Abs(this.Position.x - x) <= COMPARISON_TOLERANCE
            && Mathf.Abs(this.Position.y - y) <= COMPARISON_TOLERANCE
            && Mathf.Abs(this.Position.z - z) <= COMPARISON_TOLERANCE)
        {
            return true;
        }

        return false;
    }

    public bool IsNearEnough(float x, float z)
    {
        //Debug.Log((Mathf.Abs(this.Position.x - x) <= COMPARISON_TOLERANCE) + "::" + (Mathf.Abs(this.Position.z - z) <= COMPARISON_TOLERANCE));
        if (Mathf.Abs(this.Position.x - x) <= COMPARISON_TOLERANCE
            && Mathf.Abs(this.Position.z - z) <= COMPARISON_TOLERANCE)
        {
            return true;
        }

        return false;
    }
}
