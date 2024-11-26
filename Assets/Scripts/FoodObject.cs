using UnityEngine;

public class FoodObject : CellObject
{
    public int AmountGranted = 10;

    public override void PlayerEnterd()
    {
        Destroy(gameObject);

        // increase food
        GameManager.Instance.ChangeFood(AmountGranted);
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
