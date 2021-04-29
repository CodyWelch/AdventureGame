using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace DevionGames.AI
{
    public class UtilityAIEditor : EditorWindow
    {
		private UtilityAIInspector m_UtilityAIInspector;

		public static void ShowWindow()
		{

			UtilityAIEditor[] objArray = Resources.FindObjectsOfTypeAll<UtilityAIEditor>();
			UtilityAIEditor editor = (objArray.Length <= 0 ? ScriptableObject.CreateInstance<UtilityAIEditor>() : objArray[0]);

			editor.hideFlags = HideFlags.HideAndDontSave;
			editor.minSize = new Vector2(690, 300);
			editor.titleContent = new GUIContent("Utility AI");

			editor.Show();
		}

		private void OnEnable()
		{
			this.m_UtilityAIInspector = new UtilityAIInspector();
			this.m_UtilityAIInspector.OnEnable();
		}

		private void OnDisable()
		{
			this.m_UtilityAIInspector.OnDisable();
		}

		private void OnDestroy()
		{
			this.m_UtilityAIInspector.OnDestroy();
		}

		private void Update()
		{
			if (EditorWindow.mouseOverWindow == this)
				Repaint();
		}

		private void OnGUI()
		{
			this.m_UtilityAIInspector.OnGUI(position);
		}

	}
}