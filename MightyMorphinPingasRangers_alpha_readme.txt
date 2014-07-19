MightyMorphinPingasRangers_m4_readme.txt

TEAM:
Sebastian Monroy - sebash@gatech.edu - smonroy3
Thomas Cole Carver - tcarver3@gatech.edu - tcarver3
Chase Johnston - cjohnston8@gatech.edu - cjohnston8
Jory Folker - jfolker10@outlook.com - jfolker3

COMPLETED REQUIREMENTS:
	Environmental Animation - Programmatic Scripts:
		Animated Part #1 - Ocean Mesh:
			The ocean mesh uses a Perlin Map in order to determine wave height. There are many vertexes on the wave mesh that determine their height via the pixel color of the perlin map.
		Animated Part #2 - Ship:
			The ship consists of many moving parts. The Ship object has a cloth sail that moves depending upon motion of the ship. The ship also has 4 rotating oars where motion depends upon player input when a player is mounted on the oar zone. Similarly, the ship has 6 rotation cannons that can be aimed by any player mounted on a cannon zone. The ship also has a simple sinking animation, which rotates the ship backwards and pushes it downwards under the ocean mesh.
		Physical Collider - The Ship:
			The Ship object contains a capsule collider that interacts with outside elements. Other enemy ships (which use the same collider and movement scripts as the player ship) can ram the player ship. The player can also push forward through gates, ram the target, and bump onto land! To much damage to the hull will eventually cause the player to sink!


OUTSIDE RESOURCES:
    1. Wood Texture - https://support.solidangle.com/download/attachments/1083508/wood-flooring-041_d.jpg?api=v2
	2. Door Texture - http://www.doormodels.com/wp-content/uploads/2013/10/Modern-Wood-Door-2014.jpg
	3. Ocean Sound - https://www.sounddogs.com/previews/25/mp3/226449_SOUNDDOGS__ba.mp3
	4. Creak Sound - https://www.sounddogs.com/previews/25/mp3/311033_SOUNDDOGS__do.mp3
	5. Target Sound - http://themushroomkingdom.net/sounds/wav/smw/smw_coin.wav
	6. Boat accessories - https://3dwarehouse.sketchup.com/model.html?id=6a65cd6bd6897cbb42df9eeba89a416c
	7. Boat hull - https://3dwarehouse.sketchup.com/model.html?id=24cdab178b28271296a1d1a7ead34b37
	8. Space Invaders Pirate Texture - http://s3.gamefreaks.co.nz/wp-content/uploads/2010/11/Space_Invaders_Pirate_Flag_by_deadroach.jpg
	9. InControl - https://github.com/pbhogan/InControl

SPECIAL INSTRUCTIONS:
	Controller is required! Most gamepad controllers are accepted!
	All controllers to be used in gameplay session must be plugged in and ready prior to loading the webpage. 
	If a controller disconnects during gameplay, the controller must be reconnected and the page refreshed in order to reset the controller settings ingame.

HOW TO PLAY:
	Goal:
		Get to the hidden target!
	Players/Controls:
		A maximum of four players work together to perform different jobs aboard BoatBoat. Each player is controlled with a separate controller. The player can move around the deck of BoatBoat with the left control stick. The player can mount any station by standing within a colored zone and pressing A(xbox)/X(psx). At any time a player may dismount from that station by pressing B(xbox)/O(psx).
	Jobs/Zones/Controls:
		Rowing (Purple Zone):
			Standing in a purple zone and mounting allows the player to begin rowing. By moving the left and right sticks the player changes the position of the oars. Pushing the oars through the water moves BoatBoat relative to the direction the player is rowing!
		Cannons (Red Zone):
			Standing in a red zone and mounting allows the player to man the cannons! The player can load the cannon by pressing X(xbox)/[](psx), and fire the cannon by pressing RightTrigger(xbox)/R2(psx)! The player can also slightly change the aim of the cannons. By holding down on the left stick the cannon aims higher, and by holding up on the left stick, the cannon aims lower. The cannon can also swivel left and right a short amount via left and right on the left stick.
		Lookout (Green Zone):
			Standing in a green zone and mounting allows the player to become the Lookout! This player can use the left stick to rotate the camera around, and help get a better understanding of the area around BoatBoat. This can be used to help the manned cannons, or look for enemies sneaking in from the rear!
	Game Feel:
		To capture the cooperation required and frustration of working with others aboard a sailboat to reach a destination. This is achieved by having more positions than players, and having many reasons to be in many places occuring at once. The sailing itself is also designed to be a mirror of actual paddleboat physics. The rocking with the waves and the force required to pilot BoatBoat are similar to how a paddleboat actually functions.

MAIN SCENE FILE:
	\_Scenes\playtestScene.unity

URL:
	<insert URL here>
