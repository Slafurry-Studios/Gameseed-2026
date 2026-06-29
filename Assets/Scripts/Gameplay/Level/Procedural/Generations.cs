using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Generator : MonoBehaviour
{
    [Header("City Dimensions")]
    public int width = 60;
    public int height = 40;

    [Header("Road Generation (BSP)")]
    [Tooltip("The smallest a block can get before roads stop dividing it.")]
    public int minBlockSize = 8;
    [Tooltip("The maximum size a block can be. If it's larger, it MUST divide.")]
    public int maxBlockSize = 30;
    [Tooltip("Chance to randomly stop dividing a block, leaving a large open space (like a park).")]
    [Range(0f, 1f)]
    public float stopDividingChance = 0.15f;
    [Tooltip("Chance for a road to stop halfway, creating a dead end / cul-de-sac.")]
    [Range(0f, 1f)]
    public float deadEndChance = 0.2f;

    [Header("Building Constraints (Margin Ranges)")]
    [Tooltip("X = Min, Y = Max empty tiles required to the Left of the building")]
    public Vector2Int marginLeftRange = new Vector2Int(1, 3);
    [Tooltip("X = Min, Y = Max empty tiles required to the Right of the building")]
    public Vector2Int marginRightRange = new Vector2Int(1, 3);
    [Tooltip("X = Min, Y = Max empty tiles required Below the building")]
    public Vector2Int marginBottomRange = new Vector2Int(1, 3);
    [Tooltip("X = Min, Y = Max empty tiles required Above the building (0 = can touch top road)")]
    public Vector2Int marginTopRange = new Vector2Int(0, 1);

    [Header("Tilemaps")]
    public Tilemap bgTilemap;
    public Tilemap roadTilemap;
    
    [Header("Rule Tiles")]
    public TileBase bgRuleTile;
    public TileBase roadRuleTile;

    [Header("Building Prefabs")]
    public GameObject[] largeBuildings;
    public GameObject[] mediumBuildings;
    public GameObject[] smallBuildings;

    [Header("Building Sizes (in Tiles)")]
    public Vector2Int largeSize = new Vector2Int(4, 4);
    public Vector2Int mediumSize = new Vector2Int(3, 3);
    public Vector2Int smallSize = new Vector2Int(2, 2);

    private int[,] grid; 
    private List<GameObject> spawnedBuildings = new List<GameObject>();

    void Start()
    {
        GenerateCity();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateCity();
        }
    }

    void GenerateCity()
    {
        bgTilemap.ClearAllTiles();
        roadTilemap.ClearAllTiles();
        foreach (GameObject b in spawnedBuildings) Destroy(b);
        spawnedBuildings.Clear();

        grid = new int[width, height];

        GenerateBackground();

        GenerateRoadsBSP(0, 0, width, height, 0);

        PopulateBuildings(largeBuildings, largeSize.x, largeSize.y);
        PopulateBuildings(mediumBuildings, mediumSize.x, mediumSize.y);
        PopulateBuildings(smallBuildings, smallSize.x, smallSize.y);
    }

    void GenerateBackground()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                bgTilemap.SetTile(new Vector3Int(x, y, 0), bgRuleTile);
            }
        }
    }

    void GenerateRoadsBSP(int startX, int startY, int w, int h, int depth)
    {
        bool isTooBig = (w > maxBlockSize || h > maxBlockSize);

        if (!isTooBig)
        {
  
            if (w < minBlockSize * 2 || h < minBlockSize * 2) return;


            if (depth > 1 && Random.value < stopDividingChance) return;
        }

        bool splitHorizontal;
        
        if (w > h * 1.5f) splitHorizontal = false;
        else if (h > w * 1.5f) splitHorizontal = true;
        else splitHorizontal = Random.value > 0.5f;

        // FIX: Only allow dead-ends on deeper branches so main city arteries stay connected!
        bool isDeadEnd = (depth > 2 && Random.value < deadEndChance);

        if (splitHorizontal)
        {
            int roadY = startY + Random.Range(minBlockSize, h - minBlockSize);
            
            int drawStartX = startX;
            int drawEndX = startX + w;

            // Chance to create a dead-end (road stops halfway)
            if (isDeadEnd)
            {
                int stopPoint = Random.Range(minBlockSize, w - minBlockSize);
                if (Random.value > 0.5f) 
                    drawStartX = startX + stopPoint; // Connects to Right, ends on Left
                else 
                    drawEndX = startX + stopPoint; // Connects to Left, ends on Right
            }
            
            // Draw horizontal road
            for (int x = drawStartX; x < drawEndX; x++)
            {
                grid[x, roadY] = 1;
                roadTilemap.SetTile(new Vector3Int(x, roadY, 0), roadRuleTile);
            }
            
            // FIX: If this is a dead-end, it didn't cleanly split the block. 
            // So we stop subdividing here, making this area a nice cul-de-sac neighborhood!
            if (!isDeadEnd)
            {
                GenerateRoadsBSP(startX, startY, w, roadY - startY, depth + 1);
                GenerateRoadsBSP(startX, roadY + 1, w, (startY + h) - (roadY + 1), depth + 1);
            }
        }
        else
        {
            int roadX = startX + Random.Range(minBlockSize, w - minBlockSize);

            int drawStartY = startY;
            int drawEndY = startY + h;

            // Chance to create a dead-end (road stops halfway)
            if (isDeadEnd)
            {
                int stopPoint = Random.Range(minBlockSize, h - minBlockSize);
                if (Random.value > 0.5f) 
                    drawStartY = startY + stopPoint; // Connects to Top, ends on Bottom
                else 
                    drawEndY = startY + stopPoint; // Connects to Bottom, ends on Top
            }

            // Draw vertical road
            for (int y = drawStartY; y < drawEndY; y++)
            {
                grid[roadX, y] = 1;
                roadTilemap.SetTile(new Vector3Int(roadX, y, 0), roadRuleTile);
            }

            // FIX: Same as above. Don't subdivide dead-ends!
            if (!isDeadEnd)
            {
                GenerateRoadsBSP(startX, startY, roadX - startX, h, depth + 1);
                GenerateRoadsBSP(roadX + 1, startY, (startX + w) - (roadX + 1), h, depth + 1);
            }
        }
    }

    void PopulateBuildings(GameObject[] prefabs, int buildWidth, int buildHeight)
    {
        if (prefabs.Length == 0) return;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Generate a random margin for this specific placement attempt
                // Random.Range for ints is exclusive on the max, so we add 1 to the Y value.
                int mLeft = Random.Range(marginLeftRange.x, marginLeftRange.y + 1);
                int mRight = Random.Range(marginRightRange.x, marginRightRange.y + 1);
                int mBottom = Random.Range(marginBottomRange.x, marginBottomRange.y + 1);
                int mTop = Random.Range(marginTopRange.x, marginTopRange.y + 1);

                if (CanFitBuildingWithMargins(x, y, buildWidth, buildHeight, mLeft, mRight, mBottom, mTop))
                {
                    GameObject prefabToSpawn = prefabs[Random.Range(0, prefabs.Length)];
                    Vector3 spawnPos = roadTilemap.GetCellCenterWorld(new Vector3Int(x, y, 0));
                    
                    // Adjust to grid corners
                    spawnPos += new Vector3((buildWidth - 1) * 0.5f, (buildHeight - 1) * 0.5f, 0);

                    GameObject newBuilding = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
                    newBuilding.transform.parent = this.transform; 
                    spawnedBuildings.Add(newBuilding);

                    // Mark ONLY the building's physical footprint as occupied (2). 
                    // This allows other buildings' empty margins to overlap.
                    MarkGridOccupied(x, y, buildWidth, buildHeight);
                }
            }
        }
    }

    bool CanFitBuildingWithMargins(int startX, int startY, int bWidth, int bHeight, int mLeft, int mRight, int mBottom, int mTop)
    {
        // 1. Is the physical building itself out of bounds?
        if (startX < 0 || startY < 0 || startX + bWidth > width || startY + bHeight > height) 
            return false;

        // 2. Calculate the required empty footprint (Building Size + Margins)
        int checkX = startX - mLeft;
        int checkY = startY - mBottom;
        int checkWidth = bWidth + mLeft + mRight;
        int checkHeight = bHeight + mBottom + mTop;

        // 3. Scan the entire footprint
        for (int x = checkX; x < checkX + checkWidth; x++)
        {
            for (int y = checkY; y < checkY + checkHeight; y++)
            {
                // If a required margin cell is outside the map, pretend it's a road (fail)
                if (x < 0 || y < 0 || x >= width || y >= height)
                    return false;

                // If any cell in this footprint hits a Road (1) or a Building (2), we fail
                if (grid[x, y] != 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    void MarkGridOccupied(int startX, int startY, int bWidth, int bHeight)
    {
        for (int x = startX; x < startX + bWidth; x++)
        {
            for (int y = startY; y < startY + bHeight; y++)
            {
                grid[x, y] = 2; // 2 = building
            }
        }
    }
}