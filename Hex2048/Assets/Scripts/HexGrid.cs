using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HexGridNamespace
{
    [Serializable]
    public class HexGrid
    {
        private Dictionary<Axial, HexTile> _tiles;

        private List<Axial> _keys;
        public int EmptyTilesCount => _tiles.Count(t => !t.Value.HasElement);
        
        public HexGrid(int N)
        {
            _tiles = new Dictionary<Axial, HexTile>();
            foreach (var c in GetHexShapedCoordinates(N))
            {
                _tiles.Add(c, new HexTile(c.Q, c.R));
            }
            
            _keys = new List<Axial>(_tiles.Keys);
            
            InputHandler.SwipedDirection -= StartMove;
            InputHandler.SwipedDirection += StartMove;
        }

        public bool HasTile(Axial c)
        {
            return _tiles.ContainsKey(c);
        }

        public bool HasElement(Axial c)
        {
            return HasTile(c) && _tiles[c].HasElement;
        }
        
        public HexTile GetTile(Axial c)
        {
            if (!HasTile(c)) return null;
            return _tiles[c];
        }
        
        public void PopulateRandom(int maxExcluded = 4)
        {
            var genCount = UnityEngine.Random.Range(1, maxExcluded);
            if (EmptyTilesCount < genCount)
            {
                genCount = EmptyTilesCount;
            }
            
            for (int i = 0; i < genCount; i++)
            {
                var tile = GetRandomEmptyTile();
                if (tile == null) return;
                tile.Element = new HexTileElement(tile);
            }
        }

        public Axial GetRandomTileCoordinate()
        {
            return _keys[Random.Range(0, _keys.Count)];
        }
        
        public HexTile GetRandomEmptyTile()
        {
            var sanity = 1000;
            var c = GetRandomTileCoordinate();
            while (!HasTile(c) || HasElement(c))
            {
                sanity--;
                if (sanity <= 0)
                {
                    Debug.LogWarning("Sanity check failed");
                    return null;
                }
                c = GetRandomTileCoordinate();
            }

            return GetTile(c);
        }
        
        private IEnumerable<Axial> GetHexShapedCoordinates(int N)
        {
            for (int q = -N; q <= N; q++)
            {
                for (int r = Mathf.Max(-N,-q-N); r <= Mathf.Min(N,-q+N); r++)
                {
                    yield return new Axial(q, r);
                }
            }
        }

        public IEnumerable<HexTile> GetAllTiles()
        {
            return _tiles.Values;
        }

        public IEnumerable<HexTile> GetTilesInLine(Axial start, Axial direction)
        {
            var current = start;
            while (HasTile(current))
            {
                yield return GetTile(current);
                current += direction;
            }
        }

        public IEnumerable<HexTile> GetTilesOnEdge(Axial direction)
        {
            foreach (var (key, value) in _tiles)
            {
                if (!HasTile(key + direction)) yield return value;
            }
        }
        
        public void StartMove(Axial direction)
        {
            MergeGridInDirection(direction, out var anyActDone);
            var validMove = anyActDone;
            while (anyActDone)
            {
                MergeGridInDirection(direction, out anyActDone);
                validMove |= anyActDone;
            }
            
            if (validMove)
            {
                PopulateRandom(3);
            }
        }

        private void MergeGridInDirection(Axial direction, out bool anyActDone)
        {
            anyActDone = false;
            foreach (var tile in GetTilesOnEdge(direction))
            {
                MergeLineInDirection(tile.Coordinate, direction, out var actDone);
                anyActDone |= actDone;
            }
        }
        
        private bool CanMergeTiles(HexTile stationaryTile, HexTile movedTile)
        {
            if (!stationaryTile.HasElement || !movedTile.HasElement) return false;
            return stationaryTile.Element.CanMerge(movedTile.Element);
        }
        
        private void MergeTiles(HexTile stationaryTile, HexTile movedTile)
        {
            stationaryTile.Element.Merge(movedTile.Element);
            movedTile.Element = null;
        }
        
        private void MoveTileElement(HexTile stationaryTile, HexTile movedTile)
        {
            movedTile.Element.MoveTo(stationaryTile);
        }
        
        private void MergeLineInDirection(Axial start, Axial direction, out bool anyActDone)
        {
            anyActDone = false;
            var line = GetTilesInLine(start, -direction).ToList();
            if (line.Count == 0) return;
            if (line.FirstOrDefault(t => t.HasElement) == null) return;

            for (int m = 0; m < line.Count; m++)
            {
                var currentTile = line[m];
                for (int i = m + 1; i < line.Count; i++)
                {
                    var nextTile = line[i];
                    if (!nextTile.HasElement) continue;

                    // Check if the tiles can merge
                    if (CanMergeTiles(currentTile, nextTile))
                    {
                        MergeTiles(currentTile, nextTile);
                        anyActDone = true;
                        m = -1; // Allow the merged tile to participate in another merge
                        break;
                    }
                    else if (!currentTile.HasElement) // Move tile if current is empty
                    {
                        MoveTileElement(currentTile, nextTile);
                        anyActDone = true;
                        m = -1; // Allow the moved tile to participate in another merge
                        break;
                    }
                }
            }
        }


        

    }
}