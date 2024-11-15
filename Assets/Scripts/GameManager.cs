using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject tilePrefab; // Wooden tile prefab (or your custom tile)
    public Vector2 tileSize = new Vector2(1, 1); // Tile size in units
    public int boardSize = 15; // Scrabble board size (15x15)

    // Number of test tiles to place randomly
    public int numberOfTestTiles = 5;


    // Method to calculate the position of a tile at (row, col) relative to the board center
    public Vector3 GetTilePosition(int row, int col)
    {
        // Center the board around the origin
        float offsetX = (boardSize - 1) * tileSize.x / 2;
        float offsetZ = (boardSize - 1) * tileSize.y / 2;

        // Calculate the world position of the tile at (row, col)
        float posX = (col * tileSize.x) - offsetX;
        float posZ = (row * tileSize.y) - offsetZ;

        return new Vector3(posX, 0, posZ); // Assuming you want the tiles on the XZ plane (y=0 for flat tiles)
    }

    // Method to place test tiles randomly on the board
    public void PlaceRandomTestTiles(int numberOfTiles)
    {
        // Define the beige color
        Color beigeColor = new Color(0.96f, 0.96f, 0.86f); // RGB for beige

        for (int i = 0; i < numberOfTiles; i++)
        {
            // Randomly pick row and column indexes
            int row = Random.Range(0, boardSize); // Random row from 0 to boardSize-1
            int col = Random.Range(0, boardSize); // Random column from 0 to boardSize-1

            // Get the world position for the tile
            Vector3 tilePosition = GetTilePosition(row, col);

            // Raise the tile slightly above the board
            tilePosition.y = 0.1f; // Adjust this value to raise the tile as needed

            // Instantiate the test tile at the calculated position
            GameObject testTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);

            // Set the tile color to beige
            Renderer tileRenderer = testTile.GetComponent<Renderer>();
            if (tileRenderer != null)
            {
                tileRenderer.material.color = beigeColor;
            }
        }
    }

}
