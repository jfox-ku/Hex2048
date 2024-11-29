using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexGridNamespace
{
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
        
        private IEnumerable<HexTile> GetTilesInLine(Axial start, Axial direction)
        {
            var current = start;
            while (HasTile(current))
            {
                yield return GetTile(current);
                current += direction;
            }
        }

        private IEnumerable<HexTile> GetTilesOnEdge(Axial direction)
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
            stationaryTile.Element = movedTile.Element;
            movedTile.Element = null;
        }
        
        private void MergeLineInDirection(Axial start, Axial direction, out bool anyActDone)
        {
            anyActDone = false;
            var line = GetTilesInLine(start, -direction).ToList();
            line.Reverse();
            if (line.Count == 0) return;
            if (line.FirstOrDefault(t => t.HasElement) == null) return;

            for (var m = 0; m < line.Count; m++)
            {
                var mergeTile = line[m];
                for (int i = m + 1; i < line.Count; i++)
                {
                    var nextTile = line[i];
                    if(!nextTile.HasElement) continue;
                    if (CanMergeTiles(mergeTile,nextTile))
                    {
                        MergeTiles(mergeTile, nextTile);
                        m--;
                        anyActDone = true;
                        break;
                    }
                    else if (mergeTile.HasElement)
                    {
                        var nextToMerge = line[m + 1];
                        if(nextToMerge.Coordinate == nextTile.Coordinate) continue;
                        MoveTileElement(nextToMerge, nextTile);
                        anyActDone = true;
                    }
                    else
                    {
                        MoveTileElement(mergeTile,nextTile);
                        m--;
                        anyActDone = true;
                        break;
                    }
                }
            }
        }
        
    }
}