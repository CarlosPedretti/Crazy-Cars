using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FiringBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {

        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward);

        transform.position = new Vector3(transform.position.x, transform.parent.position.y + 2.5f, transform.position.z);
    }

    public void SetHeat(float firingHeat)
    {
        slider.value = firingHeat;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetMaxHeat(float firingHeat)
    {
        slider.maxValue = firingHeat;
        slider.value = firingHeat;
        fill.color = gradient.Evaluate(1f);
    }
}