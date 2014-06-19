BoatBoat - Milestone 2 - Physics - README.txt

TEAM:
Sebastian Monroy - sebash@gatech.edu - smonroy3
Thomas Cole Carver - tcarver3@gatech.edu - tcarver3
Chase Johnston - cjohnston8@gatech.edu - cjohnston8
Jory Folker - jfolker10@outlook.com - jfolker3

COMPLETED REQUIREMENTS:
1.	Character is controlled via physics-simulated forces.
2.	Character collides and interactions appropriately with physics objects in the world.
3.	OVERALL GAME FEEL - BoatBoat behaves like a Boat on the Ocean:
		1. Boat moves with the waves.
		2. Boat cannot turn while stationary.
		3. Boat turns most efficiently at 3/5 maximum speed.
		4. Can't turn well at maximum speed.
		5. Ocean sounds play constantly.
4.	EMOTIONAL STATE - Player should feel impatient while sailing BoatBoat.
			Old-time sailboats were not very fast and turning was not efficient, this is replicated in-game. This drives the player to sail efficiently to get to the goal as quickly as possible.
5.	GARDENS:
		Garden #1: Cole Carver - Navigation
			Synopsis - The goal of this level is to acquiant the player with simple navigation. This gives the player a sense for the controls and a feeling of weight for BoatBoat. Interacting with the large Gate allows the player to feel what they are capable of. Navigating a short distance through the level reveals a tide-trap, where the player can be stopped by the falling of the tide. The player must wait for the tide to rise once more before he/she can cross the narrow gap. At the end of the navigation level, a simple target can be bumped in order to complete the level.

			Requirements:
				Five Dynamic Actors - Terrain, Ship, Gate-Posts, Gate-Doors, Target
				Twelve Joints - Six Hinges, each featuring one fixed joint and one hinge joint. Four hinges in Gate prefab, two hinges in Target prefab.
				Variable Height - The waves effect the height of the boat at all times. Bumping into the terrain can also cause a slight elevation change.
				Three Material Sounds:
					1. Creaking gate doors while the player pushes on them.
					2. Thumping noise when coming in contact with the terrain.
					3. A bullseye chirp when hitting the target.

		Garden #2: Jory Folker - River Current
			Synposis - The purpose of this level is to demonstrate the feel of river currents. In this level there is a long stretch of water current that will propel the player forward. As the player moves through the current the sound of gushing water is prevalent. The gushing water stops upon exiting the current. Upon completion of the river segment, the player can loop back around through the gate and re-enter the rapids. There are also a mini "Death Egg" object that floats in the water. Hitting a Death Egg causes the "PINGAS" sound to play.

			Requirements:
				Five Dynamic Actors - Terrain, Ship, Gate-Posts, Gate-Doors, Death-Egg
				Eight Joints - Four Hinges, each featuring one fixed joint and one hinge joint. All located in Gate prefab.
				Variable Height - The waves effect the height of the boat at all times.
				Three Material Sounds:
					1. Unique creaking gate doors while the player pushes on them.
					2. Thumping noise when coming in contact with the terrain.
					3. PINGAS noise when coming in contact with Death-Egg.

		Garden #3: Chase Johnston - Water Spouts
			Synopsis - When the boat enters any one of the three sections, a couple of water spouts will spawn in random locations in that section. They can be heard with 3D sound and when the boat hits them while they are going up in direction, the boat will be pushed up out of the water and back down with a funny sound. Once the boat leaves that section, the water spouts stop spawning. They only last for 2 pumps.
			
			Requirements:
				Five Dynamic Actors - Terrain, Ship, Gate-Posts, Gate-Doors, Target
				Twelve Joints - Six Hinges, each featuring one fixed joint and one hinge joint. Four hinges in Gate prefab, two hinges in Target prefab.
				Variable Height - The waves effect the height of the boat at all times. Bumping into the terrain can also cause a slight elevation change.
				Three Material Sounds:
					1. Creaking gate doors while the player pushes on them.
					2. Thumping noise when coming in contact with the terrain.
					3. A bullseye chirp when hitting the target.

		Garden #4: Sebastian Monroy - Whirlpool Slingshot
			Synopsis - The "Rainy Whirlpool" garden explores the game feel associated with navigating a boat in treacherous waters around a whirlpool to get to the target destination. The ocean waves are faster and taller, causing the ship to bob up and down more violently, and a particle emitter placed in front of the camera simulates heavy downpour. The waters are murkier looking than previous gardens. The skybox texture is eerier and storm sounds are played in the background, reducing the lighting of the entire garden, while the player does his best to avoid becoming trapped in the whirlpool's vortex and reach the goal on the other side. (The trick is to slingshot around the edge of the whirlpool and avoid touching any terrain.) All of these elements enhance the overall anxious game feel of the garden.

			Requirements:
				Five Dynamic Actors - Terrain, Ship, Gate-Posts, Gate-Doors, Target
				Twelve Joints - Six Hinges, each featuring one fixed joint and one hinge joint. Four hinges in Gate prefab, two hinges in Target prefab.
				Variable Height - The waves effect the height of the boat at all times. The whirlpool also dips inwards towards the vortex, lowering the player as they fall into the trap.
				Three Material Sounds:
					1. Creaking gate doors while the player pushes on them.
					2. Thumping noise when coming in contact with the terrain.
					3. A bullseye chirp when hitting the target.
					

OUTSIDE ASSETS:
	1. Wood Texture - https://support.solidangle.com/download/attachments/1083508/wood-flooring-041_d.jpg?api=v2
	2. Door Texture - http://www.doormodels.com/wp-content/uploads/2013/10/Modern-Wood-Door-2014.jpg
	3. Death-Egg Texture - http://www.themysticalforestzone.com/November04.htm
	4. Ocean Sound - https://www.sounddogs.com/previews/25/mp3/226449_SOUNDDOGS__ba.mp3
	5. Creak Sound - https://www.sounddogs.com/previews/25/mp3/311033_SOUNDDOGS__do.mp3
	6. Target Sound - http://themushroomkingdom.net/sounds/wav/smw/smw_coin.wav
	7. Whirlpool Sound - http://www.soundjay.com/nature/sounds/river-1.mp3
	8. Boat accessories - https://3dwarehouse.sketchup.com/model.html?id=6a65cd6bd6897cbb42df9eeba89a416c
	9. Boat hull - https://3dwarehouse.sketchup.com/model.html?id=24cdab178b28271296a1d1a7ead34b37
	10. Spout Push Sound - https://www.freesound.org/people/copyc4t/sounds/214898/
	11. Spout Ambient Sound - https://www.freesound.org/people/mhtaylor67/sounds/236980/
	12. Pingas Sound - https://www.youtube.com/watch?v=15CN3QZmssQ
NO SPECIAL INSTALL INSTRUCTIONS.

MAIN SCENE TO BE OPENED FIRST - coleScene.unity

URL TO POSTED ASSIGNMENT - https://dl.dropboxusercontent.com/u/115071152/CS%206457%20Milestone%202/Web%20Build.html