using System;
using UnityEngine;


[Serializable]
public abstract class RaycastTargetCondition
{
    public abstract bool CheckHit(RaycastHit hit);

}
