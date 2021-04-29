using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DevionGames.CharacterSystem.Configuration
{
    [CustomEditor(typeof(SavingLoading))]
    public class SavingLoadingInspector : Editor
    {
        private SerializedProperty m_Script;
        private SerializedProperty m_AccountKey;
        private SerializedProperty m_Provider;

        protected virtual void OnEnable()
        {
            this.m_Script = serializedObject.FindProperty("m_Script");
            this.m_AccountKey = serializedObject.FindProperty("accountKey");
            this.m_Provider = serializedObject.FindProperty("provider");
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(this.m_Script);
            EditorGUI.EndDisabledGroup();

            serializedObject.Update();
            EditorGUILayout.PropertyField(m_AccountKey);
            EditorGUILayout.PropertyField(m_Provider);
            GUILayout.Space(2f);
            EditorTools.Seperator();

            string account = PlayerPrefs.GetString(m_AccountKey.stringValue);
            string data = PlayerPrefs.GetString(account);
            if (!string.IsNullOrEmpty(data))
            {
              
                bool state = EditorPrefs.GetBool("CharacterSavedData", false);
                bool foldout = EditorGUILayout.Foldout(state, account, true);
                Rect rect = GUILayoutUtility.GetLastRect();
                if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && rect.Contains(Event.current.mousePosition))
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Delete"), false, delegate () {
                        PlayerPrefs.DeleteKey(account);
                    });
                    menu.ShowAsContext();
                }

                if (foldout != state)
                {
                    EditorPrefs.SetBool("CharacterSavedData", foldout);
                }

                if (foldout)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(16f);
                    GUILayout.BeginVertical();

                    GUILayout.Label(data, EditorStyles.wordWrappedLabel);

                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}