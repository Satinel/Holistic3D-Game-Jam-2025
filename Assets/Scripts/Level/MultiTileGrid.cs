using UnityEngine;

// [ExecuteAlways]
public class MultiTileGrid : MonoBehaviour
{
    [SerializeField] GameObject _tilePrefab;

    [SerializeField] int _gridSizeX, _gridSizeY;
    [SerializeField] float _tileSize = 0.25f;

    [SerializeField] Color[] _colors;

    // void Awake()
    // {
    //     SpawnMultiTiles();
    // }

    void SpawnMultiTiles()
    {
        GameObject nextTile = _tilePrefab;

        for(int y = 0; y < _gridSizeY; y++)
        {
            for(int x = 0; x < _gridSizeX; x++)
            {
                GameObject tile = Instantiate(nextTile, new Vector2(x * _tileSize, y * _tileSize), Quaternion.identity, transform);
                tile.name = $"Tile {x}, {y}";
                SetTileColor(tile.GetComponent<SpriteRenderer>(), y);
            }
        }
    }

    void SetTileColor(SpriteRenderer tileRenderer, int row)
    {
        tileRenderer.color = row switch
        {
            < 8 => _colors[0],
            < 20 => _colors[1],
            < 30 => _colors[2],
            < 42 => _colors[3],
            < 50 => _colors[4],
            < 64 => _colors[5],
            _ => _colors[5],
        };

        int randomNumber = Random.Range(0, 4);

        switch(randomNumber)
        {
            case 0:
                tileRenderer.flipX = true;
                break;
            case 1:
                tileRenderer.flipY = true;
                break;
            case 2:
                tileRenderer.flipX = true;
                tileRenderer.flipY = true;
                break;
            default:
                break;
        }
    }
}
