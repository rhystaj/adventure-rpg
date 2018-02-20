using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockUnit : Unit.IInstance, IEquatable<MockUnit>
{

    private string name;

    private float _maxHealth;
    private int _alignment;
    private float _health;
    private int _position;

    private MockInstrument ins;

    public Vector3 scaleOnBoard { get { return Vector3.one; } }
    public int alignment{ get { return _alignment; } }
    public float health{ get { return _health; } set { _health = value; } }
    public int position { get; set; }

    public float maxHealth { get { return _maxHealth; } }

    public RuntimeAnimatorController animatorController
    {
        get
        {
            return null;
        }
    }

    public MockUnit(string name, int alignment, float maxHealth, float health, int position, bool attackSuccessful)
    {

        this.name = name;

        _maxHealth = maxHealth;
        _alignment = alignment;
        _health = health;
        _position = position;

        ins = new MockInstrument(attackSuccessful);

    }

    public MockUnit(string name, int alignment, float health, int position, bool attackSuccessful)
    {

        this.name = name;

        _alignment = alignment;
        _health = health;
        _position = position;

        ins = new MockInstrument(attackSuccessful);

    }

    public bool UseInstrument(Unit.IInstance target)
    {
        return ins.Use(this, target);
    }

    public bool CanUseInstrumentOn(Unit.IInstance target)
    {
        return ins.CanUse(this, target);
    }

    public override string ToString()
    {
        return name;
    }

    public bool Equals(MockUnit other)
    {
        return name.Equals(other.name);
    }

}
