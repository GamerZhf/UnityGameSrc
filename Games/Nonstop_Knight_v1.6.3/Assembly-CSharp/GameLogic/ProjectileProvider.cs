namespace GameLogic
{
    using App;
    using System;
    using UnityEngine;

    public class ProjectileProvider : IInstanceProvider<Projectile>
    {
        private GameObjectProvider m_gop;
        private static int sm_spawnCounter;

        public ProjectileProvider(int layer)
        {
            this.m_gop = new GameObjectProvider(string.Empty, App.Binder.PersistentObjectRootTm, layer);
        }

        public Projectile instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            obj2.name = "Projectile_" + sm_spawnCounter++;
            Projectile projectile = obj2.AddComponent<Projectile>();
            obj2.SetActive(false);
            return projectile;
        }

        public void onDestroy(Projectile obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(Projectile obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

