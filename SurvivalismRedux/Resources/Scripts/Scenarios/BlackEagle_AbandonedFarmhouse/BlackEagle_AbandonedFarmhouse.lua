function BlackEagle_AbandonedFarmhouse.Main()
	print(BlackEagle_AbandonedFarmhouse.entryText);
	local d = {};
	d[0] = BES_CreateDecision("Check it out.", BlackEagle_AbandonedFarmhouse.CheckoutFarmhouse, 1);
	d[1] = BES_CreateDecision("Leave.", BlackEagle_AbandonedFarmhouse.LeaveFarmhouse, 3);
	BES_SendDecisionMessage(d);
end

function BlackEagle_AbandonedFarmhouse.CheckoutFarmhouse()
	print("You walk through the tall grass and approach the house.   As you do, you hear an unearthly howl come from inside.  Your heart jumps and you lose 1 sanity point.");
	BES_ChangePlayerStats("player", "sanity", -1);
	print("You stand just feet from the door.  The howling has stopped, and you realize the thumping sound you're hearing is just the beat of your heart.");
	local d = {};
	d[0] = BES_CreateDecision("Head inside.", BlackEagle_AbandonedFarmhouse.HeadInsideFarmhouse, 1);
	d[1] = BES_CreateDecision("Leave.", BlackEagle_AbandonedFarmhouse.LeaveFarmhouse, 2);
	BES_SendDecisionMessage(d);
end

function BlackEagle_AbandonedFarmhouse.LeaveFarmhouse()
	print("You move on.  Something's too creepy about this place.");
	BES_EndScenario();	
end

function BlackEagle_AbandonedFarmhouse.HeadInsideFarmhouse()
	print("You manage to open the door to the farmhouse, creaking it open as though it were fighting against you.  You peer inside to inky blackness.")
	BES_EndScenario();	
end