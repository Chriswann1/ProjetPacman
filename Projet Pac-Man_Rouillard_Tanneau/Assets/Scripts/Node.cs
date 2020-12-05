using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int name;
    public List<Vector3Int> children;
    public Vector3Int position;
    public Node parent;

    






    public Node(List<Vector3Int> children,Vector3Int position, Node parent)
    {
        this.children = children;
        this.name = name;
        this.position = position;
        this.parent = parent;
    }
    
    
}
