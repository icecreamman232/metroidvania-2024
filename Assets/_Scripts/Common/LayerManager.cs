using UnityEngine;

namespace SGGames.Scripts.Managers
{
    public static class LayerManager
    {
        #region Layers
        public static int PlayerLayer = 6;
        public static int Obstacle = 7;
        public static int Enemy = 8;

        #endregion

        #region Layer Masks

        public static int PlayerLayerMask = 1 << PlayerLayer;
        public static int ObstacleLayerMask = 1 << Obstacle;
        public static int EnemyLayerMask = 1 << Enemy;

        //public static int PlayerMask = DoorMask | WallMask;
        #endregion
        
        public static bool IsInLayerMask(int layerWantToCheck, LayerMask layerMask)
        {
            if (((1 << layerWantToCheck) & layerMask) != 0)
            {
                return true;
            }
            return false;
        }
    }

}
