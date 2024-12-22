using System;
using System.Collections.Generic;

public class Graph
{
	private int[,] capacities; // Матрица смежности для хранения пропускных способностей
	private int vertices;

	public Graph(int vertices)
	{
		this.vertices = vertices;
		capacities = new int[vertices, vertices];
	}

	public void AddRibs(string value)
	{
		int from = value[0] - 'A';
		int to = value[1] - 'A';
		int capacity = Convert.ToInt32(value.Substring(2));
		capacities[from, to] = capacity; // Заполняем матрицу смежности
	}

	public int MaxFlow(char source, char sink)
	{
		int sourceIndex = source - 'A';
		int sinkIndex = sink - 'A';
		int totalFlow = 0;

		while (true)
		{
			List<int> parent = new List<int>(new int[vertices]); // Массив для хранения путей
			Queue<int> queue = new Queue<int>();
			queue.Enqueue(sourceIndex);

			while (queue.Count > 0 && parent[sinkIndex] == 0)
			{
				int current = queue.Dequeue();
				for (int next = 0; next < vertices; next++)
				{
					if (capacities[current, next] > 0 && parent[next] == 0)
					{
						parent[next] = current + 1; // Запоминаем путь
						queue.Enqueue(next);
						if (next == sinkIndex)
							break;
					}
				}
			}

			if (parent[sinkIndex] == 0) break; // Нет доступного пути

			int flow = int.MaxValue;
			for (int u = sinkIndex; u != sourceIndex; u = parent[u] - 1)
			{
				int v = parent[u] - 1;
				flow = Math.Min(flow, capacities[v, u]);
			}

			for (int u = sinkIndex; u != sourceIndex; u = parent[u] - 1)
			{
				int v = parent[u] - 1;
				capacities[v, u] -= flow; // Уменьшаем пропускную способность
				capacities[u, v] += flow; // Добавляем обратное ребро
			}

			totalFlow += flow; // Увеличиваем общий поток
		}

		return totalFlow;
	}
}

class Program
{
	static void Main(string[] args)
	{
		Graph graph = new Graph(6);
		graph.AddRibs("AB7");
		graph.AddRibs("AC4");
		graph.AddRibs("BC4");
		graph.AddRibs("BE2");
		graph.AddRibs("CE8");
		graph.AddRibs("CD4");
		graph.AddRibs("ED4");
		graph.AddRibs("EF5");
		graph.AddRibs("DF12");

		int maxFlow = graph.MaxFlow('A', 'F');
		Console.WriteLine($"Максимальный поток: {maxFlow}");
	}
}