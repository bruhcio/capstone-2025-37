using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesCache : MonoBehaviour
{
    public static Dictionary<int, Sprite> symbolSprites;
    public static Sprite[] symbolBackgroundSprites;




    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void InitCache()
    {
        var sprites = Resources.LoadAll<Sprite>("Sprites/Symbols");
        for(int i = 0; i < sprites.Length; i++)
        {
            symbolSprites.Add(int.Parse(sprites[i].name), sprites[i]);
        }
        symbolBackgroundSprites = Resources.LoadAll<Sprite>("Sprites/CalendarBackground");
    }

    public static Sprite GetSymbolSprite(int spriteId)
    {
        if (symbolSprites.ContainsKey(spriteId))
        {
            return symbolSprites[spriteId];
        }

        return symbolSprites[0];
    }
}
