using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * An object that performs combat actions to play the game, be it from player input, or AI.
 */ 
public interface IController{

    /**
     * Preform actions neccessary to be performed before the move can be determined. 
     */
    void PrepareForMove(Unit.IInstance[,] board, HashSet<Unit.IInstance> avaliableUnits);

    /**
     * Perform an action on the given combat if possible.
     */ 
     ICombatAction DetermineMove(Unit.IInstance[,] board, HashSet<Unit.IInstance> avaliableUnits);

    /**
     * Preform the actions neccessary before the next controller has thier turn.
     */
    void PostMove(Unit.IInstance[,] board, HashSet<Unit.IInstance> avaliableUnits);
        
}
