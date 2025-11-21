using UnityEngine;

public class Deposits : MonoBehaviour, IPlaceable
{
    [SerializeField] private SourcesSO Resource; 
    [SerializeField] private Vector2Int objectSizeInAxis;
    [SerializeField] private GameObject visual;
    [SerializeField] private int index;
    [SerializeField] private Vector2Int range;
    public GameObject Orientation { get => visual; set => visual = value; }
    public Vector2Int GetSize { get; set; }
    public Vector3Int PlacedPosition { get; set; }
    public int Index { get => index ; set => value = index ; }
    public Vector2Int GetRange { get => range; set => range = value; }

    void Awake()
    {
        GetSize = objectSizeInAxis;
    }
    public void Dig()
    {
        GameObject wood = Instantiate(Resource.SourcePrefab);
        wood.transform.position = new Vector3(transform.position.x,transform.position.y + wood.transform.localScale.y/2,transform.position.z);
        OnDestroyObject();
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;    
    }
    public void Placed()
    {
        
    }
       void OnDrawGizmos()
    {
        Vector3 center = transform.position + new Vector3(GetSize.x * 0.5f, 0.2f, GetSize.y * 0.5f);
        Gizmos.DrawCube(center, new Vector3(GetRange.x, 1, GetRange.y));
    }

    
    [ContextMenu("Destroy Tree")]
    public void OnDestroyObject()
    {
        GridSystem.Instance.DeleteOnGrid(PlacedPosition,this);
        Destroy(gameObject);
    }

    public void UnPlaced()
    {
        OnDestroyObject();
        //Obje Değiştirme Mekaniğinide değiştir

    }
}
