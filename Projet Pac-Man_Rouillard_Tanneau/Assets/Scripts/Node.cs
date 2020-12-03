using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int name;
    public Node[] children;
    public Vector3 position;
    






    public Node(Node[] children, Vector3 position, int name)
    {
        this.children = children;
        this.position = position;
        this.name = name;
    }
    
}
