using UnityEngine;
using System.Collections;

public class gun_controller : MonoBehaviour {

    //Damage Properties
    //=================================================
    [SerializeField] private float damage = 100f; //Damage done.

    //Accuracy Properties
    //=================================================
    [SerializeField] private bool accuracy_enabled                                  = false; //Enable or Disable gun accuracy.
    [Range(0.000001f, 1)] [SerializeField] private float accuracy_standing          = 0.8f;  //First shot accuracy, when standing.
    [Range(0.000001f, 1)] [SerializeField] private float accuracy_crouching         = 0.9f;  //First shot accuracy, when crouching.
    [Range(0.000001f, 1)] [SerializeField] private float accuracy_airborne          = 0.1f;  //First shot accuracy, when jumping or airborne.
    [Range(0.000001f, 1)] [SerializeField] private float accuracy_walking           = 0.6f;  //First shot accuracy, when walking.
    [Range(0.000001f, 1)] [SerializeField] private float accuracy_running           = 0.3f;  //First shot accuracy, when running.
    [Range(0.000001f, 1)] [SerializeField] private float accuracy_crouch_walking    = 0.7f;  //First shot accuracy, when crouch walking.

    //Magazine Properties
    //=================================================
    [SerializeField] private int magazine_max_capacity = 10;    //Max amount of ammo a single magazine can hold.
    private int magazine_current_amount;                        //Amount of ammo remaining in the magazine.

    //Ammo Properties
    //=================================================
    [SerializeField] private int ammo_max = 100;    //Max amount of ammo the player can hold for this weapon.
    private int ammo_current;                       //Current amount of ammo the player has for this weapon.

    //Recoil properties
    //=================================================
    [SerializeField] private bool recoil_enabled            = false;    //Enable or Disable gun recoil.
    [SerializeField] private float recoil_base              = 0.1f;     //How much each shot will effect accuracy.
    [SerializeField] private float recoil_increase_per_shot = 0.25f;    //Multiplied by remaining recoil cooldown to increase recoil of multiple back to back shots.
    [SerializeField] private float recoil_max               = 2f;       //Max amount of recoil that can be achieved when shooting continuously.
    [SerializeField] private float recoil_recovery_time     = 0.5f;     //Time until accuracy returns to first shot value.
    private float recoil_cooldown;                                      //Used in conjunction with recoil_recovery_time to determine when recoil is over.
    private float recoil_counter;                                       //Used in conjunction with recoil_increase_per_shot to increase recoil accordingly.

    //Misc Gun Properties
    //=================================================
    [SerializeField] private float fire_rate                                = 0.5f;     //How long between each shot: 1f = 1 second, 0.001f = 1 millisecond. If set to zero, gun will not shoot.
    [SerializeField] private float reload_time                              = 2f;       //How long it takes to reload: 1f = 1 second, 0.001f = 1 millisecond.
    [SerializeField] private float max_distance                             = 500f;     //How far can the bullet travel before it does zero damage.
    [SerializeField] private int shots_per_click                            = 1;        //How many bullets are fired per click. *Only applies for semi automatic weapons.
    [SerializeField] private float burst_fire_rate                          = 0.1f;     //Time between each shot of a burst fire.
    [SerializeField] public bool automatic                                  = false;    //Determines if the gun fires automatically or semi-automatically.
    [Range(0, 1)] [SerializeField] private float movement_speed_reduction   = 0.9f;     //Fraction of max movement speed the gun reduces the player to.
    private float cool_down                                                 = 0f;       //How long is left before player can shoot again.

    //Game Object Attachments
    //=================================================
    public GameObject player;                           //Player GameObject reference.
    private Animator player_animator;                   //Player Animator reference.
    public GameObject gun_muzzle;                       //Gun muzzle GameObject reference.
    private Transform gun_muzzle_transform;             //Gun muzzle Transform reference.
    public GameObject bullet_prefab;                    //Bullet prefab GameObject reference.
    private Transform bullet_prefab_transform;          //Bullet prefab Transform reference.
    public GameObject muzzle_flash_prefab;              //Muzzle flash prefab GameObject reference.
    private Transform muzzle_flash_prefab_transform;    //Muzzle flash prefab Transform reference.
    public LayerMask objects_to_hit;                    //LayerMask of what objects the RayCast should collide with.

    //Player Position Attachments
    //=================================================
    private bool is_standing    = true;     //Is the player standing?
    private bool is_crouching   = false;    //Is the player crouching?
    private bool is_walking     = false;    //Is the player walking?
    private bool is_airborne    = false;    //Is the player airborne?
    private bool is_running     = false;    //Is the player running?

