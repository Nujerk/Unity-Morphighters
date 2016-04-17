using UnityEngine;
using System.Collections;

namespace Morphighters
{
    public class KeepBetweenScenes : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }
}


