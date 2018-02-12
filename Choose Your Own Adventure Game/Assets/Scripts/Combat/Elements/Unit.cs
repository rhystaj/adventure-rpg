using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * Represents the specs of a single unit in gameplay.
 */ 
[CreateAssetMenu]
public class Unit : ScriptableObject
{

    [Header("Stats")]
    [SerializeField] private Instrument instrument; //The main item the unit uses when moving.
    [SerializeField] private float maxHealth; //The max health points of the unit.
    [SerializeField] private float effectiveness; //The unit's ability with thier instrument.
    [SerializeField] private int alignment; //The team this unit belongs to.
    [SerializeField] private int turnCooldown; //The number of turns after this unit has been used before they can be used again.
    [Space(10)]

    [Header("Graphics")]
    [SerializeField] Sprite idleSprite;
    [SerializeField] Sprite attakingSprite;
    [SerializeField] Sprite talkingDamageSprite;
    [SerializeField] Sprite generalActionSprite;
    [SerializeField] Sprite incapacitatedSprite;

    public enum State
    {
        Idle, Attacking, TakingDamage, GeneralAction, Incapacitated
    }

    public interface IInstance
    {

        float maxHealth { get; }
        int alignment { get; }


        float health { get; set; }
        int position { get; set; }
        State state { get; }

        bool UseInstrument(IInstance target);
        bool CanUseInstrumentOn(IInstance target);
        Sprite GetImageForState(State state);

    }

    /**
    * A piece that can be placed on the board during combat.
    */
    public class Instance : IInstance, IEquatable<Instance>
    {

        private Once<Unit> unit = new Once<Unit>();
        private Once<Dictionary<State, Sprite>> imagesForStates = new Once<Dictionary<State, Sprite>>();

        public float maxHealth { get { return unit.Value.maxHealth; } }
        public int alignment { get { return unit.Value.alignment; } }

        private float _health; //The health of the unit.
        public float health
        {
            get
            {
                return _health;
            }
            set
            {
                if (value < 0) _health = 0;
                else _health = value;
            }
        }

        private int _position;
        public int position { get; set; }

        public State state
        {
            //Only Incapacitated and Idle, are persitant, the other states are fleeting and used for the purposes of animation.
            get
            {
                if (health <= 0) return State.Incapacitated;
                else return State.Idle;
            }
        }

        public Instance(Unit unit)
        {

            //Preconditions
            Assert.IsNotNull(unit, "Precondition Fail: The argument unit should not be null.");
            Assert.IsNotNull(unit.instrument, "Precondition Fail: The given unit should have an instrument.");

            this.unit.Value = unit;

            health = unit.maxHealth > 0 ? unit.maxHealth : 0;


            //Store images.
            imagesForStates.Value = new Dictionary<State, Sprite>();
            imagesForStates.Value.Add(State.Idle, unit.idleSprite);
            imagesForStates.Value.Add(State.Attacking, unit.attakingSprite);
            imagesForStates.Value.Add(State.TakingDamage, unit.talkingDamageSprite);
            imagesForStates.Value.Add(State.GeneralAction, unit.generalActionSprite);
            imagesForStates.Value.Add(State.Incapacitated, unit.incapacitatedSprite);

        }

        public virtual bool UseInstrument(IInstance target)
        {
            return unit.Value.instrument.Use(this, target);
        }

        public bool CanUseInstrumentOn(IInstance target)
        {
            return unit.Value.instrument.CanUse(this, target);
        }

        public bool Equals(Instance other)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return unit.Value.name;
        }

        public Sprite GetImageForState(State state) { 
            return imagesForStates.Value[state];
        }
    }

}