MightyMorphinPingasRangers_m4_readme.txt

TEAM:
Mighty Morphin Pingas Rangers
Sebastian Monroy - sebash@gatech.edu - smonroy3
Thomas Cole Carver - tcarver3@gatech.edu - tcarver3
Chase Johnston - cjohnston8@gatech.edu - cjohnston8
Jory Folker - jfolker10@outlook.com - jfolker3

COMPLETED REQUIREMENTS:
	Environmental Animation - Programmatic Scripts:
		Animated Part #1 - Ocean Mesh:
			The Ocean Mesh object functions by using several Perlin Noise calculations to determine how to shift the vertices of the mesh up and down in realtime. The same calculations are used to generate a texture for the mesh in order to color the high points in the mesh a light blue color and the low points of the mesh a dark blue color. The mesh is circular and follows the player wherever they go, in order to prevent the whole map from requiring a procedurally generated mesh, which was extremely computationally expensive. The boat dips and bobs in the mesh using the same perlin noise calculations to find where the left, right, forward, and back extremes of the boat should rest, then using geoemetry to rotate the boat to fit those positions.
			If you're in the "Reflective" scene, the "RealtimeCubemap.cs" script, attached to the "Ocean Mesh" object, creates a camera called "Cubemap Camera" that mirrors the Main Camera underneath the surface of the water. The "Cubemap Camera" aims at the same point as the Main Camera and the script uses it to update a RenderTexture that acts as a cubemap. This cubemap is then plugged into the _Cube parameter of the "Mesh Water.shader", also attached to the "Ocean Mesh" object. Then the shader uses the normals of each of the triangles in the mesh to determine what the reflection should look like for each triangle. The "Ocean Mesh" object's _MainTex parameter updated every frame with the aforementioned light/dark blue colored texture. Then it is a simple matter of lerping between the _MainTex color and the calculated reflective color. The RenderTexture has to be limited to 512x512, unfortunately, to prevent your GPU from melting. As a result the reflections look pretty low resolution, but I think they definitely add something. Let us know if you think we should keep them!
			The "Ocean Mesh" is circular and follows the player around wherever they go. This was to prevent the whole map from requiring a procedurally generated mesh which was extremely computationally expensive. The boat also dips and bobs in the mesh, despite the mesh not having a collider (which would have also been very expensive) using the same Perlin noise calculations to find where the left, right, forward, and back extremes of the boat should rest, then using some gross geometry to rotate the boat to fit those positions.
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
	10. SSAO Post Processing - http://www.mediafire.com/download/42mq19g50t80c8b/SSAOeffect.zip

SPECIAL INSTRUCTIONS:
	Controller is required! Most gamepad controllers are accepted!
	All controllers to be used in gameplay session must be plugged in and ready prior to loading the webpage. 
	If a controller disconnects during gameplay, the controller must be reconnected and the page refreshed in order to reset the controller settings ingame.
	Press 2 to restart into Reflective Water mode! Press 1 to restart in Diffuse Water mode!
	It's possible you might need to reinstall the InControl plugin from the above GitHub link, but we've only had issues with that when syncing on different coomputers with version control.

HOW TO USE:
	Starting either of the scenes should immediately showcase how the Ocean Mesh functions.
	The ship object has several moving parts:
		The Sail - The sail begins to move automatically, where motion causes more movement.
		The Oars - The oars are moved due to controller input when mounted on an oar position. Rotating the left and right sticks control the left and right oars, respectively. The oars splash in the water!
		The Cannons - The cannons are moved due to controller input when mounted on a cannon position. The left stick controls the aim of the cannons. Holding down aims the cannon higher, while holding up aims the cannon lower. 	The cannons can also swivel left and right via left and right.


MAIN SCENE FILE:
	\_Scenes\playtestScene Diffuse.unity
	or
	\_Scenes\playtestScene Reflective.unity
	Note: The "Reflective" scene requires Unity Pro in order to render the reflective water mesh.

HOW TO PLAY:
	Goal:
		Get to the hidden target!
	Players/Controls:
		A maximum of four players work together to perform different jobs aboard BoatBoat. Each player is controlled with a separate controller. The player can move around the deck of BoatBoat with the left control stick. The player can mount any station by standing within a colored zone and pressing A(xbox)/X(psx). At any time a player may dismount from that station by pressing B(xbox)/O(psx). To demonstrate the new sinking animation, you can press the Backspace key!
	Jobs/Zones/Controls:
		Rowing (Purple Zone):
			Standing in a purple zone and mounting allows the player to begin rowing. By moving the left and right sticks the player changes the position of the oars. Pushing the oars through the water moves BoatBoat relative to the direction the player is rowing!
		Cannons (Red Zone):
			Standing in a red zone and mounting allows the player to man the cannons! The player can load the cannon by pressing X(xbox)/[](psx), and fire the cannon by pressing RightTrigger(xbox)/R2(psx)! The player can also slightly change the aim of the cannons. By holding down on the left stick the cannon aims higher, and by holding up on the left stick, the cannon aims lower. The cannon can also swivel left and right a short amount via left and right on the left stick.
		Lookout (Green Zone):
			Standing in a green zone and mounting allows the player to become the Lookout! This player can use the left stick to rotate the camera around, and help get a better understanding of the area around BoatBoat. This can be used to help the manned cannons, or look for enemies sneaking in from the rear!
	Game Feel:
		To capture the cooperation required and frustration of working with others aboard a sailboat to reach a destination. This is achieved by having more positions than players, and having many reasons to be in many places occuring at once. The sailing itself is also designed to be a mirror of actual paddleboat physics. The rocking with the waves and the force required to pilot BoatBoat are similar to how a paddleboat actually functions.

URL:
	https://dl.dropboxusercontent.com/u/115071152/CS%206457%20Milestone%204/Web%20Build.html
