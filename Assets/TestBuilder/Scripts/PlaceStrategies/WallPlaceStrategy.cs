using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WallPlaceStrategy", menuName = "PlaceStrategies/Wall")]
public sealed class WallPlaceStrategy : PlaceStrategy
{
    [SerializeField] private RaycastStrategy _raycastStrategy;
    [SerializeField] private PlaceOnPointStrategy _placeOnPointStrategy;
    [SerializeField] private bool _applyOffsetOnHit = true;
    [SerializeReference] private List<RaycastTargetCondition> _targetConditions = new();

    public override bool TryPlace(GameCamera gameCamera, Transform itemTransform, PlacementOffset offset)
    {
        if (_raycastStrategy.Raycast(gameCamera, out var hit, out var place))
        {
            var result = _placeOnPointStrategy.TryPlaceOnPoint(hit, out place);
            
            if (_applyOffsetOnHit)
            {
                var rotation = Quaternion.Euler(0, gameCamera.Rotation.eulerAngles.y, 0) * place.rotation * offset.Rotation;
                place.rotation = rotation;
            }

            itemTransform.SetPositionAndRotation(place.position, place.rotation);
            for (int i = 0; i < _targetConditions.Count; i++)
                if (!_targetConditions[i].CheckHit(hit))
                    return false;
            return result;
        }
        else
        {
            var rotation = place.rotation * offset.Rotation;
            itemTransform.SetPositionAndRotation(place.position, rotation);
            return false;
        }
    }

#if UNITY_EDITOR
    [ContextMenu("AddCheckLayer")]
    private void AddCheckLayer()
    {
        _targetConditions.Add(new TargetLayerCondition());
    }
#endif
}
