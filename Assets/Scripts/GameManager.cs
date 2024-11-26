using System.ComponentModel;
using System.Diagnostics;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public BoardManager BoardManager;
    public PlayerController PlayerController;

    public TurnManager TurnManager { get; private set; }

    public UIDocument UIDoc;

    private int m_FoodAmount = 100;
    private UnityEngine.UIElements.Label m_FoodLabel;

    private int m_CurrentLevel = 1;

    private VisualElement m_GameOverPanel;
    private UnityEngine.UIElements.Label m_GameOverMessage;

    public void ChangeFood(int amount)
    {
        m_FoodAmount += amount;
        m_FoodLabel.text = "Food : " + m_FoodAmount;

        if( m_FoodAmount <= 0 )
        {
            PlayerController.GameOver();
            m_GameOverPanel.style.visibility = Visibility.Visible;
            m_GameOverMessage.text = "GameOver!\n\nYou traveled through " + m_CurrentLevel + " Levels";
        }
    }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TurnManager = new TurnManager();
        TurnManager.OnTick += OnTurnHappen;

        NewLevel();

        m_FoodLabel = UIDoc.rootVisualElement.Q<UnityEngine.UIElements.Label>("FoodLabel");

        m_GameOverPanel = UIDoc.rootVisualElement.Q<VisualElement>("GameOverPanel");
        m_GameOverMessage = m_GameOverPanel.Q<UnityEngine.UIElements.Label>("GameOverMessage");

        StartNewGame();
    }

    public void StartNewGame()
    {
        m_CurrentLevel = 0;
        m_FoodAmount = 100;

        m_FoodLabel.text = "Food : " + m_FoodAmount;
        m_GameOverPanel.style.visibility = Visibility.Hidden;

        PlayerController.Init();
        NewLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTurnHappen()
    {
        ChangeFood(-1);
    }

    public void NewLevel()
    {
        BoardManager.CleanBoard();
        BoardManager.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1));
        
        m_CurrentLevel++;
    }
}
