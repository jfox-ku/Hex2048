using System;
using UnityEngine;

namespace HexGridNamespace
{
    public class HexTileElement
    {
        public Action<HexTile> OnTileChanged;
        
        public ElementValue Value { get; private set; }
        public HexTile Tile { get; private set; }
        public HexTileElementView View { get; private set; }
        
        public HexTileElement(HexTile tile)
        {
            Tile = tile;
            View = GameObject.Instantiate(GridRoot.Instance.HexElementPrefab).GetComponent<HexTileElementView>();
            Value = ElementValueExtensions.GetRandom(3);
            View.AssignElement(this);
        }

        public bool CanMerge(HexTileElement element)
        {
            return element.Value == Value;
        }
        
        public void Merge(HexTileElement element)
        {
            Value = Value.GetNext();
            element.Destroy();
            OnTileChanged?.Invoke(Tile);
        }

        public void Destroy()
        {
            GameObject.Destroy(View.gameObject);
        }
    }
}