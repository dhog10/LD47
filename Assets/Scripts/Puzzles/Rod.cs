using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rod : Holdable
{
    public AudioSource m_BreakSound;
    public GameObject m_ParticleObject;
    public GameObject m_Capsule;

    private RodCooler m_Cooler;
    private bool m_Broken;

    public override bool CanHold()
    {
        if (m_Cooler != null)
        {
            return false;
        }

        if (m_Broken)
        {
            return false;
        }

        return base.CanHold();
    }

    public void SetCooler(RodCooler cooler)
    {
        m_Cooler = cooler;
    }

    private bool IsRodSafe(GameObject obj)
    {
        return obj.tag == "RodSafe";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_Broken)
        {
            return;
        }

        var obj = collision.gameObject;
        if (!this.IsRodSafe(obj))
        {
            m_BreakSound.Play();
            m_Broken = true;
            m_ParticleObject.SetActive(true);
            m_Capsule.SetActive(false);
            StartCoroutine(this.DeleteRoutine());
        }
    }

    private IEnumerator DeleteRoutine()
    {
        yield return new WaitForSeconds(1f);

        GameObject.Destroy(gameObject);
    }
}
