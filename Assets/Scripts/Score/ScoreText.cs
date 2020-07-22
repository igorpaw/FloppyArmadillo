using System;
using System.Collections;
using System.Collections.Generic;
using CoreControls;
using UnityEngine;
using UnityEngine.UI;

namespace Score
{
    [RequireComponent(typeof(Text))]
    public class ScoreText : MonoBehaviour
    {
        private Text score;

        private void Start()
        {
            score = GetComponent<Text>();
            score.text = "Score: " + GameManager.Instance.Score;
        }
    }
}