    //Script Control Variables
    //=================================================
    [SerializeField] public bool debug  = true;
    private bool watch_for_fire_event   = true;
    private bool watch_for_reload_event = true;

    //Script Constants
    //=================================================
    private float c_raycast_distance = Mathf.Infinity;

    private void Awake()
    {
        //Verifying that the script has the required references.
        if (get_player())
        {
            set_player_animator(get_player().GetComponent<Animator>());
            if (get_player_animator() == null)
            {
                Debug.LogError("'player_animator' reference is Null. Make sure that the GameObject connected to the 'Player' variable has an animator component.");
            }
        }
        else
        {
            Debug.LogError("'player' GameObject is Null. Connect an appropriate GameObject to the 'Player' variable via the Inspector window.");
        }

        if (get_gun_muzzle())
        {
            set_gun_muzzle_transform(get_gun_muzzle().GetComponent<Transform>());
            if (get_gun_muzzle_transform() == null)
            {
                Debug.LogError("'gun_muzzle_transform' reference is Null. Make sure that the GameObject connected to the 'gun_muzzle' variable has a transform component.");
            }
        }
        else
        {
            Debug.LogError("'gun_muzzle' GameObject is Null. Connect an appropriate GameObject to the 'gun_muzzle' variable via the Inspector window.");
        }

        if (get_bullet_prefab())
        {
            set_bullet_prefab_transform(get_bullet_prefab().GetComponent<Transform>());
            if (get_bullet_prefab_transform() == null)
            {
                Debug.LogError("'bullet_prefab_transform' reference is Null. Make sure that the GameObject connected to the 'bullet_prefab' variable has a transform component.");
            }
        }
        else
        {
            Debug.LogError("'bullet_prefab' GameObject is Null. Connect an appropriate GameObject to the 'bullet_prefab' variable via the Inspector window.");
        }

        if (get_muzzle_flash_prefab())
        {
            set_muzzle_flash_prefab_transform(get_muzzle_flash_prefab().GetComponent<Transform>());
            if (get_muzzle_flash_prefab_transform() == null)
            {
                Debug.LogError("'muzzle_flash_prefab_transform' reference is Null.\n Make sure that the GameObject connected to the 'muzzle_flash_prefab' variable has a transform component.");
            }
        }
        else
        {
            Debug.LogError("'muzzle_flash_prefab' GameObject is Null. Connect an appropriate GameObject to the 'muzzle_flash_prefab' variable via the Inspector window.");
        }

        //Initializing some values.
        magazine_current_amount = magazine_max_capacity;
        ammo_current = ammo_max;
        cool_down = 0;
    }
	
	void Update()
    {
        if (watch_for_fire_event)
        {
            if (get_automatic())
            {
                /*  ==========
                    User Info
                    
                    It is recommended to change the input to a generic type
                    depending on your needs. Currently it is set to left click.
                    ==========
                */
                if (Input.GetMouseButton(0) && Time.time > get_cool_down())
                {
                    fire();
                }
            }
            else
            {
                /*  ==========
                    User Info
                    
                    It is recommended to change the input time to a generic type
                    depending on your needs. Currently it is set to left click.
                    ==========
                */
                if (Input.GetMouseButtonDown(0) && Time.time > get_cool_down())
                {
                    fire();
                }
            }
        }
        /*  ==========
            User Info
                    
            It is recommended to change the input time to a generic type
            depending on your needs.
            ==========
        */
        if (watch_for_reload_event)
        {
            if (Input.GetKeyDown(KeyCode.R) || Input.GetKey(KeyCode.R))
            {
                /*  ==========
                    User Info
                    
                    This would be a possible location to trigger a reload animation.
                    ==========
                */
                Invoke("reload", get_reload_time());
            }
        }
    }

