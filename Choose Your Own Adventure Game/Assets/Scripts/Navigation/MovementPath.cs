using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MovementPath : MonoBehaviour, IEnumerable<Vector3> {

    [SerializeField] Transform[] points; // The points along the path.
    public bool traversable; //   Whether or not the path can be taken.

    [HideInInspector] public bool reverseEnumeration; //Whether or not path will be enumerated backwards.


    //Assertion Fields
    Transform[] pointsOnStart;

    private void Start()
    {
        Assert.IsTrue(RecordVariables());
    }

    /**
     * Returns the start of the path, in the order the points were added.
     */
    public MapNode StartNode
    {
        get
        {
            return points[0].GetComponent<MapNode>();
        }
    }

    /**
     * Returns the end of the path in the order the points were added.
     */ 
    public MapNode EndNode
    {
        get
        {
            return points[points.Length - 1].GetComponent<MapNode>();
        }
    }

    /**
     * Create an enumerator over the given points..
     */
    private IEnumerator<Vector3> GenerateEnumerator()
    {

        //Preconditions
        Assert.IsNotNull(points, "Precondition Fail: The field 'points' should not be null.");


        Transform[] ps = new Transform[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            //  Copt the transforms to the array to be passed into the constructor, reversing it if required.
            if (reverseEnumeration) ps[i] = points[points.Length - 1 - i];
            else ps[i] = points[i];
        }


        //Postcondtions
        Assert.IsTrue(ClassInvariantsHold());
        Assert.IsTrue(new List<Transform>(points).TrueForAll(point => new List<Transform>(ps).Contains(point)),
               "Postcondition Fail: Everything contained in points should also be contained in ps.");
        Assert.IsTrue(new List<Transform>(ps).TrueForAll(p => new List<Transform>(points).Contains(p)),
               "Postcondition Fail: Everything contained in ps sould also be contained in ps.");

        return new PathEnumerator(ps);

    }

    IEnumerator<Vector3> IEnumerable<Vector3>.GetEnumerator()
    {
        return GenerateEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GenerateEnumerator();
    }

    private void OnDrawGizmos()
    {
        //Draw lines between each of the points on the path.
        for (int i = 0; i < points.Length - 1; i++)
            if(points[i] != null && points[i + 1] != null)
                Gizmos.DrawLine(points[i].position, points[i + 1].position);
    }


    //Assertion methods
    private bool RecordVariables()
    {

        pointsOnStart = new Transform[points.Length];
        Array.Copy(points, pointsOnStart, points.Length);

        return true;

    }

    private bool ClassInvariantsHold()
    {

        for(int i = 0; i < points.Length; i++)
        {
            Assert.IsTrue(points[i] == pointsOnStart[i],
                "Postcondition Fail: points[" + i + "] (" + points[i] + ") should be equal to pointsOnStart[" + i + "] (" + pointsOnStart[i] + ").");
        }

        return true;

    }


    /**
     * An enumerator, allowing for the path to be used in a foreach loop.
     */
    public class PathEnumerator : IEnumerator<Vector3>
    {
        private Transform[] points; //The points to be enumerated over.
        private int position; // The index position of the Enumerator.

        /**
         * Constructor
         */
        public PathEnumerator(Transform[] points)
        {
            this.points = points;
            position = 0;
        }

        Vector3 IEnumerator<Vector3>.Current
        {
            get
            {
                return points[position].position;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return points[position].position;
            }
        }

        bool IEnumerator.MoveNext()
        {
            position++;
            return position < points.Length;
        }

        void IEnumerator.Reset()
        {
            position = 0;
        }

        void IDisposable.Dispose() { } // Not Applicable; 
    }
}
