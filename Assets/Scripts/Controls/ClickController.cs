using System;
using System.Collections;
using System.Collections.Generic;
using CoreControls;
using UnityEngine;

namespace Controls
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ClickController : MonoBehaviour
    {
        private Rigidbody2D rigidbody2D;
        private Quaternion downRotation;
        private Quaternion fowardRotation;

        public delegate void PlayerDelegate();

        public static event PlayerDelegate OnPlayerDied;
        public static event PlayerDelegate OnPlayerScored;
        
        [SerializeField]
        private float force = 10;
        [SerializeField]
        private float smooth = 5;
        [SerializeField]
        private Vector3 startPosition;
        
        [SerializeField]
        private AudioSource clickAudio;
        [SerializeField]
        private AudioSource dieAudio;
        [SerializeField]
        private AudioSource scoreAudio;

        private GameManager game;

        private void Start()
        {
            downRotation = Quaternion.Euler(0, 0, -90);
            fowardRotation = Quaternion.Euler(0, 0, 35);
            rigidbody2D = GetComponent<Rigidbody2D>();
            game = GameManager.Instance;
            rigidbody2D.simulated = false;
        }

        private void OnEnable()
        {
            GameManager.OnGameStart += OnGameStart;
            GameManager.OnGameOver += OnGameOver;
        }

        private void OnDisable()
        {
            GameManager.OnGameStart -= OnGameStart;
            GameManager.OnGameOver -= OnGameOver;
        }

        private void OnGameStart()
        {
            rigidbody2D.velocity = Vector3.zero;
            rigidbody2D.simulated = true;
        }

        private void OnGameOver()
        {
            transform.localPosition = startPosition;
            transform.rotation = Quaternion.identity;
        }

        private void Update()
        {
            if(game.Gameover)
                return;
            
            
            if (Input.GetMouseButtonDown(0))
            {
                clickAudio.Play();
                transform.rotation = fowardRotation;
                rigidbody2D.velocity = Vector3.zero; 
                rigidbody2D.AddForce(Vector2.up * force, ForceMode2D.Force);
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, smooth * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D colider)
        {
            if (colider.gameObject.tag == "ScoreZone")
            {
                OnPlayerScored();
                scoreAudio.Play();
            }

            if (colider.gameObject.tag == "DeadZone")
            {
                rigidbody2D.simulated = false;
                OnPlayerDied();
                dieAudio.Play();
            }
        }
    }
}

