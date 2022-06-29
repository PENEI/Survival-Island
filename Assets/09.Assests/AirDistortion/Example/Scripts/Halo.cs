using UnityEngine;
using System.Collections;

public class Halo : MonoBehaviour {

	Vector3 _defaultPos;
	public float offset = 0;

	void Awake() {
		_defaultPos = transform.position;
	}
	
	void Update () {
		Vector3 toCamVector = Vector3.Normalize (Camera.main.transform.position - _defaultPos);
		Vector3 pos = _defaultPos + toCamVector * offset;
		transform.position = pos;
	}
}
