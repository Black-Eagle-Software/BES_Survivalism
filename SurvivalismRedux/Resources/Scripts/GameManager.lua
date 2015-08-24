--THIS SCRIPT IS ONLY FOR TESTING PURPOSES AND WILL NOT BE
--PART OF THE FINAL GAME LOGIC FLOW (?)

--setup Lua environment for the game
local copyright = "Survivalism: v1.0, © Black Eagle Software, 2015";

function GameManager.Main()
	print(copyright);
	GameManager.Initialize();
end

function GameManager.Initialize()	
	local d = {}
	d[0] = BES_CreateDecision("Start a new game", GameManager.StartGame, 0);
	d[1] = BES_CreateDecision("Go to testing area...", GameManager.EnterTestArea, 0);
	d[2] = BES_CreateDecision("Exit game", GameManager.ExitGame, 0);
	BES_SendDecisionMessage(d);
	debug("Finished initializing GameManager");
end

function GameManager.StartGame()
	debug("Executing the 'StartGame' lua function");
	print("Starting a new game...");
	BES_StartNewGame();	
end

function GameManager.ExitGame()
	debug("Executing the 'ExitGame' lua function");
	print("Ending the game.");
	BES_ExitGame();
end

function GameManager.EnterTestArea()
	debug("Executing the 'EnterTestArea' lua function");
	debug("This area is for testing various functions of the game without having to run the game.");
	BES_EnterTestArea();
end