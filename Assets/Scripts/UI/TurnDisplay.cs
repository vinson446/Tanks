using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TurnDisplay : MonoBehaviour
{
    [Header("Turn Stuff")]
    public float turnTextDuration;
    public float turnTextStartScale;
    public float turnTextEndScale;
    public float turnTextScaleDuration;
    Sequence turnSequenceScale;
    Sequence testSequence;

    [Header("Other References")]
    public TextMeshProUGUI turnText;

    TurnManager turnManager;

    // Start is called before the first frame update
    void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();

        turnText.transform.localScale = new Vector3(turnTextStartScale, turnTextStartScale, turnTextStartScale);

        UpdateTurn();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public void UpdateTurn()
    {
        StartCoroutine(UpdateDisplayTurnCoroutine());
    }

    void DisplayTurn()
    {
        if (TurnManager.allyTeamTurn)
        {
            turnText.text = "PLAYER TURN";
            turnText.color = Color.green;
        }
        else
        {
            turnText.text = "ENEMY TURN";
            turnText.color = Color.red;
        }

        turnSequenceScale = DOTween.Sequence();
        turnSequenceScale.Append(turnText.transform.DOScale(turnTextEndScale, turnTextScaleDuration));
    }

    void ClearDisplayTurn()
    {
        turnText.text = "";
        turnText.transform.localScale = new Vector3(turnTextStartScale, turnTextStartScale, turnTextStartScale);
    }

    IEnumerator UpdateDisplayTurnCoroutine()
    {
        // update turn on backend (restrict touch/movement and clear count for turn switching)
        if (TurnManager.allyUnitsFinished == 3)
        {
            TurnManager.allyTeamTurn = false;
            TurnManager.alliesCanMoveNow = false;

            foreach (PlayerMovement p in turnManager.playerTeam)
            {
                p.isSelected = false;
                p.hasMovedAlready = false;
                p.finishedTurn = false;
            }
        }
        else
        {
            TurnManager.allyTeamTurn = true;

            foreach (EnemyMovement e in turnManager.enemyTeam)
            {
                e.EndTurn(e);
            }
        }
        TurnManager.allyUnitsFinished = 0;
        TurnManager.enemyUnitsFinished = 0;

        DisplayTurn();

        yield return new WaitForSeconds(turnTextDuration);

        ClearDisplayTurn();
        
        // enable action again
        if (TurnManager.allyTeamTurn)
        {
            TurnManager.alliesCanMoveNow = true;
        }
        else
        {
            turnManager.enemyTeam[0].BeginTurn(turnManager.enemyTeam[0]);
        }
    }
}
