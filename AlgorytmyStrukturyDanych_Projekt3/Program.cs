using System;

class Program
{
    static void Main(string[] args)
    {
        int[,] graph = new int[,]
        {
            { 0, 1, 1, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 1, 1, 0, 0, 0 },
            { 2, 1, 0, 0, 1, 0, 0, 0 },
            { 3, 0, 0, 0, 1, 1, 0, 0 }, 
            { 4, 0, 1, 1, 0, 0, 1, 0 },
            { 5, 0, 0, 1, 0, 0, 0, 1 }, 
            { 6, 0, 0, 0, 1, 0, 0, 0 }, 
            { 7, 0, 0, 0, 0, 1, 0, 0 }  
        };

        Console.WriteLine("Graf nieskierowany:");
        for (int i = 0; i < graph.GetLength(0); i++)
        {
            Console.Write($"{i}: ");
            for (int j = 0; j < graph.GetLength(1); j++)
            {
                if (graph[i, j] == 1)
                {
                    Console.Write($"{j} ");
                }
            }
            Console.WriteLine();
        }

        int[,] weights = new int[,]
        {
            { 0, 10, 20, 0, 0, 0, 0, 5 },
            { 10, 0, 0, 15, 25, 0, 0, 0 },
            { 20, 0, 0, 0, 0, 30, 0, 0 },
            { 0, 15, 0, 0, 0, 0, 10, 0 },
            { 0, 25, 0, 0, 0, 0, 0, 20 },
            { 0, 0, 30, 0, 0, 0, 0, 10 },
            { 0, 0, 0, 10, 0, 0, 0, 0 },
            { 5, 0, 0, 0, 20, 0, 0, 0 }
        };

        Console.WriteLine("\nGrafu skierowanyo z wagami:");
        for (int i = 0; i < weights.GetLength(0); i++)
        {
            Console.Write($"{i}: ");
            for (int j = 0; j < weights.GetLength(1); j++)
            {
                if (weights[i, j] != 0)
                {
                    Console.Write($"({j}, {weights[i, j]}) ");
                }
            }
            Console.WriteLine();
        }

        Console.WriteLine("Wybierz algorytm (1 - DFS, 2 - BFS, 3 - Dijkstra): ");
        int choice = int.Parse(Console.ReadLine());

        if (choice == 1)
        {
            bool[] visited = new bool[graph.GetLength(0)];
            int[] path = new int[graph.GetLength(0)];
            int pathIndex = 0;

            Console.WriteLine("Wykonanie DFS:");
            bool found = DFS(graph, 0, visited, path, ref pathIndex);

            if (found)
            {
                Console.WriteLine("Wierzchołek końcowy osiągnięty");
            }
            else
            {
                Console.WriteLine("Nie znaleziono trasy do wierzchołka końcowego");
            }
        }
        else if (choice == 2)
        {
            bool[] visited = new bool[graph.GetLength(0)];
            int[] queue = new int[graph.GetLength(0)];
            int front = 0, rear = 0;

            Console.WriteLine("Wykonanie BFS:");
            BFS(graph, 0, visited, queue, ref front, ref rear);
        }
        else if (choice == 3)
        {
            Console.WriteLine("Podaj wierzchołek początkowy (0-7): ");
            int start = int.Parse(Console.ReadLine());
            Console.WriteLine("Podaj wierzchołek końcowy (0-7): ");
            int end = int.Parse(Console.ReadLine());

            Dijkstra(weights, start, end);
        }
        else
        {
            Console.WriteLine("Nieprawidłowy wybór.");
        }
    }

    static bool DFS(int[,] graph, int current, bool[] visited, int[] path, ref int pathIndex)
    {
        visited[current] = true;
        path[pathIndex++] = current;
        Console.WriteLine($"Odwiedzono wierzchołek: {current}");

        if (current == graph.GetLength(0) - 1)
        {
            return true;
        }

        for (int neighbor = 0; neighbor < graph.GetLength(1); neighbor++)
        {
            if (graph[current, neighbor] == 1 && !visited[neighbor])
            {
                if (DFS(graph, neighbor, visited, path, ref pathIndex))
                {
                    return true;
                }
            }
        }

        pathIndex--;
        return false;
    }

    static void BFS(int[,] graph, int start, bool[] visited, int[] queue, ref int front, ref int rear)
    {
        visited[start] = true;
        queue[rear++] = start;

        while (front < rear)
        {
            int current = queue[front++];
            Console.WriteLine($"Odwiedzono wierzchołek: {current}");

            for (int neighbor = 0; neighbor < graph.GetLength(1); neighbor++)
            {
                if (graph[current, neighbor] == 1 && !visited[neighbor])
                {
                    visited[neighbor] = true;
                    queue[rear++] = neighbor;
                }
            }
        }
    }

    static void Dijkstra(int[,] weights, int start, int end)
    {
        int[] dist = new int[weights.GetLength(0)];
        bool[] sptSet = new bool[weights.GetLength(0)];
        int[] parent = new int[weights.GetLength(0)];

        for (int i = 0; i < dist.Length; i++)
        {
            dist[i] = int.MaxValue;
            sptSet[i] = false;
            parent[i] = -1;
        }

        dist[start] = 0;

        for (int count = 0; count < weights.GetLength(0) - 1; count++)
        {
            int u = MinDistance(dist, sptSet);
            sptSet[u] = true;

            for (int v = 0; v < weights.GetLength(1); v++)
            {
                if (!sptSet[v] && weights[u, v] != 0 && dist[u] != int.MaxValue && dist[u] + weights[u, v] < dist[v])
                {
                    dist[v] = dist[u] + weights[u, v];
                    parent[v] = u;
                }
            }
        }

        PrintSolution(dist, parent, end);
    }

    static int MinDistance(int[] dist, bool[] sptSet)
    {
        int min = int.MaxValue, minIndex = -1;

        for (int v = 0; v < dist.Length; v++)
        {
            if (!sptSet[v] && dist[v] <= min)
            {
                min = dist[v];
                minIndex = v;
            }
        }

        return minIndex;
    }

    static void PrintSolution(int[] dist, int[] parent, int end)
    {
        Console.WriteLine($"Koszt najkrótszej trasy z wierzchołka do {end} wynosi: {dist[end]}");
        Console.Write("Najkrótsza trasa: ");
        PrintPath(parent, end);
        Console.WriteLine();
    }

    static void PrintPath(int[] parent, int j)
    {
        if (parent[j] == -1)
        {
            Console.Write(j + " ");
            return;
        }

        PrintPath(parent, parent[j]);
        Console.Write(j + " ");
    }
}