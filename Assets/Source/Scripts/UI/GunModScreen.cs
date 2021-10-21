using System;
using GunMods;
using Source.Scripts.Player;
using UnityEngine;

namespace Source.Scripts.UI
{
    public class GunModScreen : MonoBehaviour
    {
        private GameObject _player;
        private GameObject _canvas;
        private GunController _playerGun;
        private InventoryManger _playerInv;
        public Vector2 sizeInvBox;
        public Vector2 originInvBox;
        
        public GameObject gunModUiObjectPrefab;
        
        

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _canvas = gameObject.GetComponentInParent<Transform>().gameObject;
            
            _playerInv = _player.GetComponent<InventoryManger>();
            _playerGun = _player.GetComponent<GunController>();
            float largestHeightOnRow = 0;
            var currentPos = new Vector2();
            foreach (var gunMod in _playerInv.gunMods)
            {
                var modObject = Instantiate(gunModUiObjectPrefab, _canvas.transform, true);
                var rectTransform = modObject.GetComponent<RectTransform>();
                var rect = rectTransform.rect;
                var boxOffset = new Vector2(rect.width / 2, rect.height / 2);
                if (currentPos.x + rectTransform.rect.width > sizeInvBox.x)
                {
                    currentPos = new Vector2(0, currentPos.y + largestHeightOnRow);
                    largestHeightOnRow = 0;
                }
                rectTransform.localPosition = originInvBox + currentPos + boxOffset;
                currentPos = new Vector2(currentPos.x + rect.width, currentPos.y);
                
                
                largestHeightOnRow = rect.height > largestHeightOnRow
                    ? rect.height
                    : largestHeightOnRow;
                
                rectTransform.localScale = Vector3.one;
                rectTransform.sizeDelta = gunMod.LargeSprite.textureRect.size;
                modObject.GetComponent<GunModUIObject>().Initialize(gunMod);
            }
        }
    }
}