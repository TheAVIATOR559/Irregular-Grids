using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Grid_Builder : MonoBehaviour
{
    private List<Point> gridPoints = new List<Point>();

    private float sin0;
    private float sin60;
    private float sin120;
    private float sin180;
    private float sin240;
    private float sin300;

    private float cos0;
    private float cos60;
    private float cos120;
    private float cos180;
    private float cos240;
    private float cos300;

    // Start is called before the first frame update
    void Start()
    {
        sin0 = Mathf.Sin(0);
        sin60 = Mathf.Sin(1.0472f);
        sin120 = Mathf.Sin(2.0994f);
        sin180 = Mathf.Sin(3.14159f);
        sin240 = Mathf.Sin(4.18879f);
        sin300 = Mathf.Sin(5.23599f);

        cos0 = Mathf.Cos(0);
        cos60 = Mathf.Cos(1.0472f);
        cos120 = Mathf.Cos(2.0994f);
        cos180 = Mathf.Cos(3.14159f);
        cos240 = Mathf.Cos(4.18879f);
        cos300 = Mathf.Cos(5.23599f);

        CreatePoints();
        CreateConnections();
        CreateSubPoints();
        RemoveRandomConnections();
    }

    private void CreatePoints()
    {
        List<Point> mainPoints = new List<Point>();
        List<Point> secondaryPoints = new List<Point>();

        //main points
        for (int i = 1; i <= 6; i++)
        {
            if(i == 6)
            {
                mainPoints.Add(new Point(i * sin0, i * cos0, false));
                mainPoints.Add(new Point(i * sin60, i * cos60, false));
                mainPoints.Add(new Point(i * sin120, i * cos120, false));
                mainPoints.Add(new Point(i * sin180, i * cos180, false));
                mainPoints.Add(new Point(i * sin240, i * cos240, false));
                mainPoints.Add(new Point(i * sin300, i * cos300, false));
            }
            else
            {
                mainPoints.Add(new Point(i * sin0, i * cos0));
                mainPoints.Add(new Point(i * sin60, i * cos60));
                mainPoints.Add(new Point(i * sin120, i * cos120));
                mainPoints.Add(new Point(i * sin180, i * cos180));
                mainPoints.Add(new Point(i * sin240, i * cos240));
                mainPoints.Add(new Point(i * sin300, i * cos300));
            }

            if (i < 2)
            {
                continue;
            }

            for (int k = (i * 6) - 5; k <= i * 6; k++)
            {
                for (float j = 1; j < i; j++)
                {
                    //(x1 + k(x2 - x1), y1 + k(y2 - y1))
                    //Debug.Log(k + "::" + j + "/" + i + "=" + (j / i));
                    if (k == (i * 6))
                    {
                        if(i == 6)
                        {
                            //Debug.Log(k + "::" + mainPoints[k-1]);
                            secondaryPoints.Add(new Point(mainPoints[k - 1].X + (j / i) * (mainPoints[k - 6].X - mainPoints[k - 1].X), mainPoints[k - 1].Y + (j / i) * (mainPoints[k - 6].Y - mainPoints[k - 1].Y), false));
                        }
                        else
                        {
                            //Debug.Log(k + "::" + mainPoints[k-1]);
                            secondaryPoints.Add(new Point(mainPoints[k - 1].X + (j / i) * (mainPoints[k - 6].X - mainPoints[k - 1].X), mainPoints[k - 1].Y + (j / i) * (mainPoints[k - 6].Y - mainPoints[k - 1].Y)));
                        }
                    }
                    else
                    {
                        if(i == 6)
                        {
                            secondaryPoints.Add(new Point(mainPoints[k - 1].X + (j / i) * (mainPoints[k].X - mainPoints[k - 1].X), mainPoints[k - 1].Y + (j / i) * (mainPoints[k].Y - mainPoints[k - 1].Y), false));
                        }
                        else
                        {
                            secondaryPoints.Add(new Point(mainPoints[k - 1].X + (j / i) * (mainPoints[k].X - mainPoints[k - 1].X), mainPoints[k - 1].Y + (j / i) * (mainPoints[k].Y - mainPoints[k - 1].Y)));
                        }
                    }
                }
            }
        }

        gridPoints.Add(new Point(0, 0, false));
        gridPoints.AddRange(mainPoints);
        gridPoints.AddRange(secondaryPoints);
    }

    private void CreateConnections()
    {
        //int connCount = 0;

        foreach (Point point in gridPoints)
        {
            for (int i = 0; i < gridPoints.Count; i++)
            {
                if (gridPoints[i] == point)
                {
                    continue;
                }
                else if (gridPoints[i].IsNearEnough(point.X + sin0, point.Y + cos0))
                {
                    point.AddConnection(gridPoints[i]);
                }
                else if (gridPoints[i].IsNearEnough(point.X + sin60, point.Y + cos60))
                {
                    point.AddConnection(gridPoints[i]);
                }
                else if (gridPoints[i].IsNearEnough(point.X + sin120, point.Y + cos120))
                {
                    point.AddConnection(gridPoints[i]);
                }
                else if (gridPoints[i].IsNearEnough(point.X + sin180, point.Y + cos180))
                {
                    point.AddConnection(gridPoints[i]);
                }
                else if (gridPoints[i].IsNearEnough(point.X + sin240, point.Y + cos240))
                {
                    point.AddConnection(gridPoints[i]);
                }
                else if (gridPoints[i].IsNearEnough(point.X + sin300, point.Y + cos300))
                {
                    point.AddConnection(gridPoints[i]);
                }
            }

            //connCount += point.Connections.Count;
            //Debug.Log(point.Connections.Count + " :: " + point.X + "," + point.Y);
        }

        //Debug.Log(connCount);

        //Debug.Log(gridPoints[25].X + "," + gridPoints[25].Y);
        //Debug.Log(gridPoints[31].X + "," + gridPoints[31].Y + " :: " + (gridPoints[25].X + sin0) + "," + (gridPoints[25].Y + cos0) + " :: Is Near Enough ?? " + gridPoints[31].IsNearEnough((gridPoints[25].X + sin0), (gridPoints[25].Y + cos0)));
        //Debug.Log(gridPoints[97].X + "," + gridPoints[97].Y + " :: " + (gridPoints[25].X + sin60) + "," + (gridPoints[25].Y + cos60) + " :: Is Near Enough ?? " + gridPoints[97].IsNearEnough((gridPoints[25].X + sin60), (gridPoints[25].Y + cos60)));
        //Debug.Log(gridPoints[73].X + "," + gridPoints[73].Y + " :: " + (gridPoints[25].X + sin120) + "," + (gridPoints[25].Y + cos120) + " :: Is Near Enough ?? " + gridPoints[73].IsNearEnough((gridPoints[25].X + sin120), (gridPoints[25].Y + cos120)));
        //Debug.Log(gridPoints[19].X + "," + gridPoints[19].Y + " :: " + (gridPoints[25].X + sin180) + "," + (gridPoints[25].Y + cos180) + " :: Is Near Enough ?? " + gridPoints[19].IsNearEnough((gridPoints[25].X + sin180), (gridPoints[25].Y + cos180)));
        //Debug.Log(gridPoints[96].X + "," + gridPoints[96].Y + " :: " + (gridPoints[25].X + sin240) + "," + (gridPoints[25].Y + cos240) + " :: Is Near Enough ?? " + gridPoints[96].IsNearEnough((gridPoints[25].X + sin240), (gridPoints[25].Y + cos240)));
        //Debug.Log(gridPoints[126].X + "," + gridPoints[126].Y + " :: " + (gridPoints[25].X + sin300) + "," + (gridPoints[25].Y + cos300) + " :: Is Near Enough ?? " + gridPoints[126].IsNearEnough((gridPoints[25].X + sin300), (gridPoints[25].Y + cos300)));
        //Debug.Log(gridPoints[0].X + "," + gridPoints[0].Y + " :: " + gridPoints[103].X + "," + gridPoints[103].Y + " :: " + gridPoints[0].IsNearEnough(gridPoints[103]));
    }

    private void CreateSubPoints()
    {
        /* foreach mainPoint in gridPoints
         *      foreach neighbor in point.Connections
         *          find a point to the local left of neighbor
         *                  if local left point has connection back to main point
         *                      create a new point in the middle of the three points(mainPoint, neighbor, local left point)
         */

        List<Point> subPoints = new List<Point>();

        foreach (Point start in gridPoints)
        {
            foreach(Point neighbor in start.Connections)
            {
                foreach(Point localLeft in neighbor.Connections)
                {
                    if(localLeft == start)
                    {
                        continue;
                    }

                    if(localLeft.Connections.Contains(start) && IsLeft(start, neighbor, localLeft) && Random.Range(0f, 1f) <= 0.25f)//TODO add random fail chance to this???
                    {
                        Point newPoint = new Point((start.X + neighbor.X + localLeft.X) / 3, (start.Y + neighbor.Y + localLeft.Y) / 3);
                        subPoints.Add(newPoint);
                        newPoint.AddConnection(start);
                        newPoint.AddConnection(neighbor);
                        newPoint.AddConnection(localLeft);
                    }
                }
            }
            //break;
        }

        //Debug.Log(test.Count);
        for (int i = 0; i < subPoints.Count; i++)
        {
            for(int j = i + 1; j < subPoints.Count; j++)
            {
                //Debug.Log(i + "::" + j + "::" + test.Count);
                if(subPoints[i].IsNearEnough(subPoints[j]))
                {
                    subPoints.RemoveAt(j);
                    j--;
                }
            }
        }
        //Debug.Log(test.Count);

        foreach(Point point in subPoints)
        {
            foreach(Point connection in point.Connections)
            {
                connection.AddConnection(point);
            }
        }

        gridPoints.AddRange(subPoints);
    }

    private void RemoveRandomConnections()
    {
        ShuffleGridPoints();

        List<Point> connsToRemove = new List<Point>();
        foreach (Point point in gridPoints)
        {
            if(point.Connections.Count <= 4)
            {
                continue;
            }

            foreach (Point connection in point.Connections)
            {
                if(!point.IsModifiable && !connection.IsModifiable)
                {
                    continue;
                }

                if (point.Connections.Count - connsToRemove.Count > 4 && connection.Connections.Count > 4)
                {
                    connsToRemove.Add(connection);
                }
            }

            //Debug.Log(connsToRemove.Count);

            foreach (Point removed in connsToRemove)
            {
                point.RemoveConnectionMutual(removed);
            }
            connsToRemove.Clear();

            if (point.Connections.Count <= 3)
            {
                Debug.LogError(point.Connections.Count);
            }
        }
    }

    private void ShuffleGridPoints()
    {
        for (int i = 0; i < gridPoints.Count; i++)
        {
            Point temp = gridPoints[i];
            int randomIndex = Random.Range(i, gridPoints.Count);

            gridPoints[i] = gridPoints[randomIndex];
            gridPoints[randomIndex] = temp;
        }
    }

    private void RebalanceGrid() //LAPLACIAN SMOOTHING
    {
        //TODO add min distance btwn points
        foreach (Point point in gridPoints)
        {
            if(point.IsModifiable)
            {
                Vector2 newPos = new Vector2(point.X, point.Y);

                foreach (Point neighbor in point.Connections)
                {
                    newPos.x += neighbor.X;
                    newPos.y += neighbor.Y;
                }

                newPos = newPos / (point.Connections.Count + 1);
                point.X = newPos.x;
                point.Y = newPos.y;
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RebalanceGrid();
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (Point point in gridPoints)
        {
            foreach (Point connection in point.Connections)
            {
                Gizmos.DrawLine(PointToVec3(point), PointToVec3(connection));
            }
        }
        
        foreach (Point point in gridPoints)
        {
            if (point.IsModifiable)
            {
                Gizmos.color = Color.blue;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawSphere(PointToVec3(point), 0.1f);
        }
    }

    private Vector3 PointToVec3(Point point)
    {
        return new Vector3(point.X, 0, point.Y);
    }

    private bool IsLeft(Point a, Point b, Point c)
    {
        return ((b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X)) > 0;
    }
}
