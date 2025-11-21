using UnityEngine;

public class Deposits : MonoBehaviour, IPlaceable
{
    [SerializeField] private SourcesSO Resource; 
    [SerializeField] private Vector2Int objectSizeInAxis;
    [SerializeField] private GameObject visual;
    [SerializeField] private int index;
    public GameObject Orientation { get => visual; set => visual = value; }
    public Vector2Int GetSize { get; set; }
    public Vector3Int PlacedPosition { get; set; }
    public int Index { get => index ; set => value = index ; }

    void Awake()
    {
        GetSize = objectSizeInAxis;
    }
    public void Dig()
    {
        Debug.Log("Diged: " + Resource.name);
        Instantiate(Resource.SourcePrefab);
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;    
    }
    public void Placed()
    {
    }

    
    [ContextMenu("Destroy Tree")]
    public void OnDestroyObject()
    {
        GridSystem.Instance.DeleteOnGrid(PlacedPosition,this);
        Destroy(gameObject);
    }
}
