using GunMods;
using UnityEditor;

namespace Editor.GunMods
{
    [CustomEditor(typeof(GunMod))]
    public class GunModEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            switch (serializedObject.FindProperty(nameof(GunMod.gunModType)).enumValueIndex)
            {
                case 1:
                    (target as GunMod)?.SetGunModController(new GunModController());
                    break;
                case 2:
                    (target as GunMod)?.SetGunModController(new BaseGunModController());
                    break;
            }
            
            
            
        }
    }
}