using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyObject : CellObject
{
    public int MaxHP = 3;

    private int m_CurrentHP;

    private void Awake()
    {
        GameManager.Instance.TurnManager.OnTick += TurnHappend;
    }
    
    private void Destroy()
    {
        GameManager.Instance.TurnManager.OnTick -= TurnHappend;
    }

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        m_CurrentHP = MaxHP;
    }

    public override bool PlayerWantsToEnter()
    {
        m_CurrentHP--;

        GameManager.Instance.PlayerController.Attack();

        if (m_CurrentHP > 0)
        {
            return false;
        }

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

    void TurnHappend()
    {
        var playerCell = GameManager.Instance.PlayerController.CellPosition;

        int xDist = playerCell.x - m_cell.x;
        int yDist = playerCell.y - m_cell.y;
        int absXDist = Mathf.Abs(xDist);
        int absYDist = Mathf.Abs(yDist);

        if ((xDist == 0 && absYDist == 1) ||
            (yDist == 0 && absXDist == 1))
        {
            GameManager.Instance.ChangeFood(-3);
        }
        else
        {
            if (absXDist > absYDist)
            {
                if (!TryMoveIntX(xDist))
                {
                    TryMoveIntY(yDist);
                }
            }
            else
            {
                if (!TryMoveIntY(yDist))
                {
                    TryMoveIntX(xDist);
                }
            }
        }
    }

    bool MoveTo(Vector2Int coord)
    {
        var board = GameManager.Instance.BoardManager;
        var targetCell = board.GetCellData(coord);

        if (targetCell == null ||
            !targetCell.Passable ||
            targetCell.ContainedObject != null) {
            return false;
        }

        // remove enemy from current cell
        var currentcell = board.GetCellData(m_cell);
        currentcell.ContainedObject = null;


        // add it to the next cell
        targetCell.ContainedObject = this;
        m_cell = coord;
        UnityEngine.Debug.Log(m_cell);
        this.transform.position = board.CellToWorld(coord);

        return true;
    }

    bool TryMoveIntX(int distX)
    {
        if(distX > 0)
        {
            return MoveTo(m_cell + Vector2Int.right);
        }
        return MoveTo(m_cell + Vector2Int.left);
    }

    bool TryMoveIntY(int distY)
    {
        if (distY > 0)
        {
            return MoveTo(m_cell + Vector2Int.up);
        }
        return MoveTo(m_cell + Vector2Int.down);
    }
}
