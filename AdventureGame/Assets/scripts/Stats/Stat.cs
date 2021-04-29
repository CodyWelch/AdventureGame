using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myRPG
{
    [System.Serializable]
    public class Stat
    {
        [SerializeField]
        private int baseValue;

        private List<int> modifiers = new List<int>();

        private void Awake()
        {
            baseValue = 0;
        }

        public int GetValue()
        {
            int finalValue = baseValue;
            modifiers.ForEach(x => finalValue += x);
            return finalValue;
        }

        public void AddModifier(int modifier)
        {
            if (modifier != 0)
                modifiers.Add(modifier);
        }

        public void RemoveModifier(int modifier)
        {
            if (modifier != 0)
                modifiers.Remove(modifier);
        }
    }
}


