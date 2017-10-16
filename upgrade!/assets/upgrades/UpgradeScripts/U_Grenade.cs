using UnityEngine;
using System.Collections;

public class U_Grenade : UpgradesInterface
{

    public override void GetAttackObjectProperties(int _damage, int _size, int _knockback, int _attackSpeed, out int damage, out int size, out int knockback, out int attackSpeed)
    {
        knockback = _knockback;
        attackSpeed = _attackSpeed;

        damage = _damage * 3;
        size = _size * 4;
    }

    public override void GetAttackCoolDownProperties(float _attackCoolDown, float _range, int _numAttacks, int _angle, int _attackHealth, out float attackCoolDown, out float range, out int numAttacks, out int angle, out int attackHealth)
    {
        attackCoolDown = _attackCoolDown;
        numAttacks = _numAttacks;
        angle = _angle;

        range = _range / 4;
        attackHealth = _attackHealth / 2;
    }
}

