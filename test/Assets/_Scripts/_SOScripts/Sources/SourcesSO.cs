using UnityEngine;

[CreateAssetMenu]
public class SourcesSO : ScriptableObject
{
    public string SourceName;
    public SourceType SourceType;
    public Sprite UIIcon;
    public GameObject SourcePrefab;

    
}

public enum SourceType
{
    Wood,
    Plank
}
