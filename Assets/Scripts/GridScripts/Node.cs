using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool Passable { set; get; }

    public GridObject GridObject { get; set; }

    public float GCost { get; set; }

    public float HCost { get; set; }

    public float FCost 
    { 
        get{ return GCost + HCost; } 
    }

    public Vector2Int Parent { get; set; }
}
