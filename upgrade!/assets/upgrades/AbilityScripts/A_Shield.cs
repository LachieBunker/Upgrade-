using UnityEngine;
using System.Collections;

public class A_Shield : UpgradesInterface
{
    public GameObject shield;
    public GameObject shieldObject;
    int shieldCountDown = 0;

    public override void UseAbility(PlayerController playerScript, GameObject playerObject)
    {
        shieldObject = (GameObject)Instantiate(shield, new Vector2(playerObject.transform.position.x + (0.12f * playerScript.directionFacing), playerObject.transform.position.y), playerObject.transform.rotation);
        shieldObject.transform.localScale = new Vector2(shieldObject.transform.localScale.x * -playerScript.directionFacing, shieldObject.transform.localScale.y);
    }
}
