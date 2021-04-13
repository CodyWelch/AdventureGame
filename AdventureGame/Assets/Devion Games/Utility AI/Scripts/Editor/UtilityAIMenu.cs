using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevionGames.AI
{
    public static class UtilityAIMenu
    {
        [MenuItem("Tools/Devion Games/Utility AI/Editor", false, 0)]
        private static void OpenEditor()
        {
            UtilityAIEditor.ShowWindow();
        }
    }
}