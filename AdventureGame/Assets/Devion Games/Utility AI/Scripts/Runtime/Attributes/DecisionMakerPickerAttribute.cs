using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.AI
{
	public class DecisionMakerPickerAttribute:PropertyAttribute
	{
		public bool utility;

		public DecisionMakerPickerAttribute() : this(false) { }

		public DecisionMakerPickerAttribute(bool utility)
		{
			this.utility = utility;
		}
	}
}