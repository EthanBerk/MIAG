using UnityEngine;

namespace Utils
{
    public static class EditorUtils
    {
        public static void clear(ref Texture2D texture2D)
        {
            var fillColorArray = new Color[texture2D.GetPixels().Length];
            for (var acc = 0; acc < fillColorArray.Length; acc++)
                fillColorArray[acc] = Color.clear;
            texture2D.SetPixels(fillColorArray);
            
            
        }
    }
}