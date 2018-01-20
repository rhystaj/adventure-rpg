using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/**
 * A piece that can be placed on the board during combat.
 */
[CreateAssetMenu]
public class Unit : MonoBehaviour {

    [Header("Stats")]
    public Instrument instrument; //The main item the unit uses when moving.
    public float maxHealth; //The max health points of the unit.
    public float effectiveness; //The unit's ability with thier instrument.
    public int alignment; //The team this unit belongs to.
    public int turnCooldown; //The number of turns after this unit has been used before they can be used again.
    [Space(10)]

    [Header("Graphics")]
    [SerializeField] Sprite neutralSprite;
    [SerializeField] Sprite attakingSprite;
    [SerializeField] Sprite talkingDamageSprite;

    [HideInInspector] public int position;

    private SpriteRenderer spriteRenderer;

    public int Alignment { get { return alignment; } }

    /**
     * Constructor
     * Used to create Units for testing purposes that will not be instantiated in the scene.
     */ 
     public Unit(Instrument instrument, float maxHealth, float effectiveness, int alignment)
    {

        this.instrument = instrument;
        this.maxHealth = maxHealth;
        this.effectiveness = effectiveness;
        this.alignment = alignment;

    }


    private void OnEnable()
    {

        //Preconditions
        Assert.IsNotNull(instrument, "Precondition Fail: The property 'instrument' should not be null.");


        //Get the respective game object's sprite render, or throw an error if it doesn't have one.
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) Debug.LogError("The unit " + name + " does not have a renderer attatched.");
        spriteRenderer.sprite = neutralSprite;

        //Ensure stats aren't negative.
        if (maxHealth < 0) maxHealth = 0;
        if (effectiveness < 0) effectiveness = 0;

    }

    public bool UseInstrument(Unit target){
        return instrument.Use(this, target);
    }

}
