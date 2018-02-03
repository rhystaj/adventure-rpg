using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockInstrument : IInstrument
{

    private bool successful;

    public MockInstrument(bool successful)
    {
        this.successful = successful;
    }

    public bool Use(Unit.IInstance user, Unit.IInstance target){ return successful; }
}
