using System;
using UnityEngine;

namespace SGGames.Scripts.Managers
{
    [Serializable]
    public struct CameraBounds
    {
        public Vector2 TopLeft;
        public Vector2 BotRight;
    }
    public class CameraFollowing : Singleton<CameraFollowing>
    {
        [SerializeField] private Transform m_cameraTransform;
        [SerializeField] private Transform m_targetTransform;
        [SerializeField] private float m_followingSpeed;
        [SerializeField] private float m_aheadDistanceX;
        [SerializeField] private float m_smoothTime;
        [SerializeField] private BoxCollider2D m_roomCollider;
        [SerializeField] private CameraBounds m_cameraBounds;
        
        private Vector3 m_targetPos;
        private Vector2 m_lastTargetPos;
        private float m_flipValue = 1;

        private bool m_canFollow;
        private float m_curSpeed;
        private Vector3 m_cameraSmoothVelocity;
        private float m_viewWidth;
        private float m_viewHeight;
        
        
        private void Awake()
        {
            // var camera = FindObjectOfType<UnityEngine.Camera>();
            // if (camera == null)
            // {
            //     Debug.LogError("Camera not found!");
            // }
            // m_cameraTransform = camera.transform;
            
            m_viewHeight = Camera.main.orthographicSize * 2;
            m_viewWidth = m_viewHeight * Camera.main.aspect;
            
            m_canFollow = true;
        }

        public void SetRoomCollider(BoxCollider2D roomCollider)
        {
            m_roomCollider = roomCollider;
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

            //Compute and limit camera position to make sure its in room view
            ComputeCameraLimits();
        }

        private void ComputeCameraLimits()
        {
            UpdateCameraBounds();
            
            var modifiedPos = m_cameraTransform.position;
            
            if (!IsTopLeftInBoundary(m_roomCollider) 
                && m_roomCollider.bounds.min.x > m_cameraBounds.TopLeft.x)
            {
                modifiedPos.x += m_roomCollider.bounds.min.x - GetTopLeftViewPos().x;
            }
            
            if (!IsBotRightInBoundary(m_roomCollider)
                && m_roomCollider.bounds.max.x < m_cameraBounds.BotRight.x)
            {
                modifiedPos.x -= m_cameraBounds.BotRight.x - m_roomCollider.bounds.max.x;
            }
            
            
            if (!IsTopLeftInBoundary(m_roomCollider)
                && m_roomCollider.bounds.max.y < m_cameraBounds.TopLeft.y)
            {
                modifiedPos.y -= m_cameraBounds.TopLeft.y - m_roomCollider.bounds.max.y;
            }
            
            if (!IsBotRightInBoundary(m_roomCollider)
                && m_roomCollider.bounds.min.y > m_cameraBounds.BotRight.y)
            {
                modifiedPos.y += m_roomCollider.bounds.min.y - m_cameraBounds.BotRight.y;
            }
            
            m_cameraTransform.position = modifiedPos;
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

        private Vector3 GetTopLeftViewPos()
        {
            var topLeft = m_cameraTransform.position + new Vector3(-m_viewWidth/2,m_viewHeight/2, 0);
            return topLeft;
        }
        
        private Vector3 GetBotRightViewPos()
        {
            var botRight = m_cameraTransform.position + new Vector3(m_viewWidth/2,-m_viewHeight/2, 0);
            return botRight;
        }
        
        private void UpdateCameraBounds()
        {
            m_cameraBounds.TopLeft = GetTopLeftViewPos();
            m_cameraBounds.BotRight = GetBotRightViewPos();
        }

        private bool IsTopLeftInBoundary(Collider2D roomCollider)
        {
            return roomCollider.bounds.Contains(GetTopLeftViewPos());
        }
        
        
        private bool IsBotRightInBoundary(Collider2D roomCollider)
        {
            return roomCollider.bounds.Contains(GetBotRightViewPos());
        }
    }
}

