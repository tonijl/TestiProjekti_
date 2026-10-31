
using System.Collections.Generic;
using UnityEngine;

public class PathfindingGrid : MonoBehaviour
{
	
	public Vector2 worldSize = new Vector2 (20,20);
	public float cellSize = 0.5f;
	public LayerMask obstacleMask;
	
	private Node[,] grid;
	private int width;
	private int height;
	
	class Node
	{
		public Vector2 position;
		public bool walkable;
		public int gCost;
		public int hCost;
		public Node parent;
		
		public int fCost => gCost + hCost;
		
		public Node(Vector2 pos, bool canWalk)
		{
			position = pos;
			walkable = canWalk;
		}
	}
	
	void Awake()
	{
		CreateGrid();
	}
	
	void CreateGrid()
	{
		width = Mathf.CeilToInt(worldSize.x / cellSize);
		height = Mathf.CeilToInt(worldSize.y / cellSize);
		
		grid = new Node[width, height];
		
		Vector2 bottomLeft = (Vector2)transform.position - worldSize / 2f;
		
		for (int x = 0; x < width; x++)
		{
			for(int y = 0; y < height;y++)
			{
				Vector2 pos = bottomLeft + new Vector2(
					(x + 0.5f) * cellSize,
					(y + 0.5f) * cellSize
				);
				
				bool blocked = Physics2D.OverlapBox(
					pos,
					Vector2.one * cellSize * 0.9f,
					0f,
					obstacleMask
				);	
				
				grid[x, y] = new Node(pos, !blocked);
			
			}
		}
		
	}
	
	Node GetNode(Vector2 position)
	{
		Vector2 bottomLeft = (Vector2)transform.position - worldSize / 2f;
		
		int x = Mathf.FloorToInt((position.x - bottomLeft.x) / cellSize);
		int y = Mathf.FloorToInt((position.y - bottomLeft.y) / cellSize);
		
		if(x < 0 || y < 0 || x >= width || y >= height){
			return null;
		}
			return grid[x, y];
		
	}
	
	List<Node> GetNeighbours(Node node)
	{
		List<Node> neighbours = new List<Node>();
		int x = Mathf.RoundToInt((node.position.x - (transform.position.x - worldSize.x / 2f)) / cellSize - 0.5f);
		int y = Mathf.RoundToInt((node.position.y - (transform.position.y - worldSize.y / 2f)) / cellSize - 0.5f);
		
		int[,] directions = {
			{ 1, 0 }, { -1, 0 },
			{ 0, 1 }, { 0, -1 }
		};
		
		for (int i = 0; i < 4; i++)
		{
			int nx = x + directions[i, 0];
			int ny = y + directions[i, 1];
			
			if(nx >= 0 && ny >= 0 && nx < width && ny < height)
			{
				neighbours.Add(grid[nx, ny]);
			}
			
		}
		return neighbours;
		
	}
	
	public List<Vector2> FindPath(Vector2 start, Vector2 target)
	{
		
		if(grid == null || width <= 0 || height <= 0)
		{
			Debug.LogError("FindPath called before grid initialization!");
			return new List<Vector2>();
		}
		
		Node startNode = GetNode(start);
		Node targetNode = GetNode(target);
		
		if (startNode == null || !startNode.walkable)
		{
			startNode = GetClosestWalkableNode(start);
		}
		
		if(targetNode == null || !targetNode.walkable)
		{
			targetNode = GetClosestWalkableNode(target);
		}
		
		if(startNode == null || targetNode == null)
		{
			return new List<Vector2>();
		}
		
		
		/*Node startNode = GetNode(start);
		Node targetNode = GetNode(target);
		
		if(startNode == null || targetNode == null || !targetNode.walkable)
		{
			return new List<Vector2>();
		}*/
		
		List<Node> open = new List<Node> { startNode };
		HashSet<Node> closed = new HashSet<Node>();
		
		foreach (Node node in grid)
		{
			node.gCost = int.MaxValue;
			node.hCost = 0;
			node.parent = null;
		}
		
		startNode.gCost = 0;
		startNode.hCost = Distance(startNode, targetNode);
		
		while (open.Count > 0)
		{
			Node current = open[0];
			
			foreach (Node node in open)
			{
				if(node.fCost < current.fCost || (node.fCost == current.fCost && node.hCost < current.hCost))
				{
					current = node;
				}
			}
			
			if (current == targetNode)
			{
				return RetracePath(startNode, targetNode);
			}
			
			open.Remove(current);
			closed.Add(current);
			
			foreach (Node neighbour in GetNeighbours(current))
			{
				if(!neighbour.walkable || closed.Contains(neighbour))
				{
					continue;
				}
				
				int newCost = current.gCost + 10;
				
				if (newCost < neighbour.gCost)
				{
					neighbour.gCost = newCost;
					neighbour.hCost = Distance(neighbour, targetNode);
					neighbour.parent = current;
					
					if(!open.Contains(neighbour))
					{
						open.Add(neighbour);
					}
					
				}
			}
			
		}
		
		return new List<Vector2>();
	}
	
	int Distance(Node a, Node b)
	{
		return (Mathf.Abs(Mathf.RoundToInt((a.position.x - b.position.x) / cellSize))
		+ Mathf.Abs(Mathf.RoundToInt((a.position.y - b.position.y) / cellSize))) * 10;
		
	}
	
	List<Vector2> RetracePath(Node start, Node end)
	{
		List<Vector2> path = new List<Vector2>();
		Node current = end;
		
		while (current != start)
		{
			path.Add(current.position);
			current = current.parent;
			
			if (current == null)
			{
				return new List<Vector2>();
			}
		}
		
		path.Reverse();
		return path;
		
	}
	
	
	
Node GetClosestWalkableNode(Vector2 position)
{
    if (grid == null || width <= 0 || height <= 0)
    {
        Debug.LogError(
            "PathfindingGrid is not initialized! " + gameObject.name
        );
        return null;
    }

    Node closestNode = null;
    float closestDistance = float.MaxValue;

    for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < height; y++)
        {
            Node node = grid[x, y];

            if (node == null || !node.walkable)
                continue;

            float distance = (node.position - position).sqrMagnitude;

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestNode = node;
            }
        }
    }

    if (closestNode == null)
    {
        Debug.LogWarning(
            "No walkable nodes found in grid: " + gameObject.name
        );
    }

    return closestNode;
}

	
	/*Node GetClosestWalkableNode(Vector2 position)
	{
		Node closestNode = null;
		float closestDistance = float.MaxValue;
		
		foreach (Node node in grid)
		{
			if(!node.walkable)
			{
				continue;
			}
			
			float distance = (node.position - position).sqrMagnitude;
			
			if(distance < closestDistance)
			{
				closestDistance = distance;
				closestNode = node;
			}

		}
		
		return closestNode;
	}*/
	
	
void OnDrawGizmos()
{
    if (grid == null)
    {
        // Draw grid bounds before the grid is initialized.
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, worldSize);
        return;
    }

    for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < height; y++)
        {
            Node node = grid[x, y];

            // Green = walkable, Red = blocked by a wall.
            Gizmos.color = node.walkable
                ? new Color(0f, 1f, 0f, 0.25f)
                : new Color(1f, 0f, 0f, 0.6f);

            Gizmos.DrawCube(
                node.position,
                Vector3.one * cellSize * 0.9f
            );

            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(
                node.position,
                Vector3.one * cellSize
            );
        }
    }
}

	
	
}
