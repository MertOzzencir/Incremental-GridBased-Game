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
        MeshRenderer[] childObjects = GetComponentsInChildren<MeshRenderer>();
        foreach (var a in childObjects)
        {
            a.material.color = new Color(a.material.color.r, a.material.color.g, a.material.color.b, 1f);
        }
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
        MeshRenderer[] childObjects = GetComponentsInChildren<MeshRenderer>();
        foreach (var a in childObjects)
        {
            a.material.color = new Color(a.material.color.r, a.material.color.g, a.material.color.b, 0.2f);
        }
    }

    public void PowerChanged(bool powerState)
    {
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        if (powerState)
        {
            Power = true;
            foreach (var a in meshRenderers)
            {
                a.material.color = PowerOnColor;
            }
        }
        else
        {
            Power = false;
            foreach (var a in meshRenderers)
            {
                a.material.color = new Color(0, 0, 0, 1);
            }
        }
    }
    public bool CanTie(IElectricNode target)
    {
        if(Neighbours.Count >=3)
            return false;
        foreach (var a in Neighbours)
        {
            if (target == a)
                return false;
        }

        return true;
    }


}
