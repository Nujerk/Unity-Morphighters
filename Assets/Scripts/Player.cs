using UnityEngine;
using System.Collections;
using Morphighters.MorphShapes;

namespace Morphighters
{
    public enum MorphShapeIndex
    {
        Bruiser,
        Shooter
    }

    public class Player : MonoBehaviour
    {

        public float PlayerHealth { get; set; }
        public int ControllerIndex { get; set; }
        public int PlayerIndex { get; set; }
        public MorphShape CurrentShape {
            get
            {
                return _currentShape;
            }    
        }

        public GameObject[] morphShapes;

        private static Color[] sPlayerColor =
        {
            Color.HSVToRGB(0, 120f / 255, 1),
            Color.HSVToRGB(239f / 359, 120f / 255, 1),
            Color.HSVToRGB(120f / 359, 170f / 255, 1),
            Color.HSVToRGB(64f / 359, 170f / 255, 1)
        };
        private static int sPlayerCount = 0;

        private bool _playerDead = false;
        private float _stunnedUntil;
        private int _currentShapeIndex;
        private MorphShape _currentShape;

        public void StunPlayer(float duration)
        {
            _stunnedUntil = Time.realtimeSinceStartup + duration;
        }

        public void MorphTo(int index)
        {
            morphShapes[_currentShapeIndex].SetActive(false);
            morphShapes[index].SetActive(true);
            _currentShapeIndex = index;
            _currentShape = morphShapes[_currentShapeIndex].GetComponent<MorphShape>();
            var renderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (var renderer in renderers)
            {
                renderer.color = sPlayerColor[PlayerIndex];
            }
        }

        void Start()
        {
            PlayerHealth = 100;
            _currentShapeIndex = (int)MorphShapeIndex.Bruiser;
            _currentShape = morphShapes[_currentShapeIndex].GetComponent<MorphShape>();
            morphShapes[_currentShapeIndex].SetActive(true);
            var renderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (var renderer in renderers)
            {
                renderer.color = sPlayerColor[PlayerIndex];
            }
        }

        void Update()
        {
            if(!PlayerIncapacited())
            {
                if (PlayerHealth <= 0)
                    PlayerDeath();

                if (Input.GetButtonDown("Fire1_" + ControllerIndex))
                    _currentShape.FirstAttack();
                else if (Input.GetButtonDown("Fire2_" + ControllerIndex))
                    _currentShape.SecondAttack();

                HandleMovement();
            }
        }

        private void PlayerDeath()
        {
            _playerDead = true;
            _currentShape.Die();
            enabled = false;
        }

        private void HandleMovement()
        {
            gameObject.transform.position = _currentShape.HandleMovement(gameObject.transform.position, Input.GetAxis("Horizontal_" + ControllerIndex), Input.GetAxis("Vertical_" + ControllerIndex));
        }

        private bool PlayerIncapacited()
        {
            if (!GameManager.Instance.GameRunning)
                return true;

            if (_playerDead)
                return true;

            if (Time.realtimeSinceStartup < _stunnedUntil)
                return true;

            return false;
        }
    }
}