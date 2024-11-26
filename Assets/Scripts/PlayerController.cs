using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 5.0f;
    public Vector2Int CellPosition;

    private BoardManager m_Board;
    private bool m_IsGameOver;

    private bool m_IsMoving;
    private Vector3 m_MoveTarget;
    private Animator m_Animator;

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        m_Board = boardManager;
        CellPosition = cell;

        //let's move to the right position...
        MoveTo(cell, true);
    }

    public void MoveTo(Vector2Int cell, bool immediate)
    {
        // technically the player is not there yet, but the movement is only cosmetic
        // and we know nothing can stop it as we checked everything before starting it
        // so safe to update there
        CellPosition = cell;

        if (immediate)
        {
            m_IsMoving = false;
            transform.position = m_Board.CellToWorld(CellPosition);

        }
        else
        {
            m_IsMoving = true;
            m_MoveTarget = m_Board.CellToWorld(CellPosition);
        }
        m_Animator.SetBool("Moving", m_IsMoving);
    }

    public void GameOver()
    {
        m_IsGameOver = true;
    }

    public void Attack()
    { 
        m_Animator.SetBool("Attack", true);
    }

    public void Init()
    {
        m_IsGameOver = false;
        m_IsMoving = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( m_IsGameOver )
        {
            if( Keyboard.current.enterKey.wasPressedThisFrame)
            {
                GameManager.Instance.StartNewGame();
            }
            return;
        }

        if (m_IsMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_MoveTarget, MoveSpeed * Time.deltaTime);
            if( transform.position == m_MoveTarget )
            {
                m_IsMoving = false;
                m_Animator.SetBool("Moving",  false);
                var celldata = m_Board.GetCellData(CellPosition);
                if (celldata != null && celldata.ContainedObject != null)
                {
                    // Call PlayerEntered AFTER movind the player! otherwise not in cell yet
                    celldata.ContainedObject.PlayerEnterd();
                }
            }
            return;
        }


        Vector2Int newCellTarget = CellPosition;
        bool hasMoved = false;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y += 1;
            hasMoved = true;
        }
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y -= 1;
            hasMoved = true;
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x += 1;
            hasMoved = true;
        }
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x -= 1;
            hasMoved = true;
        }

        if (hasMoved)
        {
            //check if the new position is passable, then move there if it is.
            BoardManager.CellData cellData = m_Board.GetCellData(newCellTarget);
            if (cellData != null && cellData.Passable )
            {
                GameManager.Instance.TurnManager.Tick();

                if (cellData.ContainedObject == null)
                {
                    MoveTo(newCellTarget, false);
                }
                else if(cellData.ContainedObject.PlayerWantsToEnter() )
                {
                    MoveTo(newCellTarget, false);
                }
            }
        }

    }

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }
}
