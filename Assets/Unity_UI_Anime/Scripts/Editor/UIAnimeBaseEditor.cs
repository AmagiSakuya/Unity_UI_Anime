using Sakuya.UnityUIAnime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Sakuya.UnityUIAnimeEditor
{
    [CustomEditor(typeof(UIAnimeBase), true)]
    public class UIAnimeBaseEditor : Editor
    {
        UIAnimeBase t;
        private void OnEnable()
        {
            t = (UIAnimeBase)target;
            EditorApplication.update += ProgressIEnumerator;
        }

        private void OnDisable()
        {
            if (!Application.isPlaying)
            {
                t.KillAnime();
            }
            EditorApplication.update -= ProgressIEnumerator;
        }

        void ProgressIEnumerator()
        {
            if (!Application.isPlaying)
            {
                EditorApplication.QueuePlayerLoopUpdate();
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.BeginVertical("GroupBox");
            GUILayout.BeginHorizontal();
            Color m_defaultColor = GUI.color;
            GUI.color = Color.green;
            if (GUILayout.Button("预览动画"))
            {
                t.KillAnime();
                t.DoAnime();
            }
            GUI.color = Color.red;
            if (GUILayout.Button("停止"))
            {
                t.KillAnime();
            }
            GUI.color = m_defaultColor;
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
    }
}