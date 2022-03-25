using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WindowInfo))]
public class WindowInfoInspector : Editor {
    WindowInfo windowInfo;
    float minVal = 0f;
    float maxVal = 10.0f;
    bool animSet = true;

    private void OnEnable() {
        windowInfo = (WindowInfo)target;
    }

    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        EditorGUILayout.BeginVertical();

        //空两行
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        windowInfo.windowType = (WindowType)EditorGUILayout.EnumPopup("Window Type", windowInfo.windowType);
        EditorGUILayout.Space();

        animSet = EditorGUILayout.Foldout(animSet, "Window Anim", true);
        if (animSet) {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            windowInfo.openAnimType = (OpenAnimType)EditorGUILayout.EnumPopup("Open Anim Type", windowInfo.openAnimType);
            windowInfo.closeAnimType = (OpenAnimType)EditorGUILayout.EnumPopup("Close Anim Type", windowInfo.closeAnimType);
            if (windowInfo.openAnimType != OpenAnimType.None || windowInfo.closeAnimType != OpenAnimType.None) {
                windowInfo.animTime = EditorGUILayout.Slider("Anim Time", windowInfo.animTime, minVal, maxVal);
            }

            if (windowInfo.openAnimType == OpenAnimType.Position || windowInfo.closeAnimType == OpenAnimType.Position) {
                if (windowInfo.defaultPos == Vector3.zero && windowInfo.openPos == Vector3.zero) {
                    EditorGUILayout.HelpBox("位移动画需要填入位置信息！！", MessageType.Warning);
                }
                windowInfo.defaultPos = EditorGUILayout.Vector3Field("Default Pos", windowInfo.defaultPos);
                windowInfo.openPos = EditorGUILayout.Vector3Field("Open Pos", windowInfo.openPos);
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.Space();

        windowInfo.closeOnEmpty = EditorGUILayout.Toggle("Close On Empty", windowInfo.closeOnEmpty);
        windowInfo.mask = EditorGUILayout.Toggle("Mask", windowInfo.mask);
        windowInfo.group = EditorGUILayout.IntField("Group", windowInfo.group);


        EditorGUILayout.EndVertical();

        if (GUI.changed) {
            EditorUtility.SetDirty(windowInfo);
        }
    }
}
