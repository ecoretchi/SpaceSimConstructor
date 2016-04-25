using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class JetEffect : MonoBehaviour {

	// this script controls the jet's exhaust particle system, controlling the
	// size and colour based on the jet's current throttle value.
	//public Color minColour; // The base colour for the effect to start at

	public float effectSize;

	private ParticleSystem m_System; // The particle system that is being controlled
	private float m_OriginalStartSize; // The original starting size of the particle system
	private float m_OriginalLifetime; // The original lifetime of the particle system
	private Color m_OriginalStartColor; // The original starting colout of the particle system

	private void OnEnable() {
		m_System = GetComponent<ParticleSystem>();

		// set the original properties from the particle system
		m_OriginalLifetime = m_System.startLifetime;
		m_OriginalStartSize = m_System.startSize;
		m_OriginalStartColor = m_System.startColor;
	}


	// Update is called once per frame
	private void Update() {
		// update the particle system based on the jets throttle
		m_System.startLifetime = Mathf.Lerp( 0.0f, m_OriginalLifetime, effectSize );
		m_System.startSize = Mathf.Lerp( m_OriginalStartSize * .3f, m_OriginalStartSize, effectSize );
		m_System.startColor = Color.Lerp( Color.black, m_OriginalStartColor, effectSize );
	}
}