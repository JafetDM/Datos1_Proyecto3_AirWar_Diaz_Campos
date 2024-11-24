using System;
using System.Collections.Generic;
using System.Numerics;
public class Graph
{
    public List<GraphEdge> adjacency_list_edges = new();
    public List<GraphNode> adjacency_list_nodes = new();

    public void AddNode(Guid guid)
    {
        adjacency_list_nodes.Add(new GraphNode(guid));
    }
    public void AddNode()
    {
        adjacency_list_nodes.Add(new GraphNode(Guid.NewGuid()));
    }

    public void AddNode(UnityEngine.Vector2 coords, bool is_water)
    {
        adjacency_list_nodes.Add(new GraphNode(Guid.NewGuid(), coords, is_water));
    }

    public void AddEdge(int node1_index, int node2_index, int weight)
    {
        GraphEdge edge = new(this.adjacency_list_nodes[node1_index], this.adjacency_list_nodes[node2_index], weight);
        adjacency_list_edges.Add(edge);
        edge = new(this.adjacency_list_nodes[node2_index], this.adjacency_list_nodes[node1_index], weight);
        adjacency_list_edges.Add(edge);
    }
    
    public int FindIndexFromGUID(Guid guid)
    {
        int index = -1;
        int node_amount = adjacency_list_nodes.Count;
        for (int i = 0; i < node_amount; i++)
        {
            if (adjacency_list_nodes[i].guid.Equals(guid))
            {
                index = i;
            }
        }
        return index;
    }


    public (List<int>, int) Dijkstra(int origin_node_index, int end_node_index) // Dijkstra crashes if there are no edges.
    {
        // Starts the weights list with -1 (infinite) for nodes other than the origin and 0 for the origin.
        List<int> weights_list = new();
        int nodes_amount = adjacency_list_nodes.Count;
        for (int i = 0; i < nodes_amount; i++)
        {
            weights_list.Add(-1);
        }
        weights_list[origin_node_index] = 0;
        
        List<Guid> weights_from_list = new();
        for (int i = 0; i < nodes_amount; i++)
        {
            weights_from_list.Add(new Guid());
        }
        // weights_from_list[origin_node_index] = 0;

        List<bool> visited_nodes_list = new();
        for (int i = 0; i < nodes_amount; i++)
        {
            visited_nodes_list.Add(false);
        }
        visited_nodes_list[origin_node_index] = true;

        GraphNode current_node = adjacency_list_nodes[origin_node_index];
        Guid current_guid = current_node.guid;


        System.Console.WriteLine("Dijkstra to go from: " + adjacency_list_nodes[origin_node_index].guid.ToString() + " to: " + adjacency_list_nodes[end_node_index].guid.ToString());
        ////////////////////////////////////////////////////////////////////////////////
        System.Console.WriteLine("weights_list before:");
        foreach (var item in weights_list)
        {
            System.Console.Write(item + ", ");
        }
        ////////////////////////////////////////////////////////////////////////////////

        System.Console.WriteLine("current_guid = " + current_guid.ToString());
        while (!current_node.guid.Equals(adjacency_list_nodes[end_node_index].guid)) // while destination hasn't been reached.
        {
            System.Console.WriteLine("DIJKSTRA WHILE 1.");
            
            int current_node_index = FindIndexFromGUID(current_guid);
            foreach (var edge in adjacency_list_edges)
            {
                if (edge.origin_node.guid.Equals(current_guid))
                {
                    int current_end_node_index = FindIndexFromGUID(edge.end_node.guid);
                    int new_weight = edge.weight + weights_list[current_node_index];
                    if (weights_list[current_end_node_index] == -1 || new_weight < weights_list[current_end_node_index])
                    {
                        weights_list[current_end_node_index] = new_weight;
                        weights_from_list[current_end_node_index] = edge.origin_node.guid;
                    }
                }
            }

            ////////////////////////////////////////////////////////////////////////////////
            System.Console.WriteLine("weights_list after:");
            foreach (var item in weights_list)
            {
                System.Console.Write(item + ", ");
            }
            System.Console.WriteLine();
            System.Console.WriteLine("visited_nodes_list after:");
            foreach (var item in visited_nodes_list)
            {
                System.Console.Write(item + ", ");
            }
            System.Console.WriteLine();
            System.Console.WriteLine("weights_from_list after:");
            foreach (var item in weights_from_list)
            {
                System.Console.Write(item.ToString() + ", ");
            }
            System.Console.WriteLine();
            
            // Thread.Sleep(500);
            ////////////////////////////////////////////////////////////////////////////////

            int min_weight = -1;
            int min_weight_node_index = -1;
            for (int i = 0; i < nodes_amount; i++)
            {
                if (!visited_nodes_list[i])
                {
                    if (min_weight == -1 || min_weight > weights_list[i] && weights_list[i] != -1)
                    {
                        min_weight = weights_list[i];
                        min_weight_node_index = i;
                    }
                }
            }
            // Thread.Sleep(100);
            System.Console.WriteLine("min_weight = " + min_weight);
            
            visited_nodes_list[min_weight_node_index] = true;
            current_node = adjacency_list_nodes[min_weight_node_index];
            current_guid = current_node.guid;
            System.Console.WriteLine("current_guid = " + current_guid.ToString());
        }

        ////////////////////////////////////////////////////////////////////////////////
        System.Console.WriteLine("weights_from_list after while 1");
        foreach (var item in weights_from_list)
        {
            System.Console.WriteLine(item.ToString() + ", ");
        }
        // Thread.Sleep(5000);
        ////////////////////////////////////////////////////////////////////////////////
        
        List<int> reversed_path = new();
        // current_guid = adjacency_list_nodes[end_node_index].guid;    // It's already at the last node from previous while.
        int current_index = FindIndexFromGUID(current_guid);    // Should be last node.
        int total_weight = weights_list[current_index];
        while (!current_guid.Equals(adjacency_list_nodes[origin_node_index].guid))
        {
            System.Console.WriteLine("DIJKSTRA WHILE 2.");
            reversed_path.Add(current_index);
            current_guid = weights_from_list[current_index];
            current_index = FindIndexFromGUID(current_guid);
        }
        //(A0)-1-(Ba)-2-(Cb)-3-(Dc)
        reversed_path.Reverse();
        return (reversed_path, total_weight);


    } 






