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

    public override bool CanUse(Unit.IInstance user, Unit.IInstance target) { return successful; }

    public override bool Use(Unit.IInstance user, Unit.IInstance target){ return successful; }
}
