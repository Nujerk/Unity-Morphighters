using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Morphighters.UI
{
    public class LifeBar : MonoBehaviour
    {
        public Player Player { get; set; }
        public GameObject text;

        void Update()
        {
            if(Player != null)
            {
                GetComponent<Scrollbar>().size = Player.PlayerHealth / 100;
                text.GetComponent<Text>().text = Player.PlayerHealth + " / 100";
            }
        }
    }
}

