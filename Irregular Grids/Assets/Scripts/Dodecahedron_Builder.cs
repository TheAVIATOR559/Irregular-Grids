using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Dodecahedron_Builder : MonoBehaviour
{
    //private List<Point> gridPoints = new List<Point>();

    private float sin0;
    private float sin72;//72
    private float sin144;//144
    private float sin216;//216
    private float sin288;//288

    private float cos0;
    private float cos72;
    private float cos144;
    private float cos216;
    private float cos288;

    private List<TempPoint> subPoints = new List<TempPoint>();

    [SerializeField] float subPointChance = 0.2f;
    [SerializeField] float subPointRigidityChance = 0.1f;
    [SerializeField] int minConnectionCount = 4;
    [SerializeField] float minPointDistance = 0.35f;

    [SerializeField] private int clickCount = 0;

    [SerializeField] private List<Grid_Face> faces = new List<Grid_Face>();//TODO add faces based on notes

    // Start is called before the first frame update
    void Start()
    {
        sin0 = Mathf.Sin(0);
        sin72 = Mathf.Sin(1.25664f);
        sin144 = Mathf.Sin(2.51327f);
        sin216 = Mathf.Sin(3.76991f);
        sin288 = Mathf.Sin(5.02655f);

        cos0 = Mathf.Cos(0);
        cos72 = Mathf.Cos(1.25664f);
        cos144 = Mathf.Cos(2.51327f);
        cos216 = Mathf.Cos(3.76991f);
        cos288 = Mathf.Cos(5.02655f);
    }
    private void CreatePointsDodecahedron(Grid_Face face)
    {
        //https://en.wikipedia.org/wiki/Regular_dodecahedron
        List<Point> mainPoints = new List<Point>();
        List<Point> secondaryPoints = new List<Point>();

        //main points
        for (int i = 1; i <= 5; i++)
        {
            if (i == 5)
            {
                mainPoints.Add(new Point(i * sin0, 0, i * cos0, false));
                mainPoints.Add(new Point(i * sin72, 0, i * cos72, false));
                mainPoints.Add(new Point(i * sin144, 0, i * cos144, false));
                mainPoints.Add(new Point(i * sin216, 0, i * cos216, false));
                mainPoints.Add(new Point(i * sin288, 0, i * cos288, false));
            }
            else
            {
                mainPoints.Add(new Point(i * sin0, 0, i * cos0));
                mainPoints.Add(new Point(i * sin72, 0, i * cos72));
                mainPoints.Add(new Point(i * sin144, 0, i * cos144));
                mainPoints.Add(new Point(i * sin216, 0, i * cos216));
                mainPoints.Add(new Point(i * sin288, 0, i * cos288));
            }

            if (i < 2)
            {
                continue;
            }

            for (int k = (i * 5) - 4; k <= i * 5; k++)
            {
                for (float j = 1; j < i; j++)
                {
                    //(x1 + k(x2 - x1), y1 + k(y2 - y1))
                    //Debug.Log(k + "::" + j + "/" + i + "=" + (j / i));

                    if (k == (i * 5))
                    {
                        if (i == 5)
                        {
                            //Debug.Log(k + "::" + mainPoints[k-1]);
                            secondaryPoints.Add(new Point(mainPoints[k - 1].Position.x + (j / i) * (mainPoints[k - 5].Position.x - mainPoints[k - 1].Position.x), 0, mainPoints[k - 1].Position.z + (j / i) * (mainPoints[k - 5].Position.z - mainPoints[k - 1].Position.z), false));
                        }
                        else
                        {
                            //Debug.Log(k + "::" + mainPoints[k-1]);
                            secondaryPoints.Add(new Point(mainPoints[k - 1].Position.x + (j / i) * (mainPoints[k - 5].Position.x - mainPoints[k - 1].Position.x), 0, mainPoints[k - 1].Position.z + (j / i) * (mainPoints[k - 5].Position.z - mainPoints[k - 1].Position.z)));
                        }
                    }
                    else
                    {
                        if (i == 5)
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

        face.CreatePoint(Vector3.zero, false);

        foreach(Point point in mainPoints)
        {
            face.CreatePoint(point.Position, point.IsModifiable);
        }

        foreach (Point point in secondaryPoints)
        {
            face.CreatePoint(point.Position, point.IsModifiable);
        }
    }

    private void CreateConnectionsDodecahedron(Grid_Face face)
    {
        //int connCount = 0;

        //Debug.Log(gridPoints.Count);

        foreach (Dec_Point point in face.points)
        {
            for (int i = 0; i < face.points.Count; i++)
            {
                if (face.points[i] == point)
                {
                    continue;
                }
                else if (face.points[i].IsNearEnough(point.Position.x + sin0, point.Position.z + cos0))
                {
                    point.AddConnection(face.points[i]);
                }
                else if (face.points[i].IsNearEnough(point.Position.x + sin72, point.Position.z + cos72))
                {
                    point.AddConnection(face.points[i]);
                }
                else if (face.points[i].IsNearEnough(point.Position.x + sin144, point.Position.z + cos144))
                {
                    point.AddConnection(face.points[i]);
                }
                else if (face.points[i].IsNearEnough(point.Position.x + sin216, point.Position.z + cos216))
                {
                    point.AddConnection(face.points[i]);
                }
                else if (face.points[i].IsNearEnough(point.Position.x + sin288, point.Position.z + cos288))
                {
                    point.AddConnection(face.points[i]);
                }
            }
            //break;
        }

        UpdateConnections(face);
    }

    private void CreateSubPointsDodecahedron(Grid_Face face)
    {
        /* foreach mainPoint in gridPoints
         *      foreach neighbor in point.Connections
         *          find a point to the local left of neighbor
         *                  if local left point has connection back to main point
         *                      create a new point in the middle of the three points(mainPoint, neighbor, local left point)
         */

        //List<TempPoint> subPoints = new List<TempPoint>();

        ShuffleGridPoints(face);

        foreach (Dec_Point start in face.points)
        {
            foreach (Dec_Point neighbor in start.Connections)
            {
                foreach (Dec_Point firstLeft in neighbor.Connections)
                {
                    if (firstLeft == start || firstLeft == neighbor)
                    {
                        continue;
                    }

                    //Debug.Log("Start -> Neighbor ?? First Left :: " + IsLeft(start.Position, neighbor.Position, firstLeft.Position));

                    if (IsLeft(start.Position, neighbor.Position, firstLeft.Position))
                    {
                        foreach (Dec_Point secondLeft in firstLeft.Connections)
                        {
                            if (secondLeft == start || secondLeft == neighbor || secondLeft == firstLeft)
                            {
                                continue;
                            }

                            //SEE DOODLE ON NOTEBOOK
                            //Debug.Log("START " + start.Position + " :: NEIGHBOR " + neighbor.Position + " :: FIRST LEFT " + firstLeft.Position + " :: SECOND LEFT " + secondLeft.Position);

                            if (secondLeft.Connections.Contains(start) && IsLeft(start.Position, firstLeft.Position, secondLeft.Position) && Random.Range(0f, 1f) <= subPointChance)
                            {
                                Debug.Log("NEW SUB POINT");
                                
                                TempPoint newPoint = new TempPoint(new Vector3((start.Position.x + neighbor.Position.x + firstLeft.Position.x + secondLeft.Position.x) / 4, (start.Position.y + neighbor.Position.y + firstLeft.Position.y + secondLeft.Position.y) / 4, (start.Position.z + neighbor.Position.z + firstLeft.Position.z + secondLeft.Position.z) / 4), Random.Range(0, 1f) >= subPointRigidityChance);
                                subPoints.Add(newPoint);
                                newPoint.Connections.Add(start);
                                newPoint.Connections.Add(neighbor);
                                newPoint.Connections.Add(firstLeft);
                                newPoint.Connections.Add(secondLeft);
                            }
                        }
                    }
                }
            }
            //break;
        }

        for (int i = 0; i < subPoints.Count; i++)
        {
            for (int j = i + 1; j < subPoints.Count; j++)
            {
                //Debug.Log(i + "::" + j + "::" + test.Count);
                if (IsNearEnough(subPoints[i].Position, subPoints[j].Position))
                {
                    subPoints.RemoveAt(j);
                    j--;
                }
            }
        }

        foreach (Dec_Point point in face.points)
        {
            for (int i = 0; i < face.points.Count; i++)
            {
                if (point == face.points[i])
                {
                    continue;
                }

                if (!point.IsModifiable && !face.points[i].IsModifiable && Vector3.Distance(point.Position, face.points[i].Position) <= 1.2f)
                {
                    point.AddConnection(face.points[i]);
                    face.points[i].AddConnection(point);
                }
            }
        }
    }

    private void AddSubPointConnections(Grid_Face face)
    {
        foreach (TempPoint point in subPoints)
        {
            face.CreatePoint(point.Position, point.IsModifiable, point.Connections);
        }

        subPoints.Clear();
    }

    private void RemoveRandomConnections(Grid_Face face)
    {
        ShuffleGridPoints(face);
        UpdateConnections(face);

        List<Dec_Point> connsToRemove = new List<Dec_Point>();
        foreach (Dec_Point point in face.points)
        {
            if (point.Connections.Count <= minConnectionCount)
            {
                continue;
            }

            foreach (Dec_Point connection in point.Connections)
            {
                if (!point.IsModifiable && !connection.IsModifiable)
                {
                    continue;
                }

                if (point.Connections.Count - connsToRemove.Count > minConnectionCount && connection.Connections.Count > minConnectionCount)
                {
                    connsToRemove.Add(connection);
                }
            }

            //Debug.Log(connsToRemove.Count);

            foreach (Dec_Point removed in connsToRemove)
            {
                point.RemoveConnectionMutual(removed);
            }
            connsToRemove.Clear();

            if (point.Connections.Count < minConnectionCount)
            {
                Debug.LogWarning(point.Connections.Count);
            }
        }
    }

    private void ShuffleGridPoints(Grid_Face face)
    {
        for (int i = 0; i < face.points.Count; i++)
        {
            Dec_Point temp = face.points[i];
            int randomIndex = Random.Range(i, face.points.Count);

            face.points[i] = face.points[randomIndex];
            face.points[randomIndex] = temp;
        }
    }

    private void RebalanceGrid(Grid_Face face) //LAPLACIAN SMOOTHING
    {
        foreach (Dec_Point point in face.points)
        {
            if (point.IsModifiable)
            {
                //Vector2 newPos = point.Position;
                Vector3 newPos = Vector3.zero;

                foreach (Dec_Point neighbor in point.Connections)
                {
                    newPos += neighbor.Position;
                }

                //newPos = newPos / (point.Connections.Count + 1);
                newPos = newPos / point.Connections.Count;

                //if distance to nearest neighbor is less than 0.35f, dont move
                point.Connections = point.Connections.OrderBy(x => Vector3.Distance(newPos, x.Position)).ToList();

                if (Vector3.Distance(newPos, point.Connections[0].Position) > minPointDistance)
                {
                    point.Position = newPos;
                }
            }
        }
    }

    private void UpdateConnections(Grid_Face face)
    {
        foreach (Dec_Point core in face.points)
        {
            foreach (Dec_Point neighbor in core.Connections)
            {
                if (!neighbor.Connections.Contains(core))
                {
                    neighbor.AddConnection(core);
                    //Debug.Log("runned");
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            foreach(Grid_Face face in faces)
            {
                switch (clickCount)
                {
                    case 0:
                        Debug.Log("Creating points");
                        CreatePointsDodecahedron(face);
                        break;
                    case 1:
                        Debug.Log("Creating connections");
                        CreateConnectionsDodecahedron(face);
                        break;
                    case 2:
                        Debug.Log("Creating subpoints");
                        CreateSubPointsDodecahedron(face);
                        break;
                    case 3:
                        Debug.Log("Shuffling Subpoints into main Grid");
                        AddSubPointConnections(face);
                        break;
                    case 4:
                        Debug.Log("Removing Connections");
                        RemoveRandomConnections(face);
                        break;
                    default:
                        Debug.Log("Rebalancing Grid");
                        RebalanceGrid(face);
                        break;
                }
            }
            clickCount++;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            foreach(Grid_Face face in faces)
            {
                face.Clear();
            }
            subPoints.Clear();
            clickCount = 0;
        }
    }

    private bool IsLeft(Vector3 a, Vector3 b, Vector3 c)
    {
        return ((b.x - a.x) * (c.z - a.z) - (b.z - a.z) * (c.x - a.x)) > 0;
    }

    public static float COMPARISON_TOLERANCE = 0.1f;
    public bool IsNearEnough(Dec_Point center, Dec_Point other)
    {

        if (Mathf.Abs(center.Position.x - other.Position.x) <= COMPARISON_TOLERANCE
            && Mathf.Abs(center.Position.y - other.Position.y) <= COMPARISON_TOLERANCE
            && Mathf.Abs(center.Position.z - other.Position.z) <= COMPARISON_TOLERANCE)
        {
            return true;
        }

        return false;
    }

    public bool IsNearEnough(Vector3 a, Vector3 b)
    {
        //Debug.Log((Mathf.Abs(this.Position.x - x) <= COMPARISON_TOLERANCE) + "::" + (Mathf.Abs(this.Position.y - y) <= COMPARISON_TOLERANCE) + "::" + (Mathf.Abs(this.Position.z - z) <= COMPARISON_TOLERANCE));
        //Debug.Log(this.Position + "::(" + x + ", " + y + ", " + z + ")");

        if (Mathf.Abs(a.x - b.x) <= COMPARISON_TOLERANCE
            && Mathf.Abs(a.y - b.y) <= COMPARISON_TOLERANCE
            && Mathf.Abs(a.z - b.z) <= COMPARISON_TOLERANCE)
        {
            return true;
        }

        return false;
    }

    public bool IsNearEnough(Dec_Point center, float x, float z)
    {
        //Debug.Log((Mathf.Abs(this.Position.x - x) <= COMPARISON_TOLERANCE) + "::" + (Mathf.Abs(this.Position.z - z) <= COMPARISON_TOLERANCE));
        if (Mathf.Abs(center.Position.x - x) <= COMPARISON_TOLERANCE
            && Mathf.Abs(center.Position.z - z) <= COMPARISON_TOLERANCE)
        {
            return true;
        }

        return false;
    }

    struct TempPoint
    {
        public Vector3 Position;
        public bool IsModifiable;
        public List<Dec_Point> Connections;

        public TempPoint(Vector3 pos, bool isModifiable)
        {
            Position = pos;
            IsModifiable = isModifiable;
            Connections = new List<Dec_Point>();
        }
    }
}
