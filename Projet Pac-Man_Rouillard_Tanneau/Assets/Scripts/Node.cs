using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public List<Vector3Int> children;
    public Vector3Int position;
    public Node parent;

    public Node(List<Vector3Int> children,Vector3Int position, Node parent)
    {
        this.children = children;
        this.position = position;
        this.parent = parent;
    }
    
    
}
