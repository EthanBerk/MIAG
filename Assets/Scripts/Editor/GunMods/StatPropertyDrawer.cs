using GunMods;
using UnityEditor;
using UnityEngine;


namespace Editor.GunMods
{
    [CustomPropertyDrawer(typeof(StatEffect))]
    public class StatPropertyDrawer : PropertyDrawer
    {
        public float TotalHeight = 0;

        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            TotalHeight = 0;

            var stat = property.FindPropertyRelative(nameof(StatEffect.Stat));
            var statHeight = EditorGUI.GetPropertyHeight(stat);
            var statRect = new Rect(position.x, position.y + TotalHeight, position.width, statHeight);
            EditorGUI.PropertyField(statRect, stat);
            TotalHeight += statHeight;

            var upperBound = property.FindPropertyRelative(nameof(StatEffect.UpperBound));
            var upperBoundHeight = EditorGUI.GetPropertyHeight(upperBound);
            var upperBoundRect = new Rect(position.x, position.y + TotalHeight, position.width, upperBoundHeight);
            EditorGUI.PropertyField(upperBoundRect, upperBound);
            TotalHeight += upperBoundHeight;

            var lowerBound = property.FindPropertyRelative(nameof(StatEffect.LowerBound));
            var lowerBoundHeight = EditorGUI.GetPropertyHeight(lowerBound);
            var lowerBoundRect = new Rect(position.x, position.y + TotalHeight, position.width, lowerBoundHeight);
            EditorGUI.PropertyField(lowerBoundRect, lowerBound);
            TotalHeight += lowerBoundHeight;


            var curve = property.FindPropertyRelative(nameof(StatEffect.Curve));
            var curveHeight = EditorGUI.GetPropertyHeight(curve);
            var curveRect = new Rect(position.x, position.y + TotalHeight, position.width, curveHeight);
            TotalHeight += curveHeight;

            var ranges = new Rect(0f, lowerBound.floatValue, 1, upperBound.floatValue - lowerBound.floatValue);

            curve.animationCurveValue = EditorGUI.CurveField(curveRect, curve.animationCurveValue, Color.blue, ranges);
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (TotalHeight == 0)
            {
                return 72;
            }
            return TotalHeight;
        }
    }
}