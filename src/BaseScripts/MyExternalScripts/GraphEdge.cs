using System;
public class GraphEdge
{
    public GraphNode origin_node;
    public GraphNode end_node;
    public int weight;
    public GraphEdge(GraphNode origin_node, GraphNode end_node, int weight)
    {
        this.origin_node = origin_node;
        this. end_node = end_node;
        this.weight = weight;
    }
    
}

