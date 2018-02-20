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
    [SerializeField] private Vector3 scaleOnBoard; //The sclae of the unit's respective vessel when drawn to the board.
    [SerializeField] private Instrument instrument; //The main item the unit uses when moving.
    [SerializeField] private float maxHealth; //The max health points of the unit.
    [SerializeField] private float effectiveness; //The unit's ability with thier instrument.
    [SerializeField] private int alignment; //The team this unit belongs to.
    [SerializeField] private int turnCooldown; //The number of turns after this unit has been used before they can be used again.
    [Space(10)]

    [Header("Graphics")]
    [SerializeField] RuntimeAnimatorController animatorController; //Coordinates the animations in combat.

    public interface IInstance
    {

        float maxHealth { get; }
        int alignment { get; }


        Vector3 scaleOnBoard { get; }
        float health { get; set; }
        int position { get; set; }
        RuntimeAnimatorController animatorController { get; }

        bool UseInstrument(IInstance target);
        bool CanUseInstrumentOn(IInstance target);

    }

    public enum Pose
    {
        Idle = 0, Attacking = 1, TakingDamage = 2
    }

    /**
    * A piece that can be placed on the board during combat.
    */
    public class Instance : IInstance, IEquatable<Instance>
    {

        private Once<Unit> unit = new Once<Unit>();
        private Once<RuntimeAnimatorController> _animatorController = new Once<RuntimeAnimatorController>();

        public Vector3 scaleOnBoard { get { return unit.Value.scaleOnBoard; } }
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

        public RuntimeAnimatorController animatorController { get { return _animatorController.Value; } }

        public Instance(Unit unit)
        {

            //Preconditions
            Assert.IsNotNull(unit, "Precondition Fail: The argument unit should not be null.");
            Assert.IsNotNull(unit.instrument, "Precondition Fail: The given unit should have an instrument.");

            this.unit.Value = unit;

            health = unit.maxHealth > 0 ? unit.maxHealth : 0;


            _animatorController.Value = unit.animatorController;

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

    }

}