using System.Collections.Generic;
using UnityEngine;

public class Gather : MonoBehaviour, IPlaceable, IPickable, IElectricNode
{
    [SerializeField] private float cooldownTimer;
    [SerializeField] private Transform socketInput;
    [SerializeField] private Vector2Int objectSizeInAxis;
    [SerializeField] private GameObject visual;
    [SerializeField] private int index;
    [SerializeField] private Vector2Int range;
    [SerializeField] private SourceType depositType;
    public GameObject Orientation { get => visual; set => visual = value; }
    public Vector2Int GetSize { get => objectSizeInAxis; set => objectSizeInAxis = value; }
    public Vector3Int PlacedPosition { get; set; }
    public int Index { get => index; set => value = index; }
    public Vector2Int GetRange { get => range; set => range = value; }
    public bool IsPlaced { get; set; }
    public List<IElectricNode> Neighbours { get; set; } = new List<IElectricNode>();
    public bool Power { get; set; } = false;
    public Transform ConnectSocket { get => socketInput; set => socketInput = value; }

    private List<Vector3Int> positionsInRange = new List<Vector3Int>();
    private Animator anim;
    private Collider objectCollider;
    void Awake()
    {
        objectCollider = GetComponent<Collider>();
        objectCollider.enabled = false;
        anim = GetComponentInChildren<Animator>();
    }
    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
    [ContextMenu("Harvest")]
    public void Harvest()
    {
        List<GridSpec> gridCheck = new List<GridSpec>();
        gridCheck = GridSystem.Instance.SearchInGrid(positionsInRange);
        foreach (var a in gridCheck)
        {
            if (a.PlacedObject.PlacedPosition != PlacedPosition)
            {
                if (a.PlacedObject.GetGameObject().TryGetComponent(out Deposits deposit))
                {
                    deposit.Dig(1);
                }
            }
        }
    }
    public void Placed()
    {
        positionsInRange.Clear();
        CalculatePositions();
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

    [ContextMenu("Delete Object On Grid")]
    public void OnDestroyObject()
    {
        GridSystem.Instance.DeleteOnGrid(PlacedPosition, this);
        Destroy(GetGameObject());
    }
    private void CalculatePositions()
    {
        positionsInRange.Clear();


        Vector3 center = transform.position + new Vector3(GetSize.x * 0.5f, 0f, GetSize.y * 0.5f);

        int startX = Mathf.FloorToInt(center.x - GetRange.x * 0.5f);
        int endX = Mathf.FloorToInt(center.x + GetRange.x * 0.5f);

        int startZ = Mathf.FloorToInt(center.z - GetRange.y * 0.5f);
        int endZ = Mathf.FloorToInt(center.z + GetRange.y * 0.5f);

        for (int z = startZ; z < endZ; z++)
        {
            for (int x = startX; x < endX; x++)
            {
                positionsInRange.Add(new Vector3Int(x, 0, z));
            }
        }
    }
    void OnDrawGizmos()
    {
        Vector3 center = transform.position + new Vector3(GetSize.x * 0.5f, 0.2f, GetSize.y * 0.5f);
        Gizmos.DrawCube(center, new Vector3(GetRange.x, 1, GetRange.y));
    }


    //Electric
    public void PowerChanged(bool powerState, Material materialState)
    {
        MeshRenderer[] robe = ConnectSocket.GetComponentsInChildren<MeshRenderer>();
        foreach (var a in robe)
        {
            a.material = materialState;
        }
        if (powerState)
        {
            if (!Power)
            {
                anim.SetBool("OnCut", true);
                InvokeRepeating(nameof(Harvest), 1f, cooldownTimer);
            }
            Power = true;
        }
        else
        {
            Power = false;

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
        if (!Power)
        {
            if (target.Power)
            {
                anim.SetBool("OnCut", true);
                InvokeRepeating(nameof(Harvest), .5f, cooldownTimer);
            }
        }
        return true;
    }
}
