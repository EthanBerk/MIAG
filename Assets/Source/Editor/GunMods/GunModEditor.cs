using System;
using System.Collections.Generic;
using GunMods;
using Objects;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Editor.GunMods
{
    [CustomEditor(typeof(GunMod))]
    public class GunModEditor : UnityEditor.Editor
    {
        private SerializedProperty _gunModController;


        private void OnEnable()
        {
            
            serializedObject.Update();
            _gunModController = serializedObject.FindProperty(nameof(GunMod.gunModController));
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

            if (GUILayout.Button("OpenAttachmentEditor"))
            {
                var window = (GunModEditorWindow) EditorWindow.GetWindow(typeof(GunModEditorWindow), false);
                window.GunMod = target as GunMod;
                window.Show();
            }

            var sprites = serializedObject.FindProperty(nameof(GunMod.sprites));
            
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(sprites);
            if (EditorGUI.EndChangeCheck())
            {
                if (sprites.arraySize == 1 || sprites.arraySize > 2) throw new InvalidHenryIsABitchException("no no no 2 sprites are you trying to break my code");
                var sprite0Bigger = ((Sprite) sprites.GetArrayElementAtIndex(0).objectReferenceValue).textureRect.width >
                                  ((Sprite) sprites.GetArrayElementAtIndex(1).objectReferenceValue).textureRect.width;
                var largeSprite = sprite0Bigger
                    ? ( sprites.GetArrayElementAtIndex(0))
                    : ( sprites.GetArrayElementAtIndex(1));
                var smallSprite = !sprite0Bigger
                    ? ( sprites.GetArrayElementAtIndex(0))
                    : ( sprites.GetArrayElementAtIndex(1));
                if(((Sprite) largeSprite.objectReferenceValue).textureRect.width % 3 != 0) throw new InvalidHenryIsABitchException("You go back right now and make the large sprite divisible by 3");
                serializedObject.FindProperty(nameof(GunMod.LargeSprite)).objectReferenceValue = largeSprite.objectReferenceValue;
                serializedObject.FindProperty(nameof(GunMod.SmallSprite)).objectReferenceValue = smallSprite.objectReferenceValue;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}