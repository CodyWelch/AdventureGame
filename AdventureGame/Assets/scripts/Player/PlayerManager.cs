using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace myRPG
{

    //[RequireComponent]
    public class PlayerManager : MonoBehaviour
    {
        #region Singleton
        public static PlayerManager instance;

        void Awake()
        {
            instance = this;
        }

        #endregion

        public GameObject[] players;

        public CharacterStats characterStats;
        public int currentHealth;
        public int maxHealth;
        public PlayerLevel PlayerLevel { get; set; }

        private void Start()
        {
            PlayerLevel = GetComponent<PlayerLevel>();
            this.currentHealth = this.maxHealth;
            //        characterStats = new CharacterStats(10, 10, 10);
        }

        public void KillPlayer()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Update()
        {
            if (Input.GetKeyDown("1"))
            {
                SetPlayerActive(0);
            }
            if (Input.GetKeyDown("2"))
            {
                SetPlayerActive(1);
            }
            if (Input.GetKeyDown("3"))
            {
                SetPlayerActive(2);
            }
        }

        public void SetPlayerActive(int playerIndex)
        {
            for(int i=0; i<players.Length; i++)
            {
                if (i == playerIndex)
                {
                    Debug.Log("Set " + i + " active");
                    players[i].GetComponent<PlayerController>().SetMainPlayer(true,players[playerIndex]);
                }
                else
                {
                    players[i].GetComponent<PlayerController>().SetMainPlayer(false,players[playerIndex]);
                }
            }
        }
    }
}



