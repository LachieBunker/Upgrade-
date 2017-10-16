using UnityEngine;
using System.Collections;

public class U_ForceBlast : UpgradesInterface
{

    public override void GetAttackObjectProperties(int _damage, int _size, int _knockback, int _attackSpeed, out int damage, out int size, out int knockback, out int attackSpeed)
    {
        size = _size;
        knockback = _knockback;
        attackSpeed = _attackSpeed;

        damage = _damage / 2;
    }

    public override void GetAttackCoolDownProperties(float _attackCoolDown, float _range, int _numAttacks, int _angle, int _attackHealth, out float attackCoolDown, out float range, out int numAttacks, out int angle, out int attackHealth)
    {
        attackCoolDown = _attackCoolDown;
        range = _range;
        attackHealth = _attackHealth;

        numAttacks = _numAttacks + 10;
        angle = _angle + 180;
    }
}

