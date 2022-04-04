using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Grid_Builder : MonoBehaviour
{
    private enum GenerationType
    {
        FLAT,
        CONE,
        RANDOM,
        PERLIN
    };

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

    private float arcSin45;

    private List<Point> subPoints = new List<Point>();

    [SerializeField] GenerationType Type;

    [SerializeField] float subPointChance = 0.2f;
    [SerializeField] float subPointRigidityChance = 0.1f;
    [SerializeField] int minConnectionCount = 4;
    [SerializeField] float minPointDistance = 0.35f;

    [SerializeField] private int clickCount = 0;

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

        arcSin45 = Mathf.Asin(0.785398f);
    }

    private void CreatePoints()
    {
        List<Point> mainPoints = new List<Point>();
        List<Point> secondaryPoints = new List<Point>();

        //main points
        for (int i = 1; i <= 6; i++)
        {
            if (i == 6)
            {
                mainPoints.Add(new Point(i * sin0, 0, i * cos0, false));
                mainPoints.Add(new Point(i * sin60, 0, i * cos60, false));
                mainPoints.Add(new Point(i * sin120, 0, i * cos120, false));
                mainPoints.Add(new Point(i * sin180, 0, i * cos180, false));
                mainPoints.Add(new Point(i * sin240, 0, i * cos240, false));
                mainPoints.Add(new Point(i * sin300, 0, i * cos300, false));
            }
            else
            {
                mainPoints.Add(new Point(i * sin0, 0, i * cos0));
                mainPoints.Add(new Point(i * sin60, 0, i * cos60));
                mainPoints.Add(new Point(i * sin120, 0, i * cos120));
                mainPoints.Add(new Point(i * sin180, 0, i * cos180));
                mainPoints.Add(new Point(i * sin240, 0, i * cos240));
                mainPoints.Add(new Point(i * sin300, 0, i * cos300));
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
                        if (i == 6)
                        {
                            //Debug.Log(k + "::" + mainPoints[k-1]);
                            secondaryPoints.Add(new Point(mainPoints[k - 1].Position.x + (j / i) * (mainPoints[k - 6].Position.x - mainPoints[k - 1].Position.x), 0, mainPoints[k - 1].Position.z + (j / i) * (mainPoints[k - 6].Position.z - mainPoints[k - 1].Position.z), false));
                        }
                        else
                        {
                            //Debug.Log(k + "::" + mainPoints[k-1]);
                            secondaryPoints.Add(new Point(mainPoints[k - 1].Position.x + (j / i) * (mainPoints[k - 6].Position.x - mainPoints[k - 1].Position.x), 0, mainPoints[k - 1].Position.z + (j / i) * (mainPoints[k - 6].Position.z - mainPoints[k - 1].Position.z)));
                        }
                    }
                    else
                    {
                        if (i == 6)
                        {
                            secondaryPoints.Add(new Point(mainPoints[k - 1].Position.x + (j / i) * (mainPoints[k].Position.x - mainPoints[k - 1].Position.x), 0, mainPoints[k - 1].Position.z + (j / i) * (mainPoints[k].Position.z - mainPoints[k - 1].Position.z), false));
                        }
                        else
                        {
                            secondaryPoints.Add(new Point(mainPoints[k - 1].Position.x + (j / i) * (mainPoints[k].Position.x - mainPoints[k - 1].Position.x), 0, mainPoints[k - 1].Position.z + (j / i) * (mainPoints[k].Position.z - mainPoints[k - 1].Position.z)));
                        }
                    }
                }
            }
        }

        gridPoints.Add(new Point(0, 0, 0, false));
        gridPoints.AddRange(mainPoints);
        gridPoints.AddRange(secondaryPoints);
    }

    private void CreatePointsCone()
    {
        List<Point> mainPoints = new List<Point>();
        List<Point> secondaryPoints = new List<Point>();

        //main points
        for (int i = 1; i <= 6; i++)
        {
            if (i == 6)
            {
                mainPoints.Add(new Point(i * sin0, i, i * cos0, false));
                mainPoints.Add(new Point(i * sin60, i, i * cos60, false));
                mainPoints.Add(new Point(i * sin120, i, i * cos120, false));
                mainPoints.Add(new Point(i * sin180, i, i * cos180, false));
                mainPoints.Add(new Point(i * sin240, i, i * cos240, false));
                mainPoints.Add(new Point(i * sin300, i, i * cos300, false));
            }
            else
            {
                mainPoints.Add(new Point(i * sin0, i, i * cos0));
                mainPoints.Add(new Point(i * sin60, i, i * cos60));
                mainPoints.Add(new Point(i * sin120, i, i * cos120));
                mainPoints.Add(new Point(i * sin180, i, i * cos180));
                mainPoints.Add(new Point(i * sin240, i, i * cos240));
                mainPoints.Add(new Point(i * sin300, i, i * cos300));
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
                        if (i == 6)
                        {
                            //Debug.Log(k + "::" + mainPoints[k-1]);
                            secondaryPoints.Add(new Point(mainPoints[k - 1].Position.x + (j / i) * (mainPoints[k - 6].Position.x - mainPoints[k - 1].Position.x), i, mainPoints[k - 1].Position.z + (j / i) * (mainPoints[k - 6].Position.z - mainPoints[k - 1].Position.z), false));
                        }
                        else
                        {
                            //Debug.Log(k + "::" + mainPoints[k-1]);
                            secondaryPoints.Add(new Point(mainPoints[k - 1].Position.x + (j / i) * (mainPoints[k - 6].Position.x - mainPoints[k - 1].Position.x), i, mainPoints[k - 1].Position.z + (j / i) * (mainPoints[k - 6].Position.z - mainPoints[k - 1].Position.z)));
                        }
                    }
                    else
                    {
                        if (i == 6)
                        {
                            secondaryPoints.Add(new Point(mainPoints[k - 1].Position.x + (j / i) * (mainPoints[k].Position.x - mainPoints[k - 1].Position.x), i, mainPoints[k - 1].Position.z + (j / i) * (mainPoints[k].Position.z - mainPoints[k - 1].Position.z), false));
                        }
                        else
                        {
                            secondaryPoints.Add(new Point(mainPoints[k - 1].Position.x + (j / i) * (mainPoints[k].Position.x - mainPoints[k - 1].Position.x), i, mainPoints[k - 1].Position.z + (j / i) * (mainPoints[k].Position.z - mainPoints[k - 1].Position.z)));
                        }
                    }
                }
            }
        }

        gridPoints.Add(new Point(0, 0, 0, false));
        gridPoints.AddRange(mainPoints);
        gridPoints.AddRange(secondaryPoints);
    }

    private void CreatePointsRandom()//TODO pick up here
    {
        List<Point> mainPoints = new List<Point>();
        List<Point> secondaryPoints = new List<Point>();

        //main points
        for (int i = 1; i <= 6; i++)
        {
            if (i == 6)
            {
                mainPoints.Add(new Point(i * sin0, Random.Range(-i, i + 0f), i * cos0, false));
                mainPoints.Add(new Point(i * sin60, Random.Range(-i, i + 0f), i * cos60, false));
                mainPoints.Add(new Point(i * sin120, Random.Range(-i, i + 0f), i * cos120, false));
                mainPoints.Add(new Point(i * sin180, Random.Range(-i, i + 0f), i * cos180, false));
                mainPoints.Add(new Point(i * sin240, Random.Range(-i, i + 0f), i * cos240, false));
                mainPoints.Add(new Point(i * sin300, Random.Range(-i, i + 0f), i * cos300, false));
            }
            else
            {
                mainPoints.Add(new Point(i * sin0, Random.Range(-i, i + 0f), i * cos0));
                mainPoints.Add(new Point(i * sin60, Random.Range(-i, i + 0f), i * cos60));
                mainPoints.Add(new Point(i * sin120, Random.Range(-i, i + 0f), i * cos120));
                mainPoints.Add(new Point(i * sin180, Random.Range(-i, i + 0f), i * cos180));
                mainPoints.Add(new Point(i * sin240, Random.Range(-i, i + 0f), i * cos240));
                mainPoints.Add(new Point(i * sin300, Random.Range(-i, i + 0f), i * cos300));
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
                        if (i == 6)
                        {
                            //Debug.Log(k + "::" + mainPoints[k-1]);
                            secondaryPoints.Add(new Point(mainPoints[k - 1].Position.x + (j / i) * (mainPoints[k - 6].Position.x - mainPoints[k - 1].Position.x), Random.Range(-i, i + 0f), mainPoints[k - 1].Position.z + (j / i) * (mainPoints[k - 6].Position.z - mainPoints[k - 1].Position.z), false));
                        }
                        else
                        {
                            //Debug.Log(k + "::" + mainPoints[k-1]);
                            secondaryPoints.Add(new Point(mainPoints[k - 1].Position.x + (j / i) * (mainPoints[k - 6].Position.x - mainPoints[k - 1].Position.x), Random.Range(-i, i + 0f), mainPoints[k - 1].Position.z + (j / i) * (mainPoints[k - 6].Position.z - mainPoints[k - 1].Position.z)));
                        }
                    }
                    else
                    {
                        if (i == 6)
                        {
                            secondaryPoints.Add(new Point(mainPoints[k - 1].Position.x + (j / i) * (mainPoints[k].Position.x - mainPoints[k - 1].Position.x), Random.Range(-i, i + 0f), mainPoints[k - 1].Position.z + (j / i) * (mainPoints[k].Position.z - mainPoints[k - 1].Position.z), false));
                        }
                        else
                        {
                            secondaryPoints.Add(new Point(mainPoints[k - 1].Position.x + (j / i) * (mainPoints[k].Position.x - mainPoints[k - 1].Position.x), Random.Range(-i, i + 0f), mainPoints[k - 1].Position.z + (j / i) * (mainPoints[k].Position.z - mainPoints[k - 1].Position.z)));
                        }
                    }
                }
            }
        }

        gridPoints.Add(new Point(0, 0, 0, false));
        gridPoints.AddRange(mainPoints);
        gridPoints.AddRange(secondaryPoints);
    }

    private void CreatePointsPerlin()
    {

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
                else if (gridPoints[i].IsNearEnough(point.Position.x + sin0, 0, point.Position.z + cos0))
                {
                    point.AddConnection(gridPoints[i]);
                }
                else if (gridPoints[i].IsNearEnough(point.Position.x + sin60, 0, point.Position.z + cos60))
                {
                    point.AddConnection(gridPoints[i]);
                }
                else if (gridPoints[i].IsNearEnough(point.Position.x + sin120, 0, point.Position.z + cos120))
                {
                    point.AddConnection(gridPoints[i]);
                }
                else if (gridPoints[i].IsNearEnough(point.Position.x + sin180, 0, point.Position.z + cos180))
                {
                    point.AddConnection(gridPoints[i]);
                }
                else if (gridPoints[i].IsNearEnough(point.Position.x + sin240, 0, point.Position.z + cos240))
                {
                    point.AddConnection(gridPoints[i]);
                }
                else if (gridPoints[i].IsNearEnough(point.Position.x + sin300, 0, point.Position.z + cos300))
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

    private void CreateConnections3D()
    {
        //int connCount = 0;

        Debug.Log(gridPoints.Count);

        foreach (Point point in gridPoints)
        {
            for (int i = 0; i < gridPoints.Count; i++)
            {
                if (gridPoints[i] == point)
                {
                    continue;
                }
                else if (gridPoints[i].IsNearEnough(point.Position.x + sin0, point.Position.z + cos0))
                {
                    point.AddConnection(gridPoints[i]);
                }
                else if (gridPoints[i].IsNearEnough(point.Position.x + sin60, point.Position.z + cos60))
                {
                    point.AddConnection(gridPoints[i]);
                }
                else if (gridPoints[i].IsNearEnough(point.Position.x + sin120, point.Position.z + cos120))
                {
                    point.AddConnection(gridPoints[i]);
                }
                else if (gridPoints[i].IsNearEnough(point.Position.x + sin180, point.Position.z + cos180))
                {
                    point.AddConnection(gridPoints[i]);
                }
                else if (gridPoints[i].IsNearEnough(point.Position.x + sin240, point.Position.z + cos240))
                {
                    point.AddConnection(gridPoints[i]);
                }
                else if (gridPoints[i].IsNearEnough(point.Position.x + sin300, point.Position.z + cos300))
                {
                    point.AddConnection(gridPoints[i]);
                }
            }
        }
    }

    private void CreateSubPoints()
    {
        /* foreach mainPoint in gridPoints
         *      foreach neighbor in point.Connections
         *          find a point to the local left of neighbor
         *                  if local left point has connection back to main point
         *                      create a new point in the middle of the three points(mainPoint, neighbor, local left point)
         */

        ShuffleGridPoints();

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

                    if(localLeft.Connections.Contains(start) && IsLeft(start.Position, neighbor.Position, localLeft.Position) && Random.Range(0f, 1f) <= subPointChance)
                    {
                        Point newPoint = new Point((start.Position.x + neighbor.Position.x + localLeft.Position.x) / 3, 0, (start.Position.z + neighbor.Position.z + localLeft.Position.z) / 3, (Random.Range(0, 1f) >= subPointRigidityChance));
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
    }

    private void CreateSubPoints3D()
    {
        /* foreach mainPoint in gridPoints
         *      foreach neighbor in point.Connections
         *          find a point to the local left of neighbor
         *                  if local left point has connection back to main point
         *                      create a new point in the middle of the three points(mainPoint, neighbor, local left point)
         */

        ShuffleGridPoints();

        foreach (Point start in gridPoints)
        {
            foreach (Point neighbor in start.Connections)
            {
                foreach (Point localLeft in neighbor.Connections)
                {
                    if (localLeft == start)
                    {
                        continue;
                    }

                    if (localLeft.Connections.Contains(start) && IsLeft(start.Position, neighbor.Position, localLeft.Position) && Random.Range(0f, 1f) <= subPointChance)
                    {
                        Point newPoint = new Point((start.Position.x + neighbor.Position.x + localLeft.Position.x) / 3, (start.Position.y + neighbor.Position.y + localLeft.Position.y) / 3, (start.Position.z + neighbor.Position.z + localLeft.Position.z) / 3, (Random.Range(0, 1f) >= subPointRigidityChance));
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
            for (int j = i + 1; j < subPoints.Count; j++)
            {
                //Debug.Log(i + "::" + j + "::" + test.Count);
                if (subPoints[i].IsNearEnough(subPoints[j]))
                {
                    subPoints.RemoveAt(j);
                    j--;
                }
            }
        }
        //Debug.Log(test.Count);
    }

    private void UpdateConnections()
    {
        foreach (Point point in subPoints)
        {
            foreach (Point connection in point.Connections)
            {
                connection.AddConnection(point);
            }
        }

        gridPoints.AddRange(subPoints);
        subPoints.Clear();
    }

    private void RemoveRandomConnections()
    {
        ShuffleGridPoints();

        List<Point> connsToRemove = new List<Point>();
        foreach (Point point in gridPoints)
        {
            if(point.Connections.Count <= minConnectionCount)
            {
                continue;
            }

            foreach (Point connection in point.Connections)
            {
                if(!point.IsModifiable && !connection.IsModifiable)
                {
                    continue;
                }

                if (point.Connections.Count - connsToRemove.Count > minConnectionCount && connection.Connections.Count > minConnectionCount)
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

            if (point.Connections.Count < minConnectionCount)
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
        foreach (Point point in gridPoints)
        {
            if(point.IsModifiable)
            {
                //Vector2 newPos = point.Position;
                Vector3 newPos = Vector3.zero;

                foreach (Point neighbor in point.Connections)
                {
                    newPos += neighbor.Position;
                }

                //newPos = newPos / (point.Connections.Count + 1);
                newPos = newPos / point.Connections.Count;

                //if distance to nearest neighbor is less than 0.35f, dont move
                point.Connections = point.Connections.OrderBy(x => Vector3.Distance(newPos, x.Position)).ToList();

                if(Vector3.Distance(newPos, point.Connections[0].Position) > minPointDistance)
                {
                    point.Position = newPos;
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            switch (Type)
            {
                case GenerationType.FLAT:
                    switch (clickCount)
                    {
                        case 0:
                            CreatePoints();
                            break;
                        case 1:
                            CreateConnections();
                            break;
                        case 2:
                            CreateSubPoints();
                            break;
                        case 3:
                            UpdateConnections();
                            break;
                        case 4:
                            RemoveRandomConnections();
                            break;
                        default:
                            RebalanceGrid();
                            break;
                    }
                    break;
                case GenerationType.CONE:
                    switch (clickCount)
                    {
                        case 0:
                            CreatePointsCone();
                            break;
                        case 1:
                            CreateConnections3D();
                            break;
                        case 2:
                            CreateSubPoints3D();
                            break;
                        case 3:
                            UpdateConnections();
                            break;
                        case 4:
                            RemoveRandomConnections();
                            break;
                        default:
                            RebalanceGrid();
                            break;
                    }
                    break;
                case GenerationType.RANDOM:
                    switch (clickCount)
                    {
                        case 0:
                            CreatePointsRandom();
                            break;
                        case 1:
                            CreateConnections3D();
                            break;
                        case 2:
                            CreateSubPoints3D();
                            break;
                        case 3:
                            UpdateConnections();
                            break;
                        case 4:
                            RemoveRandomConnections();
                            break;
                        default:
                            RebalanceGrid();
                            break;
                    }
                    break;
                case GenerationType.PERLIN:
                    switch (clickCount)
                    {
                        case 0:
                            CreatePointsPerlin();
                            break;
                        case 1:
                            CreateConnections3D();
                            break;
                        case 2:
                            CreateSubPoints3D();
                            break;
                        case 3:
                            UpdateConnections();
                            break;
                        case 4:
                            RemoveRandomConnections();
                            break;
                        default:
                            RebalanceGrid();
                            break;
                    }
                    break;
                default:
                    break;
            }
            clickCount++;
        }
        else if(Input.GetMouseButtonUp(1))
        {
            gridPoints.Clear();
            subPoints.Clear();
            clickCount = 0;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (Point point in gridPoints)
        {
            foreach (Point connection in point.Connections)
            {
                Gizmos.DrawLine(point.Position, connection.Position);
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
            Gizmos.DrawSphere(point.Position, 0.1f);
        }

        foreach (Point point in subPoints)
        {
            if (point.IsModifiable)
            {
                Gizmos.color = Color.yellow;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawSphere(point.Position, 0.1f);
        }
    }

    private bool IsLeft(Vector3 a, Vector3 b, Vector3 c)
    {
        return ((b.x - a.x) * (c.z - a.z) - (b.z - a.z) * (c.x - a.x)) > 0;
    }
}
