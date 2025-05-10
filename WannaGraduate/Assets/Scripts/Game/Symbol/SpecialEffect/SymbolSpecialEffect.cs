using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolSpecialEffectParameter
{

}

public interface ISymbolSpecialEffect
{
    public void Evaluate(List<string> parameter, int selfCalendarIdx, List<int> interactingCalendarIdx);
}