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
    public Instrument instument; //The main item the unit uses when moving.
    public float maxHealth; //The max health points of the unit.
    public float effectiveness; //The unit's ability with thier instrument.
    public int alignment; //The team this unit belongs to.
    [Space(10)]

    [Header("Graphics")]
    [SerializeField] Sprite neutralSprite;
    [SerializeField] Sprite attakingSprite;
    [SerializeField] Sprite talkingDamageSprite;

    [HideInInspector] public int position;

    private SpriteRenderer spriteRenderer;

    public int Alignment { get { return alignment; } }

    private void OnEnable()
    {

        //Preconditions
        Assert.IsNotNull(instument, "Precondition Fail: The property 'instrument' should not be null.");


        //Get the respective game object's sprite render, or throw an error if it doesn't have one.
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) Debug.LogError("The unit " + name + " does not have a renderer attatched.");
        spriteRenderer.sprite = neutralSprite;

        //Ensure stats aren't negative.
        if (maxHealth < 0) maxHealth = 0;
        if (effectiveness < 0) effectiveness = 0;

    }

    public bool UseInstrument(Unit target){
        return instument.Use(this, target);
    }

}
