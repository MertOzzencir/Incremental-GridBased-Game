using System;
using UnityEngine;

public class ObjectPicker : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToPick;
    private Placement placer;
    private IPlaceable[] objects;
    void Start()
    {
        objects = new IPlaceable[objectsToPick.Length];
        InputManager.Instance.OnNumber +=PickObject;
        placer = GetComponent<Placement>();
        int i=0;
        foreach(var a in objectsToPick)
        {
            objects[i] = a.GetComponent<IPlaceable>();
            i++; 
        }
    }

    private void PickObject(int obj)
    {
        switch (obj)
        {
            case 1:
            placer.RecieveObject(objects[0]);
            break;
            case 2:
            placer.RecieveObject(objects[1]);
            break;
            case 3:
            placer.RecieveObject(objects[2]);
            break;
            case 4:
            break;
            case 5:
            break;
            case 6:
            break;
            case 7:
            break;
            case 8:
            break;
            case 9:
            break;

        }
    }
}
