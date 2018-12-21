using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CombatAnimator
{

    private Dictionary<Unit.IInstance, CombatBoard.UnitVessel> vessels = new Dictionary<Unit.IInstance, CombatBoard.UnitVessel>(); //The instances of units mapped to the vessels that house them.

    public CombatAnimator(CombatBoard.UnitVessel[] vessels)
    {

        //Preconditions
        Assert.IsNotNull(vessels, "The argument 'vessels' should not be null.");

        foreach (CombatBoard.UnitVessel vessel in vessels)
        {
            this.vessels.Add(vessel.unit, vessel);
        }


    }

    public IEnumerator UpdateUnitOverlay(Unit.IInstance unit)
    {

        //Preconditions
        Assert.IsNotNull(unit, "Precondition Fail: The argument 'unit' should not be null.");
        Assert.IsTrue(vessels.ContainsKey(unit),
                      "Precondition Fail: The given unit should be a key in state.");

        vessels[unit].UpdateStats();

        yield return null;

    }

    /**
     * Runs and returns an attack animation.
     */ 
    public CombatAnimationCooroutine RetrieveUnitAttackAnimation(bool runOnRetrievel, Action PreAnimation, Unit.IInstance unit, 
                                                                 CombatAnimationCooroutine attackConnectAnimation)
    {

        //Preconditions
        Assert.IsNotNull(PreAnimation, "Precondition Fail: The delegate 'PreAnimation' should have an Action assigned.");
        Assert.IsNotNull(unit, "Precondition Fail: The argument 'unit' should not be null.");
        Assert.IsTrue(vessels.ContainsKey(unit), "Precondition Fail: The given unit should have a vessel mapped to it.");
        Assert.IsNotNull(attackConnectAnimation, "Precondition Fail: The argument 'attackConnectAnimation' should not be null.");
        Assert.IsTrue(attackConnectAnimation is RunnableCombatAnimationCooroutine,
                      "Precondition Fail: 'attackConnectAnimation' should be of subtype 'RunnableCombatAnimationCooroutine.'");


        CombatBoard.UnitVessel targetVessel = vessels[unit];


        RunnableCombatAnimationCooroutine result = new AttackAnimation(PreAnimation, targetVessel, 
                                                                       (RunnableCombatAnimationCooroutine)attackConnectAnimation);

        if(runOnRetrievel) result.Run();
        return result;

    }

    public CombatAnimationCooroutine RetrieveEventlessAnimation(bool runOnRetievel, Action PreAnimation, Unit.IInstance unit, Unit.Pose animation)
    {

        //Preconditions
        Assert.IsNotNull(PreAnimation, "Precondition Fail: The delegate 'PreAnimation' should have an Action assigned.");
        Assert.IsNotNull(unit, "Precondition Fail: The argument 'unit' should not be null.");
        Assert.IsTrue(vessels.ContainsKey(unit), "Precondition Fail: The given unit should have a vessel mapped to it.");


        CombatBoard.UnitVessel targetVessel = vessels[unit];


        RunnableCombatAnimationCooroutine result = new EventlessAnimation(PreAnimation, animation, targetVessel);

        if(runOnRetievel) result.Run();
        return result;
    }

    /**
     * The base class for a coroutine that runs for as long as a combat animation runs.
     */ 
    public abstract class CombatAnimationCooroutine : IEnumerator
    {
        private float currentNormalisedTime = -1;
        private bool beingAnimated = false;

        protected CombatBoard.UnitVessel targetVessel; //The vessel being animated.
        protected Action PreAnimation; //The action to be preformed before the animation starts.

        public object Current { get { return null; } } //NA


        public CombatAnimationCooroutine(Action PreAnimation, CombatBoard.UnitVessel vessel)
        {

            targetVessel = vessel;
            beingAnimated = true;

            targetVessel.OnMoveEndAnimationEvent = () => targetVessel.SetPose(Unit.Pose.Idle);

        }

        public virtual bool MoveNext()
        {

            //Preconditions
            Assert.IsNotNull(targetVessel, "Precondition Fail: The field 'targetVessel' should not be null");

            
            //Update normalised time, keeping track of the previous value.
            float prevNormalisedTime = currentNormalisedTime;
            currentNormalisedTime = targetVessel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;

            if (!beingAnimated) return false;

            //If the previous normalisedTimeValue is greater than the current one, then the animation has looped, and should be stopped.
            if (prevNormalisedTime > currentNormalisedTime)
            {

                
                targetVessel.OnMoveEnd();
                beingAnimated = false;

                return false;

            }
            else return true;

        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }

    /**
     * A private animation class who's respective animation can only be started within the CombatAnimator class.
     */ 
    private abstract class RunnableCombatAnimationCooroutine : CombatAnimationCooroutine
    {

        public RunnableCombatAnimationCooroutine(Action PreAnimation, CombatBoard.UnitVessel vessel) : base(PreAnimation, vessel){}

        public virtual void Run() { if (PreAnimation != null) PreAnimation(); }
    }

    /**
     * An animation for a unit attack.
     */ 
    private class AttackAnimation : RunnableCombatAnimationCooroutine
    {

        private RunnableCombatAnimationCooroutine attackConnectAnimation; //The animation to be played when the attack connects.

        public AttackAnimation(Action PreAnimation, CombatBoard.UnitVessel vessel, RunnableCombatAnimationCooroutine attackConnectAnimation) : 
                          base(PreAnimation, vessel)
        {

            this.attackConnectAnimation = attackConnectAnimation;
            targetVessel.OnAttackConnectAnimationEvent = () => attackConnectAnimation.Run();

        }

        public override void Run()
        {
            base.Run();
            targetVessel.SetPose(Unit.Pose.Attacking);
        }

        public override bool MoveNext()
        {
            return base.MoveNext() || attackConnectAnimation.MoveNext();
        }
    }

    /**
     * An animation that has no special events, though still calls on end.
     */
    private class EventlessAnimation : RunnableCombatAnimationCooroutine
    {

        Unit.Pose animation; //The animation to play when this event is run.

        public EventlessAnimation(Action PreAnimation, Unit.Pose animation, CombatBoard.UnitVessel vessel) : base(PreAnimation, vessel)
        {
            targetVessel.SetPose(animation);
        }

        public override void Run()
        {
            base.Run();
            targetVessel.SetPose(animation);
        }

    }

}
