using UnityEngine;
using System.Collections;

public class RightArrowButton : MonoBehaviour {
	void OnPress (bool isDown) {
		if (isDown)
			CameraPivot.Instance.rotationDirection = -1f;
		else
			CameraPivot.Instance.rotationDirection = 0f;
	}
}
