using System;
using System.Drawing;
using System.Linq;
using GunMods;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

namespace Source.Scripts.UI
{
    public class GunModUIObject : MonoBehaviour, IDragHandler
    {
        private Image _image;
        private RectTransform _rectTransform;
        public GunMod GunMod { get; set; }
        
        public float snapRange;
        public LayerMask ModUiMask;
        public void Initialize(GunMod gunMod)
        {
            _image = gameObject.GetComponent<Image>();
            _rectTransform = gameObject.GetComponent<RectTransform>();
            GunMod = gunMod;
            _image.sprite = gunMod.LargeSprite;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta;
            var rect = _rectTransform.rect;
           var hit = Physics2D.OverlapBox(transform.position, new Vector2(rect.width + snapRange, rect.height + snapRange), 0, ModUiMask);
           if (hit)
           { 
               var attachmentRails = ((GunModUIObject)hit.GetComponent<GunModUIObject>()).GunMod.attachmentArea;
               foreach (var VARIABLE in attachmentRails.Where(rail => rail.AttachmentType == GunMod.gunModType))
               {
                   
               }
           }
        }
    }
}