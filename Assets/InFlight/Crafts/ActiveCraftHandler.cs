using Crafts;
using PobreConnection;
using System;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;
using UnityEngine.Networking;

using Debug = UnityEngine.Debug;

public class ActiveCraftHandler : MonoBehaviour
{
    private Stopwatch stopwatch;

    public static int activeCraftId;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(GetNewCraftID());
        stopwatch = Stopwatch.StartNew();
    }

    void Update()
    {
        if (stopwatch.ElapsedMilliseconds > 250)
        {
            StartCoroutine(PostCraftData());
            stopwatch.Restart();
        }
    }

    IEnumerator GetNewCraftID()
    {
        string uri = $"http://72.208.23.22:5096/api/pobresim/{ConnectionManager.currentServer}/newcraftid";

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
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    activeCraftId = Int32.Parse(webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    IEnumerator PostCraftData()
    {
        // Debug.Log("Sending craft data maybe hopefully please");
        string uri = $"http://72.208.23.22:5096/api/pobresim/{ConnectionManager.currentServer}/sendcraftdata";
        Vector3 linearVelocity = GetComponent<Rigidbody>().linearVelocity;
        Vector3 angularVelocity = GetComponent<Rigidbody>().angularVelocity;

        Console.WriteLine(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, JsonUtility.ToJson(
            new CraftData(
                activeCraftId,
                "didy",
                DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                1,
                new float[] { transform.position.x, transform.position.y, transform.position.z },
                new float[] { transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w },
                new float[] { linearVelocity.x, linearVelocity.y, linearVelocity.z },
                new float[] { angularVelocity.x, angularVelocity.y, angularVelocity.z }
            )), "application/json")
        )
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
                    break;
            }
        }
    }
}
