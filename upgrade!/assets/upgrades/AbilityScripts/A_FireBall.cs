using UnityEngine;
using System.Collections;

public class A_FireBall : UpgradesInterface {

    public GameObject fireBall;

    public override void UseAbility(PlayerController playerScript, GameObject playerObject)
    {
        GameObject fireball = (GameObject)Instantiate(fireBall, new Vector2(playerObject.transform.position.x + (0.05f * playerScript.directionFacing), playerObject.transform.position.y), Quaternion.Euler(0, 0, 90));
        fireball.transform.localScale = new Vector2(fireball.transform.localScale.x, fireball.transform.localScale.y * -playerScript.directionFacing);
        fireball.GetComponent<AttackObjectScript>().SetAttackValues(playerScript.playerOwner, playerScript.currentDamage * 2, 1, 50, 1, 1 * playerScript.directionFacing, 1);
    }
}