    void FixedUpdate()
    {
        if (get_accuracy_enabled())
        {
            /*  ==========
                User Info

                This is where the animation Booleans that are used for determining accuracy as set.

                There are many ways that you can update these variables, the exact way you do it
                does not matter, just as long as the variables are being updated correctly.

                If you wish to not use accuracy, simply set the accuracy_enabled variable to false
                and you will not have to implement this section.

                Here are two examples for updating variables.

                Ex.1 Using animator Boolean parameters only
                    set_is_airborne(!get_player_animator().GetBool("<insert Boolean parameter here>"));
                    set_is_standing(get_player_animator().GetBool("<insert Boolean parameter here>"));
                    set_is_crouching(get_player_animator().GetBool("<insert Boolean parameter here>"));
                    set_is_walking(get_player_animator().GetBool("<insert Boolean parameter here>"));
                    set_is_running(get_player_animator().GetBool("<insert Boolean parameter here>"));

                Ex.2 Using animator Boolean parameters and rigid body velocities
                    if (Mathf.Abs(get_player().GetComponent<Rigidbody2D>().velocity.x) > 0.01f)
                    {
                        set_is_standing(false);
                    }
                    else
                    {
                        set_is_standing(true);
                    }
                    if (Mathf.Abs(get_player().GetComponent<Rigidbody2D>().velocity.y) > 0.01f)
                    {
                        set_is_airborne(true);
                    }
                    else
                    {
                        set_is_airborne(false);
                    }
                    set_is_crouching(get_player_animator().GetBool("<insert Boolean parameter here>"));
                    set_is_walking(get_player_animator().GetBool("<insert Boolean parameter here>"));
                    set_is_running(get_player_animator().GetBool("<insert Boolean parameter here>"));
                ==========
            */
        }
    }

    /*
        fire

        Params: void
        Return: void

        Function info:
            Handles calling the fire_event function the correct number of times.

            If shots_per_click is greater than one, aka burst fire, then this function
            will handle delaying each shot in a burst by the amount of burst_fire_rate.
    */
    void fire()
    {
        //Checking if the magazine is empty.
        if (get_magazine_current_amount() > 0)
        {
            //Setting cool down to delay next shot.
            set_cool_down((Time.time + 1 / get_fire_rate()));

            //Fire the gun.
            fire_event();

            if (!get_automatic())
            {
                //If the gun is in burst fire, fire as much of the burst as possible.
                for (int i = 1; i < get_shots_per_click(); i++)
                {
                    //If the gun runs out of ammo mid burst, stop shooting.
                    if (get_magazine_current_amount() <= 0)
                    {
                        break;
                    }
                    Invoke("fire_event", burst_fire_rate);
                }
            }
        }
    }

    /*
        fire_event

        Params: void
        Return: void

        Function info:
            Handles the fire event.

            This function gathers required data for each shot,
            drawing the Raycast, and checking for a hit, as well as
            calling all the necessary calculation functions to determine
            the direction the Raycast will be drawn.
    */
    void fire_event()
    {
        if (get_gun_muzzle_transform() != null)
        {
            //Get gun and mouse positions.
            Vector2 mouse_position = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            Vector2 gun_muzzle_position = new Vector2(get_gun_muzzle_transform().position.x, get_gun_muzzle_transform().position.y);

            //Create direction vector.
            Vector2 direction_vector = calculate_directional_vector(mouse_position, gun_muzzle_position);

            /*  ==========
                User Info

                This is a possible location for triggering a recoil animation for the gun.
                ==========
            */

            //Draw a raycast from the gun along the direction vector.
            RaycastHit2D hit = Physics2D.Raycast(gun_muzzle_position, direction_vector, c_raycast_distance, objects_to_hit);

            //Trigger the creation of a bullet with the correct angle.
            effect(direction_vector);

            if (debug)
            {
                //This also requires Gizmos to be enabled.
                Debug.DrawLine(gun_muzzle_position, (direction_vector * 100), Color.red, 5f);
            }

            if (hit.collider != null)
            {
                if (debug)
                {
                    Debug.Log("Hit: " + hit.collider.name + " and did the following damage: " + calculate_damage(hit.distance));
                }

                /*  ==========
                    User Info

                    This is where you will need to add a function call to handle damage to enemies or objects.

                    There are many ways to implement this part, especially if you are going to allow damage to more than just enemies

                    Possible Example:
                        Enemy enemy = hit.collider.GetComponent<Enemy>();
                        if (enemy)
                        {
                            enemy.damage_enemy(calculate_damage(hit.distance));
                        }

                        Explanation:
                            In this example, we are getting the Enemy class from the script attached to the component the RayCast collided with.
                            This requires that the enemy GameObject would have a script attached to it called Enemy.

                            We then are making sure we received a reference.

                            Then we are calling the function 'damage_enemy' which would be a function within the hypothetical Enemy script attached to
                            the GameObject that the RayCast collided with. When calling the function 'damage_enemy' we are passing in a float variable
                            that is the damage calculated from the weapons base damage and the distance of the hit.collider.
                    ==========
                */
            }

            //Updating ammo in magazine.
            update_magazine_current_amount(-1);
        }
        else
        {
            Debug.LogError("Missing Object Reference for gun_muzzle. Add a GameObject reference to the 'Gun_muzzle' variable through the Inpector Window.");
        }

    }

