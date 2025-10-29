namespace RedSilver2.Framework.Player
{
    [System.Serializable]
    public abstract class GravityModule : PlayerExtensionModule
    {
        private float gravity;
        public float Gravity => gravity;

        private void Start()
        {
            gravity = 5f;
        }

        public void SetGravity(float gravity)
        {
            this.gravity = gravity;
        }
    }
}
