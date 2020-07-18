public interface IDamagable
{
    float DealDamage(float damage);
    float DealDamage(float damage, StatusAilments ailment, float effectPotency, float effectDuration);
}