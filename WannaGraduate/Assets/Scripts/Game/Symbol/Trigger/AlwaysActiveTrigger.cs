using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 항상 발동하는 트리거
public class AlwaysActiveTrigger : ITrigger
{
    public bool Evaluate(TriggerContext context, ref List<Vector2Int> foundPositions)
    {
        return true;
    }
}
