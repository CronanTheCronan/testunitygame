using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Characters
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] AudioClip attackSoundClip;

        AudioSource audioSource;
        Animator animator;
        Player player;

        
        private ThirdPersonCharacter character; // A reference to the ThirdPersonCharacter on the object
        private Transform mainCamera;                  // A reference to the main camera in the scenes transform
        private Vector3 cameraForwardDirection;             // The current forward direction of the camera
        private Vector3 move;
        private bool jumping;                      // the world-relative desired move direction, calculated from the camForward and user input.
        public bool attacking = false;
        private bool rolling = false;
        //private bool playerIdle = false;
        private float attackDuration;
        //private bool isPlayerAlive = true;

        //Temp for debug
        //[SerializeField] SpecialAbility[] abilities;

        const string ATTACK_TRIGGER = "Attack";
        public bool Attacking { get { return attacking; } }

        public delegate void OnPlayerAttacking(bool attacking); // declare new delegate type
        public event OnPlayerAttacking notifyPlayerAttackObservers; // instantiate an observer set

        private void Start()
        {
            player = GetComponent<Player>();

            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            character = GetComponent<ThirdPersonCharacter>();

            // get the transform of the main camera
            if (Camera.main != null)
            {
                mainCamera = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            }

        }

        private void Update()
        { 
            if(!attacking && !jumping && !rolling)
            {
                //playerIdle = true;
            }

            if(!attacking && Input.GetButtonDown("XBox_RBumper"))
            {
                AttackPlayer();
            }

            if(player.healthAsPercentage > Mathf.Epsilon)
            {
                //isPlayerAlive = true;
                //ScanForButtonDown();
            }
        }

        void AttackPlayer()
        {
            attacking = true;
            audioSource.clip = attackSoundClip;
            audioSource.Play();
            animator.SetTrigger(ATTACK_TRIGGER);
            StartCoroutine(DisableDamage());
        }

        IEnumerator DisableDamage()
        {
            yield return new WaitForSecondsRealtime(1f); // TODO change to attack duration and move attack duration to weapon based on specific animation
            attacking = false;
        }

        private void ScanForButtonDown()
        {
            throw new NotImplementedException();
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            //float triggerAxis = Input.GetAxisRaw("XBox_Triggers");
            
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

            if (!jumping)
            {
                jumping = Input.GetButtonDown("XBox_A_Button");
            }

            //if (!attacking && !jumping && triggerAxis < 0)
            //{
            //    PlayerBasicAttack();
            //}

            if(!attacking && !jumping && !rolling && Input.GetButtonDown("XBox_LBumper"))
            {
                AttemptSpecialAbility(1);
            }

            if (!rolling && !attacking && Input.GetButtonDown("XBox_B_Button"))
            {
                Roll();
            }

        }

        private void AttemptSpecialAbility(int abilityIndex)
        {
            var stamina = GetComponent<Stamina>();
            var staminaCost = player.abilities[abilityIndex].GetStaminaCost();

            if (stamina.IsStaminaAvailable(10f))
            {
                stamina.ConsumeEnergy(staminaCost);
                var abilityParams = new AbilityUseParams(player.GetBaseDamage());
                player.abilities[abilityIndex].Use(abilityParams);
            }
        }

        //private void PlayerBasicAttack()
        //{
        //    attacking = true;
        //    animator.SetTrigger(ATTACK_TRIGGER);

        //    var stamina = GetComponent<Stamina>();
        //    if (stamina.IsStaminaAvailable(10f))
        //    {
        //        stamina.ConsumeEnergy(10f);
        //    }
        //}

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