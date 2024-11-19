using UnityEngine;
using UnityEngine.UI;

public class BoardGenerator : MonoBehaviour
{
    public GameObject tilePrefab; // Used as the prefab for the tiles and the board squares
    public LineRenderer linePrefab; // Used for lines between squared on board
    public Vector2 tileSize = new Vector2(1, 1); // Used to adjust the tile sizes
    public Transform boardParent; // Assign an empty GameObject as the parent for the component that make up the board
    public Transform PlayerRackParent; // Expty GameObject to serve as a parent for the tiles on the player's rack

    public GameManager GameManager;

    // Colors for special tile positions on the board
    public Color defaultColor = new Color(214, 197, 157);
    public Color doubleLetterColor = new Color(2, 146, 246);
    public Color tripleLetterColor = new Color(38, 85, 218);
    public Color doubleWordColor = new Color(204, 117, 157);
    public Color tripleWordColor = new Color(220, 68, 72);
    public Color centerStarColor = new Color(204, 117, 157);
    public Color gridLineColor = new Color(73, 76, 80);

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

    // Scoreboard UI elements
    public GameObject scoreboardParent; // Parent object for the scoreboard
    public Text scoreText;
    public Text currentPlayerText;
    public Button menuButton;
    public Canvas canvas; // A canvas is required for the text and buttons

    void Start()
    {
        GenerateBoard();
        GeneratePlayerRack(); // Generate the player's tile rack

        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.PlaceRandomTestTiles(10); // Place 10 random tiles
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
                tile.name = $"Position_{row}_{col}";
                Renderer tileRenderer = tile.GetComponent<Renderer>();
                tileRenderer.material.color = GetTileColor(specialTiles[row, col]);
            }
        }

        // Center the board in the scene
        boardParent.position = new Vector3(-7 * tileSize.x, 0, -7 * tileSize.y);
    }

    void GeneratePlayerRack()
    {
        float padding = 0.2f; // Padding between tiles
        Vector3 startPos = new Vector3(-4 * tileSize.x, 0, -9 * tileSize.y); // Starting position for the tile rack

        // Generate 7 tiles in the tile rack
        for (int i = 0; i < 7; i++)
        {
            // Calculate position with padding
            Vector3 position = startPos + new Vector3(i * (tileSize.x + padding), 0, 0);
            GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, PlayerRackParent);

            // Assign a unique name to the tile
            tile.name = $"PlayerRack_{i}";

            // Modify properites for each tile as needed
            Renderer tileRenderer = tile.GetComponent<Renderer>();

            Color beigeColor = new Color(0.96f, 0.96f, 0.86f);
            tileRenderer.material.color = beigeColor;
        }
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
