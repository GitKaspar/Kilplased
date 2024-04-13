using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Diagnostics;

public class Score : MonoBehaviour
{
    private int score = 0;
    public TMP_Text scoreText;

    void Update()
    {
        scoreText.text = score.ToString();

        if (PauseMenu.GameIsPaused == false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                score++;

                UnityEngine.Debug.Log("Score: " + score);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                score+= 20;

                UnityEngine.Debug.Log("Score: " + score);
            }

            if (score >= 100)
            {
                UnityEngine.Debug.Log("Reached 100");
            }
        }
    }
}
