using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpgradesManager : MonoBehaviour {

    //Change if InputTypes changes
    private const int numInputTypes = 7;
    public List<InputTypes> inputTypesList = new List<InputTypes> { InputTypes.Idle, InputTypes.Move, InputTypes.Jump, InputTypes.Attack, InputTypes.AttackCoolDown, InputTypes.Ability, InputTypes.AbilityCoolDown };
    //Change if InputTypes changes

    public UpgradesDictionary upgradesDict;

	// Use this for initialization
	void Start ()
    {
        upgradesDict = gameObject.GetComponent<UpgradesDictionary>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GetPopularInputs(List<InputTypes> recentInputs, out InputTypes firstPopInput, out InputTypes secondPopInput)
    {
        InputTypes firstPopInputType = InputTypes.Idle;
        int firstPopInputCount = 0;
        InputTypes secondPopInputType = InputTypes.Idle;
        int secondPopInputCount = 0;
        int tempInputCount;
        for (int i = 1; i < inputTypesList.Count; i++)
        {
            tempInputCount = 0;
            for (int j = 0; j < recentInputs.Count; j++)
            {
                if (recentInputs[j] == inputTypesList[i])
                {
                    tempInputCount++;
                }
            }

            if (tempInputCount > firstPopInputCount)
            {
                firstPopInputType = inputTypesList[i];
                firstPopInputCount = tempInputCount;
            }
            else if (tempInputCount > secondPopInputCount)
            {
                secondPopInputType = inputTypesList[i];
                secondPopInputCount = tempInputCount;
            }
        }
        firstPopInput = firstPopInputType;
        secondPopInput = secondPopInputType;
    }

    public UpgradesInterface GetUpgrade(InputTypes input, List<UpgradesInterface> currentUpgrades, List<UpgradesInterface> availableUpgrades)
    {
        List<UpgradesInterface> possibleUpgrades = new List<UpgradesInterface>();
        for (int i = 0; i < availableUpgrades.Count; i++)
        {
            //Debug.Log(availableUpgrades[i].name);
            if (input == availableUpgrades[i].inputType1 || input == availableUpgrades[i].inputType2)
            {
                possibleUpgrades.Add(availableUpgrades[i]);
                /*if (!currentUpgrades.Contains(availableUpgrades[i]))
                {
                    possibleUpgrades.Add(availableUpgrades[i]);
                }*/
            }
        }
        if (possibleUpgrades.Count > 0)
        {
            int randNum = Random.Range(0, possibleUpgrades.Count);
            
            return possibleUpgrades[randNum];
        }
        else
        {
            //Debug.Log("null");
            return null;
        }
    }

    public UpgradesInterface GetAbility(InputTypes inputType2, List<UpgradesInterface> currentUpgrades, List<UpgradesInterface> availableUpgrades)
    {
        List<UpgradesInterface> possibleUpgrades = new List<UpgradesInterface>();
        for (int i = 0; i < availableUpgrades.Count; i++)
        {
            //Debug.Log(availableUpgrades[i].name);
            if (availableUpgrades[i].inputType1 == InputTypes.Ability)
            {
                possibleUpgrades.Add(availableUpgrades[i]);
            }
        }
        if (possibleUpgrades.Count > 0)
        {
            int randNum = Random.Range(0, possibleUpgrades.Count);

            return possibleUpgrades[randNum];
        }
        else
        {
            //Debug.Log("null");
            return null;
        }
    }
    /*
    public UpgradesInterface GetAbility(InputTypes firstPopInput, InputTypes secondPopInput, List<UpgradesInterface> currentUpgrades)
    {
        List<UpgradesInterface> possibleUpgrades = new List<UpgradesInterface>();
        for (int i = 0; i < upgradesDict.upgrades.Count; i++)
        {
            if (firstPopInput == upgradesDict.upgrades[i].inputType1 || firstPopInput == upgradesDict.upgrades[i].inputType2)
            {
                if (secondPopInput == upgradesDict.upgrades[i].inputType1 || secondPopInput == upgradesDict.upgrades[i].inputType2)
                {
                    if (!currentUpgrades.Contains(upgradesDict.upgrades[i]))
                    {
                        possibleUpgrades.Add(upgradesDict.upgrades[i]);
                    }
                }
            }
        }
        if (possibleUpgrades.Count > 0)
        {
            int randNum = Random.Range(0, possibleUpgrades.Count);
            return possibleUpgrades[randNum];
        }
        else
        {
            for (int i = 0; i < upgradesDict.upgrades.Count; i++)
            {
                if (firstPopInput == upgradesDict.upgrades[i].inputType1 || firstPopInput == upgradesDict.upgrades[i].inputType2)
                {
                    if (!currentUpgrades.Contains(upgradesDict.upgrades[i]))
                    {
                        possibleUpgrades.Add(upgradesDict.upgrades[i]);
                    }
                }
            }
            if (possibleUpgrades.Count > 0)
            {
                int randNum = Random.Range(0, possibleUpgrades.Count);
                return possibleUpgrades[randNum];
            }
            else
            {
                for (int i = 0; i < upgradesDict.upgrades.Count; i++)
                {
                    if (secondPopInput == upgradesDict.upgrades[i].inputType1 || secondPopInput == upgradesDict.upgrades[i].inputType2)
                    {
                        if (!currentUpgrades.Contains(upgradesDict.upgrades[i]))
                        {
                            possibleUpgrades.Add(upgradesDict.upgrades[i]);
                        }
                    }
                }
                if (possibleUpgrades.Count > 0)
                {
                    int randNum = Random.Range(0, possibleUpgrades.Count);
                    return possibleUpgrades[randNum];
                }
                else
                {
                    return null;
                }
            }
        }
    }
    */
}

//If changing, also change numInputTypes and inputTypesList
public enum InputTypes { Idle, Move, Jump, Attack, AttackCoolDown, Ability, AbilityCoolDown}
//If changing, also change numInputTypes and inputTypesList

[System.Serializable]
public class UpgradeDetails
{
    public string name;
    public int upgradeID;
    public InputTypes inputType1;
    public InputTypes inputType2;
}