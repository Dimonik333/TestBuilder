using UnityEngine;

/// <summary>
/// Стратегия, определяет как можно размещать объект
/// </summary>
public abstract class PlaceStrategy: ScriptableObject
{
    public abstract bool TryPlace(GameCamera gameCamera, Transform itemTransform, PlacementOffset offset);
}
