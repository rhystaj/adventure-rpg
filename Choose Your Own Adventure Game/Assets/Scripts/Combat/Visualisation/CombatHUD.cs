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

    [SerializeField] Text healthTextBase; //The text object the health text is based on.
    [SerializeField] Vector2 healthTextCentreOffset; //The distance the unit's health test is from the centre of the unit.
    [Space(10)]
    [SerializeField] RawImage unitHighlightCursor; //The cursor that shows over a moused over unit.
    [SerializeField] RawImage unitSelectedCursor; //The cursor that shows over a selcted unit.
    [SerializeField] Vector2 unitCursorCentreOffset; //The distance the unit's cursor if from the centre of the unit.


    private Dictionary<CombatBoard.UnitVessel, ConsistentStatOverlay> statOverlays = new Dictionary<CombatBoard.UnitVessel, ConsistentStatOverlay>();

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


        if (!statOverlays.ContainsKey(vessel)) statOverlays.Add(vessel, new ConsistentStatOverlay(vessel, healthTextBase, 
                                                                                                  healthTextCentreOffset, GetComponent<Canvas>()));
        else statOverlays[vessel].UpdateStatsFor();

    }

    /**
     * Moves a cursor to the selected unit to show it is highlighted.
     */ 
    public void HighlightUnit(CombatBoard.UnitVessel unit)
    {

        //Preconditions
        Assert.IsNotNull(unitHighlightCursor, "Precondition Fail: The property 'unitHighlightCursor' should not be null.");
        Assert.IsNotNull(unitSelectedCursor, "Precondition Fail: The property 'unitSelectedCursor' should not be null.");


        Debug.Log("Highlighting: " + unit.name);

        
        RepositionCursorOverUnit(unitHighlightCursor, unit);
        if (unitHighlightCursor.transform.position == unitSelectedCursor.transform.position)
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


        RepositionCursorOverUnit(unitSelectedCursor, unit);
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

    /**
     * Place the given cursor over the given unit.
     */
    private void RepositionCursorOverUnit(RawImage cursorImage, CombatBoard.UnitVessel target)
    {

        //Preconditions
        Assert.IsNotNull(cursorImage, "Precondition Fail: The argument 'cursorImage' should not be null.");
        Assert.IsNotNull(target, "Precondition Fail: The argument 'target' should not be null.");
        Assert.IsTrue(statOverlays.ContainsKey(target), "Precondition Fail: The given unit should have a stat overlay.");

        cursorImage.gameObject.SetActive(true);
        cursorImage.transform.position = statOverlays[target].unitPositionOnScreen + unitCursorCentreOffset;

    }

    /**
     * A tuple housing the UI elements that are consitently displayed on the screen, positioned in relation to thier respective unit.
     */ 
    private class ConsistentStatOverlay
    {

        private Once<Canvas> parentCanvas = new Once<Canvas>(); //The canvas displaying the text.
        private Once<CombatBoard.UnitVessel> vessel = new Once<CombatBoard.UnitVessel>(); //The vessel this overlay is displaying the stats for.
        private Once<Text> healthText = new Once<Text>(); //The text displaying the unit's current health.
        private Vector2 healthTextCentreOffset;

        public Vector2 unitPositionOnScreen
        {
            get { return parentCanvas.Value.worldCamera.WorldToScreenPoint(vessel.Value.transform.position); }
        }

        public ConsistentStatOverlay(CombatBoard.UnitVessel vessel, Text healthTextBase, Vector2 healthTextCentreOffset, Canvas parent)
        {

            //Preconditions
            Assert.IsNotNull(vessel, "Precondition Fail: The argument 'vessel' should not be null.");
            Assert.IsNotNull(vessel.unit, "Precondition Fail: The given vessel should house a unit.");


            parentCanvas.Value = parent;
            this.vessel.Value = vessel;


            //Create the Text component with the given text and position it correctly in relation to the unit.
            healthText.Value = Instantiate(healthTextBase, parent.transform);
            healthText.Value.text = vessel.unit.health + "";

            RepositionStats();


            //Postconditions
            Assert.IsNotNull(healthText.Value, "The field 'healthText' should not be null.");
            Assert.IsTrue(healthText.Value.text.Equals(vessel.unit.health + ""),
                          "Precondition Fail: 'healthTest' should be correctly displaying the unit's health.");

        }

        /**
         * Updates the position and display of the stats.
         */ 
        public void UpdateStatsFor()
        {

            //Preconditions
            Assert.IsNotNull(vessel, "Precondition Fail: The argument 'vessel' should not be null.");
            Assert.IsNotNull(healthText.Value, "Precondition Fail: The field 'healthText' should not be null.");


            healthText.Value.text = vessel.Value.unit.health + "";
            RepositionStats();


        }

        /**
         * Repositions the stats of the given unit on the screen.
         */ 
        private void RepositionStats()
        {

            //Preconditions
            Assert.IsNotNull(vessel, "Precondition Fail: The argument 'vessel' should not be null.");
            Assert.IsNotNull(parentCanvas.Value, "Precondition Fail The field 'parentCanvas' should not be null.");

            Debug.Log("Health Text Centre Offset: " + healthTextCentreOffset);
            healthText.Value.transform.position = unitPositionOnScreen + healthTextCentreOffset;


        }

    }

}
