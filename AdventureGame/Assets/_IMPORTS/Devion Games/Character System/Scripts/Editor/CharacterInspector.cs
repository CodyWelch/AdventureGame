using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DevionGames.CharacterSystem
{
	[CustomEditor(typeof(Character), true)]
	public class CharacterInspector : Editor
	{
		private ReorderableList m_Properties;

		protected virtual void OnEnable()
		{
			if (target == null) return;
			m_Properties = new ReorderableList(serializedObject, serializedObject.FindProperty("m_Properties"), true, true, true, true);
			m_Properties.elementHeight = (EditorGUIUtility.singleLineHeight + 4f) * 2;
			m_Properties.drawHeaderCallback = (Rect rect) =>
			{
				EditorGUI.LabelField(rect, "Custom Properties");
			};

			m_Properties.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
			{
				var element = m_Properties.serializedProperty.GetArrayElementAtIndex(index);
				rect.y += 2;
				rect.height = EditorGUIUtility.singleLineHeight;
				EditorGUI.PropertyField(rect, element.FindPropertyRelative("name"));
				rect.y += 2f;
				rect.y += EditorGUIUtility.singleLineHeight + 2;
				float width = rect.width;
				rect.width = EditorGUIUtility.labelWidth - 2f;
				SerializedProperty typeIndex = element.FindPropertyRelative("typeIndex");
				typeIndex.intValue = EditorGUI.Popup(rect, typeIndex.intValue, ObjectProperty.DisplayNames);
				rect.x += rect.width + 2f;
				rect.width = width - EditorGUIUtility.labelWidth;
				EditorGUI.PropertyField(rect, element.FindPropertyRelative(ObjectProperty.GetPropertyName(ObjectProperty.SupportedTypes[typeIndex.intValue])), GUIContent.none);
			};
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			DrawPropertiesExcluding(serializedObject, "m_Properties");
			EditorGUILayout.Space();
			m_Properties.DoLayoutList();
			serializedObject.ApplyModifiedProperties();
		}
	}
}