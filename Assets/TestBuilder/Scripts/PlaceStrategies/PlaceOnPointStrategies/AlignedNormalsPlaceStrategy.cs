using UnityEngine;

[CreateAssetMenu(fileName = "AlignedNormalsStrategy", menuName = "PlaceOnPoint Strategies/AlignedNormals")]
public sealed class AlignedNormalsPlaceStrategy : PlaceOnPointStrategy
{
    [SerializeField] private Vector3 _planeNormal = Vector3.up;
    [SerializeField] private float _dotThreshold = .99f;
    public override bool TryPlaceOnPoint(in RaycastHit hit, out PositionAndRotation place)
    {
        var normal = hit.normal;
        var forward = normal;
        forward.y = 0;

        place = new PositionAndRotation()
        {
            position = hit.point,
            rotation = Quaternion.identity
        };

        var dot = Vector3.Dot(_planeNormal, normal);
        return dot >= _dotThreshold;
    }
}