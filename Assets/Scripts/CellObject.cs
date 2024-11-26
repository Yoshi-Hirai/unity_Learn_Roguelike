using System;
using UnityEngine;

public class CellObject : MonoBehaviour
{
    protected Vector2Int m_cell;

    public virtual void Init(Vector2Int cell)
    { 
        m_cell = cell;
    }

    public virtual bool PlayerWantsToEnter()
    {
        return true;
    }

    // Called When the player enter the cell in which that object is
    public virtual void PlayerEnterd()
    {

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
