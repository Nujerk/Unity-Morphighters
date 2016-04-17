using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Morphighters.MorphShapes
{
    [Serializable]
    public class BruiserStats
    {
        public float defense;
        public float movementSpeed;
        public float firstAttackDamage;
        public float firstAttackReload;
        public float firstAttackRange;
        public float firstAttackCone;
        public float secondAttackDamage;
        public float secondAttackReload;
        public float secondAttackRange;
    }

    [Serializable]
    public class BuiserAnimations
    {
        public string idleAnimation;
        public string walkAnimation;
        public string attack1Animation;
        public string attack2Animation;
        public string dieAnimation;
    }

    class BruiserShape : MonoBehaviour, MorphShape
    {
        public BruiserStats stats;
        public BuiserAnimations animations;


        private float _nextfirstAttackAvailable;
        private float _nextSecondAttackAvailable;
        private bool _attacking = false;
        private bool _facingLeft = true;

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

        public void FirstAttack()
        {
            var current = Time.realtimeSinceStartup;
            if (current < _nextfirstAttackAvailable || _attacking)
                return;

            _attacking = true;
            GetComponent<Animator>().Play(animations.attack1Animation);

            _nextfirstAttackAvailable = current + stats.firstAttackReload;
        }

        public Vector2 HandleMovement(Vector2 currentPosition, float HAxisValue, float VAxisValue)
        {
            if(!_attacking)
            {
                if (Mathf.Abs(HAxisValue) > 0.1 || Mathf.Abs(VAxisValue) > 0.1)
                {
                    var direction = new Vector2(HAxisValue, VAxisValue);
                    if(direction.x > 0)
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

        public void SecondAttack()
        {
            var current = Time.realtimeSinceStartup;
            if (current < _nextSecondAttackAvailable || _attacking)
                return;

            _attacking = true;
            GetComponent<Animator>().Play(animations.attack2Animation);

            _nextSecondAttackAvailable = current + stats.secondAttackReload;
        }

        public void DealFirstAttackDamage()
        {
            var players = GameManager.Instance.Players.Values;
            var parent = gameObject.transform.parent.gameObject;
            foreach (var player in players)
            {
                if (player == parent)
                    continue;

                var directionFacing = Vector3.left;
                if (!_facingLeft)
                    directionFacing = Vector3.right;

                var vectorTarget = player.transform.position - parent.transform.position;
                var dot = Vector2.Dot(directionFacing, vectorTarget.normalized);
                Debug.Log(dot);
                var distance = Vector2.Distance(player.transform.position, parent.transform.position);
                if (dot > stats.firstAttackCone &&  dot <= 1 && distance < stats.firstAttackRange)
                {
                    player.GetComponent<Player>().CurrentShape.ReceiveDamage(stats.firstAttackDamage);                    
                }
            }
            _attacking = false;
        }

        public void DealSecondAttackDamage()
        {
            var players = GameManager.Instance.Players.Values;
            var parent = gameObject.transform.parent.gameObject;
            foreach (var player in players)
            {
                if (player == parent || player == null)
                    continue;

                if (Vector2.Distance(player.transform.position, parent.transform.position) < stats.secondAttackRange)
                {
                    player.GetComponent<Player>().CurrentShape.ReceiveDamage(stats.secondAttackDamage);
                }
            }
            _attacking = false;
        }

        public void ReceiveDamage(float damage)
        {
            var parent = gameObject.transform.parent.gameObject;
            parent.GetComponent<Player>().PlayerHealth -= damage - stats.defense;
        }

        public void Die()
        {
            GetComponent<Animator>().Play(animations.dieAnimation);
        }
    }
}
