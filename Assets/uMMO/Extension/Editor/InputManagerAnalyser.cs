using UnityEngine;
using System.Collections;
using UnityEditor;

using SoftRare;

//based on http://answers.unity3d.com/questions/566736/get-list-of-axes.html
namespace SoftRare.Editor {
    public class InputManagerAnalyser {

        public static void ReadAxes() {
            var inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];

            SerializedObject obj = new SerializedObject(inputManager);

            SerializedProperty axisArray = obj.FindProperty("m_Axes");

            if (axisArray.arraySize == 0)
                Debug.Log("No Axes");

            InputAxes inputAxes = (InputAxes)Resources.Load("InputAxes");

            inputAxes.mouseMovementAxes.Clear();
            inputAxes.keyOrMouseButtonAxes.Clear();
            inputAxes.joystickAxes.Clear();

            for (int i = 0; i < axisArray.arraySize; ++i) {
                var axis = axisArray.GetArrayElementAtIndex(i);

                string name = axis.FindPropertyRelative("m_Name").stringValue;
                int axisValue = axis.FindPropertyRelative("axis").intValue;
                InputType inputType = (InputType)axis.FindPropertyRelative("type").intValue;

                if (inputType == InputType.MouseMovement) {
                    inputAxes.mouseMovementAxes.Add(name);
                } else if (inputType == InputType.KeyOrMouseButton) {
                    inputAxes.keyOrMouseButtonAxes.Add(name);
                } else if (inputType == InputType.JoystickAxis) {
                    inputAxes.joystickAxes.Add(name);
                }
            }
        }

        public enum InputType {
            KeyOrMouseButton,
            MouseMovement,
            JoystickAxis,
        };

    }
}