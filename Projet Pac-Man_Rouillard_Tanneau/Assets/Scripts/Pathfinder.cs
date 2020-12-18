using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


public class Pathfinder : MonoBehaviour
{
    /// Tilemap
    public static Dictionary<Vector3Int, Node> TileNode; 
    public Tilemap maptilemap;                           
    [SerializeField] private Vector3Int startingtile;  
    [SerializeField] private Vector3Int enemyspawndoor;
    private bool inbase = true;
    [SerializeField] private Vector3Int[] portals;
    /// /Tilemap
    
    /// AntiCrash
    [SerializeField] private int overflowlimit;
    /// /AntiCrash>

    /// Pickable Object
    [SerializeField] private Transform ballsparent;
    [SerializeField] private GameObject ball;
    public int ballsnumber;
    [SerializeField] private GameObject energizer;
    [SerializeField] private int energizerchance;
    /// /Pickable Object




    
    


    private void Awake() //This void is mainly used to initialize the node network, creating all the nodes and stocking them in TileNode dictionary
    {
        
        Queue<Vector3Int> NodeQueue = new Queue<Vector3Int>();
        NodeQueue.Enqueue(startingtile);
        TileNode = new Dictionary<Vector3Int, Node>();
        
        while (NodeQueue.Count > 0)
        {
            List<Vector3Int> children = new List<Vector3Int>();
            Vector3Int actualnode = NodeQueue.Dequeue();
            if (actualnode == enemyspawndoor)
            {
                inbase = false;
            }
            if (!TileNode.ContainsKey(actualnode))
            {
                Debug.Log(actualnode);
                if (maptilemap.GetTile(actualnode+new Vector3Int(1,0,0)) == null)
                {
                    NodeQueue.Enqueue(actualnode+new Vector3Int(1,0,0));
                    children.Add(actualnode+new Vector3Int(1,0,0));
                }
                if (maptilemap.GetTile(actualnode+new Vector3Int(-1,0,0)) == null)
                {
                    NodeQueue.Enqueue(actualnode+new Vector3Int(-1,0,0));
                    children.Add(actualnode+new Vector3Int(-1,0,0));
                }
                if (maptilemap.GetTile(actualnode+new Vector3Int(0,1,0)) == null)
                {
                    NodeQueue.Enqueue(actualnode+new Vector3Int(0,1,0));
                    children.Add(actualnode+new Vector3Int(0,1,0));
                }
                if (maptilemap.GetTile(actualnode+new Vector3Int(0,-1,0)) == null)
                {
                    NodeQueue.Enqueue(actualnode+new Vector3Int(0,-1,0));
                    children.Add(actualnode+new Vector3Int(0,-1,0));
                }
                TileNode.Add(actualnode, new Node(children, actualnode, null));
                if (!inbase && actualnode != this.GetComponent<GameplayManager>().spawn && !portals.Contains(actualnode))
                {
                    if (Random.Range(0,101) <= energizerchance)
                    {
                        Instantiate(energizer, maptilemap.layoutGrid.GetCellCenterWorld(actualnode), transform.rotation);
                    }
                    else
                    {
                        Instantiate(ball, maptilemap.layoutGrid.GetCellCenterWorld(actualnode), transform.rotation).transform.parent = ballsparent;
                        ballsnumber++;
                        

                    }
                }
                

                foreach (Vector3Int child in children)
                {
                   
                    Debug.DrawLine(maptilemap.layoutGrid.GetCellCenterWorld(actualnode), maptilemap.layoutGrid.GetCellCenterWorld(child), Color.white, 10);
                    Debug.Log(child+" is Child of "+actualnode);

                }
            }
            else
            {
                Debug.Log("Already visited");
            }
        }
        Debug.Log("PathConstructor end");

    }

    public Stack<Vector3> Pathfind (Vector3 From_world, Vector3 To_world) //Function that navigate in the nodes network, doing the first path to the target and return the result of the next function ReversePathfind
    {
        List<Node> usedNode = new List<Node>();
        if (TileNode.ContainsValue(TileNode[maptilemap.layoutGrid.WorldToCell(From_world)]) && TileNode.ContainsValue(TileNode[maptilemap.layoutGrid.WorldToCell(To_world)]))
        {
            Node From = TileNode[maptilemap.layoutGrid.WorldToCell(From_world)];
            Node To = TileNode[maptilemap.layoutGrid.WorldToCell(To_world)];
        
            Queue<Node> NodeQueue = new Queue<Node>();
            NodeQueue.Enqueue(From);
            int overflowlevel = 0;
            while (NodeQueue.Count > 0)
            {
                overflowlevel++;
                if (overflowlevel > overflowlimit)
                {

                    //Debug.Log("Overflow Limit");

                    Debug.Log("Overflow Limit");
                    Resetparent(usedNode);

                    return new Stack<Vector3>();
                }
                Node thisnode = NodeQueue.Dequeue();
                //Debug.Log("Actual Node is "+thisnode.position);
                if (thisnode.position != To.position)
                {
                    foreach (Vector3Int child in thisnode.children)
                    {
                        if (TileNode[child].parent == null)
                        {
                            TileNode[child].parent = thisnode;
                            usedNode.Add(TileNode[child]);
                            NodeQueue.Enqueue(TileNode[child]);
                            //Debug.Log("Adding Child to queue");
                        }
                    }
                }
                else
                {
                    //Debug.Log("Going to Reverse Path");
                    return ReversePathfind(thisnode, From, usedNode);
                }

            }

            //Debug.Log("While end");

            Debug.Log("While end without reaching target");
            Resetparent(usedNode);

            return new Stack<Vector3>();
        }
        else
        {
            Debug.Log("Don't contain");
            Resetparent(usedNode);
            return new Stack<Vector3>();
        }

    }

    public Stack<Vector3> ReversePathfind(Node Target, Node Root, List<Node> usedNode) //This function use the parents set by the function Pathfind and return a stack of Vector3 waypoints
    {
        if (Target == Root)
        {
            Resetparent(usedNode);
            return new Stack<Vector3>();
        }
        Stack<Vector3> path = new Stack<Vector3>();
        path.Push(maptilemap.layoutGrid.GetCellCenterWorld(Target.position));
        Debug.DrawLine(maptilemap.layoutGrid.GetCellCenterWorld(Target.position), maptilemap.layoutGrid.GetCellCenterWorld(Target.parent.position), Color.red, 1);
        Node actualnode = Target.parent;



        while (actualnode != Root)
        {
            //Debug.Log("InReversePath with "+ actualnode.position);
            path.Push(maptilemap.layoutGrid.GetCellCenterWorld(actualnode.position));
            Debug.DrawLine(maptilemap.layoutGrid.GetCellCenterWorld(actualnode.position), maptilemap.layoutGrid.GetCellCenterWorld(actualnode.parent.position), Color.red, 1);
            actualnode = actualnode.parent;
            

        }
        
      
        actualnode.parent = null;
        Resetparent(usedNode);
        return path;
    }

    void Resetparent(List<Node> usedNode) //this function reset every parents in a the  usedNode list to avoid the "dead tree occurence"
    {
        foreach (Node used in usedNode)
        {
            used.parent = null;
        }
    }

    
}
