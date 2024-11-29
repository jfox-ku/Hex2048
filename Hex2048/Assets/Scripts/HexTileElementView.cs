using System;
using DG.Tweening;
using UnityEngine;

namespace HexGridNamespace
{
    public class HexTileElementView : MonoBehaviour
    {
        public HexTileElement Element;
        public SpriteRenderer SpriteRenderer;

        private Tween _scaleTween;
        private Tween _moveTween;
        
        public void AssignElement(HexTileElement element)
        {
            Element = element;
            transform.position = Element.Tile.View.transform.position;
            _scaleTween = transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack, 1.2f);
            SpriteRenderer.color = GridRoot.Instance.Colors.GetColor(Element.Value);
            element.OnTileChanged += OnTargetTileChanged;
        }

        private void OnDestroy()
        {
            _scaleTween.Kill();
            _moveTween.Kill();
            Element.OnTileChanged -= OnTargetTileChanged;
        }

       
        private void OnTargetTileChanged(HexTile tile)
        {
            _moveTween = transform.DOMove(tile.View.transform.position,0.25f);
        }
    }
}