using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Grid_Builder : MonoBehaviour
{
    private List<Point> gridPoints = new List<Point>();
    public GameObject sphere;


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
        RemoveRandomConnections();

        currPoint = gridPoints[0];
        edges.Add(currPoint);
    }

    private void CreatePoints()
    {
        List<Point> mainPoints = new List<Point>();
        List<Point> secondaryPoints = new List<Point>();

        //main points
        for (int i = 1; i <= 6; i++)
        {
            mainPoints.Add(new Point(i * sin0, i * cos0));
            mainPoints.Add(new Point(i * sin60, i * cos60));
            mainPoints.Add(new Point(i * sin120, i * cos120));
            mainPoints.Add(new Point(i * sin180, i * cos180));
            mainPoints.Add(new Point(i * sin240, i * cos240));
            mainPoints.Add(new Point(i * sin300, i * cos300));

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
                        //Debug.Log(k + "::" + mainPoints[k-1]);
                        secondaryPoints.Add(new Point(mainPoints[k - 1].X + (j / i) * (mainPoints[k - 6].X - mainPoints[k - 1].X), mainPoints[k - 1].Y + (j / i) * (mainPoints[k - 6].Y - mainPoints[k - 1].Y)));
                    }
                    else
                    {
                        secondaryPoints.Add(new Point(mainPoints[k - 1].X + (j / i) * (mainPoints[k].X - mainPoints[k - 1].X), mainPoints[k - 1].Y + (j / i) * (mainPoints[k].Y - mainPoints[k - 1].Y)));
                    }
                }
            }
        }

        gridPoints.Add(new Point(0, 0));
        gridPoints.AddRange(mainPoints);
        gridPoints.AddRange(secondaryPoints);

        for (int i = 0; i < gridPoints.Count; i++)
        {
            GameObject newSphere = Instantiate(sphere, PointToVec3(gridPoints[i]), Quaternion.identity);
            newSphere.name = i.ToString();
        }
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

    private void RemoveRandomConnections()
    {
        List<Point> connsToRemove = new List<Point>();
        foreach (Point point in gridPoints)
        {
            if(point.Connections.Count <= 4)
            {
                continue;
            }

            foreach (Point connection in point.Connections)
            {
                if (point.Connections.Count - connsToRemove.Count > 4 && connection.Connections.Count > 4)
                {
                    connsToRemove.Add(connection);
                }
            }

            Debug.Log(connsToRemove.Count);

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

    private void CreateShapes()
    {
        /*
         * foreach point
         *      foreach connection in point Connections
         *          check if 
         * 
         */
    }

    private void SubdivideShapes()
    {

    }

    Point currPoint;
    List<Point> edges = new List<Point>();

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            currPoint.Connections = currPoint.Connections.OrderBy(p => Mathf.Atan2(edges[edges.Count - 1].Y - p.Y, edges[edges.Count - 1].X - p.X)).ToList<Point>();

            if (!edges.Contains(currPoint.Connections[0]))
            {
                edges.Add(currPoint.Connections[0]);
                currPoint = currPoint.Connections[0];
            }
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

        Gizmos.color = Color.red;
        for(int i = 1; i < edges.Count; i++)
        {
            Gizmos.DrawLine(PointToVec3(edges[i-1]), PointToVec3(edges[i]));
        }
    }

    private Vector3 PointToVec3(Point point)
    {
        return new Vector3(point.X, 0, point.Y);
    }
}
