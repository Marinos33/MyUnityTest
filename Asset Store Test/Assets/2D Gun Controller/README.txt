********************************************
************* ASSET INFORMATION ************
********************************************
Version: 1.0
Version Notes:


Description:
************
    This asset provides a fully customizable 2D gun controller.
    Features:
        - Customizable recoil system.
        - Customizable accuracy system.
        - 19 customizable variables allow you to make each weapon completely unique.
        - Access to all script variables from external scripts through getters and setters.
        - Option to let the gun controller script handle fire input watching.

		
********************************************
************** GETTING STARTED *************
********************************************

Pre-Setup Requirements:
***********************

The following items are required before setup.

1. A "Player" GameObject with an Animator component.
    - A "Gun" GameObject.
        - A "Muzzle" GameObject (Usually an empty GameObject placed at the muzzle of the gun).
			
The following items are optional in getting the script initially setup.
However, to have full functionality of the script you will need the following items.
	
1. The animator component attached to the "Player" GameObject should have:
    - An animator controller with parameters for the following cases:
        - Determining if the player is on the ground.
        - Determining if the player is standing still.
        - Determining if the player is walking.
        - Determining if the player is running/sprinting.
        - Determining if the player is crouching.
			
2. A functioning bullet trail prefab that can be instantiated.
    - The provided bullet_trail prefab in the Example folder will work as a place holder for the setup if you do not have on ready.
	
3. A functioning muzzle flash prefab that can be instantiated.
    - The provided muzzle_flash prefab in the Example folder will work as a place holder for the setup if you do not have on ready.

Setup:
******

1. Attached gun_controller script to your "Gun" GameObject.
	
2. In the "gun"" GameObject's Inspector Window:
    2.1 Add the "Player" GameObject to the script variable named Player.
    2.2 Add the "Muzzle" GameObject to the script variable named Gun_muzzle.
    2.3 If available:
        2.3.1 Add a bullet/bullet trail prefab to the script variable name Bullet_prefab.
        2.3.2 Add a muzzle flash prefab to the script variable name Muzzle_flash_prefab.
			
3. Run the game for a couple of seconds to make sure there are no initialization errors.
    - If there are errors the text displayed with the error will tell you what is wrong.
    - Most likely, you are missing one of the requirements defined above.
		
4. Within the gun_controller script: (Search for "User Info" within the script, to find the location below.)
    4.1 In the function "FixedUpdate" starting at line 238:
        4.1.1 This is where you will need to link the previously mention animator controller parameters with variables in this script.
                - Within the "FixedUpdate" function, there is a explanation on how to do this which also includes two examples.
    4.2 (Optional) In the function "Update" starting at line 144:
        4.2.1 Change the 3 key specific Input calls to a generic class.
        4.2.2 Add code to trigger a reload animation before the reload is invoked.
    4.3 (Optional) In the function "fire_event" starting at line 294:
        4.3.1 Add code to trigger a fire animation.