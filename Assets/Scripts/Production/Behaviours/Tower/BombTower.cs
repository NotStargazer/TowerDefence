using Tools;
using UnityEngine;

/// <summary>
/// Abstract Tower, Fires exploding projectiles.
/// </summary>
public class BombTower : Tower
{
    [SerializeField] private Bomb m_Bomb;
    private static GameObjectPool s_BombPool;

    private void Start()
    {
        if (s_BombPool == null)
        {
            var go = new GameObject("Bomb Bag");
            s_BombPool = new GameObjectPool(10, m_Bomb.gameObject, 1, go.transform);
        }
    }

    public override void Fire()
    {
        var bombProjectile = s_BombPool.Rent(true).GetComponent<Bomb>();
        bombProjectile.transform.position = m_FirePoint.position;
        bombProjectile.SetTarget(m_Targets[0].transform);
    }
}
