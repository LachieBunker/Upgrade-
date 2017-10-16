using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class UpgradesInterface : MonoBehaviour {

    public string name;
    public int upgradeID;
    public Sprite upgradeImage;
    public InputTypes inputType1;
    public InputTypes inputType2;

    public virtual void GetMovementProperties(int _moveSpeed, int _health, out int moveSpeed, out int health)
    {
        moveSpeed = _moveSpeed;
        health = _health;
    }

    public virtual void GetJumpProperties(int _numJumps, int _jumpHeight, int _gravity, out int numJumps, out int jumpHeight, out int gravity)
    {
        numJumps = _numJumps;
        jumpHeight = _jumpHeight;
        gravity = _gravity;
    }

    public virtual void GetAttackObjectProperties(int _damage, int _size, int _knockback, int _attackSpeed, out int damage, out int size, out int knockback, out int attackSpeed)
    {
        damage = _damage;
        size = _size;
        knockback = _knockback;
        attackSpeed = _attackSpeed;
    }

    public virtual void GetAttackCoolDownProperties(float _attackCoolDown, float _range, int _numAttacks, int _angle, int _attackHealth, out float attackCoolDown, out float range, out int numAttacks, out int angle, out int attackHealth)
    {
        attackCoolDown = _attackCoolDown;
        range = _range;
        numAttacks = _numAttacks;
        angle = _angle;
        attackHealth = _attackHealth;
    }

    public virtual void UseAbility(PlayerController playerScript, GameObject playerObject)
    {

    }

    public virtual void GetAbilityCoolDownProperties(int _abilityCoolDown, out int abilityCoolDown)
    {
        abilityCoolDown = _abilityCoolDown;
    }
}
