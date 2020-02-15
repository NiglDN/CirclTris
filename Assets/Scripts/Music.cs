using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{

    public GameObject musicon;
    public GameObject musicoff;

    public void OnPressed()
    {

        PlayerPrefs.SetInt("Music", 1);

        musicoff.SetActive(true);
        musicon.SetActive(false);
        Debug.Log("music off");
    }

    public void OffPressed()
    {
        PlayerPrefs.SetInt("Music", 0);
        musicon.SetActive(true);
        musicoff.SetActive(false);
        Debug.Log("music on");
    }

}