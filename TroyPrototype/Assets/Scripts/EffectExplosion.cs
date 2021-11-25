using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectExplosion : MonoBehaviour
{
    [Header("Explosion Settings")]
    public SpriteRenderer m_ThisRenderer;
    public float m_fExplosionRadius=1.0f;
    public float m_fFrameInterval = 0.1f;
    [SerializeField] private int m_iNumberOfFrames = 9;
    [SerializeField] private Sprite[] m_sprtExplosions;

    private float m_fCounter = 0.0f;
    private int m_iFrameIndex = 0;
    private bool m_bEnabled = false;
    private float m_fSizeModifier = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        if(m_ThisRenderer==null)
        {
            if(GetComponent<SpriteRenderer>() != null)
            {
                m_bEnabled = true;
            }
            else
            {
                m_bEnabled = false;
            }
        }
        else
        {
            m_bEnabled = true;
        }

        if(m_bEnabled)
        {
            m_ThisRenderer = GetComponent<SpriteRenderer>();
            m_fSizeModifier = m_fExplosionRadius / 0.32f;
            transform.localScale = new Vector3(m_fSizeModifier, m_fSizeModifier, 1.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_fCounter += Time.deltaTime;
        if(m_fCounter >= m_fFrameInterval)
        {
            m_iFrameIndex ++;
            if(m_iFrameIndex<m_iNumberOfFrames)
            {
                m_ThisRenderer.sprite = m_sprtExplosions[m_iFrameIndex];
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void UpdateRadius(float _input)
    {
        m_fExplosionRadius = _input/2;
        m_fSizeModifier = m_fExplosionRadius / 0.32f;

        transform.localScale = new Vector3(m_fSizeModifier, m_fSizeModifier, 1.0f);
    }
}
