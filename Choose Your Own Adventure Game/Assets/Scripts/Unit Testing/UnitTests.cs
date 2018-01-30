using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class UnitTests : MonoBehaviour {

    /**
     * Ensures that the helth of a unit is set to its max health when it is first created.
     */ 
	[UnityTest]
    public IEnumerator UnitsHealthIsSetToMaxOnEnable()
    {

        GameObject basePrefab = Resources.Load<GameObject>("Testing/Mock Prefabs/Combat/Units/Flow Test Units/Flow Test Mock Unit 1");
        Unit testUnit = Instantiate(basePrefab).GetComponent<Unit>();

        yield return null; //Wait a frame, when everting is set up.

        Assert.AreEqual(testUnit.health, testUnit.maxHealth, 
                        "testUnit.health (" + testUnit.health + ") should be equal to testUnit.maxHealth (" + testUnit.maxHealth + ").");

    }

    /**
     * Ensures that a unit's health can not go below 0.
     */ 
    [Test]
    public void UnitsHealthCanNotGoBelow0()
    {

        Unit testUnit = CombatFlowTests.GetFlowTestUnit("Flow Test Mock Unit 1");
        testUnit.health -= 60;

        Assert.IsTrue(testUnit.health == 0,
                      "Units health should be set to 0, if the value would otherwise be negative.");

    }

}
