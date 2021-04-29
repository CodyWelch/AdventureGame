using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DevionGames.CharacterSystem
{
	[System.Serializable]
	public class CharacterDatabase : ScriptableObject
	{
		public List<Character> items = new List<Character>();
		public List<Configuration.Settings> settings = new List<Configuration.Settings>();

	}
}