    /*
        determine_accuracy

        Params: void
        Return: float

        Return info:
            A float value between 0 and 1, which represents the percent of accuracy.

        Function info:
            Determines the accuracy to use for later calculation.
            Accuracy is determined from the Boolean values that are being updated within FixedUpdate().
    */
    float determine_accuracy()
    {
        float accuracy = 0f;

        if (get_is_airborne())
        {
            accuracy = get_accuracy_airborne();
        }
        else if (get_is_crouching())
        {
            accuracy = get_accuracy_crouching();
        }
        else if (get_is_standing())
        {
            accuracy = get_accuracy_standing();
        }
        else if (get_is_crouching() && get_is_walking())
        {
            accuracy = get_accuracy_crouch_walking();
        }
        else if (get_is_running() && get_is_walking())
        {
            accuracy = get_accuracy_running();
        }
        else if (get_is_walking())
        {
            accuracy = get_accuracy_walking();
        }

        //accuracy is used in division so if it is zero, we set it close to zero instead.
        //we are also checking to make sure it isn't negative, just in case.
        if (accuracy <= 0f)
        {
            accuracy = 0.000001f;
        }
        return accuracy;
    }

    /*
        calculate_directional_vector

        Params: Vector2, Vector2
        Return: Vector2

        Parameter info:
            Two Vector2 structures, one the position of the mouse position and
            the other the position of the gun muzzle (the end of the barrel).

        Return info:
            A directional Vector2 that takes into account accuracy and recoil.

        Function info:
            Takes the position of the mouse and gun muzzle as Vector2 structures.
            Creates a directional vector.
            Applies accuracy to the vector in the vertical plane.
            Applies recoil to the vector in the vertical plane.
    */
    Vector2 calculate_directional_vector(Vector2 v_mouse_position, Vector2 v_gun_muzzle_position)
    {
        float accuracy = determine_accuracy();

        //Creating direction vector.
        Vector2 v_dir = v_mouse_position - v_gun_muzzle_position;

        //Applying accuracy to vector.
        if (get_accuracy_enabled())
        {
            v_dir.y = (Random.Range((v_dir.y - ((1 / accuracy) / 10f)), (v_dir.y + ((1 / accuracy) / 10f))));
        }

        //Applying recoil to vector.
        if (get_recoil_enabled())
        {
            v_dir.y = v_dir.y + calculate_recoil();
        }

        return v_dir;
    }

    /*
        calculate_recoil

        Params: void
        Return: float

        Return info:
            A float value which represents the amount of recoil.

        Function info:
            Calculates the recoil of the current shot based off of the recoil_cooldown
            and recoil_counter variables.
    */
    float calculate_recoil()
    {
        float recoil = 0f;

        //If the recoil_recovery_time is not over from previous shot.
        if ((Time.time - get_recoil_cooldown()) < get_recoil_recovery_time())
        {
            recoil = get_recoil_base();
            recoil += get_recoil_counter() * get_recoil_increase_per_shot();

            set_recoil_counter(get_recoil_counter() + 1);
            
            if (recoil > get_recoil_max())
            {
                recoil = get_recoil_max();
            }
        }
        else
        {
            set_recoil_counter(0f);
        }

        //Updating recoil_cooldown.
        set_recoil_cooldown(Time.time);

        return recoil;
    }

    /*
        calculate_damage

        Params: float
        Return: float

        Parameter info:
            A float that represents the distance the bullet traveled to strike the target.

        Return info:
            A float that represents the damage, based on the distance of the shot and the guns max distance.

        Function info:
            Calculates the damage based on the distance of the shot and the guns max distance.
            At zero distance the damage will be 100% of the guns base damage.
            At max_distance the damage will be 0% of the guns base damage.
            Uses a radical function to determine percentage of damage.
    */
    float calculate_damage(float distance)
    {
        float dmg = get_damage();

        //Only calculate damage is the distance is less than max.
        if (distance <= get_max_distance())
        {
            dmg = dmg * ((Mathf.Sqrt((-distance) + get_max_distance())) / (17.32f));
        }
        else
        {
            dmg = 0f;
        }

        if (dmg > get_damage())
        {
            dmg = get_damage();
        }

        return dmg;
    }

