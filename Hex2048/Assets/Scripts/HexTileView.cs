using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

namespace HexGridNamespace
{
    public class HexTileView : MonoBehaviour
    {
        public HexTile Tile;

        public bool ShowCoordinates;
        
        [SerializeField]
        private TextMeshProUGUI _q;
        [SerializeField]
        private TextMeshProUGUI _r;
        
        public Vector2 AxialToWorld => GridRoot.AxialToWorld(Tile.Coordinate);
        public Vector3 GridSizeToScale => Vector3.one * GridRoot.Size * 2f;
        
        public void AssignTile(HexTile tile)
        {
            Tile = tile;
            name = $"Tile {tile.Coordinate}";
            transform.localPosition = AxialToWorld;
            transform.localScale = GridSizeToScale;
            
            if (ShowCoordinates)
            {
                _q.text = tile.Coordinate.Q.ToString();
                _r.text = tile.Coordinate.R.ToString();
            }
            else
            {
                _q.enabled = false;
                _r.enabled = false;
            }
        }

        public void DebugBounce(float delay)
        {
            transform.DOScale(GridSizeToScale, 0.25f).From(Vector2.zero).SetDelay(delay);
        }

        private void OnDrawGizmos()
        {
           
        }
    }
}