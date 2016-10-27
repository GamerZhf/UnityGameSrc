namespace GameLogic
{
    using App;
    using System;
    using UnityEngine;

    public class RockConcentrationAreaEffect : AreaEffect
    {
        private ManualTimer m_rockSpawnTimer = new ManualTimer(ConfigSkills.Slam.RockConcentrationProjectileSpawnRate);

        protected override void onFixedUpdate(float dt)
        {
            this.m_rockSpawnTimer.tick(dt);
            if (this.m_rockSpawnTimer.Idle)
            {
                Vector2 vector = (Vector2) (UnityEngine.Random.insideUnitCircle * ConfigSkills.Slam.RockConcentrationRadius);
                Vector3 spawnWorldPt = base.Transform.position + new Vector3(vector.x, 0f, vector.y);
                Projectile projectile = GameLogic.Binder.ProjectilePool.getObject();
                projectile.gameObject.SetActive(true);
                projectile.spawn(base.OwningCharacter, spawnWorldPt, Vector3.up, (Projectile.ProjectileProperties) base.Properties.CustomProperties);
                this.m_rockSpawnTimer.reset();
            }
        }

        protected override void onPreDestroy()
        {
        }

        protected override void onSpawn()
        {
            this.m_rockSpawnTimer.end();
        }

        public override AreaEffectType Type
        {
            get
            {
                return AreaEffectType.RockConcentration;
            }
        }
    }
}

