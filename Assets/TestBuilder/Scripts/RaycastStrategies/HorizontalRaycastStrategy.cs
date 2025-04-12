using UnityEngine;

[CreateAssetMenu(fileName = "Horizontal Raycast Strategy", menuName = "Raycast Strategy")]
public sealed class HorizontalRaycastStrategy : RaycastStrategy
{
    [SerializeField] private float _forwardDistance;
    [SerializeField] private float _downDistance;
    [SerializeField] private LayerMask _layerMask;

    public override bool Raycast(GameCamera camera, out RaycastHit hit, out PositionAndRotation defaultPlace)
    {
        var direction = camera.Direction;
        var origin = camera.Position;

        var forwardRay = new Ray(origin, direction);
        var downRayPosition = origin + direction * _forwardDistance;
        var downRay = new Ray(downRayPosition, Vector3.down);

        defaultPlace = new PositionAndRotation()
        {
            position = downRayPosition,
            rotation = Quaternion.Euler(0, camera.Rotation.eulerAngles.y, 0)
        };

        Debug.DrawRay(forwardRay.origin, forwardRay.direction * _forwardDistance);
        if (Physics.Raycast(forwardRay, out hit, _forwardDistance, _layerMask)) 
            return true;

        Debug.DrawRay(downRay.origin, downRay.direction * _downDistance);
        if (Physics.Raycast(downRay, out hit, _downDistance, _layerMask))
            return true;
     
        return false;
    }
}
