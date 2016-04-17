using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Morphighters.MorphShapes
{
    public interface MorphShape
    {
        float Defense { get; }
        float MovementSpeed { get; }
        float ReloadFirstAttack { get; }
        float ReloadSecondAttack { get; }

        Vector2 HandleMovement(Vector2 currentPosition, float HAxisValue, float VAxisValue);

        void FirstAttack();
        void DealFirstAttackDamage();
        void SecondAttack();
        void DealSecondAttackDamage();
        void ReceiveDamage(float damage);
        void Die();
    }
}
