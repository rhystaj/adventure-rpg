using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockInstrument : Instrument
{
    public override bool Use(Unit user, Unit target){ return false; }
}
