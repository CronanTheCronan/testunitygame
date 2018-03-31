using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Characters
{
    [SelectionBase]
    public class Character : MonoBehaviour
    {
        const float k_Half = 0.5f;
        const string ATTACK_TRIGGER = "Attack";
        const string ROLLING_TRIGGER = "Rolling";
        const string HORIZONTAL = "Horizontal";
        const string VERTICAL = "Vertical";

        [Header("Audio")]
        [SerializeField] float audioSourceSpatialBlend = 0.5f;

        [Header("Animator")]
        [SerializeField] RuntimeAnimatorController animatorController;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] Avatar characterAvatar;

        [Header("Capsule Collider")]
        [SerializeField] Vector3 colliderCenter = new Vector3(0, 0.54f, 0);
        [SerializeField] float colliderRadius = 0.26f;
        [SerializeField] float colliderHeight = 1.07f;

        [Header("Movement")]
        [SerializeField] float stoppingDistance = 1f;
        [SerializeField] AudioClip attackSoundClip;
        [SerializeField] float groundCheckDistance = 0.1f;
        [SerializeField] float movingTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;
        [SerializeField] float jumpPower = 8f;
        [Range(1f, 4f)] [SerializeField] float gravityMultiplier = 2f;
        [SerializeField] float runCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
        [SerializeField] float moveSpeedMultiplier = 1f;
        [SerializeField] float animSpeedMultiplier = 1f;
        [SerializeField] float moveThreshold = 1f;

        AudioSource audioSource;
        Animator animator;
        Player player;
        SpecialAbilities specialAbilities;
        Rigidbody playerRigidbody;
        Transform mainCamera;  // A reference to the main camera in the scenes transform
        Vector3 groundNormal;
        Vector3 cameraForwardDirection;             // The current forward direction of the camera
        Vector3 move;
        Vector3 capsuleCenter;

        private float capsuleHeight;
        private bool jumping;                      // the world-relative desired move direction, calculated from the camForward and user input.
        private bool attacking;
        private bool rolling;
        private float attackDuration;
        private bool playerIdle;
        private bool isGrounded;
        private float origGroundCheckDistance;
        private float turnAmount;
        private float forwardAmount;

        public bool Attacking { get { return attacking; } }

        public delegate void OnPlayerAttacking(bool attacking);
        public event OnPlayerAttacking notifyPlayerAttackObservers;

        void Awake()
        {
            AddRequiredComponents();
        }

        private void AddRequiredComponents()
        {
            animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = animatorController;
            animator.avatar = characterAvatar;

            playerRigidbody = gameObject.AddComponent<Rigidbody>();
            playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = audioSourceSpatialBlend;

            var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.radius = colliderRadius;
            capsuleCollider.center = colliderCenter;
        }

        void Start()
        {
            player = GetComponent<Player>();
            specialAbilities = GetComponent<SpecialAbilities>();
            origGroundCheckDistance = groundCheckDistance;


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

        private void FixedUpdate()
        {
            //float triggerAxis = Input.GetAxisRaw("XBox_Triggers");

            // read inputs
            float h = Input.GetAxis(HORIZONTAL);
            float v = Input.GetAxis(VERTICAL);

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
            Move(move, jumping);
            jumping = false;


            if (!attacking && !jumping && !rolling)
            {
                playerIdle = true;
            }

            if (playerIdle)
            {
                jumping = Input.GetButtonDown("XBox_A_Button");
                rolling = Input.GetButtonDown("XBox_B_Button");
                attacking = Input.GetButtonDown("XBox_RBumper");
            }

            if(rolling)
            {
                Roll();
            }

            if (playerIdle && Input.GetButtonDown("XBox_LBumper"))
            {
                specialAbilities.AttemptSpecialAbility(0);
            }

            if (attacking)
            {
                AttackTarget();
            }
        }

        public void Move(Vector3 movement, bool jump)
        {
            SetForwardAndTurn(movement);
            ApplyExtraTurnRotation();
            HandleVelocity(jump);
            UpdateAnimator(movement);
        }

        public void OnAnimatorMove()
        {
            // we implement this function to override the default root motion.
            // this allows us to modify the positional speed before it's applied.
            if (isGrounded && Time.deltaTime > 0)
            {
                Vector3 velocity = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

                // we preserve the existing y part of the current velocity.
                velocity.y = playerRigidbody.velocity.y;
                playerRigidbody.velocity = velocity;
            }
        }

        public void Kill()
        {
            // TODO to allow death signaling
        }

        void NotifyPlayerAttackObservers(bool attackStatus)
        {
            if (attacking)
                notifyPlayerAttackObservers(true);
        }

        void Roll()
        {
            animator.SetTrigger(ROLLING_TRIGGER);
        }

        void AttackTarget()
        {
            player.SetAttackAnimation();
            audioSource.clip = attackSoundClip;
            audioSource.Play();
            animator.SetTrigger(ATTACK_TRIGGER);
            StartCoroutine(DisableDamage());
        }

        IEnumerator DisableDamage()
        {
            yield return new WaitForSecondsRealtime(1f); // TODO change to attack duration and move attack duration to weapon based on specific animation
        }



        private void HandleVelocity(bool jump)
        {
            if (isGrounded)
            {
                HandleGroundedMovement(jump);
            }
            else
            {
                HandleAirborneMovement();
            }
        }

        void SetForwardAndTurn(Vector3 movement)
        {
            if (movement.magnitude > moveThreshold)
            {
                movement.Normalize();
            }

            var localMove = transform.InverseTransformDirection(movement);
            CheckGroundStatus();
            movement = Vector3.ProjectOnPlane(movement, groundNormal);
            turnAmount = Mathf.Atan2(localMove.x, localMove.z);
            forwardAmount = localMove.z;
        }

        void UpdateAnimator(Vector3 move)
        {
            // update the animator parameters
            animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
            animator.SetBool("OnGround", isGrounded);
            if (!isGrounded)
            {
                animator.SetFloat("Jump", playerRigidbody.velocity.y);
            }

            // calculate which leg is behind, so as to leave that leg trailing in the jump animation
            // (This code is reliant on the specific run cycle offset in our animations,
            // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
            float runCycle = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime + runCycleLegOffset, 1);
            float jumpLeg = (runCycle < k_Half ? 1 : -1) * forwardAmount;
            if (isGrounded)
            {
                animator.SetFloat("JumpLeg", jumpLeg);
            }

            // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
            // which affects the movement speed because of the root motion.
            if (isGrounded && move.magnitude > 0)
            {
                animator.speed = animSpeedMultiplier;
            }
            else
            {
                // don't use that while airborne
                animator.speed = 1;
            }
        }

        private void ScanForButtonDown()
        {
            throw new NotImplementedException();
        }

        void HandleAirborneMovement()
        {
            // apply extra gravity from multiplier:
            Vector3 extraGravityForce = (Physics.gravity * gravityMultiplier) - Physics.gravity;
            playerRigidbody.AddForce(extraGravityForce);

            groundCheckDistance = playerRigidbody.velocity.y < 0 ? origGroundCheckDistance : 0.01f;
        }

        void HandleGroundedMovement(bool jump)
        {
            // check whether conditions are right to allow a jump:
            if (jump && animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
            {
                // jump!
                playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, jumpPower, playerRigidbody.velocity.z);
                isGrounded = false;
                animator.applyRootMotion = false;
                groundCheckDistance = 0.1f;
            }
        }



        void ApplyExtraTurnRotation()
        {
            // help the character turn faster (this is in addition to root rotation in the animation)
            float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        }

        void CheckGroundStatus()
        {
            RaycastHit hitInfo;
#if UNITY_EDITOR
            // helper to visualise the ground check ray in the scene view
            Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * groundCheckDistance));
#endif
            // 0.1f is a small offset to start the ray from inside the character
            // it is also good to note that the transform position in the sample assets is at the base of the character
            if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, groundCheckDistance))
            {
                groundNormal = hitInfo.normal;
                isGrounded = true;
                animator.applyRootMotion = true;
            }
            else
            {
                isGrounded = false;
                groundNormal = Vector3.up;
                animator.applyRootMotion = false;
            }
        }
    }     
}