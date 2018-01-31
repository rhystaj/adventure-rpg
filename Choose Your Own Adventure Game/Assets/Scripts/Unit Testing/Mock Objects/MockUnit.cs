using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockUnit : IUnit
{

    private int _alignment;
    private float _health;
    private int _position;

    private MockInstrument ins;

    public int Alignment{ get { return _alignment; } }
    public float health{ get { return _health; } set { _health = value; } }
    public int position { get; set; }

    public MockUnit(int alignment, float health, int position, bool attackSuccessful)
    {

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
        return new MockUnit(_alignment, _health, _position, ins.Use(this, this));
    }
}
