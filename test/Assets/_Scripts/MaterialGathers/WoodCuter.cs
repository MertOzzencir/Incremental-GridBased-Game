using UnityEngine;

public class WoodCuter : MonoBehaviour, IPlaceable,IPickable
{
    [SerializeField] private Vector2Int objectSizeInAxis;
    [SerializeField] private GameObject visual;
    [SerializeField] private int index;

    public GameObject Orientation { get => visual; set => value = visual; }
    public Vector2Int GetSize { get =>objectSizeInAxis; set=> objectSizeInAxis=value; }
    public Vector3Int PlacedPosition { get ; set ; }
    public int Index { get=> index ; set => value = index ; }

   
    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public void Placed()
    {
        MeshRenderer[] childObjects = GetComponentsInChildren<MeshRenderer>();
        foreach (var a in childObjects)
        {
            a.material.color = new Color(a.material.color.r, a.material.color.g, a.material.color.b, 1f);
        }
    }
    public void Picked()
    {
        MeshRenderer[] childObjects = GetComponentsInChildren<MeshRenderer>();
        foreach (var a in childObjects)
        {
            a.material.color = new Color(a.material.color.r, a.material.color.g, a.material.color.b, 0.2f);
        }
    }

    [ContextMenu("Delete Object On Grid")]
    public void OnDestroyObject()
    {
        GridSystem.Instance.DeleteOnGrid(PlacedPosition,this);
        Destroy(GetGameObject());
    }
}
