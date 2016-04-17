using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Morphighters.MorphShapes
{
    [Serializable]
    public class ShooterStats
    {
        public float defense;
        public float movementSpeed;
        public float firstAttackDamage;
        public float firstAttackReload;
        public float firstAttackRange;
        public float secondAttackReload;
        public float secondAttackRange;
        public float projectileVelocity;
    }

    [Serializable]
    public class ShooterAnimations
    {
        public string idleAnimation;
        public string walkAnimation;
        public string attack1Animation;
        public string attack2Animation;
        public string dieAnimation;
    }

    class ShooterShape : MonoBehaviour, MorphShape
    {
        public ShooterStats stats;
        public ShooterAnimations animations;
        public GameObject projectile;

        private float _nextfirstAttackAvailable;
        private float _nextSecondAttackAvailable;
        private bool _attacking = false;
        private bool _facingLeft = true;
        private bool _invulnerable = false;
        private float _formerVertical;
        private Vector2 _lastDirection;

        public float Defense
        {
            get
            {
                return stats.defense;
            }
        }

        public float MovementSpeed
        {
            get
            {
                return stats.movementSpeed;
            }
        }

        public float ReloadFirstAttack
        {
            get
            {
                return (stats.firstAttackReload - (_nextfirstAttackAvailable - Time.realtimeSinceStartup)) / stats.firstAttackReload;
            }
        }

        public float ReloadSecondAttack
        {
            get
            {
                return (stats.secondAttackReload - (_nextSecondAttackAvailable - Time.realtimeSinceStartup)) / stats.secondAttackReload;
            }
        }

        public bool FacingLeft
        {
            get
            {
                return _facingLeft;
            }
        }

        public Vector2 HandleMovement(Vector2 currentPosition, float HAxisValue, float VAxisValue)
        {
            _lastDirection = new Vector2(HAxisValue, VAxisValue);
            if (!_attacking)
            {
                if (Mathf.Abs(HAxisValue) > 0.1 || Mathf.Abs(VAxisValue) > 0.1)
                {
                    var direction = new Vector2(HAxisValue, VAxisValue);
                    if (direction.x > 0)
                    {
                        gameObject.transform.parent.transform.localScale = new Vector3(-1, 1, 1);
                        _facingLeft = false;
                    }
                    else
                    {
                        gameObject.transform.parent.transform.localScale = Vector3.one;
                        _facingLeft = true;
                    }
                    GetComponent<Animator>().Play(animations.walkAnimation);
                    return currentPosition + direction.normalized * Time.deltaTime * stats.movementSpeed;
                }
                else
                {
                    GetComponent<Animator>().Play(animations.idleAnimation);
                    return currentPosition;
                }
            }
            else
                return currentPosition;
        }

        public void FirstAttack()
        {
            var current = Time.realtimeSinceStartup;
            if (current < _nextfirstAttackAvailable || _attacking)
                return;

            _attacking = true;
            GetComponent<Animator>().Play(animations.attack1Animation);

            _nextfirstAttackAvailable = current + stats.firstAttackReload;
        }

        public void SecondAttack()
        {
            var current = Time.realtimeSinceStartup;
            if (current < _nextSecondAttackAvailable || _attacking)
                return;

            _attacking = true;
            SetInvulnerable(true);
            var parent = transform.parent.gameObject;

            var flightTo = _lastDirection.normalized + Vector2.up;
            parent.GetComponent<Rigidbody2D>().isKinematic = false;
            parent.GetComponent<Rigidbody2D>().AddForce(flightTo * 3, ForceMode2D.Impulse);

            GetComponent<Animator>().Play(animations.attack2Animation);

            _nextfirstAttackAvailable = current + stats.firstAttackReload;
        }

        public void SetInvulnerable(bool value)
        {
            _invulnerable = value;
        }

        public void ReceiveDamage(float damage)
        {
            if (_invulnerable)
                return;

            var parent = gameObject.transform.parent.gameObject;
            parent.GetComponent<Player>().PlayerHealth -= damage - stats.defense;
        }

        public void DealFirstAttackDamage()
        {
            _attacking = false;
        }

        public void DealSecondAttackDamage()
        {
            var parent = transform.parent.gameObject;
            parent.GetComponent<Rigidbody2D>().isKinematic = true;
            SetInvulnerable(false);
            _attacking = false;
        }

        public void Die()
        {
            GetComponent<Animator>().Play(animations.dieAnimation);
        }
    }
}
