using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

namespace Morphighters
{
    public enum PlayersIndex
    {
        RedPlayer,
        BluePlayer,
        GreenPlayer,
        YellowPlayer
    }

    public class GameManager : MonoBehaviour
    {
        private static GameManager sInstance;
        public static GameManager Instance
        {
            get
            {
                return sInstance;
            }
        }

        private static Vector2[] sStartingPosition =
        {
            new Vector2(-1, 1),
            new Vector2(1, 1),
            new Vector2(-1, -1),
            new Vector2(1, -1)
        };

        public Dictionary<PlayersIndex, GameObject> Players { get; set; }
        public bool GameRunning { get; set; }
        public float TimeRemainingToJoin
        {
            get
            {
                if (_gameStartAt == 0)
                    return 42;

                return Mathf.Max(_gameStartAt - Time.realtimeSinceStartup, 0);
            }
        }
        public bool GameOver
        {
            get
            {
                return _gameEnded;
            }
        }

        public float joinDelay;
        public GameObject playerPrefab;

        private float _gameStartAt = 0;
        private bool _gameStarted = false;
        private bool _gameEnded = false;
        private float _resetTime = 0;

        void Start()
        {
            if (sInstance == null)
                sInstance = this;

            Players = new Dictionary<PlayersIndex, GameObject>();
            GameRunning = false;
        }

        void Update()
        {
            if(_gameEnded && Time.realtimeSinceStartup > _resetTime)
            {
                Reset();
            }

            // Handle game start countdown
            if (Players.Count > 0)
            {
                if (_gameStartAt == 0)
                    _gameStartAt = Time.realtimeSinceStartup + joinDelay;

                if (Time.realtimeSinceStartup > _gameStartAt && !_gameStarted)
                {
                    SceneManager.LoadScene("Morphighters", LoadSceneMode.Single);
                    _gameStarted = true;
                    GameRunning = true;
                }
            }
            else
                _gameStartAt = 0;


            // Handle join and leave in the lobby
            if(!_gameStarted)
            {
                bool playerInLobby = false;
                for(int i = 0; i < 5; i++)
                {
                    playerInLobby = ControllerUsed(i);
                    if (Players.Count < 4 && !playerInLobby && Input.GetButtonDown("Fire1_" + i))
                    {
                        AddPlayer(i);
                    }

                    if(playerInLobby && Input.GetButtonDown("Fire2_" + i))
                    {
                        RemovePlayer(i);
                    }
                    
                }
            }

            if(_gameStarted && !_gameEnded)
            {
                var aliveCount = 0;
                foreach(var player in Players.Values)
                {
                    if (player.GetComponent<Player>().PlayerHealth > 0)
                        aliveCount++;
                }

                if (aliveCount <= 1)
                    EndGame();

                if (Input.GetKeyDown(KeyCode.Escape))
                    GameRunning = !GameRunning;
            }
            
        }

        private void AddPlayer(int controllerIndex)
        {
            var availableSlotIndex = GetFirstAvailableSlot();
            if(availableSlotIndex != -1)
            {
                GameObject player = GameObject.Instantiate(playerPrefab, sStartingPosition[availableSlotIndex], Quaternion.identity) as GameObject;
                player.GetComponent<Player>().ControllerIndex = controllerIndex;
                player.GetComponent<Player>().PlayerIndex = availableSlotIndex;
                Players.Add((PlayersIndex)availableSlotIndex, player);
            }
        }

        private void RemovePlayer(int controllerIndex)
        {
            if (Players.Count < 1)
                return;

            var player = Players.First(kp => kp.Value.GetComponent<Player>().ControllerIndex == controllerIndex);
            Players.Remove(player.Key);
            Destroy(player.Value);
        }

        private bool ControllerUsed(int controllerIndex)
        {
            if (Players.Count < 1)
                return false;

            if (Players.Where(kp => kp.Value.GetComponent<Player>().ControllerIndex == controllerIndex).Count() > 0)
                return true;

            return false;
        }

        private int GetFirstAvailableSlot()
        {
            for(int i = 0; i < 4; i++)
            {
                if (!Players.Keys.Contains((PlayersIndex)i))
                    return i;

                if (Players[(PlayersIndex)i] == null)
                    return i;
            }

            return -1;
        }

        private void EndGame()
        {
            _gameEnded = true;
            _resetTime = Time.realtimeSinceStartup + 5;
        }

        private void Reset()
        {
            foreach(var player in Players.Values)
            {
                Destroy(player);
            }
            Players.Clear();

            GameRunning = false;
            _gameStartAt = 0;
            _gameStarted = false;
            _gameEnded = false;
            _resetTime = 0;

            Destroy(gameObject);
            SceneManager.LoadScene("Morphighters_lobby");
        }
    }
}
