using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RP_Symbol_0 : RP_Symbol
{
    public override IEnumerator SpecialEffect()
    {
        Debug.Log("specical effecT!");

        for(int i = 0; i < 30; i++)
        {
            yield return null;
        }
        Debug.Log("yeah");

        yield return base.SpecialEffect();
    }
}
