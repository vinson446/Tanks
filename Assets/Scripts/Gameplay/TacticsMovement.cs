using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsMovement : MonoBehaviour
{
    // init
    GameObject[] tiles;
    float halfHeight = 0;

    public bool isPlayer;
    public bool myTurn = false;
    public bool moving = false;
    public int moveSpaces = 5;
    public float moveSpeed;

    Tile currentTile;
    List<Tile> selectableTiles = new List<Tile>();
    Stack<Tile> path = new Stack<Tile>();

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    // A*
    public Tile actualTargetTile;

    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        halfHeight = GetComponent<Collider>().bounds.extents.y;

        TurnManager.AddUnit(this);
    }

    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;

        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
    }

    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.current = true;
    }

    public void ComputeAdjacencyList(Tile target)
    {
        // if tiles can be created or destroyed, refind tiles
        // tiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(1, target);
        }
    }

    // BFS- breadth first search to find selectable tiles
    public void FindSelectableTiles()
    {
        ComputeAdjacencyList(null);
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;
        // leave currentTile's parent as null

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            selectableTiles.Add(t);
            t.selectable = true;

            if (t.distance < moveSpaces)
            {
                // add all a tile's neighbors to the queue
                foreach (Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;

                        // make sure each tile is only processed once
                        tile.visited = true;

                        // keeps track of how far we are from start
                        tile.distance = 1 + t.distance;

                        // tile enters the process
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }

    public void MoveToTile(Tile tile)
    {
        path.Clear();

        tile.target = true;
        moving = true;

        // end location
        Tile next = tile;

        // when next is null, we reached the start tile
        while (next != null)
        {
            // push everything to stack in reverse order so we can walk through it
            path.Push(next);

            // go from end location to start location
            next = next.parent;
        }
    }

    public void Move()
    {
        if (path.Count > 0)
        {
            Tile t = path.Peek();

            // calculate unit's position on top of the target tile
            Vector3 target = t.transform.position;
            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

            if (Vector3.Distance(transform.position, target) >= 0.05f)
            {
                CalculateHeading(target);
                SetHorizontalVelocity();

                // for tank i have to make heading negative for some reason
                // locomotion
                transform.forward = -heading;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                transform.position = target;
                path.Pop();
            }
        }
        else
        {
            RemoveSelectableTiles();
            moving = false;

            TurnManager.EndTurn();
        }
    }

    protected void RemoveSelectableTiles()
    {
        if (currentTile != null)
        {
            currentTile.current = false;
            currentTile = null;
        }

        foreach (Tile tile in selectableTiles)
        {
            tile.Reset();
        }

        selectableTiles.Clear();
    }

    void CalculateHeading(Vector3 target)
    {
        heading = (target - transform.position).normalized;  
    }

    void SetHorizontalVelocity()
    {
        velocity = heading * moveSpeed;
    }

    protected Tile FindLowestF(List<Tile> list)
    {
        Tile lowest = list[0];

        foreach (Tile t in list)
        {
            if (t.f < lowest.f)
            {
                lowest = t;
            }
        }

        list.Remove(lowest);

        return lowest;
    }

    // stand next to target tile AND check movement range based on enemy's move spaces
    protected Tile FindEndTile(Tile t)
    {
        Stack<Tile> tempPath = new Stack<Tile>();

        // created path from destination tile's parent to start tile
        Tile next = t.parent;
        while (next != null)
        {
            tempPath.Push(next);

            next = next.parent;
        }

        if (tempPath.Count <= moveSpaces)
        {
            return t.parent;
        }

        // out of movement range
        Tile endTile = null;
        for (int i = 0; i <= moveSpaces; i++)
        {
            endTile = tempPath.Pop();
        }

        return endTile;
    }

    // for enemy ai
    protected void FindPath(Tile target)
    {
        ComputeAdjacencyList(target);
        GetCurrentTile();

        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(currentTile);
        // leave currentTile parent == null

        currentTile.h = Vector3.Distance(currentTile.transform.position, target.transform.position);
        currentTile.f = currentTile.h;

        while (openList.Count > 0)
        {
            Tile t = FindLowestF(openList);

            closedList.Add(t);

            if (t == target)
            {
                actualTargetTile = FindEndTile(t);
                MoveToTile(actualTargetTile);
                return;
            }

            foreach (Tile tile in t.adjacencyList)
            {
                // do nothing, already processed
                if (closedList.Contains(tile))
                {
                    
                }
                // found two paths to the same tile, need to see which path is faster by comparing g scores
                else if (openList.Contains(tile))
                {
                    float tempG = t.g + Vector3.Distance(tile.transform.position, transform.position);

                    if (tempG < tile.g)
                    {
                        tile.parent = t;

                        tile.g = tempG;
                        tile.f = tile.g + tile.h;
                    }
                }
                // never processed tile, add tile to open list
                else
                {
                    tile.parent = t;

                    // distance from the beginning
                    tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                    // distance to the end
                    tile.h = Vector3.Distance(tile.transform.position, target.transform.position);
                    tile.f = tile.g + tile.h;

                    openList.Add(tile);
                }
            }
        }
    }

    public void BeginTurn()
    {
        myTurn = true;
    }

    public void EndTurn()
    {
        myTurn = false;
    }
}
