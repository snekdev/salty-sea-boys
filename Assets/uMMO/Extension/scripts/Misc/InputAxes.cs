using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InputAxes : ScriptableObject {

    public static string[] mergedValues() {
        InputAxes inputAxes = (InputAxes)Resources.Load("InputAxes");
        return inputAxes.mouseMovementAxes.Concat(inputAxes.keyOrMouseButtonAxes.Concat(inputAxes.joystickAxes)).ToArray();
    }



    public List<string> mouseMovementAxes = new List<string>();
    public List<string> keyOrMouseButtonAxes = new List<string>();
    public List<string> joystickAxes = new List<string>();

}
