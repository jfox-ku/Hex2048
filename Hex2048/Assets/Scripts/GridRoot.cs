using System;
using System.Collections;
using System.Collections.Generic;
using HexGridNamespace;
using NaughtyAttributes;
using UnityEngine;

public class GridRoot : MonoBehaviour
{
    public static GridRoot Instance;
    public int InitialPopulatedTiles = 5;
    public ElementColorData Colors;
    public float size = 1f;
    public static float Size => Instance.size;
    
    public static Vector2 BasisVectorQ => new Vector2(3f / 2f, -Mathf.Sqrt(3) / 2f);
    public static Vector2 BasisVectorR => new Vector2(0, -Mathf.Sqrt(3));

    public static Vector2 AxialToWorld(Axial c)
    {
        return (BasisVectorQ * c.Q + BasisVectorR * c.R) * Size;
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
        Grid.GetTile(Axial.Zero).Element = new HexTileElement(Grid.GetTile(Axial.Zero),ElementValue.Two);
        Grid.GetTile(Axial.Up).Element = new HexTileElement(Grid.GetTile(Axial.Up),ElementValue.Two);
    }
    
    public void Generate()
    {
        Grid = new HexGrid(N);
    }
    
    public Axial TestDirection;
    [Button()]
    public void TestMove()
    {
        Grid.StartMove(TestDirection);
    }
        
    [Button()]
    public void TestEdges()
    {
        var delay = 0f;
        foreach (var hexTile in Grid.GetTilesOnEdge(TestDirection))
        {
            hexTile.View.DebugBounce(delay);
            delay += 0.1f;
        }
    }
    [Button()]
    public void TestLine()
    {
        var delay = 0f;
        foreach (var hexTile in Grid.GetTilesInLine(new Axial(0, 0), TestDirection))
        {
            hexTile.View.DebugBounce(delay);
            delay += 0.1f;
        }
    }
    
    [Button()]
    public void TestHasElement()
    {
        foreach (var tile in Grid.GetAllTiles())
        {
            if(!tile.HasElement) continue;
            tile.View.DebugBounce(0);
        }
    }


    private void OnDrawGizmos()
    {
        
    }
}
