using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown dropdown;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void ChangeClass()
    {
        LevelManager.Instance.playerlocal.SetClass(dropdown.value);      
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene("Gameplay");
        
    }

    public void Quitgame()
    {
        Application.Quit();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
