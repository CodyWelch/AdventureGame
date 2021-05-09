using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myRPG
{

    public class EnemyManager : MonoBehaviour
    {
        #region Singleton
        public static EnemyManager instance;

        void Awake()
        {
            instance = this;
        }

        #endregion

        // Start is called before the first frame update
        public GameObject[] enemies;
        
        private void Start()
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");

            Vector3 newEnemypos = enemies[0].transform.position;
            newEnemypos.z += 10;

            CreateEnemy(0, newEnemypos);

        }

        public void CreateEnemy(int type, Vector3 pos)
        {
            switch (type)

            {
                case 0:
                    Instantiate(enemies[0], pos, Quaternion.identity);
                    break;
                case 1:
                    Instantiate(enemies[1], pos, Quaternion.identity);
                    break;

            }

        }

    }
}
