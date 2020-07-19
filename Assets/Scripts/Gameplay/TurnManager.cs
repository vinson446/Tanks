using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    // string = tean, list = hold members in a team
    static Dictionary<string, List<TacticsMovement>> units = new Dictionary<string, List<TacticsMovement>>();
    static Queue<string> turnKey = new Queue<string>();
    static Queue<TacticsMovement> turnTeam = new Queue<TacticsMovement>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (turnTeam.Count == 0)
        {
            InitTeamTurnQueue();
        }
    }

    static void InitTeamTurnQueue()
    {
        // get first team
        List<TacticsMovement> teamList = units[turnKey.Peek()];

        foreach (TacticsMovement unit in teamList)
        {
            turnTeam.Enqueue(unit);
        }

        StartTurn();
    }

    public static void StartTurn()
    {
        if (turnTeam.Count > 0)
        {
            turnTeam.Peek().BeginTurn();
        }
    }

    public static void EndTurn()
    {
        TacticsMovement unit = turnTeam.Dequeue();
        unit.EndTurn();

        if (turnTeam.Count > 0)
        {
            StartTurn();
        }
        else
        {
            string team = turnKey.Dequeue();
            turnKey.Enqueue(team);
            InitTeamTurnQueue();
        }
    }

    public static void AddUnit(TacticsMovement unit)
    {
        List<TacticsMovement> list;

        if (!units.ContainsKey(unit.tag))
        {
            // create team
            list = new List<TacticsMovement>();
            units[unit.tag] = list;

            // add new team to turn queue
            if (!turnKey.Contains(unit.tag))
            {
                turnKey.Enqueue(unit.tag);
            }
        }
        else
        {
            list = units[unit.tag];
        }

        list.Add(unit);
    }

    public static void RemoveUnit(TacticsMovement unit)
    {
        List<TacticsMovement> list;

        list = units[unit.tag];

        list.Remove(unit);
    }
}