    /*
        reload

        Params: void
        Return: void

        Function info:
            Handles all of the reloading logic, and setting and updating all ammo related variables.
    */
    void reload()
    {
        //If the magazine is not empty, and there is ammo to reload.
        if ((get_magazine_current_amount() < get_magazine_max_capacity()) && (get_ammo_current() > 0))
        {
            //Checking if there is any ammo available before reloading.
            if (get_ammo_current() >= 0)
            {
                int ammo = get_ammo_current();

                //Accounting for any ammo left in the magazine.
                ammo += get_magazine_current_amount();
                set_magazine_current_amount(magazine_max_capacity);
                ammo -= get_magazine_max_capacity();

                //If there was not enough ammo for a full mag, only fill the mag with how much was available.
                if (ammo < 0)
                {
                    update_magazine_current_amount(ammo);
                    ammo = 0;
                }
                set_ammo_current(ammo);
            }

            //This check is just an extra percaution.
            if (get_ammo_current() < 0)
            {
                set_ammo_current(0);
            }
        }
    }

    /*
        effect

        Params: Vector2
        Return: Void

        Parameter info:
            A direction vector based on the position of the gun muzzle, the mouse, accuracy, and recoil.

        Function info:
            Handle all of the effects for firing a gun.

            Instantiates a bullet prefab at the position of the gun muzzle, with 
            a rotation value calculated from the given directional vector.

            Instantiates a muzzle flash at the gun muzzle for dramatic effect.
    */
    void effect(Vector2 direction_vector)
    {
        //Creating a Quaternion using the direction vector.
        Quaternion rotation = get_effect_rotation(direction_vector);

        //Attempting to instantiate the bullet_prefab with the correct rotation at the position of the gun_muzzle.
        if (get_bullet_prefab_transform() && get_gun_muzzle_transform())
        {
            Instantiate(get_bullet_prefab_transform(), get_gun_muzzle_transform().position, rotation);
        }
        else if (debug)
        {
            if (get_bullet_prefab_transform())
            {
                Debug.LogWarning("Unable to Instantiate bullet_prefab, because gun_muzzle_transform is Null. Make sure that the gun_muzzle GameObject is attached to this script in the Inspector Window.");
            }
            else
            {
                Debug.LogWarning("Unable to Instantiate bullet_prefab, because bullet_prefab_transform is Null. Make sure that the bullet_prefab GameObject is attached to this script in the Inspector Window.");
            }
        }

        //Attempting to instantiate the muzzle_flash_prefab at the position of the gun_muzzle.
        if (get_muzzle_flash_prefab() && get_gun_muzzle_transform())
        {
            Instantiate(get_muzzle_flash_prefab(), get_gun_muzzle_transform().position, get_gun_muzzle_transform().rotation);
        }
        else if (debug)
        {
            if (get_muzzle_flash_prefab())
            {
                Debug.LogWarning("Unable to Instantiate muzzle_flash_prefab, because gun_muzzle_transform is Null. Make sure that the gun_muzzle GameObject is attached to this script in the Inspector Window.");
            }
            else
            {
                Debug.LogWarning("Unable to Instantiate muzzle_flash_prefab, because muzzle_flash_prefab_transform is Null. Make sure that the muzzle_flash_prefab GameObject is attached to this script in the Inspector Window.");
            }
        }
    }

    /*
		get_effect_rotation

		Params: Vector2
		Return: Quaternion

		Parameter info:
			A Vector2 representing a directional vector.

		Return info:
			A Quaternion based off of the given directional vector.

		Function info:
			Creates a Quaternion from a given Vector2.
	*/
    Quaternion get_effect_rotation(Vector2 direction_vector)
    {
        //Turning the direction vector into a Quaternion.
        float rotation = Mathf.Atan2(direction_vector.y, direction_vector.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0f, 0f, rotation);
    }


    /*
    =================================================
    The following functions are all of the getters, setters, and updaters
    for almost all of the variables defined within this script, as well
    as some helper functions that are only used by the following code.

    All of these functions can be reached from outside of this script by 
    instantiating this class or by other means.
    =================================================
    */
    //==============
    // Helper Functions
    //==============
        //Determines if a given float or integer is negative. If it is, then it returns 0.
        float verify_not_negative(float n)
        {
            if (n < 0f)
            {
                return 0f;
            }
            return n;
        }
        int verify_not_negative(int n)
        {
            if (n < 0)
            {
                return 0;
            }
            return n;
        }

