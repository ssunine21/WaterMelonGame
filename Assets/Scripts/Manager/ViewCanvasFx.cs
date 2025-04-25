using System.Collections.Generic;
using UnityEngine;

namespace DataInfo.Manager
{
    public class ViewCanvasFx : ViewCanvas
    {
        [SerializeField] private ItemAcquireFx _itemPrefab;

        private Queue<ItemAcquireFx> _itemFxs;

        public void ItemExplosion(Vector2 from, Vector2 to, float range, Sprite sprite, int count) 
        {
            for (var i = 0; i < count; ++i)
            {
                var item = GetPrefab();
                item.GoodImage.sprite = sprite;
                item.Explosion(from, to, range, () =>
                {
                    item.gameObject.SetActive(false);
                    _itemFxs.Enqueue(item);
                });
            }
        }

        private ItemAcquireFx GetPrefab()
        {
            _itemFxs ??= new Queue<ItemAcquireFx>();
            if (_itemFxs.Count > 0)
            {
                var item = _itemFxs.Dequeue();
                item.gameObject.SetActive(true);
                return item;
            }
            
            return Instantiate(_itemPrefab, transform);
        }
    }
}