using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public void ResetHighScore()
    {
        PlayerPrefs.SetInt("HScore", 0);
        Debug.Log("Highscore reset");
    }
}
