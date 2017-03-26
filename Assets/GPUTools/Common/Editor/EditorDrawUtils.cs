using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.GPUTools.Common.Editor
{
    public class EditorDrawUtils : MonoBehaviour
    {
        public static void ListObjectGUI<T>(string itemName, List<T> list) where T : Object
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label(itemName + "s", EditorStyles.boldLabel);

            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    GUILayout.BeginHorizontal();

                    list[i] = (T)EditorGUILayout.ObjectField(itemName, list[i], typeof(T), true);

                    if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.MaxWidth(20)))
                    {
                        list.RemoveAt(i);
                    }

                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.BeginHorizontal();

            GUILayout.Label(list == null || list.Count == 0 ? "Add " + itemName : "");
            if (GUILayout.Button("+", EditorStyles.miniButton, GUILayout.Width(20)))
            {
                if (list != null) list.Add(default(T));
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        public static void ListColorGUI(string itemName, List<Color> list)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label(itemName + "s", EditorStyles.boldLabel);

            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    GUILayout.BeginHorizontal();

                    list[i] = EditorGUILayout.ColorField(itemName, list[i]);

                    if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.MaxWidth(20)))
                    {
                        list.RemoveAt(i);
                    }

                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.BeginHorizontal();

            GUILayout.Label(list == null || list.Count == 0 ? "Add " + itemName : "");
            if (GUILayout.Button("+", EditorStyles.miniButton, GUILayout.Width(20)))
            {
                if (list != null) list.Add(new Color());
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }
    }
}
