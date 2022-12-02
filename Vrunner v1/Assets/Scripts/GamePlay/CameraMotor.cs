using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour {
    public static CameraMotor Instance { get; set; }

    public Transform lookAt;
    public Vector3 offsset = new Vector3(0, -1, -5);
    Vector3 desirePosition;

    public float lookspeed = 3;
    [HideInInspector]
	public bool canback = false;

	public static bool deadzone;

    public float time;
    int totalInputsDeadzone;
    [HideInInspector]
    public int totalHitsInDeadzone;

    private void Awake()
    {
        Instance = this;
		deadzone = false;
    }

    void LateUpdate () {
        Vector3 desiredRotation = lookAt.position - transform.position;
        transform.LookAt(lookAt, Vector3.Lerp(Vector3.forward, desiredRotation, Time.deltaTime));

        StartCoroutine(WaitCameraAnimation());

        #region Tap Mecanic && DeadZone

        if(!deadzone)
        {
            time = 0;
            Deadzone(2);
            totalInputsDeadzone = 0;
            totalHitsInDeadzone = 0;

            GameManager.Instance.pcamera.GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration>().intensity = 0.036f;
        }
        if (transform.localPosition.z >= -4f)
            canback = true;
        if (deadzone)
        {
            time += Time.deltaTime;
            if (MobileInputs.Instance.Tap)
            {
                lookspeed = 1.5f;
                totalInputsDeadzone++;
                transform.Translate(Vector3.Lerp(transform.localPosition, new Vector3(0, 0, -0.5f), Time.deltaTime * 300));

                if (GameManager.Instance.pcamera.GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration>().intensity >= 0.036f)
                    GameManager.Instance.pcamera.GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration>().intensity -= 0.8f * Time.deltaTime * 10;
            }

            if ((time >= 2000f && GameModeBH.modo==1)  || totalHitsInDeadzone >= 1)
                GameManager.Instance.OnDeath();

        }
        if (transform.localPosition.z <= -4.3f && canback || totalInputsDeadzone > 5)
        {
            canback = false;
            Deadzone(2);
        }
        #endregion
    }

    IEnumerator WaitCameraAnimation()
    {
        desirePosition = lookAt.localPosition + offsset;
        desirePosition.x = 0;

        yield return new WaitForSeconds(1);

        transform.localPosition = Vector3.Lerp(transform.localPosition, desirePosition, Time.deltaTime* lookspeed);
    }

    public void Deadzone(int pos)
    {        
        switch (pos)
        {
            case 1:
                offsset = new Vector3(0, 1, -3);
                lookspeed = 5f;
                deadzone = true; break;
            case 2:
                offsset = new Vector3(0, 2, -5);
                lookspeed = 3;
                deadzone = false; break;
        }
    }
}