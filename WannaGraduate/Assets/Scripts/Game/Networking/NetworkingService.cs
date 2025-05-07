using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using Defective.JSON;
using UnityEngine.Events;
using System.Security.Cryptography;
using System.Text;
using System;
using System.IO;
using Assaz;

public class NetworkingService : MonoBehaviour
{
    public static string gameId = "com.DominoGames.WannaGraduate";
    public static string channelId = "";
    public static string uid = "";

    static SHA256 sha256 = null;

    [Button]
    public static void LoadRankingOne(string boardId, UnityAction<JSONObject> responseCallback)
    {
        JSONObject data = new JSONObject();
        data.AddField("gameId", gameId);
        data.AddField("channelId", channelId);
        data.AddField("uid", uid);

        data.AddField("boardId", boardId);
        data.AddField("withscore", false);
        RequestBuilderStarter.New("lond_ranking_one").SetData(data).BlockingType(RequestBlocking.NONE).ResponseCallback(responseCallback).Send();
    }

    [Button]
    public static void LoadRankings(string boardId, int startRank, int endRank, UnityAction<JSONObject> responseCallback)
    {
        JSONObject data = new JSONObject();
        data.AddField("gameId", gameId);
        data.AddField("channelId", channelId);
        data.AddField("uid", uid);

        data.AddField("boardId", boardId);
        data.AddField("startRank", startRank);
        data.AddField("endRank", endRank);
        RequestBuilderStarter.New("load_rankings").SetData(data).BlockingType(RequestBlocking.NONE).ResponseCallback(responseCallback).Send();
    }

    [Button]
    public static void PostRanking(string boardId, double postValue, JSONObject attachmentData, UnityAction onResponse = null)
    {
        JSONObject data = new JSONObject();
        data.AddField("gameId", gameId);
        data.AddField("channelId", channelId);
        data.AddField("uid", uid);

        data.AddField("boardId", boardId);
        data.AddField("postValue", postValue);
        data.AddField("attachmentData", attachmentData.ToString());
        
        Debug.Log("Request Sending...");

        RequestBuilderStarter.New("post_ranking").RequestType(RequestType.POST).BlockingType(RequestBlocking.NONE).SetData(data).ResponseCallback((obj) =>
        {
            onResponse?.Invoke();
        }).Send();
    }





    public static void LoadUserData(UnityAction<JSONObject> onResponse = null)
    {
        JSONObject data = new JSONObject();
        data.AddField("gameId", gameId);
        data.AddField("uid", uid);

        RequestBuilderStarter.New("user_data/load_user").RequestType(RequestType.GET).BlockingType(RequestBlocking.NONE).SetData(data).ResponseCallback((obj) =>
        {
            if (obj["isDataExists"].boolValue && obj["result"]["saveFile"].stringValue.Length > 0)
            {
                WriteFile("save", obj["result"]["saveFile"].stringValue);
            }

            Debug.Log(ReadFile("save"));
            onResponse?.Invoke(obj);
        }).Send();
    }
    public static void CreateUserData(UnityAction<JSONObject> onResponse = null)
    {
        JSONObject data = new JSONObject();
        data.AddField("gameId", gameId);
        data.AddField("nickname", PlayerSaveDataModel.data.playerName);
        data.AddField("uid", uid);

        RequestBuilderStarter.New("user_data/create_user").RequestType(RequestType.POST).BlockingType(RequestBlocking.NONE).SetData(data).ResponseCallback((obj) =>
        {
            onResponse?.Invoke(obj);
        }).Send();
    }
    public static void CheckIAP(string orderId, int purchased, UnityAction onResponse = null)
    {
        JSONObject data = new JSONObject();
        data.AddField("gameId", gameId);
        data.AddField("uid", uid);
        data.AddField("orderId", orderId);
        data.AddField("purchased", purchased);

        RequestBuilderStarter.New("user_data/iap").RequestType(RequestType.POST).BlockingType(RequestBlocking.NONE).SetData(data).ResponseCallback((obj) =>
        {
            onResponse?.Invoke();
        }).Send();
    }

    public static void SaveDataFile(UnityAction<JSONObject> onResponse = null)
    {
        string fileString = ReadFile("save");

        JSONObject data = new JSONObject();
        data.AddField("gameId", gameId);
        data.AddField("uid", uid);
        data.AddField("saveFile", fileString);

        RequestBuilderStarter.New("user_data/save_data_file").RequestType(RequestType.POST).BlockingType(RequestBlocking.NONE).SetData(data).ResponseCallback((obj) =>
        {
            onResponse?.Invoke(obj);
        }).Send();
    }

    public static void WriteFile(string fileName, string context)
    {
        string filePath = Application.persistentDataPath + "/" + fileName;
        File.WriteAllText(filePath, context);
    }

    public static string ReadFile(string fileName)
    {
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/" + fileName);
        string value = "";

        if (fileInfo.Exists)
        {
            StreamReader reader = new StreamReader(Application.persistentDataPath + "/" + fileName);
            value = reader.ReadToEnd();
            reader.Close();
        }
        else
            value = "파일이 없습니다.";

        return value;
    }






    public static void GetLatestVersion(UnityAction<JSONObject> onResponse = null)
    {
        JSONObject data = new JSONObject();
        data.AddField("gameId", gameId);
        RequestBuilderStarter.New("latest_version").RequestType(RequestType.GET).BlockingType(RequestBlocking.NONE).SetData(data).ResponseCallback(obj =>
        {
            onResponse?.Invoke(obj);
        }).Send();
    }



    public static string GetHashedUID()
    {
        sha256 ??= new SHA256Managed();

        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(uid));
        return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
    }
}