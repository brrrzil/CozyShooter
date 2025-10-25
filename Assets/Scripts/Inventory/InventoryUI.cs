using UnityEngine;

public class InventoryUI : MonoBehaviour
    // Здесь будет управление панелью инвентаря (появление, скрытие и т.д.)
{
    [SerializeField] private GameObject inventoryPanel;
    private static bool isInventoryActive = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) InventoryAppearance();
    }

    // Появление панели инвентаря
    private void InventoryAppearance()
    {
        if (!isInventoryActive)
        {
            inventoryPanel.SetActive(true);
            isInventoryActive = true;
        }

        else
        {
            inventoryPanel.SetActive(false);
            isInventoryActive = false;
        }
    }
}