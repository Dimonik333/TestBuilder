using UnityEngine;

[CreateAssetMenu(fileName = "FloorPlaceStrategy", menuName = "PlaceStrategies/Floor")]
public sealed class FloorPlaceStrategy : PlaceStrategy
{
    [SerializeField] private RaycastStrategy _raycastStrategy;
    [SerializeField] private PlaceOnPointStrategy _placeOnPointStrategy;

    public override bool TryPlace(GameCamera gameCamera, Transform itemTransform, PlacementOffset offset)
    {
        if (_raycastStrategy.Raycast(gameCamera, out var hit, out var place))
        {
            var result = _placeOnPointStrategy.TryPlaceOnPoint(hit, out place);
            var rotation = Quaternion.Euler(0, gameCamera.Rotation.eulerAngles.y, 0) * place.rotation * offset.Rotation;
            itemTransform.SetPositionAndRotation(place.position, rotation);
            return result;
        }
        else
        {
            var rotation = place.rotation * offset.Rotation;
            itemTransform.SetPositionAndRotation(place.position, rotation);
            return false;
        }
    }
}
