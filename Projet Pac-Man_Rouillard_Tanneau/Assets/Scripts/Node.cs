using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public List<Vector3Int> children; //3,5,0
    public Vector3Int position; //2,5,0
    public Node parent;

    public Node(List<Vector3Int> children,Vector3Int position, Node parent)
    {
        this.children = children;
        this.position = position;
        this.parent = parent;
    }
    
    
}
