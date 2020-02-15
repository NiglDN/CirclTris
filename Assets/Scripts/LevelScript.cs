using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelScript : MonoBehaviour
{
    public static int levelValue = 1;
    public static int levelUpValue = 0;
    Text level;
    // Start is called before the first frame update
    void Start()
    {
        level = GetComponent<Text> ();
    }

    // Update is called once per frame
    void Update()
    {
        level.text = "Level: " + levelValue;
    }

    public static void updateLevel(int lines) {
        levelUpValue += lines;
        if (levelUpValue >= 10){
            levelValue++;
            levelUpValue = 0;
        }
    }
}
