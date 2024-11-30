using UnityEngine;

namespace HexGridNamespace
{
    public class HexTile
    {
        public Axial Coordinate;
        public HexTileView View;

        private HexTileElement _element;
        public HexTileElement Element
        {
            get => _element;
            set => _element = value;
        }
        
        public bool HasElement => Element != null;
        
        public HexTile(int q, int r)
        {
            Coordinate = new Axial(q, r);
            View = GameObject.Instantiate(GridRoot.Instance.HexPrefab).GetComponent<HexTileView>();
            View.transform.SetParent(GridRoot.Instance.transform);
            View.AssignTile(this);
        }
        
    }
}