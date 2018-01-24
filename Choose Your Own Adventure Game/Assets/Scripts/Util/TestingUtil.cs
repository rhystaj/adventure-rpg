using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Contains static helper methods for testing.
 */ 
public class TestingUtil {

	public static string PrintsItemsOf<T>(IEnumerable<T> set)
    {

        string returnString = "\n";

        foreach(T item in set)
        {
            returnString = returnString + "- " + item + "\n";
        }

        return returnString;

    }

    /**
     * Counts the number of items produced by the given Emumerable for which the given condition holds.
     */ 
    public static int CountItemsForWhichHolds<T>(IEnumerable<T> collection, Predicate<T> condition)
    {

        int count = 0;

        foreach (T item in collection)
            if (condition.Invoke(item)) count++;

        return count;

    }

}
