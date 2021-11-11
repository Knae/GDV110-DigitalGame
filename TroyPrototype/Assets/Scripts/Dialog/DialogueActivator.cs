
using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{

    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] public DialogueUI DialogueDisplay;

    private bool bDialogueDone = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !bDialogueDone/* && other.TryGetComponent(out PlayerDialog player)*/)
        {
            DialogueDisplay.ShowDialogue(dialogueObject);
        }


    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")/* && other.TryGetComponent(out PlayerDialog player)*/)
        {
            bDialogueDone = true;
        }
    }

    public void Interact(PlayerDialog player)
    {
        player.DialogueUI.ShowDialogue(dialogueObject);
    }

}
