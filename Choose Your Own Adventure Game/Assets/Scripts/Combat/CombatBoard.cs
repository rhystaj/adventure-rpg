using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/**
 * The user interface for controlling and displaying a combat scenario.
 */ 
public class CombatBoard : MonoBehaviour, IController {

    [SerializeField] Vector2 distancesBetweenUnits;
    [SerializeField] float centreGapWidth;

    private CombatScenario currentScenario; //The scenario being displayed on the board.
    private CombatAnimator animator;

    private UnitVessel subject; //The unit taking the turn.
    private UnitVessel target; //The unit the turn's action is being performed on.

    private bool locked; //When true, the board can not be interacted with.

    public void DisplayScenario(ICombatScenario scenario)
    {

        //Preconditions
        Assert.IsNotNull(scenario, "Precondition Fail: The argument 'combatScenario' should not be null.");
        Assert.IsFalse(scenario.Board.Length <= 0, "Precondition Fail: The board of the given should contain at least one unit.");


        //Destroy all current children to make way for the new scenario.
        foreach (UnitVessel v in transform.GetComponentsInChildren<UnitVessel>())
            Destroy(v.gameObject);

        Assert.IsTrue(transform.childCount == 0, "Afterbeing cleared, the transform of the board should have no children.");


        //Use the vessel base to create UI representations of the unit in combat board.
        Unit.IInstance[,] board = scenario.Board;
        for(int row = 0; row < board.GetLength(0); row++)
        {
            for(int column = 0; column < board.GetLength(1); column++)
            {

                //Create a new gameObject with the vessel component and other required components, and comfigure the unit.
                GameObject vesselBase = new GameObject("Test Unit @ (" + column + ", " + row + ")", new Type[]
                {
                    typeof(SpriteRenderer), typeof(BoxCollider), typeof(UnitVessel)
                });
                UnitVessel vessel = vesselBase.GetComponent<UnitVessel>();
                vessel.parent = this;
                vessel.unit = board[row, column];


                //Position the unit based its position on the board and the distances between units.
                vessel.transform.SetParent(transform);

                float xGapOffset = centreGapWidth / 2 * (column < board.GetLength(1) / 2 ? -1 : 1);

                vessel.transform.position = new Vector3(
                    (column * distancesBetweenUnits.x) - ((board.GetLength(1) - 1) * distancesBetweenUnits.x / 2) + xGapOffset, 
                    (board.GetLength(0) - 1) * distancesBetweenUnits.y / 2 - row * distancesBetweenUnits.y,
                    -row
                );
           
            }
        }


        //Postconditions
        Assert.IsFalse(transform.childCount <= 0, "Postconditon Fail: Something should have been added as a child of the board's transform.");

    }

    private void SelectUnit(UnitVessel vessel)
    {

        //Preconditions
        Assert.IsNotNull(vessel, "Precondition Fail: The argument 'vessel' is not null.");

        if (!locked)
        {
            Debug.Log(vessel.name + "selected");
        }

    }

    public ICombatAction DetermineMove(Unit.IInstance[,] board, HashSet<Unit.IInstance> avaliableUnits)
    {
        throw new NotImplementedException();
    }

    /**
     * Represents a single interactable unit on the board.
     */ 
    private class UnitVessel : MonoBehaviour
    {
        public CombatBoard parent;

        private SpriteRenderer sprRenderer; //The renderer used to render the unit's visuals.
        private BoxCollider bCollider;

        private Unit.IInstance _unit;
        public Unit.IInstance unit
        {
            set {
                
                //Preconditions
                Assert.IsNotNull(parent, "The field 'parent' should not be null.");
                Assert.IsNotNull(value, "The given value should not be null.");


                //Set up a render to display the unit's sprite.
                sprRenderer = GetComponent<SpriteRenderer>();
                sprRenderer.sprite = value.GetImageForState(Unit.State.Idle);


                //Configure the collider that will detect mouse clicks.
                bCollider = GetComponent<BoxCollider>();
                bCollider.size = sprRenderer.size;

                _unit = value;


                //Postcondition
                Assert.IsNotNull(sprRenderer, "Postconditon Fail: The field 'sprRenderer' should not be null");
                Assert.IsNotNull(GetComponent<SpriteRenderer>(), "Postcondition Fail: The vessel's game object should have a sprite renderer attatched.");
                Assert.IsNotNull(sprRenderer.sprite, "Postcondition Fail: sprRenderer should be displaying a sprite.");
                Assert.IsNotNull(bCollider, "Postcondition Fail: The field bCollider should not be null.");
                Assert.IsNotNull(GetComponent<BoxCollider>(), "Postcondition Fail: The vessel's GameObject should have a BoxCollider attached.");
                Assert.IsFalse(bCollider.size.Equals(Vector3.zero), "Postcondition Fail: The collider's size should not be 0");
                Assert.IsTrue(bCollider.center.Equals(Vector3.zero), "Postcondition Fail: The collider should be centred on the vessel.");

            }

            get { return _unit; }

        }

        public void SetPose(Unit.State poseState)
        {
            sprRenderer.sprite = _unit.GetImageForState(poseState);
        }

        private void OnMouseDown()
        {
            parent.SelectUnit(this);
        }

    }

    private class CombatAnimator : ICombatAnimator
    {

        private Dictionary<Unit.IInstance, UnitVessel> vessels = new Dictionary<Unit.IInstance, UnitVessel>(); //The instances of units mapped to the vessels that house them.

        public CombatAnimator(UnitVessel[] vessels)
        {

            //Preconditions
            Assert.IsNotNull(vessels, "The argument 'vessels' should not be null.");


            foreach (UnitVessel vessel in vessels)
                this.vessels.Add(vessel.unit, vessel);


        }


        public IEnumerator PoseUnit(Unit.IInstance unit, Unit.State poseState)
        {

            //Preconditions
            Assert.IsNotNull(unit, "Precondition Fail: The argument 'unit' should not be null.");
            Assert.IsTrue(vessels.ContainsKey(unit),
                          "Precondition Fail: The given unit should be a key in state.");


            vessels[unit].SetPose(poseState);
            yield return null;

        }

    }

}
