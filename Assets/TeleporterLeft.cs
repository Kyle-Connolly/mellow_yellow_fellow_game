using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterLeft : MonoBehaviour
{
    public Transform landingZone;
    public string travelList = "|Fellow|Ghost|";

    public void OnTriggerEnter(Collider other)
    {
        if (travelList.Contains(string.Format("|{0}|", other.tag)))
        {
            other.transform.position = landingZone.transform.position;
        }
    }
}
