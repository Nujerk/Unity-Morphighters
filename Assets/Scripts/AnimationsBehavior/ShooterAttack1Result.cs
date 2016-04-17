using UnityEngine;
using System.Collections;
using Morphighters.MorphShapes;

namespace Morphighters.AnimationsBehavior
{
    public class ShooterAttack1Result : StateMachineBehaviour
    {
        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var shape = animator.gameObject.GetComponent<ShooterShape>();
            var spawn = shape.projectile.transform.position + shape.transform.position;
            if (shape.FacingLeft)
                spawn = new Vector3(-shape.projectile.transform.position.x, shape.projectile.transform.position.y, shape.projectile.transform.position.z) + shape.transform.position;
            var proj = GameObject.Instantiate(shape.projectile, spawn, Quaternion.identity) as GameObject;

            var direction = Vector2.right;
            if (shape.FacingLeft)
                direction = Vector2.left;

            proj.GetComponent<ShooterProjectile>().Damages = shape.stats.firstAttackDamage;
            proj.GetComponent<ShooterProjectile>().Owner = animator.gameObject;
            proj.GetComponent<Rigidbody2D>().AddForce(direction * shape.stats.projectileVelocity, ForceMode2D.Impulse);

            shape.DealFirstAttackDamage();
        }
    }
}

