using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public List<Vector3Int> children; //used to stock childrens position
    public Vector3Int position; //used to stock himself's position
    public Node parent; //usesd to set his parent for reverse pathfinding

    public Node(List<Vector3Int> children,Vector3Int position, Node parent) //constructor of Node class
    {
        this.children = children;
        this.position = position;
        this.parent = parent;
    }
    
    
}
