using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeController : MonoBehaviour
{
    public static GameModeController Instance;
    public GameObject canvasRoot;
    [HideInInspector]
    public int enemiesAlive;
    [SerializeField]
    private TextMeshProUGUI finalText;

    public GameObject[] player;
    // Start is called before the first frame update
    private void Awake()
    {   
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
        DontDestroyOnLoad(this);

        
    }

    public void BackMenu()
    {
        Application.Quit();
    }
    public void EnemyDeath()
    {
        enemiesAlive--;
        if (enemiesAlive == 0)
        {
            Win();
        }
    }

    public void Win()
    {
        canvasRoot.SetActive(true);
        finalText.text = "Você ganhou!";
    }
    public void Defeat()
    {
        canvasRoot.SetActive(true);
        finalText.text = "Você perdeu";
    }
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player");
    }

    public GameObject[] GetPlayers()
    {
        return player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
