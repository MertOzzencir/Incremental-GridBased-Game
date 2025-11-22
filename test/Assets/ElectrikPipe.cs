using System.Collections.Generic;
using UnityEngine;

public class ElectrikPipe : MonoBehaviour, IElectrikInput, IElectrikOutput, IPlaceable, IPickable
{
    [SerializeField] private GameObject visual;
    [SerializeField] private float powerConsumePerSecond;
    [SerializeField] private int index;
    [SerializeField] private Vector2Int size;
    [SerializeField] private Vector2Int range;
    [SerializeField] private Transform inputSocket;
    [SerializeField] private Transform outputSocket;
    [SerializeField] private int outputLimit;

    public float PowerConsumePerSecond { get => powerConsumePerSecond; set => powerConsumePerSecond = value; }
    public int Index { get => index; set => index = value; }
    public Vector2Int GetSize { get => size; set => size = value; }
    public Vector2Int GetRange { get => range; set => range = value; }
    public Vector3Int PlacedPosition { get; set; }
    public GameObject Orientation { get => visual; set => visual = value; }
    public Transform InputSocket { get => inputSocket; set => inputSocket = value; }
    public Transform OutputSocket { get => outputSocket; set => outputSocket = value; }
    public bool IsPlaced { get; set; }
    public int OutputConnectLimit { get => outputLimit; set => outputLimit = value; }
    public List<IElectrikOutput> ElectrikSources { get; set; } = new List<IElectrikOutput>();

    public void ConnectWithOutput(IElectrikOutput source)
    {
        ElectrikSources.Add(source);
        GetCharge(PowerConsumePerSecond);
    }
    public void GetCharge(float energyCost)
    {
        Generator.TotalEnergyPerSecond -= energyCost;
    }
    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public bool IsAvaliable(IElectrikOutput selectedOutput)
    {
        if (ElectrikSources.Count >= OutputConnectLimit)
        {
            return false;
        }
        foreach (var a in ElectrikSources)
        {
            if (selectedOutput == a)
                return false;
        }
        if (!IsPlaced)
            return false;

        return true;
    }

    public void OnDestroyObject()
    {
        GridSystem.Instance.DeleteOnGrid(PlacedPosition, this);
        Destroy(GetGameObject());
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

    public void Placed()
    {
        IsPlaced = true;
        Cursor.visible = true;
        MeshRenderer[] childObjects = GetComponentsInChildren<MeshRenderer>();
        foreach (var a in childObjects)
        {
            a.material.color = new Color(a.material.color.r, a.material.color.g, a.material.color.b, 1f);
        }
    }

    public void UnPlaced()
    {
        Cursor.visible = true;
        OnDestroyObject();
    }
}
