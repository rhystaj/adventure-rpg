using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentTests {

    private Weapon strongShortRange;
    private Weapon weakLongRange;

    /**
     * Automatiaclly generates weapons for testing.
     */ 
    [OneTimeSetUp]
    public void GenerateTestWeapons()
    {
  
        strongShortRange = ScriptableObject.CreateInstance<Weapon>();
        strongShortRange.damage = 10;
        strongShortRange.maxRange = 0;
        strongShortRange.minRange = 0;

        weakLongRange = ScriptableObject.CreateInstance<Weapon>();
        weakLongRange.damage = 3;
        weakLongRange.minRange = 2;
        weakLongRange.maxRange = 5;

    }

    /**
     * Ensures that a valid attack subtracts 0 from the target's health.
     */ 
    [Test]
    public void TestDamageIsSubtractedFromValidTarget()
    {

        Unit.IInstance testUser = new MockUnit("Unit 1", 0, 50, 0, true);
        Unit.IInstance testTarget = new MockUnit("Unit 2", 1, 50, 0, true);

        float targetHealthBeforeAttack = testTarget.health;

        Assert.IsTrue(strongShortRange.Use(testUser, testTarget), "Attack should be successful.");
        Assert.IsTrue(testTarget.health == targetHealthBeforeAttack - strongShortRange.damage,
                      strongShortRange.damage + " health should be subtracted from the unit, " + (targetHealthBeforeAttack - testTarget.health) + " " +
                      "was instead.");

    }

    /**
     * Ensures that two units of the same allignent can't attack each other.
     */ 
    [Test]
    public void TestUnitsCanNotAttackTargetsOfTheSameAlignment()
    {

        Unit.IInstance testUser = new MockUnit("Unit 1", 0, 50, 0, true);
        Unit.IInstance testTarget = new MockUnit("Unit 2", 0, 50, 0, true);

        Assert.IsFalse(strongShortRange.Use(testUser, testTarget));

    }

    /**
     * Ensures than any otherwise valid unit within a weapons range can be attacked.
     */ 
    [Test]
    public void UnitCanHitAnywhereInItsRange()
    {

        Unit.IInstance testUser = new MockUnit("Unit 1", 0, 50, 0, true);
        Unit.IInstance testTarget = new MockUnit("Unit 2", 1, 50, 0, true);

        for (int pos = weakLongRange.minRange; pos <= weakLongRange.maxRange; pos++)
        {
            testTarget.position = pos;
            Assert.IsTrue(weakLongRange.Use(testUser, testTarget),
                          "Weapon should be able to hit a unit at position " + pos, " as it is between the ranges of " + weakLongRange.maxRange + " and " +
                          weakLongRange.maxRange);
        }

    } 

    /**
     * Ensures that any unit outside of a weapons range can not be attacked.
     */ 
    [Test]
    public void TestUnitsCantHitTargetsOutOfRange()
    {

        Unit.IInstance testUser = new MockUnit("Unit 1", 0, 50, 0, true);


        //Test weapon can't hot under its range.
        Unit.IInstance testTarget = new MockUnit("Unit 2", 1, 50, 0, true);
        testTarget.position = 1;

        Assert.IsFalse(weakLongRange.Use(testUser, testTarget), "A weapon can not attack a target below its range.");


        //Test weapon can't hit over its range.
        testTarget = new MockUnit("Unit 3", 1, 50, 0, true);
        testTarget.position = 7;

        Assert.IsFalse(weakLongRange.Use(testUser, testTarget));

    }

}
