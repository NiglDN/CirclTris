using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{

    Text Highscoretext;
    public static int Highscore;

    // Start is called before the first frame update
    void Start()
    {
        Highscoretext = GetComponent<Text>();
        Highscore = PlayerPrefs.GetInt("HScore");
    }

    // Update is called once per frame
    void Update()
    {
        Highscoretext.text = "Highscore: " + Highscore;
    }
}
