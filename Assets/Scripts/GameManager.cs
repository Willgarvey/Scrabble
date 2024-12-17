using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject tilePrefab; // Wooden tile prefab
    public GameObject playerRackPrefab;
    public Vector2 tileSize = new Vector2(1, 1); // Tile size in units
    public int boardSize = 15; // Scrabble board size (15x15)
    public Transform boardParent;     // The parent of the board (used to calculate position)
    public Texture2D[] tileTextures; // Assign the letter and point to each tile top
    private GameObject selectedPlayerRack = null;

    void Start()
    {
        selectedPlayerRack = Instantiate(playerRackPrefab);
        Renderer rackRenderer = selectedPlayerRack.GetComponent<Renderer>();
        if (rackRenderer != null)
        {
            rackRenderer.material.color = Color.yellow;  // Set initial color to yellow
            int randInt = Random.Range(0, 3);
            ApplyTopFaceTexture(rackRenderer.material, tileTextures[randInt]);
        }

        // Place some random test tiles
        PlaceRandomTestTiles(10);
    }

    void Update()
    {
        //MovePointerWithMouse();

        if (Input.GetMouseButtonDown(0))  // Left mouse button clicked
        {
            HandleTileSelectionOrPlacement();
        }
    }

    // Method to calculate the position of a tile at (row, col) relative to the board center
    public Vector3 GetTilePosition(int row, int col)
    {
        // Center the board around the origin
        float offsetX = (boardSize - 1) * tileSize.x / 2;
        float offsetZ = (boardSize - 1) * tileSize.y / 2;

        // Calculate the world position of the tile at (row, col)
        float posX = (col * tileSize.x) - offsetX;
        float posZ = (row * tileSize.y) - offsetZ;

        return new Vector3(posX, 0, posZ);
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

            // Assign a unique name to the tile
            testTile.name = $"Tile_{row}_{col}";

            // Set the tile color to beige
            Renderer tileRenderer = testTile.GetComponent<Renderer>();
            if (tileRenderer != null)
            {
                tileRenderer.material.color = beigeColor;
                int randInt = Random.Range(0, 3);
                ApplyTopFaceTexture(tileRenderer.material, tileTextures[randInt]);
            }
        }
    }

    //void MovePointerWithMouse()
    //{   // This method moves the pointer based on mouse position in world space
    //    // Get mouse position in screen space (from the top-left corner)
    //    Vector3 mouseScreenPos = Input.mousePosition;

    //    // Convert the screen position to world position above the board
    //    Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);
    //    RaycastHit hit;

    //    // Cast a ray from the camera through the mouse position to the board
    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        // We use hit.point as the point on the board where the ray hits
    //        Vector3 pointerPosition = hit.point;
    //        pointerPosition.y = 0.2f;  // Adjust the pointer's Y position (above the board)

    //        // Update pointer's position
    //        pointer.transform.position = pointerPosition;
    //    }
    //}

    // Handle tile selection and placement

    void HandleTileSelectionOrPlacement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // If we hit an object
        if (Physics.Raycast(ray, out hit))
        {
            GameObject clickedObject = hit.collider.gameObject;

            // Check if the hit object's name starts with "PlayerRack_"
            if (clickedObject.name.StartsWith("PlayerRack_"))
            {
                // If a PlayerRack is already selected and it's not the clicked one
                if (selectedPlayerRack != null && selectedPlayerRack != clickedObject)
                {
                    // Revert the color of the currently selected PlayerRack to beige
                    Renderer previousRackRenderer = selectedPlayerRack.GetComponent<Renderer>();
                    if (previousRackRenderer != null)
                    {
                        previousRackRenderer.material.color = new Color(0.96f, 0.96f, 0.86f); // Beige color
                    }
                }

                if (selectedPlayerRack != clickedObject)
                {
                    selectedPlayerRack = clickedObject;

                    // Set the color of the new selected PlayerRack to yellow
                    Renderer newRackRenderer = selectedPlayerRack.GetComponent<Renderer>();
                    if (newRackRenderer != null)
                    {
                        newRackRenderer.material.color = Color.yellow;
                    }
                }
                else
                {
                    // Deselect the currently selected PlayerRack
                    Renderer rackRenderer = selectedPlayerRack.GetComponent<Renderer>();
                    if (rackRenderer != null)
                    {
                        rackRenderer.material.color = new Color(0.96f, 0.96f, 0.86f); // Beige color
                    }

                    selectedPlayerRack = null;
                }
            }

            // Check if the hit object's name starts with "Position_"
            else if (clickedObject.name.StartsWith("Position_"))
            {
                // If a PlayerRack is selected, move it to the clicked tile
                if (selectedPlayerRack != null)
                {
                    // Position the PlayerRack above the clicked tile
                    Vector3 placePosition = clickedObject.transform.position;
                    placePosition.y = 0.1f;  // Raise slightly above the board

                    selectedPlayerRack.transform.position = placePosition;

                    // Revert the PlayerRack color to beige
                    Renderer rackRenderer = selectedPlayerRack.GetComponent<Renderer>();
                    if (rackRenderer != null)
                    {
                        rackRenderer.material.color = new Color(0.96f, 0.96f, 0.86f); // Beige color
                    }

                    selectedPlayerRack = null; // Deselect after placing
                }
            }
        }
        else
        {
            // If nothing is hit and a PlayerRack is selected
            if (selectedPlayerRack != null)
            {
                ReturnPlayerRackToRack(selectedPlayerRack);

                // Revert the PlayerRack color to beige
                Renderer rackRenderer = selectedPlayerRack.GetComponent<Renderer>();
                if (rackRenderer != null)
                {
                    rackRenderer.material.color = new Color(0.96f, 0.96f, 0.86f); // Beige color
                }

                selectedPlayerRack = null; // Deselect after returning
            }
        }
    }

    // Method to return a PlayerRack to its original rack position
    void ReturnPlayerRackToRack(GameObject playerRack)
    {
        // Define the default rack position or logic to determine where the rack is
        // For example, you could have a predefined list of rack positions
        Vector3 rackPosition = GetPlayerRackDefaultPosition(playerRack);

        // Place the PlayerRack at its default position
        playerRack.transform.position = rackPosition;
    }

    // Example logic to get the default position of a PlayerRack
    Vector3 GetPlayerRackDefaultPosition(GameObject playerRack)
    {
        // Starting position for the tile rack
        Vector3 startPos = new Vector3(-4 * tileSize.x, 0, -9 * tileSize.y);
        float padding = 0.2f; // Padding between tiles

        if (playerRack != null && playerRack.name.StartsWith("PlayerRack_"))
        {
            // Extract the index from the player rack name (e.g., "PlayerRack_0")
            string[] parts = playerRack.name.Split('_');
            if (parts.Length == 2 && int.TryParse(parts[1], out int rackIndex))
            {
                // Calculate the position for the tile based on its index
                return startPos + new Vector3(rackIndex * (tileSize.x + padding), 0, 0);
            }
        }

        return Vector3.zero; // Default fallback position
    }

    void ApplyTopFaceTexture(Material material, Texture2D topFaceTexture)
    {
        if (topFaceTexture == null)
        {
            Debug.LogError("Passed texture is null!");
            return;
        }

        // Set the texture on the material's main property
        material.SetTexture("_MainTex", topFaceTexture);

        // Flip the texture 180 degrees using tiling and offset
        material.SetTextureScale("_MainTex", new Vector2(-1, -1)); // Negative scale flips the texture
        material.SetTextureOffset("_MainTex", new Vector2(1, 1));  // Adjust offset to align properly
    }


}
