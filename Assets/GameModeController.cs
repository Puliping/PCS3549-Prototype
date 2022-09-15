using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameModeController : MonoBehaviour
{
    public static GameModeController Instance;
    public GameObject canvasRoot;
    [HideInInspector]
    public int enemiesAlive;
    [SerializeField]
    private TextMeshProUGUI finalText;
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
        finalText.text = "Você ganhou";
    }
    public void Defeat()
    {
        canvasRoot.SetActive(true);
        finalText.text = "Você perdeu";
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
