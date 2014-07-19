MightyMorphinPingasRangers_m4_readme.txt

TEAM:
Sebastian Monroy - sebash@gatech.edu - smonroy3
Thomas Cole Carver - tcarver3@gatech.edu - tcarver3
Chase Johnston - cjohnston8@gatech.edu - cjohnston8
Jory Folker - jfolker10@outlook.com - jfolker3

COMPLETED REQUIREMENTS:
	Environmental Animation - Programmatic Scripts:
		Animated Part #1 - Ocean Mesh:
			The Ocean Mesh object functions by using several Perlin Noise calculations to determine how to shift the vertices of the mesh up and down in realtime. The same calculations are used to generate a texture for the mesh in order to color the high points in the mesh a light blue color and the low points of the mesh a dark blue color. The mesh is circular and follows the player wherever they go, in order to prevent the whole map from requiring a procedurally generated mesh, which was extremely computationally expensive. The boat dips and bobs in the mesh using the same perlin noise calculations to find where the left, right, forward, and back extremes of the boat should rest, then using gross geoemetry to rotate the boat to fit those positions.
		Animated Part #2 - Ship:
			The ship consists of many moving parts. The Ship object has a cloth sail that moves depending upon motion of the ship. The ship also has 4 rotating oars where motion depends upon player input when a player is mounted on the oar zone. Similarly, the ship has 6 rotation cannons that can be aimed by any player mounted on a cannon zone. The ship also has a simple sinking animation, which rotates the ship backwards and pushes it downwards under the ocean mesh.
		Physical Collider - The Ship:
			The Ship object contains a capsule collider that interacts with outside elements. Other enemy ships (which use the same collider and movement scripts as the player ship) can ram the player ship. The player can also push forward through gates, ram the target, and bump onto land! The player can also press Backspace in order to see the sinking animation to be used when the ship takes too much damage!
		Inspector Control - Ocean Mesh:
			Several aspects of both models can be controlled in the inspector. In the inspector the ocean mesh can be severely editted. The wave maximum height, the vertex count, the pixel density, and even wave color at max and min heights can be changed.


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
	Press 2 to restart into Reflective Water mode!

HOW TO USE:
	Starting either of the scenes should immediately showcase how the Ocean Mesh functions. 
	The ship object has several moving parts:
		The Sail - The sail begins to move automatically, where motion causes more movement.
		The Oars - The oars are moved due to controller input when mounted on an oar position. Rotating the left and right sticks control the left and right oars, respectively.
		The Cannons - The cannons are moved due to controller input when mounted on a cannon position. The left stick controls the aim of the cannons. Holding down aims the cannon higher, while holding up aims the cannon lower. The cannons can also swivel left and right via left and right.


MAIN SCENE FILE:
	\_Scenes\playtestScene.unity

URL:
	<insert URL here>
