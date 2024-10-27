using System;
using UnityEngine;

namespace SGGames.Scripts.Core
{
    public class Controller2D : MonoBehaviour
{
    private const float m_skinWidth = .015f;
    [SerializeField] private float m_gravity;
    [SerializeField] private bool m_gravityActive;
    [SerializeField] private Vector2 m_velocity;
    [SerializeField] private float m_horizontalRayCount;
    [SerializeField] private float m_verticalRayCount;
    [SerializeField] private LayerMask m_obstacleMask;

    private bool m_isFreeze;
    private float m_horizontalRaySpacing;
    private float m_verticalRaySpacing;
    private float m_lastGround;
    private bool m_isGrounded;
    private LayerMask m_savedLayerMask;
    
    [Serializable]
    private struct RaycastOrigins
    {
        public Vector2 TopLeft, TopRight;
        public Vector2 BotLeft, BotRight;
    }

    [Serializable]
    public struct CollisionInfo
    {
        public bool CollideLeft, ColliderRight;
        public bool CollideAbove, CollideBelow;

        public void Reset()
        {
            CollideLeft = ColliderRight = false;
            CollideAbove = CollideBelow = false;
        }
    }

    private RaycastOrigins m_raycastOrigins;
    [SerializeField] private CollisionInfo m_collisionInfo;
    private BoxCollider2D m_collider2D;
    private bool m_isCollisionOff;

    public float Gravity => m_gravity;

    public bool IsGravityActive => m_gravityActive;
    public bool IsGrounded => m_isGrounded;
    public CollisionInfo CollisionInfos => m_collisionInfo;
    
    public Vector2 Velocity => m_velocity;

    public float LastGroundY => m_lastGround;

    public void CollisionOff()
    {
        m_isCollisionOff = true;
        m_obstacleMask = 0;
    }

    public void CollisionOn()
    {
        m_isCollisionOff = false;
        m_obstacleMask = m_savedLayerMask;
    }

    private void Start()
    {
        m_collider2D = GetComponent<BoxCollider2D>();
        SetGravityActive(true);
        CalculateRaySpacing();
        m_savedLayerMask = m_obstacleMask;
    }

    private void Update()
    {
        if (m_isFreeze)
        {
            return;
        }
        
        if (m_gravityActive)
        {
            m_velocity.y += m_gravity * Time.deltaTime;
        }
        
        Move(m_velocity * Time.deltaTime);
        if (m_gravityActive  && m_collisionInfo.CollideBelow)
        {
            m_lastGround = transform.position.y;
            m_velocity.y = 0;
        }
    }

    private void UpdateRaycastOrigins()
    {
        Bounds bounds = m_collider2D.bounds;
        bounds.Expand(m_skinWidth * -2);
        m_raycastOrigins.BotLeft = new Vector2(bounds.min.x,bounds.min.y);
        m_raycastOrigins.BotRight = new Vector2(bounds.max.x,bounds.min.y);
        m_raycastOrigins.TopLeft = new Vector2(bounds.min.x,bounds.max.y);
        m_raycastOrigins.TopRight = new Vector2(bounds.max.x,bounds.max.y);
        
    }

    private void CalculateRaySpacing()
    {
        Bounds bounds = m_collider2D.bounds;
        bounds.Expand(m_skinWidth * -2);
        m_horizontalRayCount = Mathf.Clamp(m_horizontalRayCount, 2, int.MaxValue);
        m_verticalRayCount = Mathf.Clamp(m_verticalRayCount, 2, int.MaxValue);

        m_horizontalRaySpacing = bounds.size.y / (m_horizontalRayCount - 1);
        m_verticalRaySpacing = bounds.size.x / (m_verticalRayCount - 1);
        
    }

    private void VerticalCollision(ref Vector2 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + m_skinWidth;
        
        for (int i = 0; i < m_verticalRayCount; i++)
        {
            Vector2 rayOrigin = directionY == -1 ? m_raycastOrigins.BotLeft : m_raycastOrigins.TopLeft;
            rayOrigin += Vector2.right * (m_verticalRaySpacing * i + velocity.x);

            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, m_obstacleMask);
            Debug.DrawRay(rayOrigin, Vector2.up * (rayLength * directionY),Color.red);

            if (hit2D)
            {
                velocity.y = (hit2D.distance - m_skinWidth) * directionY;
                rayLength = hit2D.distance;
                
                m_collisionInfo.CollideBelow = directionY == -1;
                m_isGrounded = m_collisionInfo.CollideBelow;
                m_collisionInfo.CollideAbove = directionY == 1;
            }
        }
        
    }
    
    private void HorizontalCollision(ref Vector2 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + m_skinWidth;
        
        for (int i = 0; i < m_horizontalRayCount; i++)
        {
            Vector2 rayOrigin = directionX == -1 ? m_raycastOrigins.BotLeft : m_raycastOrigins.BotRight;
            rayOrigin += Vector2.up * (m_horizontalRaySpacing * i);

            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, m_obstacleMask);
            Debug.DrawRay(rayOrigin, Vector2.right * (rayLength * directionX),Color.red);

            if (hit2D)
            {
                velocity.x = (hit2D.distance - m_skinWidth) * directionX;
                rayLength = hit2D.distance;

                m_collisionInfo.CollideLeft = directionX == -1;
                m_collisionInfo.ColliderRight = directionX == 1;
            }
        }
    }

    public void Move(Vector2 velocity)
    {
        UpdateRaycastOrigins();
        m_collisionInfo.Reset();
        
        HorizontalCollision(ref velocity);
        VerticalCollision(ref velocity);

        transform.Translate(velocity);
        Physics2D.SyncTransforms();
    }

    public void SetHorizontalVelocity(float value)
    {
        m_velocity.x = value;
    }
    
    public void SetVerticalVelocity(float value)
    {
        m_velocity.y = value;
    }

    public void AddVerticalVelocity(float value)
    {
        m_velocity.y += value;
    }

    public void SetVelocity(Vector2 vec)
    {
        m_velocity = vec;
    }

    public void SetGravityActive(bool isActive)
    {
        m_gravityActive = isActive;
    }
    

    public void Freeze()
    {
        m_velocity = Vector2.zero;
        m_isFreeze = true;
    }

    public void UnFreeze()
    {
        m_velocity = Vector2.zero;
        m_isFreeze = false;
    }
}
}

