using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsMovement : MonoBehaviour
{
    // init
    GameObject[] tiles;
    float halfHeight = 0;

    [Header("Tank Stats")]
    public bool moving = false;
    public int move = 5;
    public float jumpHeight = 2;
    public float moveSpeed;

    Tile currentTile;
    List<Tile> selectableTiles = new List<Tile>();
    Stack<Tile> path = new Stack<Tile>();

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        halfHeight = GetComponent<Collider>().bounds.extents.y;
    }

    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;

        // 1 because of tile height
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

    public void ComputeAdjacencyList()
    {
        // if tiles can be created or destroyed, refind tiles
        // tiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(jumpHeight);
        }
    }

    // BFS- breadth first search to find selectable tiles
    public void FindSelectableTiles()
    {
        ComputeAdjacencyList();
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

            if (t.distance < move)
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

        Tile next = tile;
        while (next != null)
        {
            path.Push(next);
            next = next.parent;
        }
    }
}
