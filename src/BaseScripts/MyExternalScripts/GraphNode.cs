using System;
using UnityEngine;
public class GraphNode
{
    public Guid guid = new();
    public Vector2 coords;
    public bool is_water;
    public GraphNode(Guid guid)
    {
        this.guid = guid;
    }
    public GraphNode(Guid guid, Vector2 coords, bool is_water)
    {
        this.guid = guid;
        this.coords = coords;
        this.is_water = is_water;
    }
    
}