    public void ShowGraph()
    {
        int node_amount = this.adjacency_list_nodes.Count;
        int edge_amount = this.adjacency_list_edges.Count;
        for (int i = 0; i < node_amount; i++)
        {
            for (int j = 0; j < edge_amount; j++)
            {
                Guid i_node_guid = adjacency_list_nodes[i].guid;
                Guid j_node_guid_1 = adjacency_list_edges[j].origin_node.guid;
                Guid j_node_guid_2 = adjacency_list_edges[j].end_node.guid;
                if (i_node_guid.Equals(j_node_guid_1))
                {
                    System.Console.WriteLine("(" + i_node_guid + ") -" + adjacency_list_edges[j].weight + "-> (" + j_node_guid_2 + ")");
                }
            }
            System.Console.WriteLine();
        }
    }
    public void ShowGraph(string message)
    {
        System.Console.WriteLine(message);
        ShowGraph();
    }

    public void ShowNodes()
    {
        int node_amount = this.adjacency_list_nodes.Count;
        for (int i = 0; i < node_amount; i++)
        {
            System.Console.WriteLine("(" + adjacency_list_nodes[i].guid + ")");
        }
    }
    public void ShowNodes(string message)
    {
        System.Console.WriteLine(message);
        ShowNodes();
    }


    public string ShowGraphString()
    {
        string msg = "";
        int node_amount = this.adjacency_list_nodes.Count;
        int edge_amount = this.adjacency_list_edges.Count;
        for (int i = 0; i < node_amount; i++)
        {
            for (int j = 0; j < edge_amount; j++)
            {
                Guid i_node_guid = adjacency_list_nodes[i].guid;
                Guid j_node_guid_1 = adjacency_list_edges[j].origin_node.guid;
                Guid j_node_guid_2 = adjacency_list_edges[j].end_node.guid;
                if (i_node_guid.Equals(j_node_guid_1))
                {
                    msg += "(" + i_node_guid.ToString() + ") -" + adjacency_list_edges[j].weight + "-> (" + j_node_guid_2.ToString() + ")" + "\n";
                    System.Console.WriteLine("(" + i_node_guid + ") -" + adjacency_list_edges[j].weight + "-> (" + j_node_guid_2 + ")");
                }
            }
            msg += "\n";
            System.Console.WriteLine();
        }
        return msg;
    }

    public string ShowGraphString(string message)
    {
        return message + "\n" + ShowGraphString();
    }




}

