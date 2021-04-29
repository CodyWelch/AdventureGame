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

        public GameObject player;

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
    }
}



