using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Game/Tiles/Road Tile")]
public class RoadTile : GameTileBase
{
    //public int numCopies = 1;

    public Sprite[] sprites;

    public override void RefreshTile(Vector3Int location, ITilemap tilemap)
    {
        var directions = HexVectorExtensions.hexDirections;
        var parity = location.y & 1;

        for (int i = 0; i < 6; i++)
        {
            var dir = new Vector3Int(directions[parity][i, 0], directions[parity][i, 1], 0);
            var pos = new Vector3Int(location.x + directions[parity][i, 0], location.y + directions[parity][i, 1], 0);

            if (HasRoadTile(tilemap, pos))
            {
                tilemap.RefreshTile(pos);
            }
        }
    }

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        int finalMask = 0;
        int blockMask = GetMaskFromTile(tilemap, location);
        
        var directions = HexVectorExtensions.hexDirections;
        var parity = location.y & 1;

        for (int i = 0; i < 6; i++)
        {
            var dir = new Vector3Int(directions[parity][i, 0], directions[parity][i, 1], 0);
            var pos = new Vector3Int(location.x + directions[parity][i, 0], location.y + directions[parity][i, 1], 0);

            if(HasRoadTile(tilemap, pos))
            {
                var otherBlockMask = GetMaskFromTile(tilemap, pos);
                                
                if((blockMask & GetMask(i)) == 0 && (GetReverseMask(pos, dir) & otherBlockMask) == 0)
                {
                    finalMask = finalMask | GetMask(i);
                }
            }
        }
        
        tileData.sprite = this.sprite;
        tileData.color = this.color;
        tileData.transform = this.transform;
        tileData.gameObject = this.gameObject;
        tileData.flags = this.flags;

        //int randCopy = Random.Range(1, numCopies + 1);
        int index = finalMask - 1;
        if (index >= 0 && index < sprites.Length)
        {
            tileData.sprite = sprites[index];
        }
        else
        {
            Debug.LogWarning("Not enough sprites");
        }
    }

    private int GetReverseMask(Vector3Int position, Vector3Int dir)
    {
        var directions = HexVectorExtensions.hexDirections;
        var parity = position.y & 1;

        for (int i = 0; i < 6; i++)
        {
            var checkDir = new Vector3Int(directions[parity][i, 0], directions[parity][i, 1], 0);

            if(checkDir == new Vector3Int(-dir.x, -dir.y, dir.z))
            {
                return GetMask(i);
            }
        }

        return 0;
    }

    private bool HasRoadTile(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;
    }

    private int GetMaskFromTile(ITilemap tilemap, Vector3Int position)
    {
        var matrix = tilemap.GetTransformMatrix(position);
        var mask = matrix.lossyScale.z - 1;

        //var mask = color.a - .937f;

        return (int)mask;
    }

    private int GetMask(int i)
    {
        switch (i)
        {
            case 0: //right
                return 8;
            case 1: //down
                return 4;
            case 2: //left down
                return 2;
            case 3: //left
                return 1;
            case 4: //left up
                return 32;
            case 5: //up
                return 16;
            default:
                return 0;
        }
    }
}