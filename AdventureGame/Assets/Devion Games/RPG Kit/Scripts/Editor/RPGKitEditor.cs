using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DevionGames.InventorySystem;
using DevionGames.AI;
using DevionGames.QuestSystem;
using DevionGames.CharacterSystem;
using DevionGames.StatSystem;

namespace DevionGames.RPGKit
{
    public class RPGKitEditor : EditorWindow
    {
		public int m_SystemIndex = 0;
		public GUIContent[] m_SystemContent;
		private GUISkin m_Skin;

		private InventorySystemInspector m_InventorySystemInspector;
		private UtilityAIInspector m_UtilityAIInspector;
		private QuestSystemInspector m_QuestSystemInspector;
		private CharacterSystemInspector m_CharacterSystemInspector;
		private StatSystemInspector m_StatSystemInspector;

		public static void ShowWindow()
		{
			RPGKitEditor[] objArray = Resources.FindObjectsOfTypeAll<RPGKitEditor>();
			RPGKitEditor editor = (objArray.Length <= 0 ? ScriptableObject.CreateInstance<RPGKitEditor>() : objArray[0]);

			editor.hideFlags = HideFlags.HideAndDontSave;
			editor.minSize = new Vector2(690, 300);
			editor.titleContent = new GUIContent("RPG Kit");
			editor.Show();
		}

        private void OnEnable()
        {
			this.m_Skin = Resources.Load<GUISkin>("EditorSkin");
			this.m_SystemContent = new GUIContent[5];
			this.m_SystemContent[0] = new GUIContent(Resources.Load<Texture>("Item"),"Inventory System");
			this.m_SystemContent[1] = new GUIContent((Texture)EditorGUIUtility.LoadRequired("d_Animator Icon"), "Utility AI");
			this.m_SystemContent[2] = new GUIContent(Resources.Load<Texture>("Quest"), "Quest System");
			this.m_SystemContent[3] = new GUIContent((Texture)EditorGUIUtility.LoadRequired("d_NavMeshAgent Icon"), "Character System");
			this.m_SystemContent[4] = new GUIContent(Resources.Load<Texture>("Stats"), "Stat System");

			this.m_InventorySystemInspector = new InventorySystemInspector();
			this.m_InventorySystemInspector.OnEnable();

			this.m_UtilityAIInspector = new UtilityAIInspector();
			this.m_UtilityAIInspector.OnEnable();

			this.m_QuestSystemInspector = new QuestSystemInspector();
			this.m_QuestSystemInspector.OnEnable();

			this.m_CharacterSystemInspector = new CharacterSystemInspector();
			this.m_CharacterSystemInspector.OnEnable();

			this.m_StatSystemInspector = new StatSystemInspector();
			this.m_StatSystemInspector.OnEnable();

		}

        private void OnDisable()
        {
			this.m_InventorySystemInspector.OnDisable();
			this.m_UtilityAIInspector.OnDisable();
			this.m_QuestSystemInspector.OnDisable();
			this.m_CharacterSystemInspector.OnDisable();
			this.m_StatSystemInspector.OnDisable();
        }

        private void OnDestroy()
        {
			this.m_InventorySystemInspector.OnDestroy();
			this.m_UtilityAIInspector.OnDestroy();
			this.m_QuestSystemInspector.OnDestroy();
			this.m_CharacterSystemInspector.OnDestroy();
			this.m_StatSystemInspector.OnDestroy();
        }

        private void Update()
        {
			if (EditorWindow.mouseOverWindow == this)
				Repaint();
        }

        private void OnGUI()
        {
			Color color = GUI.backgroundColor;
			GUI.backgroundColor = new Color(0.17647f, 0.17647f, 0.17647f,1);
			GUILayout.BeginArea(new Rect(0f, 0f, 50f, position.height), (GUIStyle)"WhiteBackground");
			GUI.backgroundColor = color;

		
			for (int i = 0; i < this.m_SystemContent.Length; i++) {
				GUIStyle style = this.m_SystemIndex == i ? this.m_Skin.FindStyle("roundbuttonactive") : this.m_Skin.FindStyle("roundbutton") ;
				if (GUILayout.Button(this.m_SystemContent[i],style,GUILayout.Width(40f), GUILayout.Height(40f))) {
					this.m_SystemIndex = i;
				}
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(50f, 0f, position.width - 50f, position.height));
			Rect rect = new Rect(0f, 0f, position.width - 50f, position.height);
			switch (this.m_SystemIndex) {
				case 0:
					this.m_InventorySystemInspector.OnGUI(rect);
					break;
				case 1:
					this.m_UtilityAIInspector.OnGUI(rect);
					break;
				case 2:
					this.m_QuestSystemInspector.OnGUI(rect);
					break;
				case 3:
					this.m_CharacterSystemInspector.OnGUI(rect);
					break;
				case 4:
					this.m_StatSystemInspector.OnGUI(rect);
					break;

			}
			GUILayout.EndArea();

		}
    }
}