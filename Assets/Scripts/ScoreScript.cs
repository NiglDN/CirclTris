using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public static int scoreValue = 0;
    Text score;
    public  static int currHighScore;
    public static AudioClip clearSound;
    static AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        clearSound = Resources.Load<AudioClip> ("TetrisSound");
        audioSource = GetComponent<AudioSource> ();
        score = GetComponent<Text> ();
        currHighScore = PlayerPrefs.GetInt("HScore");
        scoreValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        score.text = "Score: " + scoreValue;
    }

    public static void updatePoints(int lines) {
        if (lines == 1){
            audioSource.PlayOneShot(clearSound);
            scoreValue += 40 * LevelScript.levelValue;
        }
        if (lines == 2){
            audioSource.PlayOneShot(clearSound);
            scoreValue += 100 * LevelScript.levelValue;
        }
        if (lines == 3){
            audioSource.PlayOneShot(clearSound);
            scoreValue += 300 * LevelScript.levelValue;
        }
        if (lines == 4){
            audioSource.PlayOneShot(clearSound);
            scoreValue += 1200 * LevelScript.levelValue;
        }

        if (scoreValue> currHighScore) PlayerPrefs.SetInt("HScore", scoreValue);





        LevelScript.updateLevel(lines);
    }

}
