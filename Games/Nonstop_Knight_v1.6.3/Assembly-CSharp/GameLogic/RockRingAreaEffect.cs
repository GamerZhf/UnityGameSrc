namespace GameLogic
{
    using App;
    using System;
    using UnityEngine;

    public class RockRingAreaEffect : AreaEffect
    {
        private int m_rockWaveCounter;
        private ManualTimer m_waveTimer = new ManualTimer(ConfigSkills.Slam.RockRingWaveInterval);

        protected override void onFixedUpdate(float dt)
        {
            if (this.m_rockWaveCounter < ConfigSkills.Slam.RockRingWaveCount)
            {
                this.m_waveTimer.tick(dt);
                if (this.m_waveTimer.Idle)
                {
                    float rockRingEndRadius = ConfigSkills.Slam.RockRingEndRadius;
                    if (base.OwningCharacter.IsPet)
                    {
                        rockRingEndRadius *= ConfigSkills.Slam.RockRingScalePet;
                    }
                    float num2 = 360f / ((float) ConfigSkills.Slam.RockRingProjectileCount);
                    float num3 = (rockRingEndRadius - ConfigSkills.Slam.RockRingStartRadius) / ((float) (ConfigSkills.Slam.RockRingWaveCount - 1));
                    for (int i = 0; i < ConfigSkills.Slam.RockRingProjectileCount; i++)
                    {
                        Vector3 vector3 = (Vector3) (Quaternion.AngleAxis(num2 * i, Vector3.up) * base.Transform.forward);
                        Vector3 vector = Vector3Extensions.ToXzVector3(vector3.normalized);
                        float num5 = ConfigSkills.Slam.RockRingStartRadius + (this.m_rockWaveCounter * num3);
                        Vector3 spawnWorldPt = base.Transform.position + ((Vector3) (vector * num5));
                        Projectile projectile = GameLogic.Binder.ProjectilePool.getObject();
                        projectile.gameObject.SetActive(true);
                        projectile.spawn(base.OwningCharacter, spawnWorldPt, Vector3.up, (Projectile.ProjectileProperties) base.Properties.CustomProperties);
                    }
                    this.m_rockWaveCounter++;
                    this.m_waveTimer.reset();
                }
            }
        }

        protected override void onPreDestroy()
        {
        }

        protected override void onSpawn()
        {
            this.m_rockWaveCounter = 0;
            this.m_waveTimer.end();
        }

        public override AreaEffectType Type
        {
            get
            {
                return AreaEffectType.RockRing;
            }
        }
    }
}

