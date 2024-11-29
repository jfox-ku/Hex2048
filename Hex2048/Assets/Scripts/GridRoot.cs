using System;
using System.Collections;
using System.Collections.Generic;
using HexGridNamespace;
using UnityEngine;

public class GridRoot : MonoBehaviour
{
    public static GridRoot Instance;
    public int InitialPopulatedTiles = 5;
    public ElementColorData Colors;
    public float size = 1f;
    public static float Size => Instance.size;

    public static Vector2 AxialToWorld(Axial c)
    {
        return new Vector2(
            Size * (3f/2f * c.Q),
            Size * (Mathf.Sqrt(3)/2f * c.Q + Mathf.Sqrt(3f) * c.R)
        );
    }

    public GameObject HexPrefab;
    public GameObject HexElementPrefab;
    public HexGrid Grid;
    
    public int N = 5;

    private void Awake()
    {
        Instance = this;
    }

    
    void Start()
    {
        Generate();
        Grid.PopulateRandom();
    }
    
    public void Generate()
    {
        Grid = new HexGrid(N);
    }

    
    
}
