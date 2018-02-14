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

    private Dictionary<CombatBoard.UnitVessel, ConsistentStatOverlay> statOverlays = new Dictionary<CombatBoard.UnitVessel, ConsistentStatOverlay>();

    /**
     * Update the UI to display the correct stats for the given unit.
     */ 
    public void UpdateStatsFor(CombatBoard.UnitVessel vessel)
    {

        //Preconditions
        Assert.IsNotNull(vessel, "Precondition Fail: The argument 'vessel' should not be null.");
        Assert.IsNotNull(GetComponent<Canvas>(), "Precondition Fail: This component's GameObject should also have a Canvas attatched.");


        if (!statOverlays.ContainsKey(vessel)) statOverlays.Add(vessel, new ConsistentStatOverlay(vessel, healthTextBase, GetComponent<Canvas>()));
        else statOverlays[vessel].UpdateStatsFor();


    }

    /**
     * A tuple housing the UI elements that are consitently displayed on the screen, positioned in relation to thier respective unit.
     */ 
    private class ConsistentStatOverlay
    {

        private Once<Canvas> parentCanvas = new Once<Canvas>(); //The canvas displaying the text.
        private Once<CombatBoard.UnitVessel> vessel = new Once<CombatBoard.UnitVessel>(); //The vessel this overlay is displaying the stats for.
        private Once<Text> healthText = new Once<Text>(); //The text displaying the unit's current health.

        public ConsistentStatOverlay(CombatBoard.UnitVessel vessel, Text healthTextBase, Canvas parent)
        {

            //Preconditions
            Assert.IsNotNull(vessel, "Precondition Fail: The argument 'vessel' should not be null.");
            Assert.IsNotNull(vessel.unit, "Precondition Fail: The given vessel should house a unit.");


            parentCanvas.Value = parent;
            this.vessel.Value = vessel;


            //Create the Text component with the given text and position it correctly in relation to the unit.
            healthText.Value = Instantiate(healthTextBase, parent.transform);
            healthText.Value.text = vessel.unit.health + "";

            RepositionStats(vessel);


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
            RepositionStats(vessel.Value);


        }

        /**
         * Repositions the stats of the given unit on the screen.
         */ 
        private void RepositionStats(CombatBoard.UnitVessel vessel)
        {

            //Preconditions
            Assert.IsNotNull(vessel, "Precondition Fail: The argument 'vessel' should not be null.");
            Assert.IsNotNull(parentCanvas.Value, "Precondition Fail The field 'parentCanvas' should not be null.");


            Vector3 unitPositionOnScreen = parentCanvas.Value.worldCamera.WorldToScreenPoint(vessel.transform.position);

            healthText.Value.transform.position = unitPositionOnScreen + new Vector3(HEALTH_TEXT_CENTRE_OFFEST_X, HEALTH_TEXT_CENTRE_OFFEST_Y, 0);


        }

    }

}
