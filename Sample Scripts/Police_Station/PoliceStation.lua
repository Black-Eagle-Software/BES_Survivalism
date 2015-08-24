function Police_Station.Main()
	print(Police_Station.entryText);
	local d={}
	d[0]=BES_CreateDecision("Enter the "..Police_Station.title..".", Police_Station.EnterPoliceStation, 1);
	d[1]=BES_CreateDecision("Leave.", Police_Station.ExitPoliceStation, 3);
	BES_SendDecisionMessage(d);
end

function Police_Station.EnterPoliceStation()
	print("You enter the "..Police_Station.title..".  It's oddly quiet, yet you can see people moving about the entry way.");
	local d={}
	d[0]=BES_CreateDecision("Try and get someone's attention.", Police_Station.AttractAttention, 1);
	d[1]=BES_CreateDecision("Sneak past the entrance to try and find the evidence lock-up.", Police_Station.SneakInPoliceStation, 3);
	d[2]=BES_CreateDecision("Leave.", Police_Station.ExitPoliceStation, 2);
	BES_SendDecisionMessage(d);
end

function Police_Station.AttractAttention()
end

function Police_Station.SneakInPoliceStation()
	if(BES_GetStatValue("agility")<=5) then
		print("You slowly make your way around the edges of the room, until you come to a hallway.  Casually, you make your way down the hall, past offices and cubicles.  From behind you someone shouts out, 'HEY! HEY YOU, STOP RIGHT THERE!'");
		--WhereAmI();
	else
		print("You manage to walk straight to the evidence locker without anyone noticing you.  Their attention seems to be elsewhere, judging by everyone staring at various screens and monitors.  Something's going on, but you can figure out what after you rummage thru the lock-up.");  
	end
	BES_EndScenario();
end

function Police_Station.ExitPoliceStation()
	print("It's probably not a good idea to try and break into a police station.  Best be on our way...");
	BES_EndScenario();
end
