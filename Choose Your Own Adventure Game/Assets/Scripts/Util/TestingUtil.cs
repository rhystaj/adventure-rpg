using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Contains static helper methods for testing.
 */ 
public class TestingUtil {

    public delegate string Stringify<T>(T t);
    public delegate float Floatify<T>(T t);

	public static string PrintsItemsOf<T>(IEnumerable<T> set)
    {

        string returnString = "\n";

        foreach(T item in set)
        {
            returnString = returnString + "- " + item + "\n";
        }

        return returnString;

    }

    public static string PrintsItemsAs<T>(IEnumerable<T> set, Stringify<T> str)
    {

        string returnString = "\n";

        foreach (T item in set)
        {
            returnString = returnString + "- " + str(item) + "\n";
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

    /**
     * Returns the max formatted float value of the given set.
     */
     public static float FindMaxAs<T>(IEnumerable<T> set, Func<T, float> func)
    {

        float max = func.Invoke(new List<T>(set)[0]);

        foreach (T item in set)
        {
            float currentItemValue = func.Invoke(item);
            if (currentItemValue > max) max = currentItemValue;
        }

        return max;

    }

    /**
    * Returns the min formatted float value of the given set.
    */
    public static float FindMinAs<T>(IEnumerable<T> set, Func<T, float> func)
    {

        float min = func.Invoke(new List<T>(set)[0]);

        foreach (T item in set)
        {
            float currentItemValue = func.Invoke(item);
            if (currentItemValue < min) min = currentItemValue;
        }

        return min;

    }

}
