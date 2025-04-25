using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ButtonExpansion))]
public class ButtonExpansionEditor : Editor {


    public override void OnInspectorGUI() {
        ButtonExpansion _target = target as ButtonExpansion;

        EditorGUI.BeginChangeCheck();
        var _upScale = EditorGUILayout.FloatField("UpScale", _target.UpScale);
        var _downScale = EditorGUILayout.FloatField("DownScale", _target.DownScale);

        var _lockPanel = EditorGUILayout.ObjectField("LockPanel", _target.LockPanel, typeof(GameObject), true);
        var _unLockPanel = EditorGUILayout.ObjectField("LockPanel", _target.UnLockPanel, typeof(GameObject), true);

        if (EditorGUI.EndChangeCheck()) {
            _target.UpScale = _upScale;
            _target.DownScale = _downScale;

            _target.LockPanel = _lockPanel as GameObject;
            _target.UnLockPanel = _unLockPanel as GameObject;
        }

        if(GUI.changed) EditorUtility.SetDirty(target);
    }
}