namespace RedSilver2.Framework.Stats
{
    public abstract class Health : RegenerativeNumberStat, IHittable
    {
        public void Hit(float damage) {
            if (damage > 0f) {
                SetCurrentValue(CurrentValue - damage);
            }
        }
    }
}
