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



            
            EditorGUI.PropertyField(new Rect(position.x, position.y/4, position.width, position.height/4), property.FindPropertyRelative(nameof(StatEffect.Stat)));
            EditorGUI.PropertyField(new Rect(position.x, position.y/2, position.width, position.height/4), property.FindPropertyRelative(nameof(StatEffect.LowerBound)));
            EditorGUI.PropertyField(new Rect(position.x, position.y/4 * 3, position.width, position.height/4), property.FindPropertyRelative(nameof(StatEffect.UpperBound)));
            
            property.FindPropertyRelative(nameof(StatEffect.Curve)).animationCurveValue = EditorGUI.CurveField(new Rect(position.x, position.y, position.width, position.height/4), property.FindPropertyRelative(nameof(StatEffect.Curve)).animationCurveValue,
            Color.blue,
            new Rect(0f, property.FindPropertyRelative(nameof(StatEffect.LowerBound)).floatValue, 1,
                property.FindPropertyRelative(nameof(StatEffect.UpperBound)).floatValue -
                property.FindPropertyRelative(nameof(StatEffect.LowerBound)).floatValue));



            



        }

        

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 300f;
        }
    }
}