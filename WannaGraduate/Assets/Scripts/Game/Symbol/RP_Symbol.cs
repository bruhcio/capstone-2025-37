using DominoGames.Core.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RP_Symbol : MonoBehaviour
{
    public virtual IEnumerator SpecialEffect()
    {
        DominoEventSystem.Pub(EEventTypes.OnOneDaySpecialEffectEnd);
        yield break;
    }
}
