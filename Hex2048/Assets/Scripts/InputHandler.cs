using System;
using System.Collections;
using System.Collections.Generic;
using HexGridNamespace;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float MinDistanceToSwipe = 50f;
    
    public static event Action<Axial> SwipedDirection;
    public static event Action<Axial> SwipePotentialDirection;
    
    public Material SwipeMaterial;
    public float Speed;
    private string BlendKey = "_OverlayBlend";
    private string XKey = "_OverlayTextureScrollXSpeed";
    private string YKey = "_OverlayTextureScrollYSpeed";
    
    private Vector3 _startPos;
    private Vector3 _draggedPos;
    private Vector2 _direction =>  ((Vector2)(_draggedPos - _startPos)).normalized;

    private void Awake()
    {
        SetSwipePotentialVisual(Vector2.zero);
    }
    
    private Vector2 _lastDir;
    private void SetSwipePotentialVisual(Vector2 dir)
    {
        if (dir == Vector2.zero)
        {
            SwipeMaterial.SetFloat(BlendKey,0f);
        }
        else
        {
            dir = dir.SnapTo60Degrees();
            if (dir != _lastDir)
            {
                _lastDir = dir;
                SwipeMaterial.SetFloat(BlendKey,1f);
                SwipeMaterial.SetFloat(XKey,-dir.x * Speed);
                SwipeMaterial.SetFloat(YKey,-dir.y * Speed);
            }
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startPos = Input.mousePosition;
            return;
        }

        if (Input.GetMouseButton(0))
        {
            var dist = Vector3.Distance(_startPos, Input.mousePosition);
            if (dist > MinDistanceToSwipe)
            {
                _draggedPos = Input.mousePosition;
                SwipePotentialDirection?.Invoke(_direction.ToAxialDirection());
                SetSwipePotentialVisual(_direction);
            }
            else
            {
                SetSwipePotentialVisual(Vector2.zero);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            var dist = Vector3.Distance(_startPos, Input.mousePosition);
            if (dist > MinDistanceToSwipe)
            {
                _draggedPos = Input.mousePosition;
                SwipedDirection?.Invoke(_direction.ToAxialDirection());
            }
            SetSwipePotentialVisual(Vector2.zero);
        }
        
    }
}
