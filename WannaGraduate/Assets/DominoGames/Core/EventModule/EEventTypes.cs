using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DominoGames.Core.EventSystem
{
    public enum EEventTypes
    {
        OnSpinStart,
        OnSpinEnd,
        
        OnSpecialEffectStart,
        OnOneDaySpecialEffectEnd,
        OnSpecialEffectEnd,

        OnEarnBaseRevenueStart,
        OnEarnBaseRevenueEnd,
    }
}
