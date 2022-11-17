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
        Frenzy,
        HookGuy
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
            Instantiate(playerClasses[(int)playerSelectedClass], new Vector3(-20, -21, 0), Quaternion.identity);
            Debug.Log("playerSelectedClass = " + (int)playerSelectedClass);
            /*
            switch (playerSelectedClass)
            {
                case Class.HumanFighter:
                    Instantiate(playerClasses[0], new Vector3(-20, -21, 0), Quaternion.identity);
                    break;
                case Class.Frenzy:
                    Instantiate(playerClasses[1], new Vector3(-20, -21, 0), Quaternion.identity);
                    break;
            }
            */
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
            case 2:
                playerSelectedClass = Class.HookGuy;
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
