using System;
using UnityEngine;
using UnityEngine.UI;

namespace Score
{
    [RequireComponent(typeof(Text))]
    public class HighScoreText : MonoBehaviour
    {
        private Text highScore;

        private void OnEnable()
        {
            highScore = GetComponent<Text>();
            highScore.text = "High Score: " +  PlayerPrefs.GetInt("HighestScore").ToString();
        }
    }
}

