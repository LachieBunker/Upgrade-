using UnityEngine;
using System.Collections;

public class U_DoubleAttackHealth : UpgradesInterface
{

    public override void GetAttackCoolDownProperties(float _attackCoolDown, float _range, int _numAttacks, int _angle, int _attackHealth, out float attackCoolDown, out float range, out int numAttacks, out int angle, out int attackHealth)
    {
        attackCoolDown = _attackCoolDown;
        range = _range;
        numAttacks = _numAttacks;
        angle = _angle;


        attackHealth = _attackHealth * 2;
    }
}