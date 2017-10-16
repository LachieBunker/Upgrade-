using UnityEngine;
using System.Collections;

public class A_BlackHole : UpgradesInterface
{
    public GameObject blackHole;

    public override void UseAbility(PlayerController playerScript, GameObject playerObject)
    {
        GameObject bHObject = (GameObject)Instantiate(blackHole, new Vector2(playerObject.transform.position.x + (0.01f * playerScript.directionFacing), playerObject.transform.position.y), Quaternion.Euler(0, 0, 90));
        BlackHoleScript bHScript = bHObject.GetComponent<BlackHoleScript>();
        bHScript.owner = playerObject;
        bHScript.SetSpeed(1 * playerScript.directionFacing);
    }
}
