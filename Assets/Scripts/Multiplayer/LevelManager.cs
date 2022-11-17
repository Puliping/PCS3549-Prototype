using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public MultiplayerManager localManager;
    public NetworkManager networkManager;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        // Inicia o host instantaneamente
        // TODO: retirar
        networkManager.StartHost();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
