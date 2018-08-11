using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseGender : MonoBehaviour {

	void FixedUpdate () {
		if (Input.GetMouseButtonDown (0)) {
			GameObject camera = GameObject.Find("Main Camera");

			Vector2 origin = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
				Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
			
			RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero, 0f);
			if (hit) {
				GameObject.FindGameObjectWithTag("QuestionManager").GetComponent<QuestionManager> ().gender = hit.transform.gameObject.name;
			}
		}
	}
}
