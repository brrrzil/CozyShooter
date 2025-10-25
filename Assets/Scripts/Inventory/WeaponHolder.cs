using UnityEngine;

public class WeaponHolder : MonoBehaviour
    // Этим должен был управлять скрипт инвентаря, но времени хватило только на это
{
    [SerializeField] GameObject[] armory;

    private void Update()
    {
        // Выбор оружия на цифры 1, 2, 3...
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) EquipWeapon(2);
        //if (Input.GetKeyDown(KeyCode.Alpha1)) EquipWeapon(3;  // Планировался ещё один тип вооружения,
                                                                // которое будет стирать следы первых двух орудий
    }

    private void EquipWeapon(int i)
    {
        foreach (var weapon in armory)
        {
            weapon.gameObject.SetActive(false);
        }

        armory[i - 1].gameObject.SetActive(true);
    }
}