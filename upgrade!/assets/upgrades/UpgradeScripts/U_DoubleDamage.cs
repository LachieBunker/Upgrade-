using UnityEngine;
using System.Collections;

public class U_DoubleDamage : UpgradesInterface
{

    public override void GetAttackObjectProperties(int _damage, int _size, int _knockback, int _attackSpeed, out int damage, out int size, out int knockback, out int attackSpeed)
    {
        size = _size;
        knockback = _knockback;
        attackSpeed = _attackSpeed;

        damage = _damage * 2;
    }
}
