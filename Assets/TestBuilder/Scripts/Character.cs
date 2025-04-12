using UnityEngine;
using UnityEngine.EventSystems;

public sealed class Character : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _verticalClamp = 80f;
    [Space]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _bodyTransform;
    [Space]
    [SerializeField] private float _playerHeight = 2;
    [SerializeField] private float _playerRadius = 0.5f;
    [SerializeField] private LayerMask _layerMask;

    private float _verticalRotation = 0f;

    public void Rotate(Vector2 rotation)
    {
        _bodyTransform.Rotate(Vector3.up * rotation.x);

        _verticalRotation -= rotation.y;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -_verticalClamp, _verticalClamp);
        _cameraTransform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
    }

    public void Move(Vector2 move)
    {
        var moveDirection = transform.right * move.x + transform.forward * move.y;
        var moveDistance = _moveSpeed * Time.deltaTime;
        if (TryMove(ref moveDirection, moveDistance))
            _bodyTransform.position += moveDistance * moveDirection;
    }

    private bool TryMove(ref Vector3 moveDirection, float moveDistance)
    {
        var sourceDirection = moveDirection;

        var downPoint = _bodyTransform.position;
        var upPoint = _bodyTransform.position + Vector3.up * _playerHeight;

        bool canMove = !Physics.CapsuleCast(downPoint, upPoint, _playerRadius, sourceDirection.normalized, moveDistance, _layerMask);
        if (canMove)
            return true;


        Vector3 moveDirX = new Vector3(sourceDirection.x, 0, 0).normalized;
        canMove = Mathf.Abs(sourceDirection.x) > 0.5f &&
            !Physics.CapsuleCast(downPoint, upPoint, _playerRadius, moveDirX, moveDistance, _layerMask);
        if (canMove)
        {
            moveDirection = moveDirX;
            return true;
        }

        Vector3 moveDirZ = new Vector3(0, 0, sourceDirection.z).normalized;
        canMove = Mathf.Abs(sourceDirection.z) > 0.5f &&
            !Physics.CapsuleCast(downPoint, upPoint, _playerRadius, moveDirZ, moveDistance, _layerMask);
        if (canMove)
        {
            moveDirection = moveDirZ;
            return true;
        }

        return false;
    }


}
