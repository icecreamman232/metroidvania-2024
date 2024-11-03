using UnityEngine;

namespace SGGames.Scripts.Managers
{
    public class CameraFollowing : Singleton<CameraFollowing>
    {
        [SerializeField] private Transform m_cameraTransform;
        [SerializeField] private Transform m_targetTransform;
        [SerializeField] private float m_followingSpeed;
        [SerializeField] private float m_aheadDistanceX;
        [SerializeField] private float m_smoothTime;
        private Vector3 m_targetPos;
        private Vector2 m_lastTargetPos;
        private float m_flipValue = 1;

        private bool m_canFollow;
        private float m_curSpeed;
        private Vector3 m_cameraSmoothVelocity;
        
        private void Awake()
        {
            // var camera = FindObjectOfType<UnityEngine.Camera>();
            // if (camera == null)
            // {
            //     Debug.LogError("Camera not found!");
            // }
            // m_cameraTransform = camera.transform;
            m_canFollow = true;
        }

        public void SetPermission(bool value)
        {
            m_canFollow = value;
        }
        public void SetTarget(Transform target)
        {
            m_targetTransform = target;
        }

        public void SetCameraPosition(Vector3 newPosition)
        {
            m_cameraTransform.position = newPosition;
        }
        private void Update()
        {
            if (!m_canFollow) return;
            if (m_targetTransform == null) return;
            m_targetPos = m_targetTransform.position + Vector3.right * (m_aheadDistanceX * m_flipValue);
            m_targetPos.z = -10;
            
            //Smooth camera movement
            //m_cameraSmoothVelocity = Vector3.zero;
            m_cameraTransform.position = Vector3.SmoothDamp(m_cameraTransform.position, m_targetPos, ref m_cameraSmoothVelocity,m_smoothTime);
        }

        public void Flip(bool isFlip)
        {
            m_flipValue = isFlip ? -1 : 1;
        }

        public void ResetSmoothValue()
        {
            m_cameraSmoothVelocity = Vector3.zero;
        }

        public void ResetCamera()
        {
            m_cameraTransform.position = new Vector3(0, 0, -10);
        }
    }
}

