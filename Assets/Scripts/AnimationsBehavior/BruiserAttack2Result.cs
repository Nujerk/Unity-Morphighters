using UnityEngine;
using System.Collections;
using Morphighters.MorphShapes;

namespace Morphighters.AnimationsBehavior
{
    public class BruiserAttack2Result : StateMachineBehaviour
    {
        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var shape = animator.gameObject.GetComponent<BruiserShape>();
            shape.DealSecondAttackDamage();
        }
    }
}

