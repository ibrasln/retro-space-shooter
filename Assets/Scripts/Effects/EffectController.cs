using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public static EffectController instance;

    public GameObject[] effects;

    private void Start()
    {
        instance = this;
    }

    /// <summary>
    /// <para>Create Effects</para>
    /// <br>0 - HitEffect</br>
    /// <br>1 - ExplosionEffect</br>
    /// <br>2 - PlayerBombExplosionEffect</br>
    /// <br>3 - MissileExplosionEffect</br>
    /// <br>4 - EnemyDeathEffect</br>
    /// <br>5 - EnemyDeathEffect2</br>
    /// <br>6 - DeathEffect</br>
    /// <br>7 - BossDeathEffect</br>
    /// <br>8 - TeleportStart</br>
    /// <br>9 - TeleportFinish</br>
    /// </summary>
    /// <param name="chooseEffect"></param>
    /// <param name="go"></param>
    public void CreateEffect(int chooseEffect, Vector2 pos, float destroyTime)
    {
        GameObject effect = Instantiate(effects[chooseEffect], pos, Quaternion.identity);
        effect.hideFlags = HideFlags.HideAndDontSave;
        Destroy(effect, destroyTime);
    }

}
