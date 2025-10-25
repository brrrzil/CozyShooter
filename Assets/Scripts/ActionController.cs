using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField, Range(1, 100)] private float zoomFoV;

    [SerializeField] private GameObject crosshair;
    [SerializeField] Sprite crosshair32, crosshair64, crosshair128, crosshair256;

    [SerializeField] private GameObject weaponHolder;
    private static IWeapon activeWeapon;

    // Отметки зон изменения размера прицела
    // Нужно для оружия "Травосад", чтобы менять размер кисти (само оружие в разработке)
    [Header("Метки на шкале вариативности")]
    [SerializeField] private float minCrosshair;
    [SerializeField] private float mark2;
    [SerializeField] private float mark3;
    [SerializeField] private float mark4;
    [SerializeField] private float mark5;
    [SerializeField] private float mark6;
    [SerializeField] private float mark7;
    [SerializeField] private float maxCrosshair;

    private float defaultFoV;
    float crosshairScale;

    void Start()
    {
        defaultFoV = virtualCamera.m_Lens.FieldOfView;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            activeWeapon = weaponHolder.GetComponentInChildren<IWeapon>();
            activeWeapon.Shoot();
        }

        if (Input.GetMouseButton(1)) // Прицеливание реализовано в соответствии с ТЗ
        {
            PlayerMovement.SetAimSlowdown(0.25f);
            virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, zoomFoV, 0.1f);
        }

        if (!Input.GetMouseButton(1))
        {
            PlayerMovement.SetAimSlowdown(1);
            virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, defaultFoV, 0.2f);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0) // Управление размером прицела
                                                    // Нужно для изменения размера кисти оружия "Травосад"
        {
            MouseScrollUp();
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0) 
        {
            MouseScrollDown();
        }
    }

    private void MouseScrollUp()
    {
        crosshairScale = crosshair.GetComponent<RectTransform>().sizeDelta.x;

        if (crosshairScale < maxCrosshair)
        {
            crosshairScale += 10;
            crosshair.GetComponent<RectTransform>().sizeDelta = new Vector2(crosshairScale, crosshairScale);
        }

        // Спрайты с разной толщиной линии для более комфортного оттображения прицела в разных размерах
        if (crosshairScale > mark3) crosshair.GetComponent<Image>().sprite = crosshair64;
        if (crosshairScale > mark5) crosshair.GetComponent<Image>().sprite = crosshair128;
        if (crosshairScale > mark7) crosshair.GetComponent<Image>().sprite = crosshair256;
    }

    private void MouseScrollDown()
    {
        crosshairScale = crosshair.GetComponent<RectTransform>().sizeDelta.x;

        if (crosshairScale > minCrosshair)
        {
            crosshairScale -= 10;
            crosshair.GetComponent<RectTransform>().sizeDelta = new Vector2(crosshairScale, crosshairScale);
        }

        if (this.crosshairScale < mark6) crosshair.GetComponent<Image>().sprite = crosshair128;
        if (this.crosshairScale < mark4) crosshair.GetComponent<Image>().sprite = crosshair64;
        if (this.crosshairScale < mark2) crosshair.GetComponent<Image>().sprite = crosshair32;
    }
}