using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Point
{
    public static float COMPARISON_TOLERANCE = 0.01f;

    //public float X
    //{
    //    get;
    //    private set;
    //}

    //public float Y
    //{
    //    get;
    //    private set;
    //}

    //public List<Point> Connections
    //{
    //    get;
    //    private set;
    //}
    public Vector2 Position;
    public List<Point> Connections;
    public bool IsModifiable = true;

    public Point(float x, float y, bool isModifiable = true)
    {
        Position = new Vector2(x, y);
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
        if (other.Position.x == this.Position.x && other.Position.y == this.Position.y)
        {
            return true;
        }

        return false;
    }

    public bool IsNearEnough(Point other)
    {
        float xDiff = Mathf.Abs(this.Position.x * COMPARISON_TOLERANCE);
        float yDiff = Mathf.Abs(this.Position.y * COMPARISON_TOLERANCE);

        if (Mathf.Abs(this.Position.x - other.Position.x) <= COMPARISON_TOLERANCE
            && Mathf.Abs(this.Position.y - other.Position.y) <= COMPARISON_TOLERANCE)
        {
            return true;
        }

        return false;
    }

    public bool IsNearEnough(float x, float y)//does not acount for 0 and almost 0
    {
        float xDiff = Mathf.Abs(this.Position.x * COMPARISON_TOLERANCE);
        float yDiff = Mathf.Abs(this.Position.y * COMPARISON_TOLERANCE);

        if (Mathf.Abs(this.Position.x - x) <= COMPARISON_TOLERANCE
            && Mathf.Abs(this.Position.y - y) <= COMPARISON_TOLERANCE)
        {
            //Debug.Log("this.X : " + this.X + " :: other.x : " + x + " :: this.X - other.x : " + Mathf.Abs(this.X - x) + " :: xDiff : " + xDiff);
            //Debug.Log("this.Y : " + this.Y + " :: other.y : " + y + " :: this.Y - other.y : " + Mathf.Abs(this.Y - y) + " :: yDiff : " + yDiff);
            return true;
        }

        return false;
    }
}
