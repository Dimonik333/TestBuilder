using UnityEngine;

public abstract class PlaceOnPointStrategy : ScriptableObject
{
    public abstract bool TryPlaceOnPoint(in RaycastHit hit, out PositionAndRotation place);
}
