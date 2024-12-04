using System.Collections;
using UnityEngine;

namespace SGGames.Scripts.Player
{
    public class PlayerHorizontalMovement : PlayerBehavior
    {
        [SerializeField] private float m_moveSpeed;
        //private CameraFollowing m_cameraFollowing;

        private readonly int m_runningAnimParam = Animator.StringToHash("Running");

        //private PlayerHealth m_health;
        private Animator m_animator;
        private SpriteRenderer m_spriteRenderer;
        private float m_velocityXSmoothing;
        private Vector2 m_rawInputValue;
        private bool m_isFlip;

        public bool IsFlip => m_isFlip;

        protected override void Start()
        {
            m_isAllow = true;
            //m_health = GetComponent<PlayerHealth>();
            m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            m_animator = GetComponentInChildren<Animator>();
            //m_cameraFollowing = Camera.main.GetComponent<CameraFollowing>();

            //m_cameraFollowing.SetTarget(transform);
            //m_cameraFollowing.SetCameraPosition(transform.position);
            //m_cameraFollowing.SetPermission(true);

            //m_health.OnDeath += HandleHorizontalMovementOnDead;
            base.Start();
        }

        private void OnDestroy()
        {
            //m_health.OnDeath -= HandleHorizontalMovementOnDead;
        }

        private void HandleHorizontalMovementOnDead()
        {
            m_controller.SetVelocity(Vector2.zero);
            m_controller.SetGravityActive(false);
            m_isAllow = false;
        }

        private void Update()
        {
            if (!m_isAllow) return;
            m_rawInputValue = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (m_rawInputValue.x == 0)
            {
                if (m_controller.CollisionInfos.CollideBelow)
                {
                    m_playerController.ChangeState(PlayerState.IDLE);
                }
                m_controller.SetHorizontalVelocity(0);
            }
            else
            {
                if (m_controller.CollisionInfos.CollideBelow)
                {
                    m_playerController.ChangeState(PlayerState.RUNNING);
                }
                //Smooth horizontal movement
                float targetVelocityX = m_rawInputValue.x * m_moveSpeed;
                m_controller.SetHorizontalVelocity(Mathf.SmoothDamp(
                    m_controller.Velocity.x, targetVelocityX,
                    ref m_velocityXSmoothing, 0.1f));

                //Flip character sprite if go to the left
                Flip(m_controller.Velocity.x < 0);
            }

            UpdateAnimator();
        }

        private void Flip(bool isFlip)
        {
            m_isFlip = isFlip;
            m_spriteRenderer.flipX = isFlip;
            m_playerController.ChangeFaceDirection(isFlip ? PlayerFaceDirection.LEFT : PlayerFaceDirection.RIGHT);
            //m_cameraFollowing.Flip(isFlip);
        }

        public override void ToggleAllow(bool value)
        {
            base.ToggleAllow(value);
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            m_animator.SetBool(m_runningAnimParam, m_controller.Velocity.x != 0);
        }

        private IEnumerator OnStopCameraForDuration(float duration)
        {
            //m_cameraFollowing.SetPermission(false);
            yield return new WaitForSeconds(duration);
            //m_cameraFollowing.SetPermission(true);
            //m_cameraFollowing.ResetSmoothValue();
        }

        public void StopCameraForDuration(float duration)
        {
            StartCoroutine(OnStopCameraForDuration(duration));
        }

        public void ToggleCamera(bool value)
        {
            //m_cameraFollowing.SetPermission(value);
        }

        public void StopRunning()
        {
            m_animator.SetBool(m_runningAnimParam, false);
            m_controller.SetVelocity(Vector2.zero);
            m_playerController.ChangeState(PlayerState.IDLE);
        }
    }
}

