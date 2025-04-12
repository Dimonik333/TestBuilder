using UnityEngine;

/// <summary>
/// ���������, ���������� ��� ����� ��������� ������
/// </summary>
public abstract class PlaceStrategy: ScriptableObject
{
    public abstract bool TryPlace(GameCamera gameCamera, Transform itemTransform, PlacementOffset offset);
}
