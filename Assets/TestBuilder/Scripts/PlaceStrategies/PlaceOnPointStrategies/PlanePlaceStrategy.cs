using UnityEngine;

[CreateAssetMenu(fileName = "PlaneStrategy", menuName = "PlaceOnPoint Strategies/Plane")]

public sealed class PlanePlaceStrategy : PlaceOnPointStrategy
{
    [SerializeField] private Vector3 _planeNormal = Vector3.up;
    [SerializeField] private float _dotThreshold = 0.01f;

    public override bool TryPlaceOnPoint(in RaycastHit hit, out PositionAndRotation place)
    {
        var normal = hit.normal;
        var forward = normal;
        forward.y = 0;
        var rotation = forward.sqrMagnitude < 0.01
            ? Quaternion.identity
            : Quaternion.LookRotation(forward, _planeNormal);


        place = new PositionAndRotation()
        {
            position = hit.point,
            rotation = rotation
        };

        var dot = Vector3.Dot(_planeNormal, normal);
        return Mathf.Abs(dot) <= _dotThreshold;
    }
}
