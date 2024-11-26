using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    public Tile ObstacleTile;
    public Tile ObstacleDamageTile;
    public int MaxHP = 3;

    private int m_CurrentHP;
    private Tile m_OriginalTile;

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);

        m_CurrentHP = MaxHP;
        m_OriginalTile = GameManager.Instance.BoardManager.GetCellTile(cell);
        GameManager.Instance.BoardManager.SetCellTile(cell, ObstacleTile);
    }

    public override bool PlayerWantsToEnter()
    { 
        m_CurrentHP--;

        GameManager.Instance.PlayerController.Attack();

        if( m_CurrentHP > 0)
        {
            if(m_CurrentHP == 1)
            {
                GameManager.Instance.BoardManager.SetCellTile(m_cell, ObstacleDamageTile);
            }
            return false;
        }

        GameManager.Instance.BoardManager.SetCellTile(m_cell, m_OriginalTile);
        Destroy(gameObject);
        return true;
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
