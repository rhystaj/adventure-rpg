using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnceTests {

	[Test]
    public void ValueInOnceCanBeAssignedOnce()
    {

        Once<string> testOnce = new Once<string>();
        testOnce.Value = "test";

    }

    [Test]
    public void ValueInOnceCanNotBeReassigned()
    {

        Once<string> testOnce = new Once<string>();
        testOnce.Value = "test";

        try
        {
            testOnce.Value = "test2";
            Assert.Fail();
        }
        catch(Once<string>.ValueAlreadyAssignedException e) { }

    }

}
