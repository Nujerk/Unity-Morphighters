using UnityEngine;
using System.Collections;
using Morphighters.MorphShapes;

namespace Morphighters.AnimationsBehavior
{
    public class ShooterAttack2Result : StateMachineBehaviour
    {
        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var shape = animator.gameObject.GetComponent<ShooterShape>();
            shape.DealSecondAttackDamage();
        }
    }
}

