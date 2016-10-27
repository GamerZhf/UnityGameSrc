namespace GameLogic
{
    using App;
    using System;
    using UnityEngine;

    public class RockRingSectorAreaEffect : AreaEffect
    {
        private Vector3 m_direction;
        private int m_rockWaveCounter;
        private ManualTimer m_waveTimer = new ManualTimer(ConfigSkills.Slam.RockRingWaveInterval);

        protected override void onFixedUpdate(float dt)
        {
            if (this.m_rockWaveCounter < ConfigSkills.Slam.RockRingSectorWaveCount)
            {
                this.m_waveTimer.tick(dt);
                if (this.m_waveTimer.Idle)
                {
                    float num = ConfigSkills.Slam.RockRingSectorSize / ((float) ConfigSkills.Slam.RockRingSectorProjectileCount);
                    float num2 = (ConfigSkills.Slam.RockRingSectorEndRadius - ConfigSkills.Slam.RockRingStartRadius) / ((float) (ConfigSkills.Slam.RockRingSectorWaveCount - 1));
                    float num3 = (ConfigSkills.Slam.RockRingSectorProjectileCount - 1) * 0.5f;
                    for (int i = 0; i < ConfigSkills.Slam.RockRingSectorProjectileCount; i++)
                    {
                        Vector3 vector3 = (Vector3) (Quaternion.AngleAxis(num * (i - num3), Vector3.up) * this.m_direction);
                        Vector3 vector = Vector3Extensions.ToXzVector3(vector3.normalized);
                        float num5 = ConfigSkills.Slam.RockRingStartRadius + (this.m_rockWaveCounter * num2);
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
            this.m_direction = base.OwningCharacter.Facing;
            this.m_rockWaveCounter = 0;
            this.m_waveTimer.end();
        }

        public override AreaEffectType Type
        {
            get
            {
                return AreaEffectType.RockRingSector;
            }
        }
    }
}

