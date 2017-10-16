using UnityEngine;
using System.Collections;

public class U_ElectricBall : UpgradesInterface
{

    public override void GetAttackObjectProperties(int _damage, int _size, int _knockback, int _attackSpeed, out int damage, out int size, out int knockback, out int attackSpeed)
    {
        damage = _damage;
        knockback = _knockback;

        size = _size * 2;
        attackSpeed = _attackSpeed / 2;
    }

    public override void GetAttackCoolDownProperties(float _attackCoolDown, float _range, int _numAttacks, int _angle, int _attackHealth, out float attackCoolDown, out float range, out int numAttacks, out int angle, out int attackHealth)
    {

        attackCoolDown = _attackCoolDown;
        range = _range;
        numAttacks = _numAttacks;
        angle = _angle;

        attackHealth = _attackHealth + 5;
    }
}

