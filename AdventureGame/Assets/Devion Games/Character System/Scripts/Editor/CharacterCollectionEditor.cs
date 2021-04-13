using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace DevionGames.CharacterSystem
{
	public class CharacterCollectionEditor : ScriptableObjectCollectionEditor<Character>
	{

		public override string ToolbarName
		{
			get
			{
				return "Characters";
			}
		}


		public CharacterCollectionEditor(UnityEngine.Object target, List<Character> items, List<string> searchFilters) : base(target, items)
		{
		}
	}
}