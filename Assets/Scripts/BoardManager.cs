using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool Passable;
        public CellObject ContainedObject;
    }

    public int Width;
    public int Height;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;

    public PlayerController Player;
    public FoodObject[] FoodPrefab;
    public WallObject[] WallPrefab;
    public ExitObject ExitPrefab;
    public EnemyObject[] EnemyPrefab;

    private Grid m_Grid;
    private Tilemap m_Tilemap;
    private CellData[,] m_BoardData;
    private List<Vector2Int> m_EmptyCellsList;

    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return m_Grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }

    public CellData GetCellData(Vector2Int cellIndex)
    {
        if ( cellIndex.x < 0 || cellIndex.x >= Width || cellIndex.y < 0 || cellIndex.y >= Height )
        {
            return null; 
        }
        return m_BoardData[cellIndex.x, cellIndex.y];
    }

    public Tile GetCellTile(Vector2Int cellIndex)
    {
        return m_Tilemap.GetTile<Tile>(new Vector3Int(cellIndex.x, cellIndex.y, 0));
    }
    public void SetCellTile(Vector2Int cellIndex, Tile tile)
    {
        m_Tilemap.SetTile(new Vector3Int(cellIndex.x, cellIndex.y, 0), tile);
    }

    public void Init()
    {
        m_Tilemap = GetComponentInChildren<Tilemap>();
        m_Grid = GetComponentInChildren<Grid>();
        // Initialize empty list
        m_EmptyCellsList = new List<Vector2Int>();

        m_BoardData = new CellData[Width, Height];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Tile tile;

                m_BoardData[x, y] = new CellData();

                if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
                {
                    tile = WallTiles[UnityEngine.Random.Range(0, WallTiles.Length)];
                    m_BoardData[x, y].Passable = false;
                }
                else
                {
                    tile = GroundTiles[UnityEngine.Random.Range(0, GroundTiles.Length)];
                    m_BoardData[x, y].Passable = true;

                    // this is a passable empty cell, add it to the empty cell list.
                    m_EmptyCellsList.Add(new Vector2Int(x, y));
                }

                m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }

        //remove the starting point of the player! It's not empty, the player is there
        m_EmptyCellsList.Remove(new Vector2Int(1, 1));

        GenerateExit();
        GenerateEnemy();
        GenerateWall();
        GenerateFood();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateFood()
    {
        int foodCount = UnityEngine.Random.Range(1,5);
//        UnityEngine.Debug.Log("Food " + foodCount);
        for (int i = 0; i < foodCount; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex];

            m_EmptyCellsList.RemoveAt(randomIndex);
            int foodindex = UnityEngine.Random.Range(0, FoodPrefab.Length);
            FoodObject newFood = Instantiate(FoodPrefab[foodindex]);
//           UnityEngine.Debug.Log("FoodIndex " + foodindex);
            AddObject(newFood, coord);
        }
    }

    void GenerateWall()
    {
        int wallCount = UnityEngine.Random.Range(1, 4);
//        UnityEngine.Debug.Log("Wall " + wallCount);

        for (int i = 0; i < wallCount; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex];

            m_EmptyCellsList.RemoveAt(randomIndex);
            int wallindex = UnityEngine.Random.Range(0, WallPrefab.Length);
            WallObject newWall = Instantiate(WallPrefab[wallindex]);
//            UnityEngine.Debug.Log("Wallindex " + wallindex);
            AddObject(newWall, coord);
        }
    }

    private void GenerateExit()
    {
        Vector2Int coord = new Vector2Int(Width-2, Height-2);
        m_EmptyCellsList.Remove(coord);
        ExitObject newExit = Instantiate(ExitPrefab);
        AddObject(newExit, coord);
    }

    private void GenerateEnemy()
    {
        Vector2Int coord = new Vector2Int(Width - 3, Height - 3);
        m_EmptyCellsList.Remove(coord);
        EnemyObject newEnemy = Instantiate(EnemyPrefab[0]);
        AddObject(newEnemy, coord);
    }

    public void AddObject(CellObject obj, Vector2Int coord)
    {
        CellData data = m_BoardData[coord.x, coord.y];
        obj.transform.position = CellToWorld(coord);
        data.ContainedObject = obj;
        obj.Init(coord);
    }

    public void CleanBoard()
    {
        // no board data, so exit early, nothing to clean
        if( m_BoardData ==  null )
        {
            return;
        }

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var cellData = m_BoardData[x, y];

                if (cellData.ContainedObject != null)
                {
                    // Careful!
                    // Destroy the GameObject NOT just cellData.ContainedObject
                    // Otherwise what you are destroying is the JUST CellObject COMPONENT
                    // and not the whole gameobject with sprite
                    Destroy(cellData.ContainedObject.gameObject);
                }

                // Delete Tile
                SetCellTile(new Vector2Int(x, y), null);
            }
        }

    }
}
