using System;
using UnityEngine;

namespace HexGridNamespace
{
    public class HexTileElement
    {
        public Action<HexTile> OnTileChanged;
        public Action<ElementValue> OnValueChanged;
        
        public ElementValue Value { get; private set; }
        public HexTile Tile { get; private set; }
        public HexTileElementView View { get; private set; }
        
        public HexTileElement(HexTile tile, ElementValue? val = null)
        {
            Tile = tile;
            View = GameObject.Instantiate(GridRoot.Instance.HexElementPrefab).GetComponent<HexTileElementView>();
            Value = val ?? ElementValueExtensions.GetRandom(3);
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
            OnValueChanged?.Invoke(Value);
        }
        
        public void MoveTo(HexTile tile)
        {
            Tile.Element = null;
            Tile = tile;
            Tile.Element = this;
            OnTileChanged?.Invoke(Tile);
        }

        public void Destroy()
        {
            View.SetToBeDestroyed();
        }
    }
}