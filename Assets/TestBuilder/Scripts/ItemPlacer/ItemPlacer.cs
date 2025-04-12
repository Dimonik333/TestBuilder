using UnityEngine;

/// <summary>
/// Отвечает за размещение объекта по указанному чертежу и отображение его превью.
/// </summary>
public class ItemPlacer : MonoBehaviour
{
    private PreviewItem _currentPreviewItem;
    private readonly PlacementOffset _placementOffset = new();

    [SerializeField] private GameCamera _camera;
    [SerializeField] private float _rotationAngle = 45;
    [SerializeField] private ItemBlueprint _blueprint;

    private int _scroll;
    private bool _isPlaceCorrect;

    public void SetBlueprint(ItemBlueprint item)
    {
        if (_currentPreviewItem != null)
            Destroy(_currentPreviewItem.gameObject);

        _blueprint = item;
        _currentPreviewItem = Instantiate(_blueprint.PreviewPrefab);
        _isPlaceCorrect = false;
        _scroll = 0;
        enabled = true;
    }

    public void RemoveBlueprint()
    {
        if (_currentPreviewItem != null)
            Destroy(_currentPreviewItem.gameObject);
        _blueprint = null;

        enabled = false;
    }

    public void Build()
    {
        if (!_isPlaceCorrect || _currentPreviewItem == null)
            return;

        var item = Instantiate(_blueprint.ItemPrefab);
        item.transform.SetPositionAndRotation(_currentPreviewItem.transform.position, _currentPreviewItem.transform.rotation);
        Destroy(_currentPreviewItem.gameObject);
        _currentPreviewItem = null;
        _isPlaceCorrect = false;
        enabled = false;
    }

    public void SetScroll(Vector2 scroll)
    {        
        _scroll += (int)scroll.y;
        _scroll = RepeatValue(_scroll, 0, 7);
        _placementOffset.Rotation = Quaternion.Euler(0, _rotationAngle * _scroll, 0); ;
    }

    private void Update()
    {
        _isPlaceCorrect = _blueprint.PlaceStrategy.TryPlace(_camera, _currentPreviewItem.transform, _placementOffset);
        _isPlaceCorrect &= !_currentPreviewItem.CheckCollision();
        _currentPreviewItem.SetViewMode(_isPlaceCorrect);
    }

    private int RepeatValue(int value, int min, int max)
    {
        int range = max - min + 1;
        return (value - min + range) % range + min;
    }
}
