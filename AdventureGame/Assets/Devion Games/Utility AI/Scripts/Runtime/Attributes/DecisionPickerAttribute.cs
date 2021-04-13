using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.AI
{
	public class DecisionPickerAttribute : PropertyAttribute
	{
		public bool utility;

		public DecisionPickerAttribute() : this(false) { }

		public DecisionPickerAttribute(bool utility)
		{
			this.utility = utility;
		}
	}
}