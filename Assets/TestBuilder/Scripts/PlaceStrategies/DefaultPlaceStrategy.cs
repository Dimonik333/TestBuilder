using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultPlaceStrategy", menuName = "PlaceStrategies/Default")]
public sealed class DefaultPlaceStrategy : PlaceStrategy
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

            if (!result || _applyOffsetOnHit)
            {
                var rotation = Quaternion.Euler(0, gameCamera.Rotation.eulerAngles.y, 0) * place.rotation * offset.Rotation;
                place.rotation = rotation;
            }

            itemTransform.SetPositionAndRotation(place.position, place.rotation);

            if (!result)
                return false;

            for (int i = 0; i < _targetConditions.Count; i++)
                if (!_targetConditions[i].CheckHit(hit))
                    return false;

            return true;
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
