using System;
using System.Drawing;
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
        private GunMod _gunMod;

        public void Initialize(GunMod gunMod)
        {
            _image = gameObject.GetComponent<Image>();
            _rectTransform = gameObject.GetComponent<RectTransform>();
            _gunMod = gunMod;
            _image.sprite = gunMod.LargeSprite;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta;
        }
    }
}