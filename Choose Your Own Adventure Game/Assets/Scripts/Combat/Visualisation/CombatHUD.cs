using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

/**
 * Displays information pertaining the the state of units.
 */
public class CombatHUD : MonoBehaviour {

    private const float HEALTH_TEXT_CENTRE_OFFEST_X = 0; //How far along the x axis from the centre of a unit it's health is displayed.
    private const float HEALTH_TEXT_CENTRE_OFFEST_Y = 0; //How far along the y axis from the centre of a unit it's health is displayed.

    [SerializeField] StatDisplay healthDisplay; //The text object the health text is based on.
    [SerializeField] VesselCursor unitHighlightCursor; //The cursor that shows over a moused over unit.
    [SerializeField] VesselCursor unitSelectedCursor; //The cursor that shows over a selcted unit.


    private Dictionary<CombatBoard.UnitVessel, StatDisplay> healthDisplays = new Dictionary<CombatBoard.UnitVessel, StatDisplay>();

    private void Start()
    {

        //Preconditions
        Assert.IsNotNull(unitHighlightCursor, "Precondition Fail: The property 'unitHighlightCursor' should not be null.");
        Assert.IsNotNull(unitSelectedCursor, "Precondition Fail: The property 'unitSelectedCursor' should not be null.");
        Assert.IsNotNull(GetComponent<Canvas>(), "Precondition Fail: The component 'Canvas' should be attacthed to the GameObject.");


        //Instantiate the cursors.
        unitHighlightCursor = Instantiate(unitHighlightCursor, GetComponent<Canvas>().transform);
        unitSelectedCursor = Instantiate(unitSelectedCursor, GetComponent<Canvas>().transform);


        //Hide both cursors initially.
        unitHighlightCursor.gameObject.SetActive(false);
        unitSelectedCursor.gameObject.SetActive(false);


        //Preconditions
        Assert.IsNotNull(unitHighlightCursor, "Postcondition Fail: The property 'unitHighlightCursor' should not be null.");
        Assert.IsNotNull(unitSelectedCursor, "Postcondition Fail: The property 'unitSelectedCursor' should not be null.");

    }

    /**
     * Update the UI to display the correct stats for the given unit.
     */
    public void UpdateStatsFor(CombatBoard.UnitVessel vessel)
    {

        //Preconditions
        Assert.IsNotNull(vessel, "Precondition Fail: The argument 'vessel' should not be null.");
        Assert.IsNotNull(GetComponent<Canvas>(), "Precondition Fail: This component's GameObject should also have a Canvas attatched.");


        if (!healthDisplays.ContainsKey(vessel)) healthDisplays.Add(vessel, Instantiate(healthDisplay, transform));
        healthDisplays[vessel].ReconfigureAppearance(GetComponent<Canvas>().worldCamera, vessel);
        healthDisplays[vessel].VisualiseStat(vessel.unit.health, vessel.unit.maxHealth);

    }

    /**
     * Moves a cursor to the selected unit to show it is highlighted.
     */ 
    public void HighlightUnit(CombatBoard.UnitVessel unit)
    {

        //Preconditions
        Assert.IsNotNull(unitHighlightCursor, "Precondition Fail: The property 'unitHighlightCursor' should not be null.");
        Assert.IsNotNull(unitSelectedCursor, "Precondition Fail: The property 'unitSelectedCursor' should not be null.");


        unitHighlightCursor.ReconfigureAppearance(GetComponent<Canvas>().worldCamera, unit);
        if (unitSelectedCursor.vesselCurrentlyHighlighting != null && 
            ReferenceEquals(unitSelectedCursor.vesselCurrentlyHighlighting, unitHighlightCursor.vesselCurrentlyHighlighting))
            unitHighlightCursor.gameObject.SetActive(false);


        //Postconditions
        Assert.IsTrue(unitSelectedCursor.transform.position != unitHighlightCursor.transform.position || !unitHighlightCursor.gameObject.activeInHierarchy,
                      "Postcondition Fail: If both the selected and highlighted cursor are set to be over the same unit, only the selected cursor should" +
                      "be shown.");

    }

    public void HideHighlight()
    {

        //Preconditions
        Assert.IsNotNull(unitHighlightCursor, "Precondition Fail: The property 'unitHighlightCursor' should not be null.");


        unitHighlightCursor.transform.position = Vector3.zero;
        unitHighlightCursor.gameObject.SetActive(false);

    }

    /**
     * Moves a cursor to the selected unit to show it is selected.
     */
    public void SelectUnit(CombatBoard.UnitVessel unit)
    {

        //Preconditions
        Assert.IsNotNull(unitHighlightCursor, "Precondition Fail: The property 'unitHighlightCursor' should not be null.");
        Assert.IsNotNull(unitSelectedCursor, "Precondition Fail: The property 'unitSelectedCursor' should not be null.");


        unitSelectedCursor.ReconfigureAppearance(GetComponent<Canvas>().worldCamera, unit);
        if (unitHighlightCursor.transform.position == unitSelectedCursor.transform.position)
            unitHighlightCursor.gameObject.SetActive(false);


        //Postconditions
        Assert.IsTrue(unitSelectedCursor.transform.position != unitHighlightCursor.transform.position || !unitHighlightCursor.gameObject.activeInHierarchy,
                      "Postcondition Fail: If both the selected and highlighted cursor are set to be over the same unit, only the selected cursor should" +
                      "be shown.");

    }

    public void HideSelection()
    {

        //Preconditions
        Assert.IsNotNull(unitHighlightCursor, "Precondition Fail: The property 'unitHighlightCursor' should not be null.");


        unitSelectedCursor.transform.position = Vector3.zero;
        unitSelectedCursor.gameObject.SetActive(false);

    }

}
