using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BattlePathfinding
{
    public class PathNode
    {
        public Vector3Int position;

        public float startTime;
        public float endTime;

        public PathNode(Vector3Int pos, float start, float end)
        {
            this.position = pos;
            this.startTime = start;
            this.endTime = end;
        }

        public Vector3 GetRealCoord(float nodeSize)
        {
            return new Vector3(position.x * nodeSize, position.y * nodeSize);
        }
    }
}