        //Determines if a given float is between 0-1. If not it will set to closest edge.
        float verify_in_range(float n)
        {
            if (n < 0f)
            {
                return 0f;
            }
            else if (n > 1f)
            {
                return 1f;
            }
            return n;
        }


    //==============
    // Damage Getter and Setter.
    //==============
        //Sets the damage variable to a given value.
        public void set_damage(float new_damage)
        {
            damage = verify_not_negative(new_damage);
        }

        //Returns the damage variable of this weapon.
        public float get_damage()
        {
            return damage;
        }


    //==============
    // Accuracy Getters and Setters.
    //==============
        //Sets the accuracy_enabled variable to True or False.
        public void set_accuracy_enabled(bool enabled)
        {
            accuracy_enabled = enabled;
        }

        //Returns the accuracy_enabled variable.
        public bool get_accuracy_enabled()
        {
            return accuracy_enabled;
        }

        //Sets the accuracy_standing variable to a given value.
        public void set_accuracy_standing(float accuracy)
        {
            accuracy_standing = verify_in_range(accuracy);
        }

        //Returns the first shot accuracy_standing variable of this weapon.
        public float get_accuracy_standing()
        {
            return accuracy_standing;
        }

        //Sets the accuracy_crouching variable to a given value.
        public void set_accuracy_crouching(float accuracy)
        {
            accuracy_crouching = verify_in_range(accuracy);
        }

        //Returns the first shot accuracy_crouching variable of this weapon.
        public float get_accuracy_crouching()
        {
            return accuracy_crouching;
        }

        //Sets the accuracy_airborne variable to a given value.
        public void set_accuracy_airborne(float accuracy)
        {
            accuracy_airborne = verify_in_range(accuracy);
        }

        //Returns the first shot accuracy_airborne variable of this weapon.
        public float get_accuracy_airborne()
        {
            return accuracy_airborne;
        }

        //Sets the accuracy_walking variable to a given value.
        public void set_accuracy_walking(float accuracy)
        {
            accuracy_walking = verify_in_range(accuracy);
        }

        //Returns the first shot accuracy_walking variable of this weapon.
        public float get_accuracy_walking()
        {
            return accuracy_walking;
        }

        //Sets the accuracy_running variable to a given value.
        public void set_accuracy_running(float accuracy)
        {
            accuracy_running = verify_in_range(accuracy);
        }

        //Returns the first shot accuracy_running variable of this weapon.
        public float get_accuracy_running()
        {
            return accuracy_running;
        }

        //Sets the accuracy_crouch_walking variable to a given value.
        public void set_accuracy_crouch_walking(float accuracy)
        {
            accuracy_crouch_walking = verify_in_range(accuracy);
        }

        //Returns the first shot accuracy_crouch_walking variable of this weapon.
        public float get_accuracy_crouch_walking()
        {
            return accuracy_crouch_walking;
        }


    //==============
    // Magazine Getters, Setters, and Updaters.
    //==============
        //Sets the magazine_max_capacity variable to a given value.
        public void set_magazine_max_capacity(int capacity)
        {
            magazine_max_capacity = verify_not_negative(capacity);
        }

        //Returns the magazine_max_capacity variable.
        public int get_magazine_max_capacity()
        {
            return magazine_max_capacity;
        }

        //Sets the magazine_current_capacity variable to a given value.
        public void set_magazine_current_amount(int ammo)
        {
            magazine_current_amount = verify_not_negative(ammo);
        }

        //Updates the magazine_current_capacity variable by a given amount.
        public void update_magazine_current_amount(int update)
        {
            magazine_current_amount += update;
            magazine_current_amount = verify_not_negative(magazine_current_amount);
        }

        //Returns the magazine_current_capacity variable.
        public int get_magazine_current_amount()
        {
            return magazine_current_amount;
        }


    //==============
    // Ammo Getters, Setters, and Updaters.
    //==============
        //Sets the ammo_max variable to a given value.
        public void set_ammo_max(int max)
        {
            ammo_max = verify_not_negative(max);
        }

        //Returns the ammo_max variable.
        public int get_ammo_max()
        {
            return ammo_max;
        }

        //Sets the ammo_current variable to a given value.
        public void set_ammo_current(int ammo)
        {
            ammo_current = verify_not_negative(ammo);
        }

        //Updates the ammo_current variable by a given amount. 
        public void update_ammo_current(int update)
        {
            ammo_current += update;
            ammo_current = verify_not_negative(ammo_current);
        }

