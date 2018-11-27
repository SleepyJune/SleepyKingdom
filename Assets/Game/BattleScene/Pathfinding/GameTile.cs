using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BattlePathfinding
{
    public class GameTile
    {
        public Vector3Int position;
                
        public double gScore;
        public double fScore;

        public GameTile parent = null;

        public HashSet<GameTile> neighbours;
        public HashSet<BattleUnit> units;

        public GameTile(Vector3Int pos)
        {
            this.position = pos;

            units = new HashSet<BattleUnit>();
        }

        public double Distance(GameTile b)
        {
            return Vector3.Distance(position, b.position);
        }

        public void AddUnit(BattleUnit unit)
        {
            if (!units.Contains(unit))
            {
                units.Add(unit);
            }
        }

        public void DeleteUnit(BattleUnit unit)
        {
            units.Remove(unit);
        }
    }
}