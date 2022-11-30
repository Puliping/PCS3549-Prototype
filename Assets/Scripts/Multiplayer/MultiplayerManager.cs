using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerManager : NetworkBehaviour
{
    [SerializeField]
    private List<GameObject> playerClasses;
    [HideInInspector] public Player playerRef {get; private set;}

    public enum Class
    {
        HumanFighter,
        Frenzy,
        HookGuy,
        WeaponMaster
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
            playerRef = Instantiate(playerClasses[(int)playerSelectedClass], new Vector3(-20, -21, 0), Quaternion.identity).GetComponent<Player>();
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
                LevelManager.Instance.localManager = this;
            }
            return;
        }
    }

    public void SetClass(int number)
    {
        playerSelectedClass = (Class) number;
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