        //Returns the ammo_current variable.
        public int get_ammo_current()
        {
            return ammo_current;
        }


    //==============
    // Recoil Getters and Setters.
    //==============
        //Sets the recoil_enabled variable to True or False.
        public void set_recoil_enabled(bool enabled)
        {
            recoil_enabled = enabled;
        }

        //Returns the recoil_enabled variable.
        public bool get_recoil_enabled()
        {
           return recoil_enabled;
        }

        //Sets the recoil_base variable to a given value.
        public void set_recoil_base(float recoil)
        {
            recoil_base = verify_not_negative(recoil);
        }

        //Returns the recoil_base variable of this weapon.
        public float get_recoil_base()
        {
            return recoil_base;
        }

        //Sets the recoil_increase_per_shot variable to a given value.
        public void set_recoil_increase_per_shot(float recoil)
        {
            recoil_increase_per_shot = verify_not_negative(recoil);
        }

        //Returns the recoil_increase_per_shot variable of this weapon.
        public float get_recoil_increase_per_shot()
        {
            return recoil_increase_per_shot;
        }

        //Sets the recoil_max variable to a given value.
        public void set_recoil_max(float recoil)
        {
            recoil_max = verify_not_negative(recoil);
        }

        //Returns the recoil_max variable of this weapon.
        public float get_recoil_max()
        {
            return recoil_max;
        }

        //Sets the recoil_recovery_time variable to a given value.
        public void set_recoil_recovery_time(float time)
        {
            recoil_recovery_time = verify_not_negative(time);
        }

        //Returns the recoil_recovery_time variable of this weapon.
        public float get_recoil_recovery_time()
        {
            return recoil_recovery_time;
        }

        //Sets the recoil_cooldown variable to a given value.
        public void set_recoil_cooldown(float time)
        {
            recoil_cooldown = verify_not_negative(time);
        }

        //Returns the recoil_cooldown variable of this weapon.
        public float get_recoil_cooldown()
        {
            return recoil_cooldown;
        }

        //Sets the recoil_counter variable to a given value.
        public void set_recoil_counter(float counter)
        {
            recoil_counter = verify_not_negative(counter);
        }

        //Returns the recoil_counter variable of this weapon.
        public float get_recoil_counter()
        {
            return recoil_counter;
        }


    //==============
    // Miscellaneous Gun Property Getters, Setters, and Updaters.
    //==============
        //Sets the fire_rate variable to a given value.
        public void set_fire_rate(float rate)
        {
            fire_rate = verify_not_negative(rate);
        }

        //Returns the fire_rate variable of this weapon.
        public float get_fire_rate()
        {
            return fire_rate;
        }

        //Sets the reload_time variable to a given value.
        public void set_reload_time(float time)
        {
            fire_rate = verify_not_negative(time);
        }

        //Returns the reload_time variable of this weapon.
        public float get_reload_time()
        {
            return reload_time;
        }

        //Sets the max_distance variable to a given value.
        public void set_max_distance(float distance)
        {
            max_distance = verify_not_negative(distance);
        }

        //Returns the max_distance variable of this weapon.
        public float get_max_distance()
        {
            return max_distance;
        }

        //Sets the shots_per_click variable to a given value.
        public void set_shots_per_click(int shots)
        {
            if (shots < 1)
            {
                shots_per_click = 1;
            }
            else if (shots > get_magazine_max_capacity())
            {
                shots_per_click = get_magazine_max_capacity();
            }
            else
            {
                shots_per_click = shots;
            }
        }

        //Returns the shots_per_click variable of this weapon.
        public int get_shots_per_click()
        {
            return shots_per_click;
        }

        //Sets the burst_fire_rate variable to a given value.
        public void set_burst_fire_rate(float rate)
        {
            burst_fire_rate = verify_not_negative(rate);
        }

        //Returns the burst_fire_rate variable of this weapon.
        public float get_burst_fire_rate()
        {
            return burst_fire_rate;
        }

        //Sets the automatic variable to True or False.
        public void set_automatic(bool auto)
        {
            automatic = auto;
        }

        //Returns the automatic variable.
        public bool get_automatic()
        {
            return automatic;
        }

        //Sets the movement_speed_reduction variable to a given value.
        public void set_movement_speed_reduction(float percentage)
        {
            movement_speed_reduction = verify_in_range(percentage);
        }

