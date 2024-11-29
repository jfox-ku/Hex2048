using System;
using UnityEngine;

namespace HexGridNamespace
{
    public class HexTileView : MonoBehaviour
    {
        public HexTile Tile;
        
        public Vector2 AxialToWorld => GridRoot.AxialToWorld(Tile.Coordinate);
        public Vector3 GridSizeToScale => Vector3.one * GridRoot.Size * 2f;
        
        public void AssignTile(HexTile tile)
        {
            Tile = tile;
            name = $"Tile {tile.Coordinate}";
            transform.localPosition = AxialToWorld;
            transform.localScale = GridSizeToScale;
        }
        
    }
}