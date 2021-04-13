using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevel : MonoBehaviour
{
    public int Level { get; set; }
    public int CurrentExperience { get; set; }
    public int RequiredExperience { get { return Level * 25; } }
    public PlayerLevel playerLevel { get; set; }


    // Use this for initialization
    void Start()
    {
        CombatEvents.OnEnemyDeath += EnemyToExperience;
        Level = 1;    
    }

    public void EnemyToExperience(CharacterStats enemyStats)
    {
       GrantExperience(enemyStats.experience);
    }

    public void GrantExperience(int amount)
    {
        CurrentExperience += amount;
        while(CurrentExperience >= RequiredExperience)
        {
            CurrentExperience -= RequiredExperience;
            Level++;
        }
       UIEventHandler.PlayerLevelChanged();
    }




}
