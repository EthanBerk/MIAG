using System;
using GunMods;
using UnityEditor;
using UnityEngine;

namespace Editor.GunMods
{
    [CustomEditor(typeof(GunMod))]
    public class GunModEditor : UnityEditor.Editor
    {
        private SerializedProperty GunModController;
        private void OnEnable()
        {
            GunModController = serializedObject.FindProperty(nameof(GunMod.GunModController));
        }

        public override void OnInspectorGUI()
        {
            
            base.OnInspectorGUI();
            serializedObject.Update();
            

            if(GunModController.objectReferenceValue != null)
                
            switch (serializedObject.FindProperty(nameof(GunMod.gunModType)).enumValueIndex)
            {
                case 0:
                    if (GunModController.objectReferenceValue.GetType() != typeof(BaseGunModController))
                        GunModController.objectReferenceValue = CreateInstance<BaseGunModController>();
                    break;
                case 1:
                    if (GunModController.objectReferenceValue.GetType() != typeof(GunModController))
                        GunModController.objectReferenceValue = CreateInstance<GunModController>();
                    
                    break;
            }
            Debug.Log(GunModController.objectReferenceValue != null);
            if(GunModController.objectReferenceValue != null) 
                EditorUtility.SetDirty(GunModController.objectReferenceValue);
            serializedObject.ApplyModifiedProperties();
            
            
            
            
        }
    }
}