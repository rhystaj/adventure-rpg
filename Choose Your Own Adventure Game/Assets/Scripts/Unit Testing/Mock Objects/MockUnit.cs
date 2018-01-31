using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockUnit : IUnit
{
    public int Alignment
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public float health { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }

    public bool UseInstrument(Unit target)
    {
        throw new NotImplementedException();
    }
}
