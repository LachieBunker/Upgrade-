using UnityEngine;
using System.Collections;

public class A_Dash : UpgradesInterface
{
    public float dashAmount;

    public override void UseAbility(PlayerController playerScript, GameObject playerObject)
    {
	    if(playerScript.directionFacing < 0)
	    {
	    	if(playerObject.transform.position.x - dashAmount > GameObject.Find("BoundaryL").transform.position.x)
	    	{
				playerObject.transform.position = new Vector2(playerObject.transform.position.x + (dashAmount * playerScript.directionFacing), playerObject.transform.position.y);
	    	}
	    	else
	    	{
				playerObject.transform.position = new Vector2(GameObject.Find("BoundaryL").transform.position.x + (-0.2f * playerScript.directionFacing), playerObject.transform.position.y);
	    	}
	    }
	    else
	    {
	    	if(playerObject.transform.position.x + dashAmount < GameObject.Find("BoundaryR").transform.position.x)
	    	{
				playerObject.transform.position = new Vector2(playerObject.transform.position.x + (dashAmount * playerScript.directionFacing), playerObject.transform.position.y);
	    	}
			else
	    	{
				playerObject.transform.position = new Vector2(GameObject.Find("BoundaryR").transform.position.x + (-0.2f * playerScript.directionFacing), playerObject.transform.position.y);
	    	}
	    }
        
    }
}
