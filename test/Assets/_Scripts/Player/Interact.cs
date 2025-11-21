using System;
using UnityEngine;

public class Interact : MonoBehaviour
{

    void Start()
    {
        InputManager.Instance.OnInteract += OnInteractAction;
    }

    void OnInteractAction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);
        foreach(var a in hits)
        {
            if(a.transform.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact();
            }
        }
    }
        
}
