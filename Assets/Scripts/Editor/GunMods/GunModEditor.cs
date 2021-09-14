using System;
using GunMods;
using Objects;
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
            var sprite = serializedObject.FindProperty(nameof(GunMod.LargeSprite));
            
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(sprite);
            if (EditorGUI.EndChangeCheck())
            {
                ((GunMod) target).attachmentArea = new Serializable2DArray<bool>(Mathf.RoundToInt(((Sprite) sprite.objectReferenceValue).textureRect.width + 2), Mathf.RoundToInt(((Sprite) sprite.objectReferenceValue).textureRect.height + 2));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}