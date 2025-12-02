using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour, IGenerator
{
    [SerializeField] private Transform inputSocket;
    public List<IElectricNode> Neighbours { get; set; } = new List<IElectricNode>();
    public bool Power { get; set; } = true;
    public Transform ConnectSocket { get => inputSocket; set => inputSocket = value; }

    public bool CanTie(IElectricNode target)
    {
        foreach (var a in Neighbours)
        {
            if (target == a)
                return false;
        }

        return true;
    }

    public void PowerChanged(bool powerState, Material state)
    {

        MeshRenderer[] robe = ConnectSocket.GetComponentsInChildren<MeshRenderer>();
        foreach (var a in robe)
        {
            a.material = state;

            if (powerState)
            {
                Power = true;


            }
            else
            {
                Power = false;

            }
        }
    }
}
