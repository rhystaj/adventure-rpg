using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * An object that performs combat actions to play the game, be it from player input, or AI.
 */ 
public interface IController{

    /**
     * Perform an action on the given combat if possible.
     */ 
     ICombatAction DetermineMove(Unit.IInstance[,] board, HashSet<Unit.IInstance> avaliableUnits);

}
