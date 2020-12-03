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
    private int nameindex = 0;

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
                
                TileNode.Add(actualnode, new Node(children, nameindex, actualnode));
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

    public Queue<Vector3> Pathfind (Node targetposition, Node currentposition)
    {
        
        return null;
    }

}
