using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace myRPG
{

    [RequireComponent(typeof(CharacterStats))]
    public class Enemy : Interactable
    {
        PlayerManager playerManager;
        public CharacterStats myStats;



        private void Start()
        {
            playerManager = PlayerManager.instance;
            myStats = GetComponent<CharacterStats>();
            myStats.experience = 100;
        }

        public override void Interact()
        {
            base.Interact();
            CharacterCombat playerCombat = playerManager.player.GetComponent<CharacterCombat>();
            if (playerCombat != null)
            {
                playerCombat.Attack(myStats);
            }

            // attack the enemy
        }
        /*
          public NavMeshAgent agent;

          public bool isActive;
          public float agroRange;

       //   public GameObject player;

          public void Awake()
          {
              isActive = false;
              agroRange = 10.0f;
          }

          void Update()
          {


            if (agroRange >= Vector3.Distance(player.GetComponent<Transform>().position, this.GetComponent<Transform>().position))
               { isActive = true;
              }
              else
              {
                  isActive = false;
              }

              if (isActive)
              {
                  agent.SetDestination(player.GetComponent<Transform>().position);
              }*/
    }
}
