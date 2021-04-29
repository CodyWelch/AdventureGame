using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevionGames.AI
{
    [System.Serializable]
    public class UtilityAIInspector 
    {
        private UtilityDatabase m_Database;
        private List<ICollectionEditor> m_ChildEditors;

        [SerializeField]
        private int toolbarIndex;

        private string[] toolbarNames
        {
            get
            {
                string[] items = new string[m_ChildEditors.Count];
                for (int i = 0; i < m_ChildEditors.Count; i++)
                {
                    items[i] = m_ChildEditors[i].ToolbarName;
                }
                return items;
            }
        }

        public void OnEnable()
        {
            this.m_Database = AssetDatabase.LoadAssetAtPath<UtilityDatabase>(EditorPrefs.GetString("UtilityDatabasePath"));
            if (this.m_Database == null)
            {
                string[] guids = AssetDatabase.FindAssets("t:UtilityDatabase");
                if (guids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    this.m_Database = AssetDatabase.LoadAssetAtPath<UtilityDatabase>(path);
                }
            }
            this.toolbarIndex = EditorPrefs.GetInt("UtilityAIInspector.toolbarIndex",0);
            ResetChildEditors();
        }

        public void OnDisable()
        {
            if (this.m_Database != null)
            {
                EditorPrefs.SetString("UtilityDatabasePath", AssetDatabase.GetAssetPath(this.m_Database));
            }
            if (m_ChildEditors != null)
            {
                for (int i = 0; i < m_ChildEditors.Count; i++)
                {
                    m_ChildEditors[i].OnDisable();
                }
            }
            EditorPrefs.SetInt("UtilityAIInspector.toolbarIndex", this.toolbarIndex);
        }


        public void OnDestroy()
        {
            if (m_ChildEditors != null)
            {
                for (int i = 0; i < m_ChildEditors.Count; i++)
                {
                    m_ChildEditors[i].OnDestroy();
                }
            }
        }

        public void OnGUI(Rect position)
        {

            DoToolbar();

            if (m_ChildEditors != null)
            {
                m_ChildEditors[toolbarIndex].OnGUI(new Rect(0f, 30f, position.width, position.height - 30f));
            }
        }

        private void DoToolbar()
        {
            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            SelectDatabaseButton();

            if (this.m_ChildEditors != null)
                toolbarIndex = GUILayout.Toolbar(toolbarIndex, toolbarNames, GUILayout.MinWidth(200));

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void SelectDatabaseButton()
        {
            GUIStyle buttonStyle = EditorStyles.objectField;
            GUIContent buttonContent = new GUIContent(this.m_Database != null ? this.m_Database.name : "Null");
            Rect buttonRect = GUILayoutUtility.GetRect(180f, 18f);
            if (GUI.Button(buttonRect, buttonContent, buttonStyle))
            {
                ObjectPickerWindow.ShowWindow(buttonRect, typeof(UtilityDatabase),
                    (UnityEngine.Object obj) => {
                        this.m_Database = obj as UtilityDatabase;
                        ResetChildEditors();
                    },
                    () => {
                        UtilityDatabase db = EditorTools.CreateAsset<UtilityDatabase>(true);
                        if (db != null)
                        {
                            this.m_Database = db;
                            ResetChildEditors();
                        }
                    });
            }
        }

        private void ResetChildEditors()
        {

            if (this.m_Database != null)
            {
                this.m_ChildEditors = new List<ICollectionEditor>();

                this.m_ChildEditors.Add(new ScriptableObjectCollectionEditor<DecisionMaker>(this.m_Database, this.m_Database.decisionMakers, false));
                this.m_ChildEditors.Add(new ScriptableObjectCollectionEditor<DecisionScoreEvaluator>(this.m_Database, this.m_Database.decisionScoreEvaluators, false));
                this.m_ChildEditors.Add(new ScriptableObjectCollectionEditor<Decision>(this.m_Database, this.m_Database.decisions, false));

                for (int i = 0; i < this.m_ChildEditors.Count; i++)
                {
                    this.m_ChildEditors[i].OnEnable();
                }
            }
        }
    }
}