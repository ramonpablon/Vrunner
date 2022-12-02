using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorMutBar : MonoBehaviour {

    public static ColorMutBar Instance { get; set; }

    private float pingPong;

    public Color32 color1, color2;

    Color alpha;

	// Use this for initialization
	void Awake () {
        Instance = this;
	}

    public void Colorbar(float speed)
    {
        pingPong = Mathf.PingPong(Time.time, speed);

        alpha = Color32.Lerp(color1, color2, pingPong);

        gameObject. GetComponent<Image>().color = alpha;
    }
}
