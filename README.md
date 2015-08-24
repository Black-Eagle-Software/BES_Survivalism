# BES_Survivalism
A WPF/Lua story framework in .Net 4.6

This framework is broken up into three sections:
* **WPF renderer** - renders all output text into a FlowDocument, with Decisions rendering as buttons in a StackPanel
* **C# logic** - controls (will control?) all game-specific logic, such as saving, loading, and decision history
* **Lua scripting** - the meat of the game play will be controlled via Lua scripts interacting with the C# logic

The main entry point of the app is the `ApplicationManager` class, which sets up the required directories and files, starts the `LuaScriptingEngine`, reads in any `Scenario` files that might be on disk, starts the `GameManager`, then starts the `MainWindowVM` and shows `MainWindow`.

Interaction between Lua and C# is handled via the `LuaScriptingEngine`.  Each set of `Scenario` files gets a table of its own within the Lua state, named for the `Scenario`.  Each `Scenario` has access to any of the methods described in the `LuaCustomFunctions` which allows Lua scripts to call C# methods via actions.  Similar functions are described within the `LuaOverrideFunction` class which enables special handling of the Lua `print()` function, for instance.

A typical custom function for use via Lua would look like the following:
```C#
[RegisterScriptFunction("String_Function_Name", "String_Description", "String_Parameters_Array")]
public void CustomVoidFunction() {
  //custom function logic goes here
}
```
Functions may return values, and have values passed in as any type that the Lua implementation understands.  A sample script has been included in the `Sample Scripts` folder.  The contents of this folder are intended to live in your `Documents\Black Eagle Software\Survivalism\Scripts\Scenarios` folder for use while running the game.  All scenario folders must be named uniquely, and contain a `.toc` file named the same as the scenario folder.  The contents of this file are still being finalized, but at a minimum it must contain the full names (filename + extension) for each of the script files within the scenario folder.  Refer to  https://github.com/gramseyBES/BES_Survivalism/blob/master/Sample%20Scripts/Police_Station/Police_Station.toc for the how to list these script files. 

###This is still very much a work-in-progress.
Many features are currently in a placeholder state, if they're even mentioned, such as the ability for a script to set limits on when it can be called (player needs X item, or not before day Y).  Eventually, the framework will allow for creating a game that handles saving and loading state, player-facing decision trees, recording of player story decisions as the game progresses, items and inventory management, npc party systems, and many more features.  
