using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Pathfinder : MonoBehaviour
{
    public static Dictionary<Vector3Int, Node> TileNode;
    [SerializeField] private Tilemap maptilemap;
    [SerializeField] private Vector3Int startingtile;

    private void Awake()
    {
        
        Queue<Vector3Int> NodeQueue = new Queue<Vector3Int>();
        NodeQueue.Enqueue(startingtile);
        TileNode = new Dictionary<Vector3Int, Node>();
        
        while (NodeQueue.Count > 0)
        {
            List<Vector3Int> children = new List<Vector3Int>();
            Vector3Int actualnode = NodeQueue.Dequeue();
            
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
                foreach (Vector3Int child in children)
                {
                   
                    Debug.DrawLine(maptilemap.layoutGrid.GetCellCenterWorld(actualnode), maptilemap.layoutGrid.GetCellCenterWorld(child), Color.white, 120);
                    Debug.Log(child+" is Child of "+actualnode);

                }
            }
            else
            {
                Debug.Log("I am not the droid you are searching");
            }
        }
        Debug.Log("PathConstructor end");
        
    }

    public Stack<Vector3> Pathfind (Node From, Node To)
    {
        Queue<Node> NodeQueue = new Queue<Node>();
        NodeQueue.Enqueue(From);
        
        while (NodeQueue.Count > 1)
        {
            Node thisnode = NodeQueue.Dequeue();

            if (thisnode.position != To.position)
            {
                foreach (Vector3Int child in thisnode.children)
                {
                    TileNode[child].parent = thisnode;
                    NodeQueue.Enqueue(TileNode[child]);
                }
            }
            else
            {
                return ReversePathfind(thisnode, From);
            }

        }
        
        return null;
    }

    private Stack<Vector3> ReversePathfind(Node Target, Node Root)
    {
        Stack<Vector3> path = new Stack<Vector3>();
        path.Push(maptilemap.layoutGrid.CellToWorld(Target.position));
        Node actualnode = Target.parent;
        Debug.DrawLine(maptilemap.layoutGrid.GetCellCenterWorld(actualnode.position), maptilemap.layoutGrid.GetCellCenterWorld(Target.position), Color.red, 120);


        while (actualnode != Root)
        {
            path.Push(maptilemap.layoutGrid.CellToWorld(actualnode.position));
            actualnode = actualnode.parent;
            Debug.DrawLine(maptilemap.layoutGrid.GetCellCenterWorld(actualnode.position), maptilemap.layoutGrid.GetCellCenterWorld(Target.position), Color.red, 120);

        }
        return path;
    }

}
