using Crafts;
using System.Collections.Generic;
using UnityEngine;

public class GlorifiedCraftList : MonoBehaviour
{
    public static List<Craft> crafts;

    public static void RemoveCraft(int id)
    {
        for (int i = 0; i < crafts.Count; i++)
        {
            if (crafts[i].id == id)
            {
                Destroy(crafts[i].craftGameObject);
                crafts.RemoveAt(i);
            }
        }
    }


}
