using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevionGames.CharacterSystem
{
    public static class CharacterSystemMenu
    {
        [MenuItem("Tools/Devion Games/Character System/Editor", false, 0)]
        private static void OpenEditor()
        {
            CharacterSystemEditor.ShowWindow();
        }

		[MenuItem("Tools/Devion Games/Character System/Create Character Manager", false, 1)]
		private static void CreateCharacterManager()
		{
			GameObject go = new GameObject("Character Manager");
			go.AddComponent<CharacterManager>();
			Selection.activeGameObject = go;
		}

		[MenuItem("Tools/Devion Dames/Character System/Create Character Manager", true)]
		static bool ValidateCreateCharacterManager()
		{
			return GameObject.FindObjectOfType<CharacterManager>() == null;
		}
	}
}