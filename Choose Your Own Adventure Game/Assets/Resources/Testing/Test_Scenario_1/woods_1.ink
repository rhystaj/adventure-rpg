EXTERNAL openStoryPath(destinationNodeName, scenarioScriptName)

VAR keyFound = 0

You find youself in the woods. After a while you discover a tattered and abandoned tent.
* Look inside the tent.
    The tent is small. Inside, there is a sleeping mat, a box, and a half eaten can of food.
    -> TentSearch
* Move on.
    -> LeaveWoods

= TentSearch
* Look under sleeping mat.
    Under the mat, you find a key. What could it be for?
    ~ keyFound = 1
    -> TentSearch
* Try to open box.
    The box is sealed shut.
    -> TentSearch
* Examine food.
    Aside from the maggots, nothing catches you eye.
    -> TentSearch
* MoveOn
    -> LeaveWoods

= LeaveWoods
You decide it is time to move on.
There is no point in going back to the rock. That hut on the cliff might be of interest.
{
    - keyFound:
        ~ openStoryPath("Node:Hut", "Testing/Test_Scenario_1/hut_2")
    - else:
         ~ openStoryPath("Node:Hut", "Testing/Test_Scenario_1/hut_1")
}
-> DONE

=== function openStoryPath(destinationNodeName, scenarioScriptName) ===
    > A path to {destinationNodeName} becomes avaliable with scenario: {scenarioScriptName}.