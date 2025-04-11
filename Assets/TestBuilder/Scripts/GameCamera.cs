using UnityEngine;

public sealed class GameCamera : MonoBehaviour
{
    public Transform Transform;
    public Vector3 Position => Transform.position;
    public Vector3 Direction => Transform.forward;
    public Quaternion Rotation => transform.rotation;
}
