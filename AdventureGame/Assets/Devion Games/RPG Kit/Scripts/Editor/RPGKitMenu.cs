using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevionGames.RPGKit
{
    public static class RPGKitMenu
    {
        [MenuItem("Tools/Devion Games/RPG Kit/Editor", false, 0)]
        private static void OpenItemEditor()
        {
            RPGKitEditor.ShowWindow();
        }
    }
}