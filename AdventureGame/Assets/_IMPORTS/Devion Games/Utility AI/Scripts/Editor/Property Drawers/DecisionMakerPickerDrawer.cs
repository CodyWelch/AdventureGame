using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace DevionGames.AI
{
	[CustomPropertyDrawer(typeof(DecisionMakerPickerAttribute), true)]
	public class DecisionMakerPickerDrawer : PropertyDrawer
	{

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			DecisionMaker current = property.GetValue() as DecisionMaker;
			position = EditorGUI.PrefixLabel(position, label);
			DoSelection(position, property, label, current);
			EditorGUI.EndProperty();
		}

		protected virtual void DoSelection(Rect buttonRect, SerializedProperty property, GUIContent label, DecisionMaker current)
		{

			GUIStyle buttonStyle = EditorStyles.objectField;
			GUIContent buttonContent = new GUIContent(current != null ? current.Name : "Null");
			if (GUI.Button(buttonRect, buttonContent, buttonStyle))
			{
				ObjectPickerWindow.ShowWindow(buttonRect, typeof(UtilityDatabase), BuildSelectableObjects(),
					(UnityEngine.Object obj) => {
						property.serializedObject.Update();
						property.objectReferenceValue = obj;
						property.serializedObject.ApplyModifiedProperties();
					},
					() => {
						UtilityDatabase db = EditorTools.CreateAsset<UtilityDatabase>(true);
					});
			}
		}

		protected virtual List<DecisionMaker> GetItems(UtilityDatabase database)
		{
			System.Type type = fieldInfo.FieldType;
			if (typeof(IList).IsAssignableFrom(fieldInfo.FieldType))
			{
				type = Utility.GetElementType(fieldInfo.FieldType);
			}
			return database.decisionMakers.Where(x => type.IsAssignableFrom(x.GetType())).ToList();
		}

		protected Dictionary<UnityEngine.Object, List<UnityEngine.Object>> BuildSelectableObjects()
		{
			Dictionary<UnityEngine.Object, List<UnityEngine.Object>> selectableObjects = new Dictionary<UnityEngine.Object, List<UnityEngine.Object>>();

			string[] guids = AssetDatabase.FindAssets("t:UtilityDatabase");
			for (int i = 0; i < guids.Length; i++)
			{
				string path = AssetDatabase.GUIDToAssetPath(guids[i]);
				UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(UtilityDatabase));
				List<UnityEngine.Object> items = GetItems(obj as UtilityDatabase).Cast<UnityEngine.Object>().ToList();
				for (int j = 0; j < items.Count; j++)
				{
					items[j].name = (items[j] as INameable).Name;
				}
				selectableObjects.Add(obj, items);
			}
			return selectableObjects;
		}
	}
}