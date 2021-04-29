using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myRPG
{
    public class UIEventHandler : MonoBehaviour
    {
        public delegate void ItemEventHandler(Item item);
        public static event ItemEventHandler OnItemAddedToInventory;

        public static void PlayerLevelChanged()
        {

        }
    }
}
