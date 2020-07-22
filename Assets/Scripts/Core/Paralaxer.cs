using System;
using System.Collections;
using System.Collections.Generic;
using CoreControls;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;
using Random = System.Random;

namespace Core
{
    public class Paralaxer : MonoBehaviour
    {
        [SerializeField]
        private GameObject Prefab;
        [SerializeField]
        private int poolSize;
        [SerializeField]
        private float shiftSpeed;
        [SerializeField]
        private float spawnRate;
        

        [SerializeField]
        private SpawnRange spawnRange;
        [SerializeField]
        private Vector3 defaultPosition;
        [SerializeField]
        private bool spawnAtStart;
        [SerializeField]
        private Vector3 imSpawnPosition;
        [SerializeField]
        private Vector2 targetAspectRatio;

        private float spawnTimer;
        private float targetAspect;
        private Pool[] poolObjects;

        private GameManager game;

        private void Awake()
        {
            Configure();
        }

        private void Start()
        {
            game = GameManager.Instance;
        }

        private void OnEnable()
        {
            GameManager.OnGameOver += OnGameOver;
        }

        private void OnDisable()
        {
            GameManager.OnGameOver -= OnGameOver;
        }

        private void OnGameOver()
        {
            for (int i = 0; i < poolObjects.Length; i++)
            {
                poolObjects[i].Dispose();
                poolObjects[i].transform.position = Vector3.one * 1000;
            }
            if (spawnAtStart)
                SpawnImmediate();
        }

        private void Update()
        {
            if(game.Gameover)
                return;
            
            Shift();
            spawnTimer += Time.deltaTime;
            if (spawnTimer > spawnRate)
            {
                Spawn();
                spawnTimer = 0;
            }
        }

        private void Configure()
        {
            targetAspect = targetAspectRatio.x / targetAspectRatio.y;
            poolObjects = new Pool[poolSize];
            for (int i = 0; i < poolObjects.Length; i++)
            {
                GameObject go = Instantiate(Prefab) as GameObject;
                Transform t = go.transform;
                t.SetParent(transform);
                t.position = Vector3.one * 1000;
                poolObjects[i] = new Pool(t);
            }

            if (spawnAtStart)
                SpawnImmediate();
        }

        private void Spawn()
        {
            Transform t = GetPool();
            if(t == null)
                return;
            Vector3 position = Vector3.zero;
            position.x = (defaultPosition.x * Camera.main.aspect) / targetAspect;
            position.y = UnityEngine.Random.Range(spawnRange.min, spawnRange.max);
            t.position = position;
        }
        
        private void SpawnImmediate()
        {
            Transform t = GetPool();
            if(t == null)
                return;
            Vector3 position = Vector3.zero;
            position.x = (imSpawnPosition.x * Camera.main.aspect) / targetAspect;
            position.y = UnityEngine.Random.Range(spawnRange.min, spawnRange.max);
            t.position = position;
            Spawn();
        }

        private void Shift()
        {
            for (int i = 0; i < poolObjects.Length; i++)
            {
                poolObjects[i].transform.position += -Vector3.right * shiftSpeed * Time.deltaTime;
                CheckDisposeObject(poolObjects[i]);
            }
        }

        private void CheckDisposeObject(Pool poolObject)
        {
            if (poolObject.transform.position.x < (-defaultPosition.x * Camera.main.aspect) / targetAspect)
            {
                poolObject.Dispose();
                poolObject.transform.position = Vector3.one * 1000;
            }
        }

        private Transform GetPool()
        {
            for (int i = 0; i < poolObjects.Length; i++)
            {
                if (!poolObjects[i].inUse)
                {
                    poolObjects[i].Use();
                    return poolObjects[i].transform;
                }
            }
            return null;
        }
    }
}

