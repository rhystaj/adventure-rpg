EXTERNAL openStoryPath(destinationNodeName, scenarioScriptName)

You awaken on a rock.

But why this rock? Perhaps it is special.
* Attempt to discern why this rock is special.
    You look around at the rock you awoke on and its surrounding rocks. There doesn't seen to be anything special about this rock.
* Ignore it, you have more important matters to worry about.
- Not wanting to waste any more time you look around.
You see a forest across the plains, and a cliff with an old hut. Perhaps it is time to move on.
{openStoryPath("Node:Woods", "Testing/Test_Scenario_1/woods_1")}
{openStoryPath("Node:Hut", "Testing/Test_Scenario_1/Hut_1")}

->DONE

=== function openStoryPath(destinationNodeName, scenarioScriptName) ===
    > A path to {destinationNodeName} becomes avaliable with scenario: {scenarioScriptName}.