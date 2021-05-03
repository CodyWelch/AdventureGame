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
            int maxPlayers = 3;
            CharacterCombat[] playerCombats = new CharacterCombat[maxPlayers];
            int i = 0;
            foreach(GameObject player in playerManager.players)
            {
                playerCombats[i] = player.GetComponent<CharacterCombat>();
                i++;
            }

            // single player
            //CharacterCombat playerCombat = playerManager.player.GetComponent<CharacterCombat>();
            foreach (CharacterCombat playerCombat in playerCombats)
            {
                if (playerCombat != null)
                {
                    playerCombat.Attack(myStats);
                }
            }
            // single player
           /* if (playerCombat != null)
            {
                playerCombat.Attack(myStats);
            }*/

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
