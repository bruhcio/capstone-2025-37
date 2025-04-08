using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RP_Symbol : MonoBehaviour
{
    public virtual int GetBaseRevenue(int csvBaseRevenue)
    {
        return csvBaseRevenue;
    }

    public virtual void BindSpecialEffect()
    {

    }
}
