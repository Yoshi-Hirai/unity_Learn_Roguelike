using UnityEngine;
using UnityEngine.Tilemaps;

public class ExitObject : CellObject
{
    public Tile EndTile;

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        GameManager.Instance.BoardManager.SetCellTile(cell, EndTile);
    }

    public override bool PlayerWantsToEnter()
    {
        return true;
    }

    public override void PlayerEnterd()
    {
        GameManager.Instance.NewLevel();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
