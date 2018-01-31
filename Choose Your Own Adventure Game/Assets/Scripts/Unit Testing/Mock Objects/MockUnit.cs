using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockUnit : IUnit, IEquatable<MockUnit>
{

    private string name;

    private int _alignment;
    private float _health;
    private int _position;

    private MockInstrument ins;

    public int Alignment{ get { return _alignment; } }
    public float health{ get { return _health; } set { _health = value; } }
    public int position { get; set; }

    public MockUnit(string name, int alignment, float health, int position, bool attackSuccessful)
    {

        this.name = name;

        _alignment = alignment;
        _health = health;
        _position = position;

        ins = new MockInstrument(attackSuccessful);

    }

    public bool UseInstrument(IUnit target)
    {
        return ins.Use(this, target);
    }

    public IUnit InstantiateClone()
    {
        return new MockUnit(name, _alignment, _health, _position, ins.Use(this, this));
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
