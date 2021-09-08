using System;
using GunMods;
using UnityEditor;
using UnityEngine;

namespace Editor.GunMods
{
    [CustomEditor(typeof(GunMod))]
    public class GunModEditor : UnityEditor.Editor
    {
        private SerializedProperty _gunModController;

        private void OnEnable()
        {
            
            serializedObject.Update();
            _gunModController = serializedObject.FindProperty(nameof(GunMod.GunModController));
            // if (_gunModController.objectReferenceValue == null)
            // {
            //     Debug.Log("wow");
            //     var gunModController = CreateInstance<GunModController>();
            //     EditorUtility.SetDirty(gunModController);
            //     AssetDatabase.AddObjectToAsset(gunModController,
            //         AssetDatabase.GetAssetPath(serializedObject.targetObject));
            //     AssetDatabase.SaveAssets();
            //     _gunModController.objectReferenceValue = gunModController;
            // }
            
            // var gunModController = CreateInstance<GunModController>();
            // AssetDatabase.AddObjectToAsset(gunModController,
            //     AssetDatabase.GetAssetPath(serializedObject.targetObject));
            // AssetDatabase.SaveAssets();
            // _gunModController.objectReferenceValue = gunModController;

            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();
            if (GUILayout.Button("GenerateController"))
            {
                if (_gunModController.objectReferenceValue == null)
                {
                    var gunModController = CreateInstance<GunModController>();
                    EditorUtility.SetDirty(gunModController);
                    AssetDatabase.AddObjectToAsset(gunModController,
                        AssetDatabase.GetAssetPath(serializedObject.targetObject));
                    AssetDatabase.SaveAssets();
                    _gunModController.objectReferenceValue = gunModController;
                }
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}