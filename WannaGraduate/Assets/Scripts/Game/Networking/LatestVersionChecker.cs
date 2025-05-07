using Assaz;
using BBB;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LatestVersionChecker : MonoBehaviour
{
    public static LatestVersionChecker Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(Instance);
            return;
        }
    }

    [Button]
    public void CheckLatestVersion()
    {
        NetworkingService.GetLatestVersion(data =>
        {
            var versionString = data["result"].ToString();
            if (!CheckVersion(versionString))
            {
                //PopupManager.instance.Popup(StartScenePopupIndex.UpdateInfo_Popup);
                return;
            }
            
            //StartSceneUI.SetStateText("터치해서 게임 시작!"); //Temp Message
            //StartSceneUI.SetSceneLoadButtonActive(true);
        });
    }

    public bool CheckVersion(string versionString)
    {
        try
        {
            List<string> latestVersionCode = versionString.Split('.').ToList();
            List<string> clientVersionCode = Application.version.Split('.').ToList();

            for (int i = latestVersionCode.Count; i < 3; i++)
            {
                latestVersionCode.Add("0");
            }

            if (System.Convert.ToInt32(clientVersionCode[0]) < System.Convert.ToInt32(latestVersionCode[0]))
            {
                return false;
            }
            else if (System.Convert.ToInt32(clientVersionCode[0]) == System.Convert.ToInt32(latestVersionCode[0]))
            {
                if (System.Convert.ToInt32(clientVersionCode[1]) < System.Convert.ToInt32(latestVersionCode[1]))
                {
                    return false;
                }
                else if (System.Convert.ToInt32(clientVersionCode[1]) == System.Convert.ToInt32(latestVersionCode[1]))
                {
                    if (System.Convert.ToInt32(clientVersionCode[2]) < System.Convert.ToInt32(latestVersionCode[2]))
                    {
                        return false;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return true;
        }

        return true;
    }
}
