using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    [Header("Stats")]
    public float currentHP;
    public float maxHP;

    [Header("UI")]
    public HealthBar hpBar;

    // references
    PlayerMovement playerMovement;
    TurnManager turnManager;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;

        playerMovement = GetComponent<PlayerMovement>();
        turnManager = FindObjectOfType<TurnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        hpBar.ShowHealthChange(currentHP / maxHP);

        if (currentHP <= 0)
        {
            turnManager.playerTeam.RemoveAt(playerMovement.tankNum);
            Destroy(gameObject);
        }
    }
}
