using NUnit.Framework;
using System;
using UnityEngine;

/**
 * Represents the specs of a single unit in gameplay.
 */ 
public class Unit : ScriptableObject
{

    [Header("Stats")]
    [SerializeField] private IInstrument instrument; //The main item the unit uses when moving.
    [SerializeField] private float maxHealth; //The max health points of the unit.
    [SerializeField] private float effectiveness; //The unit's ability with thier instrument.
    [SerializeField] private int alignment; //The team this unit belongs to.
    [SerializeField] private int turnCooldown; //The number of turns after this unit has been used before they can be used again.
    [Space(10)]

    [Header("Graphics")]
    [SerializeField] Sprite neutralSprite;
    [SerializeField] Sprite attakingSprite;
    [SerializeField] Sprite talkingDamageSprite;

    public interface IInstance
    {

        float maxHealth { get; }
        int alignment { get; }


        float health { get; set; }
        int position { get; set; }

        bool UseInstrument(IInstance target);
        bool CanUseInstrumentOn(IInstance target);

    }

    /**
    * A piece that can be placed on the board during combat.
    */
    public class Instance : IInstance, IEquatable<Instance>
    {

        private Unit unit;

        public float maxHealth { get { return unit.maxHealth; } }
        public int alignment { get { return unit.alignment; } }


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

 

        public Instance(Unit unit)
        {

            //Preconditions
            Assert.IsNotNull(unit, "Precondition Fail: The argument unit should not be null.");
            Assert.IsNotNull(unit.instrument, "Precondition Fail: The given unit should have an instrument.");

            this.unit = unit;

            health = unit.maxHealth > 0 ? unit.maxHealth : 0;

        }

        public virtual bool UseInstrument(IInstance target)
        {
            return unit.instrument.Use(this, target);
        }

        public bool CanUseInstrumentOn(IInstance target)
        {
            return unit.instrument.CanUse(this, target);
        }

        public bool Equals(Instance other)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return unit.name;
        }
    }

}