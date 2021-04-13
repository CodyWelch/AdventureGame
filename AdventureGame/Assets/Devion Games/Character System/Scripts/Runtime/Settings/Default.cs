using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace DevionGames.CharacterSystem.Configuration
{
    [System.Serializable]
    public class Default : Settings
    {
        public override string Name
        {
            get
            {
                return "Default";
            }
        }

        public string selectCharacterScene = "Select Character";
        public string createCharacterScene = "Create Character";
        public string playScene = "Main Scene";

        public bool debugMessages = true;
    }
}