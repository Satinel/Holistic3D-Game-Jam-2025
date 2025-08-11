using UnityEngine;

public class MultiTileGrid : MonoBehaviour
{
    [SerializeField] GameObject _multiTileWhite, _multiTileBlack;

    [SerializeField] int _gridSizeX, _gridSizeY, _tileSize = 1;

    void Start()
    {
        SpawnMultiTiles();
    }

    void SpawnMultiTiles()
    {
        GameObject nextTile = _multiTileWhite;

        for(int y = 0; y < _gridSizeY; y++)
        {
            for(int x = 0; x < _gridSizeX; x++)
            {
                if(nextTile == _multiTileWhite)
                {
                    nextTile = _multiTileBlack;
                }
                else
                {
                    nextTile = _multiTileWhite;
                }
                GameObject tile = Instantiate(nextTile, new Vector2(x * _tileSize, y * _tileSize), Quaternion.identity, transform);
                tile.name = $"Tile {x}, {y}";
            }
        }
    }
}
