using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesCache : MonoBehaviour
{
    public static Sprite[] symbolSprites;
    public static Sprite[] symbolBackgroundSprites;




    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void InitCache()
    {
        symbolSprites = Resources.LoadAll<Sprite>("Sprites/Symbols");
        symbolBackgroundSprites = Resources.LoadAll<Sprite>("Sprites/CalendarBackground");
    }
}
