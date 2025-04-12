using UnityEngine;

public abstract class RaycastStrategy : ScriptableObject
{
    public  abstract bool Raycast(GameCamera camera, out RaycastHit hit, out PositionAndRotation defaultPlace);
}
