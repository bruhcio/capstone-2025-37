using LKAIROS.Assist;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchToGameScene : MonoBehaviour
{
    public string sceneName;

    public void GoTargetScene()
    {
        SoundManager.instance.Play("Sounds/SFX/ChangeTab", false, false, 0, "SFX");
        SceneManager.LoadScene(sceneName);
    }
}
