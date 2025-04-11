
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemPlacer : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;
    [SerializeField] private GameCamera _camera;
    [SerializeField] private float _distance;

    [SerializeField] private GameObject _prefab;
    [SerializeField] private LayerMask _layerMask;

    private PreviewItem _previewItem;
    [SerializeField] private HorizontalRaycastStrategy _strategy;
    [SerializeField] private DefaultPlaceStrategy _defaultPlaceStrategy;
    private HorizontalCondition _condition = new();
    private PlaceStrategy _placeStrategy = new();
    private bool _correct;

    public void SetBlueprint(ItemData item)
    {
        _itemData = item;
        enabled = true;

    }
    private void OnEnable()
    {
        _previewItem = Instantiate(_itemData.PreviewPrefab);
    }

    private void OnDisable()
    {
        if (_previewItem != null)
            Destroy(_previewItem.gameObject);
    }

    public void RemoveBlueprint()
    {
        enabled = false;
        _itemData = null;

    }

    private void Update()
    {
        _correct = false;
        if (_strategy.Raycast(_camera, out var hit))
        {
            var place = _placeStrategy.Temp(_camera, hit);
            _previewItem.transform.SetPositionAndRotation(place.position, place.rotation * _rotation);
            _correct = _previewItem.CanPlaced() && _condition.Check(hit);
            _previewItem.SetMode(_correct);
            return;
        }
        var defaultPlace = _defaultPlaceStrategy.Temp(_camera);
        _previewItem.transform.SetPositionAndRotation(defaultPlace.position, defaultPlace.rotation);
        _previewItem.SetMode(false);
    }

    public void Build()
    {
        if (_correct && _previewItem != null)
        {
            var item = Instantiate(_itemData.ItemPrefab);
            item.transform.SetPositionAndRotation(_previewItem.transform.position, _previewItem.transform.rotation);
            Destroy(_previewItem.gameObject);
            _previewItem = null;
            enabled = false;
        }
    }

    private Quaternion _rotation;
    private float _angle = 45;
    [SerializeField] private int _scroll;

    public void SetScroll(Vector2 scroll)
    {
        _scroll += (int)scroll.y;
        _scroll = WrapValue(_scroll, 0, 7);
        var angle = _angle * _scroll;
        _rotation = Quaternion.Euler(0, angle, 0);
    }

    int WrapValue(int value, int min, int max)
    {
        int range = max - min + 1; // Диапазон значений, включая границы
        return (value - min + range) % range + min;
    }
}


public abstract class RaycastStrategy
{
    //public abstract void GetPositionAndRotation(GameCamera camera);
}

[Serializable]
public sealed class HorizontalRaycastStrategy : RaycastStrategy
{
    [SerializeField] private float _forwardDistance;
    [SerializeField] private float _downDistance;
    [SerializeField] private LayerMask _layerMask;

    public bool Raycast(GameCamera camera, out RaycastHit hit)
    {
        var direction = camera.Direction;
        var origin = camera.Position;

        var forwardRay = new Ray(origin, direction);

        Debug.DrawRay(forwardRay.origin, forwardRay.direction * _forwardDistance);
        if (Physics.Raycast(forwardRay, out hit, _forwardDistance, _layerMask))
            return true;

        var downRayPosition = origin + direction * _forwardDistance;
        var downRay = new Ray(downRayPosition, Vector3.down);

        Debug.DrawRay(downRay.origin, downRay.direction * _downDistance);
        if (Physics.Raycast(downRay, out hit, _downDistance, _layerMask))
            return true;

        return false;
    }
}

public sealed class PlaceStrategy
{
    public PositionAndRotation Temp(GameCamera camera, in RaycastHit hit)
    {
        var position = hit.point;
        var rotation = Quaternion.Euler(0, camera.Rotation.eulerAngles.y, 0);
        return new PositionAndRotation()
        {
            position = position,
            rotation = rotation
        };
    }
}

[Serializable]
public sealed class DefaultPlaceStrategy
{
    [SerializeField] private float _distance;
    public PositionAndRotation Temp(GameCamera gameCamera)
    {
        var direction = gameCamera.Direction;
        var rotation = gameCamera.Rotation;
        var itemPosition = gameCamera.Position + direction * _distance;
        var itemRotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);

        return new PositionAndRotation()
        {
            position = itemPosition,
            rotation = itemRotation,
        };
    }
}

public struct PositionAndRotation
{
    public Vector3 position;
    public Quaternion rotation;
}

public sealed class HorizontalCondition
{
    private Vector3 _up = Vector3.up;

    public bool Check(in RaycastHit hit)
    {
        return Vector3.Dot(_up, hit.normal) >= .9;
    }
}



[CreateAssetMenu(fileName = "ItemBlueprint", menuName = "Custom Create/Item Blueprint")]
public sealed class ItemData : ScriptableObject
{
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private PreviewItem _previewPrefab;

    public GameObject ItemPrefab => _itemPrefab;
    public PreviewItem PreviewPrefab => _previewPrefab;
}
