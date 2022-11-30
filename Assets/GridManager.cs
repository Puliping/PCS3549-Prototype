using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    ProceduralGridMover gridMover;
    ProceduralGridMoverEditor editor;
    private GameObject player;
    void Start()
    {
        player = GameModeController.Instance.GetPlayers()[0];
        gridMover.target = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
