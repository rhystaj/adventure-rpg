using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockInstrument : Instrument
{

    private bool successful;

    public MockInstrument(bool successful)
    {
        this.successful = successful;
    }

    public override bool Use(IUnit user, IUnit target){ return successful; }
}
