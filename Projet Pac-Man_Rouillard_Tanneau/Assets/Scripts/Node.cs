using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int name;
    public List<Vector3Int> children;
    public Vector3 position;

    






    public Node(List<Vector3Int> children, int name, Vector3 position)
    {
        this.children = children;
        this.name = name;
        this.position = position;

    }
    
    
}
