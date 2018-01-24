using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingUtilTests {

    [Test]
    public void CountItemsForWhichHoldsReturnsCorrectNumber()
    {

        string[] strings = new string[]
        {
            "rhys",
            "ryan",
            "brett",
            "saylor",
            "rachel"
        };

        Assert.IsTrue(TestingUtil.CountItemsForWhichHolds(strings, str => str.StartsWith("r")) == 3);

    }

}
