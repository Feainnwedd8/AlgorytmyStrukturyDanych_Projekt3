using System;

class Program
{
    static void Main(string[] args)
    {
        // Definiowanie grafu nieskierowanego jako tablicy sąsiedztwa z 8 wierzchołkami
        int[,] graph = new int[,]
        {
            { 0, 1, 1, 0, 0, 0, 0, 1 }, // Wierzchołek 0 połączony z 1, 2 i 7
            { 1, 0, 0, 1, 1, 0, 0, 0 }, // Wierzchołek 1 połączony z 0, 3 i 4
            { 2, 1, 0, 0, 1, 0, 0, 0 }, // Wierzchołek 2 połączony z 0 i 4
            { 3, 0, 0, 0, 1, 1, 0, 0 }, // Wierzchołek 3 połączony z 1 i 4
            { 4, 0, 1, 1, 0, 0, 1, 0 }, // Wierzchołek 4 połączony z 1, 2 i 6
            { 5, 0, 0, 1, 0, 0, 0, 1 }, // Wierzchołek 5 połączony z 3 i 7
            { 6, 0, 0, 0, 1, 0, 0, 0 }, // Wierzchołek 6 połączony z 4
            { 7, 0, 0, 0, 0, 1, 0, 0 }  // Wierzchołek 7 połączony z 5
        };

        // Wizualizacja grafu nieskierowanego
        Console.WriteLine("Wizualizacja grafu nieskierowanego:");
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

        // Definiowanie grafu skierowanego z wagami
        int[,] weights = new int[,]
        {
            { 0, 10, 20, 0, 0, 0, 0, 5 }, // Wagi dla wierzchołka 0
            { 10, 0, 0, 15, 25, 0, 0, 0 }, // Wagi dla wierzchołka 1
            { 20, 0, 0, 0, 0, 30, 0, 0 }, // Wagi dla wierzchołka 2
            { 0, 15, 0, 0, 0, 0, 10, 0 }, // Wagi dla wierzchołka 3
            { 0, 25, 0, 0, 0, 0, 0, 20 }, // Wagi dla wierzchołka 4
            { 0, 0, 30, 0, 0, 0, 0, 10 }, // Wagi dla wierzchołka 5
            { 0, 0, 0, 10, 0, 0, 0, 0 }, // Wagi dla wierzchołka 6
            { 5, 0, 0, 0, 20, 0, 0, 0 }  // Wagi dla wierzchołka 7
        };

        // Wizualizacja grafu skierowanego z wagami
        Console.WriteLine("\nWizualizacja grafu skierowanego z wagami:");
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

        // Wybór algorytmu
        Console.WriteLine("Wybierz algorytm (1 - DFS, 2 - BFS, 3 - Dijkstra): ");
        int choice = int.Parse(Console.ReadLine());

        if (choice == 1)
        {
            bool[] visited = new bool[graph.GetLength(0)];
            int[] path = new int[graph.GetLength(0)];
            int pathIndex = 0;

            Console.WriteLine("Wykonanie DFS:");
            bool found = DFS(graph, 0, visited, path, ref pathIndex); // Zawsze startujemy od 0

            if (found)
            {
                Console.WriteLine("Dotarliśmy do wierzchołka końcowego.");
            }
            else
            {
                Console.WriteLine("Nie znaleziono trasy do wierzchołka końcowego.");
            }
        }
        else if (choice == 2)
        {
            bool[] visited = new bool[graph.GetLength(0)];
            int[] queue = new int[graph.GetLength(0)];
            int front = 0, rear = 0;

            Console.WriteLine("Wykonanie BFS:");
            BFS(graph, 0, visited, queue, ref front, ref rear); // Zawsze startujemy od 0
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
        path[pathIndex++] = current; // Zapisz wierzchołek w ścieżce
        Console.WriteLine($"Odwiedzono wierzchołek: {current}");

        // Sprawdzenie, czy dotarliśmy do ostatniego wierzchołka
        if (current == graph.GetLength(0) - 1)
        {
            return true; // Dotarliśmy do końca
        }

        for (int neighbor = 0; neighbor < graph.GetLength(1); neighbor++)
        {
            if (graph[current, neighbor] == 1 && !visited[neighbor])
            {
                if (DFS(graph, neighbor, visited, path, ref pathIndex))
                {
                    return true; // Zwróć, jeśli dotarłeś do końca
                }
            }
        }

        pathIndex--; // Cofnij indeks, jeśli nie znaleziono ścieżki
        return false; // Nie znaleziono ścieżki
    }

    static void BFS(int[,] graph, int start, bool[] visited, int[] queue, ref int front, ref int rear)
    {
        visited[start] = true;
        queue[rear++] = start; // Dodaj wierzchołek startowy do kolejki

        while (front < rear)
        {
            int current = queue[front++]; // Pobierz wierzchołek z przodu kolejki
            Console.WriteLine($"Odwiedzono wierzchołek: {current}");

            for (int neighbor = 0; neighbor < graph.GetLength(1); neighbor++)
            {
                if (graph[current, neighbor] == 1 && !visited[neighbor])
                {
                    visited[neighbor] = true;
                    queue[rear++] = neighbor; // Dodaj sąsiada do kolejki
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