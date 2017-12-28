using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A piece that can be placed on the board during combat.
 */ 
[CreateAssetMenu]
public class Unit : ScriptableObject {

    [Header("Stats")]
    [SerializeField] Instrument instument; //The main item the unit uses when moving.
    [SerializeField] float maxHealth; //The max health points of the unit.
    [SerializeField] float effectiveness; //The unit's ability with thier instrument.
    [SerializeField] int alignment; //The team this unit belongs to.
    [Space(10)]

    [Header("Graphics")]
    [SerializeField] Sprite neutralSprite;
    [SerializeField] Sprite attakingSprite;
    [SerializeField] Sprite talkingDamageSprite;
    
    [HideInInspector] public float health;

    public int Alignment { get { return alignment; } }

    private void OnEnable()
    {

        //Ensure stats aren't negative.
        if (maxHealth < 0) maxHealth = 0;
        if (effectiveness < 0) effectiveness = 0;

        health = maxHealth;

    }

    public bool UseInstrument(Unit target){
        return instument.Use(this, target);
    }

}
