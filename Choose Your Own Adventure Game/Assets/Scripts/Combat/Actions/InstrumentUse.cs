using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/**
 * Respresents the use of an instrument in combat.
 */ 
public class InstrumentUse : ICombatAction
{

    private Once<Unit.IInstance> user = new Once<Unit.IInstance>();
    private Once<Unit.IInstance> target = new Once<Unit.IInstance>();

    public InstrumentUse(Unit.IInstance user, Unit.IInstance target)
    {

        //Preconditions
        Assert.IsNotNull(user, "Precondition Fail: The argument 'user' should not be null.");
        Assert.IsNotNull(target, "Precondition Fail: The argument 'target should not be null.");
        Assert.IsFalse(user.health == 0, "Precondition Fail: The health of the user should not be 0.");
        Assert.IsTrue(user.CanUseInstrumentOn(target),
                      "Precondition Fail: The instrument use should be valid for the given user and target.");


        this.user.Value = user;
        this.target.Value = target;


        //Postconditions
        Assert.IsTrue(ClassInvariantsHold());
        Assert.IsNotNull(this.user.Value, "Postcondition Fail: The field 'user' should not be null.");
        Assert.IsNotNull(this.target.Value, "Postcondition Fail: The field 'target' should not be null.");


    }

    public IEnumerator Perform(CombatAnimator animator, ICombatScenario scenario) {

        //Preconditions
        Assert.IsNotNull(animator, "Precondition Fail: The argument 'animator' should not be null");
        Assert.IsNotNull(scenario, "Precondition Fail: The argument 'scenario' should not be null.");
        Assert.IsNotNull(user.Value, "Precondition Fail: The field 'user' should not be null.");
        Assert.IsNotNull(target.Value, "Precondition Fail: The field 'target' should not be null.");
        Assert.IsTrue(TestingUtil.Convert2DArrayToList(scenario.Board).Contains(user.Value),
                      "Precondition Fail: The board of the given scenario should contain 'user'");
        Assert.IsTrue(TestingUtil.Convert2DArrayToList(scenario.Board).Contains(target.Value),
                      "Precondition Fail: The board of the given scenario should contain 'target'");
        Assert.IsTrue(scenario.unitsAvaliableToMove.Contains(user.Value),
                      "Preconditon Fail: The 'user' should be avaliable in the given scenario.");

        yield return animator.RetrieveUnitAttackAnimation(true, () => { }, user.Value,
                                                     animator.RetrieveEventlessAnimation(
                                                        false,
                                                        () =>
                                                        {
                                                            scenario.UseInstrument(user.Value, target.Value);
                                                            animator.UpdateUnitOverlay(target.Value);
                                                        },
                                                        target.Value, Unit.Pose.TakingDamage
                                                     ));


        //Postconditions
        Assert.IsTrue(ClassInvariantsHold());

    }


    //Assertion
    private bool ClassInvariantsHold()
    {

        Assert.IsFalse(user.Value.health == 0, "Precondition Fail: The health of the user should not be 0.");
        Assert.IsTrue(user.Value.CanUseInstrumentOn(target.Value),
                      "Precondition Fail: The instrument use should be valid for the given user and target.");

        return true;

    }

}
