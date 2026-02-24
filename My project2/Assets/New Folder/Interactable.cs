using NUnit.Framework.Constraints;
using UnityEngine;

public class Interactable : MonoBehaviour, IInterface
{
    private bool interacted = false;

    void OnMouseDown()
    {
        Interact();
    }


    public void Interact()
    {
        if(interacted) return;
        interacted = true;
        gameObject. SetActive(false);

    }



}
               