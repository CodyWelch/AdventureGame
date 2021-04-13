using UnityEngine;
using UnityEditor;


namespace DevionGames.CharacterSystem
{
	public class CharacterSystemEditor : EditorWindow
	{
		private CharacterSystemInspector m_CharacterSystemInspector;

		public static void ShowWindow()
		{

			CharacterSystemEditor[] objArray = Resources.FindObjectsOfTypeAll<CharacterSystemEditor>();
			CharacterSystemEditor editor = (objArray.Length <= 0 ? ScriptableObject.CreateInstance<CharacterSystemEditor>() : objArray[0]);

			editor.hideFlags = HideFlags.HideAndDontSave;
			editor.minSize = new Vector2(690, 300);
			editor.titleContent = new GUIContent("Character System");

			editor.Show();
		}

		private void OnEnable()
		{
			this.m_CharacterSystemInspector = new CharacterSystemInspector();
			this.m_CharacterSystemInspector.OnEnable();
		}

		private void OnDisable()
		{
			this.m_CharacterSystemInspector.OnDisable();
		}

		private void OnDestroy()
		{
			this.m_CharacterSystemInspector.OnDestroy();
		}

		private void Update()
		{
			if (EditorWindow.mouseOverWindow == this)
				Repaint();
		}

		private void OnGUI()
		{
			this.m_CharacterSystemInspector.OnGUI(position);
		}
	}
}