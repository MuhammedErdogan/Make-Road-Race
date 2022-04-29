/**
 * Copyright (c) 2021-present Compactive Game Studio. All rights reserved.
 * 'CGameStudio' can not be copied and/or distributed without the express permission of Compactive Game Studio.
 */

using UnityEngine;
using UnityEditor;

namespace CGameStudio
{
    [CustomEditor(typeof(CameraController))]
    public class ObjectBuilderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            CameraController cameraController = (CameraController)target;
            if (GUILayout.Button("Set a new fixed view"))
            {
                cameraController.SetNewFixedView();
            }
        }
    }
}