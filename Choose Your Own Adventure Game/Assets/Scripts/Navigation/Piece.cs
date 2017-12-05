using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * An object that moves across the path on the map.
 */ 
public class Piece : MonoBehaviour {

    public float movementSpeed; // The speed at which a piece moves along a path, in units per second.

    private bool beingAnimated; //   Whether or not this piece's movement is being animated.

    /**
     * Returns whether a piece is moving along a path.
     */ 
    public bool isMoving()
    {
        return beingAnimated;
    }

    /**
     * Has the piece move along the given path, and fire an event when it gets there.
     */ 
    public IEnumerator MoveAlongPath(MovementPath path)
    {

        beingAnimated = true;

        //Iterate over the points, animating movement to each.
        foreach (Vector3 point in path)
            yield return OverTime.Move(transform, transform.position, point, Vector3.Distance(transform.position, point) / movementSpeed, null);

        beingAnimated = false;


        //  Notify the node at the end of the path that it has been reached.
        if (path.reverseEnumeration) path.Start.NotifyReached();
        else path.End.NotifyReached();

    }

}
