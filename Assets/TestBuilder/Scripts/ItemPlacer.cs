using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ItemPlacer : MonoBehaviour
{
    [SerializeField] private float _distance;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private LayerMask _layerMask;

    private GameObject _target;

    private void Start()
    {
        _target = Instantiate(_prefab);
    }

    private void Update()
    {
        var direction = _cameraTransform.forward;
        var origin = _cameraTransform.position;

        var ray = new Ray(origin, direction);
        if (!Physics.Raycast(ray, out var hitInfo, _distance, _layerMask))
        {
            _target.transform.SetPositionAndRotation(origin + direction * _distance, _cameraTransform.rotation);
            return;
        }
        _target.transform.SetPositionAndRotation(hitInfo.point, _cameraTransform.rotation);
    }
}

public sealed class GameCamera : MonoBehaviour
{
    public Transform Transform;
    public Vector3 Position;
    public Vector3 Direction;
    public Quaternion Rotation;
}


public abstract class RaycastStrategy
{
    //public abstract void GetPositionAndRotation(GameCamera camera);
}

public sealed class HorizontalRaycast : RaycastStrategy
{
    [SerializeField] private float _forwardDistance;
    [SerializeField] private float _downDistance;
    [SerializeField] private LayerMask _layerMask;

    public bool Raycast(GameCamera camera, out RaycastHit hit)
    {
        var direction = camera.Direction;
        var origin = camera.Position;

        var forwardRay = new Ray(origin, direction);

        if (Physics.Raycast(forwardRay, out hit, _forwardDistance, _layerMask))
            return true;

        var downRayPosition = origin + direction * _forwardDistance;
        var downRay = new Ray(downRayPosition, Vector3.down);

        if (Physics.Raycast(forwardRay, out hit, _downDistance, _layerMask))
            return true;

        return false;
    }
}

public sealed class DefaultPlaceStrategy
{
    [SerializeField] private float _distance;
    public PositionAndRotation Temp(GameCamera gameCamera)
    {
        var direction = gameCamera.Direction;
        direction.y = 0;
        direction.Normalize();

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
        return Vector3.Dot(_up, hit.normal) == 1;
    }
}



public sealed class HorizontalPlaceStrategy
{
    private HorizontalRaycast _raycastStrategy;
    private HorizontalCondition _placeCondition;
    private DefaultPlaceStrategy _placeStrategy;

    public void Temp(GameCamera gameCamera, PreviewItem item)
    {
        if (_raycastStrategy.Raycast(gameCamera, out var hit))
        {
            var position = hit.point;
            var rotation = Quaternion.Euler(0, gameCamera.Rotation.eulerAngles.y, 0);

            if (_placeCondition.Check(hit))
            {
                if (item.Check(0))
                { }
                // true;
            }
            else
            {
                // false;
            }
        }

        var place = _placeStrategy.Temp(gameCamera);
    }




}



public sealed class Item : ScriptableObject
{
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private GameObject _previewItemPrefab;
}
