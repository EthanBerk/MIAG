using System;
using UnityEngine;


namespace GunMods
{
    
    
    [Serializable]
    public class GunModAttachmentLine
    {
        public Vector2 Start{ get; }
        public int Length{ get; }
        public bool Up{ get; }
        public Rect Rect { get; }
        

        public GunModAttachmentLine(Vector2 start, int length, bool up)
        {
            Start = start;
            Length = length;
            Up = up;
            var pos = Math.Sign(length) == 1;
            if (up)
                Rect = new Rect(start.x, pos? start.y: start.y + Length, 1, Math.Abs(length));
            else
                Rect = new Rect(!pos? start.x + length: start.x, start.y, Math.Abs(length), 1);
        }
        
    }

    [Serializable]
    public class GunModAttachmentRail
    {
        public GunModAttachmentLine largeSpriteLine, smallSpriteLine;

        public GunModAttachmentRail(GunModAttachmentLine largeSpriteLine, GunModAttachmentLine smallSpriteLine)
        {
            this.largeSpriteLine = largeSpriteLine;
            this.smallSpriteLine = smallSpriteLine;
        }
    }
    
}