using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/**
 * The user interface for controlling and displaying a combat scenario.
 */ 
public class CombatBoard : MonoBehaviour {

    [SerializeField] CombatHUD overlay; //The GUI that displays further information about the state of the board
    [Space(10)]
    [SerializeField] Vector2 distancesBetweenUnits; //The distances alonlg the x and y axis the CENTRES of the units are from each other.
    [SerializeField] float centreGapWidth; //The size of the vertical gab between the two halves of the board.
    [Space(10)]
    [SerializeField] float invalidUnitOpacityChange; //The opacity of any unit not specified as valid for selection.

    public delegate void UnitVesselEvent(UnitVessel vessel);
    private UnitVesselEvent _UnitVesselSelected;
    public UnitVesselEvent UnitVesselSelected
    {
        get { return _UnitVesselSelected; }
        set { _UnitVesselSelected = value; }
    }

    private UnitVessel lastVesselHighlighted; //The most recent vessel to be moused-over.

    private bool _locked; //When true, the board can not be interacted with.
    public bool locked
    {
        get { return _locked; }

        set {

            if (value) //If the board is locked, emphasise all units, and hide all highlights.
            {

                overlay.HideHighlight();
                overlay.HideSelection();

                SpecifyValidUnits(v => true);

             };

            _locked = value;

        }

    }

    public UnitVessel[] vessels { get { return transform.GetComponentsInChildren<UnitVessel>(); } }

    public HashSet<Unit.IInstance> validUnits = new HashSet<Unit.IInstance>();

    public void Display(Unit.IInstance[,] board)
    {

        //Preconditions
        Assert.IsNotNull(board, "Precondition Fail: The argument 'combatScenario' should not be null.");
        Assert.IsFalse(board.Length <= 0, "Precondition Fail: The board of the given should contain at least one unit.");


        //Destroy all current children to make way for the new scenario.
        foreach (UnitVessel v in transform.GetComponentsInChildren<UnitVessel>())
            Destroy(v.gameObject);

        Assert.IsTrue(transform.childCount == 0, "Afterbeing cleared, the transform of the board should have no children.");


        //Use the vessel base to create UI representations of the unit in combat board.

        for(int row = 0; row < board.GetLength(0); row++)
        {
            for(int column = 0; column < board.GetLength(1); column++)
            {

                //Create a new gameObject with the vessel component and other required components, and comfigure the unit.
                GameObject vesselBase = new GameObject("Test Unit @ (" + column + ", " + row + ")", new Type[]
                {
                    typeof(SpriteRenderer), typeof(BoxCollider), typeof(UnitVessel), typeof(Animator)
                });
                UnitVessel vessel = vesselBase.GetComponent<UnitVessel>();
                vessel.parent.Value = this;
                vessel.unit = board[row, column];


                //Position the unit based its position on the board and the distances between units.
                vessel.transform.SetParent(transform);

                float xGapOffset = centreGapWidth / 2 * (column < board.GetLength(1) / 2 ? -1 : 1);

                vessel.transform.position = new Vector3(
                    (column * distancesBetweenUnits.x) - ((board.GetLength(1) - 1) * distancesBetweenUnits.x / 2) + xGapOffset, 
                    (board.GetLength(0) - 1) * distancesBetweenUnits.y / 2 - row * distancesBetweenUnits.y,
                    -row
                );

                overlay.UpdateStatsFor(vessel);
           
            }
        }


        //Postconditions
        Assert.IsFalse(transform.childCount <= 0, "Postconditon Fail: Something should have been added as a child of the board's transform.");

    }

