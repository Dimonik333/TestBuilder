using UnityEngine;


/// <summary>
/// Чёртёж объекта.
/// Представляет сам объект, его превью и то как и куда его можно разместить.
/// </summary>
[CreateAssetMenu(fileName = "ItemBlueprint", menuName = "Custom Create/Item Blueprint")]
public sealed class ItemBlueprint : ScriptableObject
{
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private PreviewItem _previewPrefab;
    [SerializeField] private PlaceStrategy _placeStrategy;

    public GameObject ItemPrefab => _itemPrefab;
    public PreviewItem PreviewPrefab => _previewPrefab;

    public PlaceStrategy PlaceStrategy => _placeStrategy;
}
