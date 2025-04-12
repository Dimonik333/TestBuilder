using System;
using UnityEngine;

[Serializable]
public sealed class TargetLayerCondition : RaycastTargetCondition
{
    [SerializeField] private LayerMask _permittedLayers;
    public override bool CheckHit(RaycastHit hit)
    {
        var layer = hit.collider.gameObject.layer;
        return (_permittedLayers.value & (1 << layer)) != 0;
    }
}