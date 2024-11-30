using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace HexGridNamespace
{
    public class HexTileElementView : MonoBehaviour
    {
        public HexTileElement Element;
        public SpriteRenderer SpriteRenderer;
        public TextMeshProUGUI Text;

        private Tween _scaleTween;
        private Tween _moveTween;
        public bool DestroyFlag;
        
        
        public void AssignElement(HexTileElement element)
        {
            Element = element;
            transform.position = Element.Tile.View.transform.position;
            _scaleTween = transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack, 1.2f).From(Vector2.zero);
            SpriteRenderer.color = GridRoot.Instance.Colors.GetColor(Element.Value);
            Text.text = Element.Value.GetName();
            element.OnTileChanged += OnTargetTileChanged;
            element.OnValueChanged += OnValueChanged;
        }

        private void OnValueChanged(ElementValue obj)
        {
            SpriteRenderer.color = GridRoot.Instance.Colors.GetColor(obj);
            Text.text = obj.GetName();
        }
        
        public void SetToBeDestroyed()
        {
            if (_moveTween.IsActive())
            {
                DestroyFlag = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void CheckDestroy()
        {
            if (DestroyFlag)
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            _scaleTween.Kill();
            _moveTween.Kill();
            Element.OnTileChanged -= OnTargetTileChanged;
            Element.OnValueChanged += OnValueChanged;
        }
        
        private void OnTargetTileChanged(HexTile tile)
        {
            _moveTween.Kill();
            _moveTween = transform.DOMove(tile.View.transform.position,0.25f).OnComplete(CheckDestroy);
        }
    }
}