using UnityEngine;
using System.Collections;

public class A_ShockBlast : UpgradesInterface
{

    public GameObject shockBlast;

    public override void UseAbility(PlayerController playerScript, GameObject playerObject)
    {
        GameObject fireball = (GameObject)Instantiate(shockBlast, new Vector2(playerObject.transform.position.x + (0.05f * playerScript.directionFacing), playerObject.transform.position.y), Quaternion.Euler(0, 0, 90));
        fireball.GetComponent<AttackObjectScript>().SetAttackValues(playerScript.playerOwner, 0, 1, 150, 0.25f, 1 * playerScript.directionFacing, 100);
        GameObject fireball2 = (GameObject)Instantiate(shockBlast, new Vector2(playerObject.transform.position.x + (0.05f * -playerScript.directionFacing), playerObject.transform.position.y), Quaternion.Euler(0, 0, 90));
        fireball2.GetComponent<AttackObjectScript>().SetAttackValues(playerScript.playerOwner, 0, 1, 150, 0.25f, 1 * -playerScript.directionFacing, 100);
    }
}