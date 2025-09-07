using UnityEngine;


namespace RedSilver2.Framework.Player
{
    public abstract class TiltMotionModule : PlayerExtensionModule
    {
        [Space]
        [SerializeField] private Vector2 minRotation;
        [SerializeField] private Vector2 maxRotation;

        public Vector2 MinRotation => minRotation;
        public Vector2 MaxRotation => maxRotation;
    }
}
