using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace RPG.Characters
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class PlayerMovement : MonoBehaviour
    {

        Animator animator;
        private ThirdPersonCharacter character; // A reference to the ThirdPersonCharacter on the object
        private Transform mainCamera;                  // A reference to the main camera in the scenes transform
        private Vector3 cameraForwardDirection;             // The current forward direction of the camera
        private Vector3 move;
        private bool jumping;                      // the world-relative desired move direction, calculated from the camForward and user input.
        private bool attacking = false;
        private bool rolling = false;
        private float baseDamage = 10f;

        //Temp for debug
        [SerializeField] SpecialAbility[] abilities;


        public bool Attacking { get { return attacking; } }

        public delegate void OnPlayerAttacking(bool attacking); // declare new delegate type
        public event OnPlayerAttacking notifyPlayerAttackObservers; // instantiate an observer set

        private void Start()
        {
            abilities[0].AddComponent(gameObject);
            animator = GetComponent<Animator>();
            // get the transform of the main camera
            if (Camera.main != null)
            {
                mainCamera = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            character = GetComponent<ThirdPersonCharacter>();
        }

        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            float triggerAxis = Input.GetAxisRaw("XBox_Triggers");
            if (!jumping)
            {
                jumping = Input.GetButtonDown("XBox_A_Button");
            }

            if (triggerAxis == 0)
                attacking = false;

            if (!attacking && !jumping && triggerAxis < 0)
            {
                PlayerBasicAttack();
            }

            if(!attacking && !jumping && !rolling && Input.GetButtonDown("XBox_RBumper"))
            {
                AttemptSpecialAbility(0);
            }

            if (!rolling && !attacking && Input.GetButtonDown("XBox_B_Button"))
            {
                Roll();
            }

            // read inputs
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);

            // calculate move direction to pass to character
            if (mainCamera != null)
            {
                // calculate camera relative direction to move:
                cameraForwardDirection = Vector3.Scale(mainCamera.forward, new Vector3(1, 0, 1)).normalized;
                move = v * cameraForwardDirection + h * mainCamera.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                move = v * Vector3.forward + h * Vector3.right;
            }


            // pass all parameters to the character control script
            character.Move(move, crouch, jumping);
            jumping = false;

        }

        private void AttemptSpecialAbility(int abilityIndex)
        {
            var stamina = GetComponent<Stamina>();
            var staminaCost = abilities[abilityIndex].GetStaminaCost();

            if (stamina.IsStaminaAvailable(10f))
            {
                stamina.ConsumeEnergy(staminaCost);
                var abilityParams = new AbilityUseParams(baseDamage);
                abilities[abilityIndex].Use(abilityParams);
            }
        }

        private void PlayerBasicAttack()
        {
            attacking = true;
            animator.SetTrigger("Attack");

            var stamina = GetComponent<Stamina>();
            if (stamina.IsStaminaAvailable(10f))
            {
                stamina.ConsumeEnergy(10f);
            }
        }

        void NotifyPlayerAttackObservers(bool attackStatus)
        {
            if (attacking)
                notifyPlayerAttackObservers(true);
        }

        void Roll()
        {
            rolling = true;
            animator.SetTrigger("Rolling");
            rolling = false;
        }
    }
}