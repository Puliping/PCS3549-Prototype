using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePlayer : MonoBehaviour
{
    public Player player;
    public List<Interactables> interactables;
    public Interactables interactable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
    private void OnTriggerExit2D(Collider2D collision)
    {

    }
    public void Interact()
    {
        interactable.Interact();
    }
}
