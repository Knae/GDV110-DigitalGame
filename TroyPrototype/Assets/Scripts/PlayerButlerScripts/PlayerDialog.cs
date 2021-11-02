using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialog : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;


    public DialogueUI DialogueUI => dialogueUI;

    public IInteractable Interactable { get; set; }
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
             Interactable?.Interact(this);
        }
    }
}