        //Returns the movement_speed_reduction variable.
        public float get_movement_speed_reduction()
        {
            return movement_speed_reduction;
        }

        //Sets the cool_down variable to a given value.
        public void set_cool_down(float time)
        {
            cool_down = verify_not_negative(time);
        }

        //Updates the cool_down variable by a given amount.
        public void update_cool_down(float update)
        {
            cool_down += update;
            cool_down = verify_not_negative(cool_down);
        }

        //Returns the cool_down variable.
        public float get_cool_down()
        {
            return cool_down;
        }


    //==============
    // GameObject/Component Getters and Setters.
    //==============
        //Sets the player GameObject.
        public void set_player(GameObject obj)
        {
            if (obj)
            {
                player = obj;
            }
        }

        //Returns the player GameObject linked to this script.
        public GameObject get_player()
        {
            return player;
        }

        //Sets the player_animator reference.
        public void set_player_animator(Animator anim)
        {
            if (anim)
            {
                player_animator = anim;
            }
        }

        //Returns the player_animator Animator linked to this script.
        public Animator get_player_animator()
        {
            return player_animator;
        }

        //Sets the gun_muzzle GameObject.
        public void set_gun_muzzle(GameObject obj)
        {
            if (obj)
            {
                gun_muzzle = obj;
            }
        }

        //Returns the gun_muzzle GameObject linked to this script.
        public GameObject get_gun_muzzle()
        {
            return gun_muzzle;
        }

        //Sets the gun_muzzle_transform reference.
        public void set_gun_muzzle_transform(Transform trans)
        {
            if (trans)
            {
                gun_muzzle_transform = trans;
            }
        }

        //Returns the gun_muzzle_transform Transform linked to this script.
        public Transform get_gun_muzzle_transform()
        {
            return gun_muzzle_transform;
        }

        //Sets the bullet_prefab GameObject.
        public void set_bullet_prefab(GameObject obj)
        {
            if (obj)
            {
                bullet_prefab = obj;
            }
        }

        //Returns the bullet_prefab GameObject linked to this script.
        public GameObject get_bullet_prefab()
        {
            return bullet_prefab;
        }

        //Sets the bullet_prefab_transform reference.
        public void set_bullet_prefab_transform(Transform trans)
        {
            if (trans)
            {
                bullet_prefab_transform = trans;
            }
        }

        //Returns the bullet_prefab_transform Transform linked to this script.
        public Transform get_bullet_prefab_transform()
        {
            return bullet_prefab_transform;
        }

        //Sets the muzzle_flash_prefab GameObject.
        public void set_muzzle_flash_prefab(GameObject obj)
        {
            if (obj)
            {
                muzzle_flash_prefab = obj;
            }
        }

        //Returns the muzzle_flash_prefab GameObject linked to this script.
        public GameObject get_muzzle_flash_prefab()
        {
            return muzzle_flash_prefab;
        }

        //Sets the muzzle_flash_prefab_transform reference.
        public void set_muzzle_flash_prefab_transform(Transform trans)
        {
            if (trans)
            {
                muzzle_flash_prefab_transform = trans;
            }
        }

        //Returns the muzzle_flash_prefab_transform Transform linked to this script.
        public Transform get_muzzle_flash_prefab_transform()
        {
            return muzzle_flash_prefab_transform;
        }


    //==============
    // Player Position/Stance Getters and Setters.
    //==============
        //Sets the is_standing variable to True or False.
        public void set_is_standing(bool standing)
        {
            is_standing = standing;
        }

        //Returns the is_standing variable.
        public bool get_is_standing()
        {
            return is_standing;
        }

        //Sets the is_crouching variable to True or False.
        public void set_is_crouching(bool crouching)
        {
            is_crouching = crouching;
        }

        //Returns the is_crouching variable.
        public bool get_is_crouching()
        {
            return is_crouching;
        }

        //Sets the is_walking variable to True or False.
        public void set_is_walking(bool walking)
        {
            is_walking = walking;
        }

        //Returns the is_walking variable.
        public bool get_is_walking()
        {
            return is_walking;
        }

        //Sets the is_airborne variable to True or False.
        public void set_is_airborne(bool airborne)
        {
            is_airborne = airborne;
        }

        //Returns the is_airborne variable.
        public bool get_is_airborne()
        {
            return is_airborne;
        }

        //Sets the is_running variable to True or False.
        public void set_is_running(bool running)
        {
            is_running = running;
        }

        //Returns the is_running variable.
        public bool get_is_running()
        {
            return is_running;
        }
}