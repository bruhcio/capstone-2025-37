using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RP_Symbol_1 : RP_Symbol
{
    public override IEnumerator SpecialEffect(List<int> foundPositions)
    {
        yield return base.SpecialEffect(foundPositions);
    }
}
