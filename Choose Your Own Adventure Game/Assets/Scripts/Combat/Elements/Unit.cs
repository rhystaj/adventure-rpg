using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/**
 * A piece that can be placed on the board during combat.
 */
public class Unit : MonoBehaviour {

    //The path to the prefab used as a base for construted mock objects for testing.
    public const string MOCK_BASE_PATH = "Testing/Mock Prefabs/Combat/Mock Unit Base"; 

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


    [HideInInspector] public float health; //The health of the unit.
    public int Alignment { get { return alignment; } }

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


        health = maxHealth;


    }

    public bool UseInstrument(Unit target){
        return instrument.Use(this, target);
    }

    public static Unit InstantiateMock(Instrument instrument, float maxHealth, float effectiveness, int alignment, int turnCooldown)
    {

        //  Create a clone from the mock unit prefab.
        GameObject mockBase = Resources.Load<GameObject>(MOCK_BASE_PATH);
        Unit newUnit = Instantiate(mockBase).GetComponent<Unit>();

        
        //  Set the properties of the clonne to given ones.
        newUnit.instrument = instrument;
        newUnit.maxHealth = maxHealth;
        newUnit.effectiveness = effectiveness;
        newUnit.alignment = alignment;
        newUnit.turnCooldown = turnCooldown;

        return newUnit;

    }

}
