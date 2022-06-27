using System;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UIElements;


namespace GunMods
{
    
    
    [Serializable]
    public class GunModAttachmentLine
    {
        public Vector2 Start{ get; }
        public int Length{ get; }
        public bool Up{ get; }
        public Rect Rect { get; }


        public GunModAttachmentLine()
        {
            Rect = new Rect();
        }
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
    public class GunModPlacement
    {
        public GunMod GunMod { get; set; }
        public Double Position { get; set; }

        public GunModPlacement(GunMod mod, Double position)
        {
            GunMod = mod;
            Position = position;
        }
    }

    [Serializable]
    public class GunModAttachmentRail
    {
        public GunModAttachmentLine LargeSpriteLine { get; set; } = new GunModAttachmentLine();
        public bool IsEmpty { get; set; }
        public GunModAttachmentLine SmallSpriteLine { get; set; }  = new GunModAttachmentLine();

        public GunModType AttachmentType { get; set; }

        public List<GunModPlacement> GunMods = new List<GunModPlacement>();
        

        public GunModAttachmentRail()
        {
            IsEmpty = true; 
        }

        public GunModAttachmentRail(GunModAttachmentLine largeSpriteLine, GunModAttachmentLine smallSpriteLine, GunModType attachmentType)
        {
            LargeSpriteLine = largeSpriteLine;
            SmallSpriteLine = smallSpriteLine;
            AttachmentType = attachmentType;
            IsEmpty = false;
        }
    }
    
    
    

}