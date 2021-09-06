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
            
        }

        public override void OnInspectorGUI()
        {
          
            base.OnInspectorGUI();
            serializedObject.Update();
            _gunModController = serializedObject.FindProperty(nameof(GunMod.GunModController));
            if (_gunModController.objectReferenceValue == null)
            {
                Debug.Log("wow");
                var gunModController = CreateInstance<GunModController>();
                AssetDatabase.AddObjectToAsset(gunModController, AssetDatabase.GetAssetPath(serializedObject.targetObject));
                AssetDatabase.SaveAssets();
                _gunModController.objectReferenceValue = gunModController;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}