    /**
     * Makes all units that fulfill the given condition valid to be selected for a move, and viduslly distingushes them.
     */
    public void SpecifyValidUnits(Predicate<Unit.IInstance> condition)
    {

        //Preconditions
        Assert.IsNotNull(condition, "Precondition Fail: The argument 'condition' should not be null.");
        Assert.IsTrue(new List<UnitVessel>(GetComponentsInChildren<UnitVessel>()).TrueForAll(v => v.GetComponent<SpriteRenderer>() != null),
                      "Precondition Fail: All units on the board should have a sprite renderer attatched.");


        //Clear the set of valid units to make way for a new one of units that satisfy the new condition.
        validUnits.Clear();


        //Iterate over each of the vessels on the board to determine which fulfill the given condition and therefore should be highlighted.
        foreach (UnitVessel v in GetComponentsInChildren<UnitVessel>())
        {
            SpriteRenderer renderer = v.GetComponent<SpriteRenderer>();
            if (condition.Invoke(v.unit))
            {
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1);
                validUnits.Add(v.unit);
            }
            else renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, invalidUnitOpacityChange);
        }
    }

    /**
     * Have the unit be considered selected.
     */
    private void SelectUnit(UnitVessel vessel)
    {

        //Preconditions
        Assert.IsNotNull(vessel, "Precondition Fail: The argument 'vessel' is not null.");


        if (!locked && validUnits.Contains(vessel.unit))
        {
            overlay.SelectUnit(vessel);
            _UnitVesselSelected(vessel);
        }

    }

    private void UpdateStatsFor(UnitVessel vessel)
    {
        overlay.UpdateStatsFor(vessel);
    }

    private void HightlightUnit(UnitVessel vessel)
    {

        //Preconditions
        Assert.IsNotNull(vessel, "The argument 'vessel' should not be null.");

        if (!locked)
        {
            lastVesselHighlighted = vessel;
            overlay.HighlightUnit(vessel);
        }


    }

    private void UnhighlightUnit(UnitVessel vessel)
    {

        //Preconditions
        Assert.IsNotNull(vessel, "Precondition Fail: the arguemnt 'vessel' should not be null.");


        if (ReferenceEquals(lastVesselHighlighted, vessel))
        {
            overlay.HideHighlight();
            lastVesselHighlighted = null;
        }

    }

    /**
     * Represents a single interactable unit on the board.
     */
    public class UnitVessel : MonoBehaviour
    {
        public Once<CombatBoard> parent = new Once<CombatBoard>(); //The board hosting this vessel.

        private Animator animator;
        private SpriteRenderer sprRenderer;
        private BoxCollider bCollider; //The collider used to detect mouse usages.

        private Unit.IInstance _unit;
        public Unit.IInstance unit
        {
            set
            {

                //Preconditions
                Assert.IsNotNull(parent.Value, "The field 'parent' should not be null.");
                Assert.IsNotNull(value, "The given value should not be null.");
                Assert.IsNotNull(GetComponent<Animator>(), 
                                 "Precondition Fail: This component's game object should have an Animator component attatched.");


                transform.localScale = value.scaleOnBoard;

                //Configure the animator.
                animator = GetComponent<Animator>();
                animator.runtimeAnimatorController = value.animatorController;


                //Configure the collider that will detect mouse clicks.
                sprRenderer = GetComponent<SpriteRenderer>();
                bCollider = GetComponent<BoxCollider>();


                _unit = value;


                //Postcondition
                Assert.IsNotNull(animator, "Postconditon Fail: The field 'animator' should not be null");
                Assert.IsNotNull(GetComponent<SpriteRenderer>(), "Postcondition Fail: The vessel's game object should have a sprite renderer attatched.");
                Assert.IsNotNull(bCollider, "Postcondition Fail: The field bCollider should not be null.");
                Assert.IsNotNull(GetComponent<BoxCollider>(), "Postcondition Fail: The vessel's GameObject should have a BoxCollider attached.");
                Assert.IsFalse(bCollider.size.Equals(Vector3.zero), "Postcondition Fail: The collider's size should not be 0");
                Assert.IsTrue(bCollider.center.Equals(Vector3.zero), "Postcondition Fail: The collider should be centred on the vessel.");

            }

            get { return _unit; }

        }

        private void OnAnimatorMove()
        {
            if(sprRenderer.sprite != null)
                bCollider.size = sprRenderer.sprite.bounds.size;
        }

        public void UpdateStats()
        {
            parent.Value.UpdateStatsFor(this);
        }

        public void SetPose(Unit.Pose pose)
        {
            animator.SetInteger("Pose", (int)pose);
        }

        private void OnMouseDown()
        {
            parent.Value.SelectUnit(this);
        }

        private void OnMouseEnter()
        {
            parent.Value.HightlightUnit(this);
        }

        private void OnMouseExit()
        {
            parent.Value.UnhighlightUnit(this);
        }

    }

}
