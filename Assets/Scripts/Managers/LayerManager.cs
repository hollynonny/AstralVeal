using UnityEngine;

namespace Managers
{
    public static class LayerManager
    {
        private const string GROUND_LAYER_NAME = "Ground";
    
        public static readonly LayerMask GroundLayerMask = LayerMask.GetMask(GROUND_LAYER_NAME);
    }
}
