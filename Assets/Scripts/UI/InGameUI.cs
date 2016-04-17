using UnityEngine;
using System.Collections;

namespace Morphighters.UI
{
    public class InGameUI : MonoBehaviour
    {
        public GameObject[] LifeBars;
        public GameObject EndGame;

        void Start()
        {
            var players = GameManager.Instance.Players.Values;
            Player player;
            LifeBar bar;
            foreach(var playerObject in players)
            {
                player = playerObject.GetComponent<Player>();
                bar = LifeBars[player.PlayerIndex].GetComponent<LifeBar>();
                bar.Player = player.GetComponent<Player>();
                bar.gameObject.SetActive(true);
            }
        }

        void Update()
        {
            if(GameManager.Instance.GameOver)
            {
                EndGame.SetActive(true);
            }
        }
    }
}


