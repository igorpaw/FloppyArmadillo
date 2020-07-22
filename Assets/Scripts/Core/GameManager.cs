using System;
using System.Collections;
using System.Collections.Generic;
using Controls;
using Score;
using UnityEngine;
using UnityEngine.UI;

namespace CoreControls
{
    enum State
    {
        None,
        Start,
        GameOver,
        Countdown,
    }
    public class GameManager : MonoBehaviour
    {
        public delegate void GameDelagate();

        public static event GameDelagate OnGameStart;
        public static event GameDelagate OnGameOver;
        
        public static GameManager Instance;
        
        [SerializeField]
        private GameObject start;
        [SerializeField]
        private GameObject gameOverPage;
        [SerializeField]
        private GameObject countdown;
        [SerializeField]
        private Text scoreText;

        private int score = 0;

        private bool gameover = true;
        
        public  bool Gameover
        {
            get { return gameover; }
        }
        public int Score
        {
            get { return score; }
        }

        private void OnEnable()
        {
            ClickController.OnPlayerDied += OnPlayerDied;
            ClickController.OnPlayerScored += OnPlayerScored;
            CountdownText.OnCountdownFinished += OnCountdownFinished;
        }

        private void OnDisable()
        {
            CountdownText.OnCountdownFinished -= OnCountdownFinished;
            ClickController.OnPlayerDied -= OnPlayerDied;
            ClickController.OnPlayerScored -= OnPlayerScored;
        }

        private void OnPlayerDied()
        {
            gameover = true;
            int savedScore = PlayerPrefs.GetInt("HighestScore");
            if (score > savedScore)
            {
                PlayerPrefs.SetInt("HighestScore", score);
            }
            SetState(State.GameOver);
        }
        
        private void OnPlayerScored()
        {
            score++;
            scoreText.text = score.ToString();
        }

        private void OnCountdownFinished()
        {
            SetState(State.None);
            OnGameStart();
            score = 0;
            gameover = false;
        }

        private void Awake()
        {
            Instance = this;
        }

        void SetState(State state)
        {
            switch (state)
            {
                case State.None:
                    start.SetActive(false);
                    gameOverPage.SetActive(false);
                    countdown.SetActive(false);
                    break;
                case State.Start:
                    start.SetActive(true);
                    gameOverPage.SetActive(false);
                    countdown.SetActive(false);
                    break;
                case State.GameOver:
                    start.SetActive(false);
                    gameOverPage.SetActive(true);
                    countdown.SetActive(false);
                    break;
                case State.Countdown:
                    start.SetActive(false);
                    gameOverPage.SetActive(false);
                    countdown.SetActive(true);
                    break;
            }
        }

        public void ConfirmGameOver()
        {
            SetState(State.Start);
            OnGameOver();
            scoreText.text = "0";
        }
        
        public void StartGame()
        {
            SetState(State.Countdown);
        }
    }
}

