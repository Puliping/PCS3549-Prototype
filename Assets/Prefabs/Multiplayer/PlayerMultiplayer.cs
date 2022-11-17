using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMultiplayer : NetworkBehaviour
{
    [SerializeField]
    private List<GameObject> playerClasses;

    public enum Class
    {
        HumanFighter,
        Frenzy
    }
    public Class playerSelectedClass = Class.HumanFighter;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            switch (playerSelectedClass)
            {
                case Class.HumanFighter:
                    Instantiate(playerClasses[0], new Vector3(-20, -21, 0), Quaternion.identity);
                    break;
                case Class.Frenzy:
                    break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (this.gameObject.name == "LocalGamePlayer")
            {
                LevelManager.Instance.playerlocal = this;
            }
            return;
        }
    }

    public void SetClass(int number)
    {
        switch (number)
        {
            case 0:
                playerSelectedClass = Class.HumanFighter;
                break;
            case 1:
                playerSelectedClass = Class.Frenzy;
                break;
        }
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        gameObject.name = "LocalGamePlayer";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
