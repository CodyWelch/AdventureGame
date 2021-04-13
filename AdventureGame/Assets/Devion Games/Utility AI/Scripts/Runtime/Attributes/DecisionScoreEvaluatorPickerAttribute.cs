using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.AI
{
	public class DecisionScoreEvaluatorPickerAttribute : PropertyAttribute
	{
		public bool utility;

		public DecisionScoreEvaluatorPickerAttribute() : this(false) { }

		public DecisionScoreEvaluatorPickerAttribute(bool utility)
		{
			this.utility = utility;
		}
	}
}