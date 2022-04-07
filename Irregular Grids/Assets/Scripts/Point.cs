using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Point
{
    public static float COMPARISON_TOLERANCE = 0.1f;

    public Vector3 Position;
    public List<Point> Connections;
    public bool IsModifiable = true;

    public Point(float x, float y, float z, bool isModifiable = true)
    {
        Position = new Vector3(x, y, z);
        IsModifiable = isModifiable;
        Connections = new List<Point>();
    }

    public void AddConnection(Point other)
    {
        this.Connections.Add(other);
    }

    public void RemoveConnection(Point other)
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

    public void RemoveConnectionMutual(Point other)
    {
        RemoveConnection(other);
        other.RemoveConnection(this);
    }

    public virtual bool Equals(Point other)
    {
        if (other.Position.x == this.Position.x 
            && other.Position.y == this.Position.y
            && other.Position.z == this.Position.z)
        {
            return true;
        }

        return false;
    }

    public bool IsNearEnough(Point other)
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
