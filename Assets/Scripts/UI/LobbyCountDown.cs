using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Morphighters.UI
{
    public class LobbyCountDown : MonoBehaviour
    {
        void Update()
        {
            if(GameManager.Instance.GameRunning)
                gameObject.SetActive(false);

            var remaining = GameManager.Instance.TimeRemainingToJoin;
            if (remaining == 0)
                GetComponent<Text>().text = "GO !";
            else if (remaining == 42)
                GetComponent<Text>().text = "Waiting for players... (Fire1 to join)";
            else
                GetComponent<Text>().text = Mathf.Round(GameManager.Instance.TimeRemainingToJoin).ToString();
        }
    }
}


