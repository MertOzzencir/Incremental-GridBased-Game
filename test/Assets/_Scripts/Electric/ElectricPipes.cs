using System;
using System.Collections.Generic;
using UnityEngine;

public class ElectricPipes : MonoBehaviour, IElectricNode, IPlaceable, IPickable
{
    
    [SerializeField] private Transform inputSocket;
    [SerializeField] private Color PowerOnColor;
    [SerializeField] private GameObject visual;
    [SerializeField] Vector2Int size;
    [SerializeField] int index;


    //IElectricNode
    public List<IElectricNode> Neighbours { get; set; } = new List<IElectricNode>();
    public bool Power { get; set; } = false;
    public Transform ConnectSocket { get => inputSocket; set => inputSocket = value; }

    //IPlaceable
    public bool IsPlaced { get; set; }
    public int Index { get => index; set => index = value; }
    public Vector2Int GetSize { get => size; set => size = value; }
    public Vector2Int GetRange { get => size; set => size = value; }
    public Vector3Int PlacedPosition { get; set; }
    public GameObject Orientation { get => visual; set => visual = value; }

    private Collider objectCollider;
    void Awake()
    {
        objectCollider = GetComponent<Collider>();
        objectCollider.enabled = false;
    }
    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    [ContextMenu("Delete Object On Grid")]
    public void OnDestroyObject()
    {
        GridSystem.Instance.DeleteOnGrid(PlacedPosition, this);
        Destroy(GetGameObject());
    }


    public void Placed()
    {
        IsPlaced = true;
        Cursor.visible = true;
        objectCollider.enabled = true;

    }
    public void UnPlaced()
    {
        //Obje Değiştirme Mekaniğinide değiştir
        Cursor.visible = true;
        OnDestroyObject();
    }
    public void Picked()
    {
        Cursor.visible = false;

    }

    public void PowerChanged(bool powerState, Material electricStateMaterial)
    {
        
       MeshRenderer[] robe = ConnectSocket.GetComponentsInChildren<MeshRenderer>();
        foreach (var a in robe)
        {
            a.material = electricStateMaterial;
        }
        if (powerState)
        {
            Power = true;
            visual.GetComponentInChildren<MeshRenderer>().material.color = Color.green;
        }
        else
        {
            Power = false;
            visual.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
        }
    }
    public bool CanTie(IElectricNode target)
    {
        if (Neighbours.Count >= 3)
            return false;
        foreach (var a in Neighbours)
        {
            if (target == a)
                return false;
        }

        return true;
    }


}
