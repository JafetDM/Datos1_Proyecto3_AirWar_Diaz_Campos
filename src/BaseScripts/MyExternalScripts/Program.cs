using System.Collections.Generic;
public class Program
    {
        static void Main(string[] args)
        {
            Graph grafito = new();
            
            for (int i = 0; i < 7; i++)
            {
                grafito.AddNode();    
            }
            
            // for (int i = 0; i < 3; i++)
            // {
            //     grafito.AddEdge(i, i+1, 10*(i+1));    
            // }
            grafito.AddEdge(0, 2, 3);
            grafito.AddEdge(0, 5, 2);
            grafito.AddEdge(1, 3, 1);
            grafito.AddEdge(1, 4, 2);
            grafito.AddEdge(1, 5, 6);
            grafito.AddEdge(1, 6, 2);
            grafito.AddEdge(2, 3, 4);
            grafito.AddEdge(2, 4, 1);
            grafito.AddEdge(2, 4, 2);
            grafito.AddEdge(4, 5, 3);
            grafito.AddEdge(5, 6, 5);
            
            
            grafito.ShowNodes("Los nodos del grafito son:");
            grafito.ShowGraph("Grafito dice: ");

            (List<int> path, int weight) resultado = grafito.Dijkstra(2,6);

            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();
            
            System.Console.WriteLine("A,B,C,D,E,F,G");
            System.Console.WriteLine("0,1,2,3,4,5,6");
            System.Console.WriteLine("Path:");
            foreach (var item in resultado.path)
            {
                System.Console.Write(item + ", ");
            }
            System.Console.WriteLine();
            System.Console.WriteLine("weight = " + resultado.weight);


        }
    }




