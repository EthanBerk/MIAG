using GunMods;
using UnityEditor;
using UnityEngine;


namespace Editor.GunMods
{
    [CustomPropertyDrawer(typeof(StatEffect))]
    public class pro : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
           
            
                property.FindPropertyRelative(nameof(StatEffect.Curve)).animationCurveValue = EditorGUILayout.CurveField(
                    property.FindPropertyRelative(nameof(StatEffect.Curve)).animationCurveValue,
                    Color.blue,
                    new Rect(0f, property.FindPropertyRelative(nameof(StatEffect.LowerBound)).floatValue, 1,
                        property.FindPropertyRelative(nameof(StatEffect.UpperBound)).floatValue -
                        property.FindPropertyRelative(nameof(StatEffect.LowerBound)).floatValue));
            
            
           
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}