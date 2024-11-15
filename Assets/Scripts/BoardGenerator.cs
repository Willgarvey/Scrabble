using UnityEngine;

public class ScrabbleBoardGenerator : MonoBehaviour
{
    public GameObject tilePrefab; // Assign a simple square prefab here
    public LineRenderer linePrefab; // Assign a LineRenderer prefab for the grid lines
    public Vector2 tileSize = new Vector2(1, 1); // Adjust based on your prefab size
    public Transform boardParent; // Assign an empty GameObject as the parent
   
    public GameManager GameManager;

    // Colors for special tiles
    public Color defaultColor = Color.white;
    public Color doubleLetterColor = Color.blue;
    public Color tripleLetterColor = Color.red;
    public Color doubleWordColor = Color.magenta;
    public Color tripleWordColor = Color.yellow;
    public Color centerStarColor = Color.green;
    public Color gridLineColor = Color.black; // Color of the grid lines

    // Special tiles map
    private int[,] specialTiles = new int[15, 15]
    {
        {4, 0, 0, 1, 0, 0, 0, 4, 0, 0, 0, 1, 0, 0, 4},
        {0, 3, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0, 3, 0},
        {0, 0, 3, 0, 0, 0, 1, 0, 1, 0, 0, 0, 3, 0, 0},
        {1, 0, 0, 3, 0, 0, 0, 1, 0, 0, 0, 3, 0, 0, 1},
        {0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0},
        {0, 2, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0, 2, 0},
        {0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0},
        {4, 0, 0, 1, 0, 0, 0, 5, 0, 0, 0, 1, 0, 0, 4},
        {0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0},
        {0, 2, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0, 2, 0},
        {0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0},
        {1, 0, 0, 3, 0, 0, 0, 1, 0, 0, 0, 3, 0, 0, 1},
        {0, 0, 3, 0, 0, 0, 1, 0, 1, 0, 0, 0, 3, 0, 0},
        {0, 3, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0, 3, 0},
        {4, 0, 0, 1, 0, 0, 0, 4, 0, 0, 0, 1, 0, 0, 4}
    };

    void Start()
    {
        GenerateBoard();

        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.PlaceRandomTestTiles(10); //  place 10 random tiles
    }

    void GenerateBoard()
    {
        for (int row = 0; row < 15; row++)
        {
            for (int col = 0; col < 15; col++)
            {
                // Calculate position
                Vector3 position = new Vector3(col * tileSize.x, 0, row * tileSize.y);
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, boardParent);

                // Assign a name and color to the tile
                tile.name = $"Tile_{row}_{col}";
                Renderer tileRenderer = tile.GetComponent<Renderer>();
                tileRenderer.material.color = GetTileColor(specialTiles[row, col]);
            }
        }

        // Center the board in the scene
        boardParent.position = new Vector3(-7 * tileSize.x, 0, -7 * tileSize.y);
    }

    Color GetTileColor(int tileType)
    {
        switch (tileType)
        {
            case 1: return doubleLetterColor;
            case 2: return tripleLetterColor;
            case 3: return doubleWordColor;
            case 4: return tripleWordColor;
            case 5: return centerStarColor;
            default: return defaultColor;
        }
    }
}
