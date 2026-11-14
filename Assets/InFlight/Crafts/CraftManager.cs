using Crafts;
using PobreConnection;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

public class CraftManager : MonoBehaviour
{

    private Stopwatch stopwatch;

    [SerializeField] private GameObject prefabToClone;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Debug.Log(JsonUtility.ToJson(new CraftDatasContainer(new List<CraftData>{
        //    new CraftData(
        //        10293,
        //        "didy",
        //        new DateTimeOffset().ToUnixTimeSeconds(),
        //        1,
        //        new float[]{ 0.0f,10.0f,0.0f },
        //        new float[]{ 0.0f,0.0f,0.0f,1.0f },
        //        new float[]{ 0.0f,0.0f,0.0f },
        //        new float[]{ 0.0f,0.0f,100.0f }
        //    )
        //})));

        GlorifiedCraftList.crafts = new List<Craft>();

        stopwatch = Stopwatch.StartNew();
    }

    void Update()
    {
        if(stopwatch.ElapsedMilliseconds > 250)
        {
            StartCoroutine(FetchAllCraftData());
            stopwatch.Restart();
        }
    }

    IEnumerator FetchAllCraftData()
    {
        string uri = $"http://72.208.23.22:5096/api/pobresim/{ConnectionManager.currentServer}/getallcraftdata";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    // Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    updateCraftGameObjects(webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    void updateCraftGameObjects(string json)
    {
        CraftDatasContainer craftDatasContainer = JsonUtility.FromJson<CraftDatasContainer>(json);
        // Debug.Log(JsonUtility.ToJson(craftDatasContainer));

        List<CraftData> craftDatas = craftDatasContainer.craftDatas;
        // Debug.Log(craftDatas[0].id);

        List<int> existingIds = new List<int>();

        foreach (Craft craft in GlorifiedCraftList.crafts)
            existingIds.Add(craft.id);

        List<int> incomingIds = new List<int>();

        foreach (CraftData craftData in craftDatas)
            incomingIds.Add(craftData.id);

        foreach (int existingId in existingIds)
        {
            if (!incomingIds.Contains(existingId))
            {
                GlorifiedCraftList.RemoveCraft(existingId);
                Debug.Log("Removed craft with id " + existingId);
            }
        }

        foreach (CraftData craftData in craftDatas) 
        {
            if (craftData.id == ActiveCraftHandler.activeCraftId)
                continue;
            if (!existingIds.Contains(craftData.id))
            {
                Vector3 position = new Vector3(craftData.position[0], craftData.position[1], craftData.position[2]);
                Quaternion rotation = new Quaternion(craftData.rotation[0], craftData.rotation[1], craftData.rotation[2], craftData.rotation[3]);
                GlorifiedCraftList.crafts.Add(new Craft(craftData, Instantiate(prefabToClone, position, rotation)));
            }

            int item = GlorifiedCraftList.crafts.FindIndex(craft => craft.id == craftData.id);
            Debug.Log($"Updating craft {GlorifiedCraftList.crafts[item].id}...");
            GlorifiedCraftList.crafts[item].updateCraft(craftData);
        }
    }
}
