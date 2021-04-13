using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.CharacterSystem.Configuration
{
    [System.Serializable]
    public class SavingLoading : Settings
    {
        public override string Name
        {
            get
            {
                return "Saving & Loading";
            }
        }

        public string accountKey = "Account";
        public SavingProvider provider = SavingProvider.PlayerPrefs;

        public enum SavingProvider
        {
            PlayerPrefs,
        }
    }
}