using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EditUnitSelectionDropdown : MonoBehaviour
{
    [SerializeField] TMP_Dropdown unitSelectionDropdown;

    private void Start()
    {
        PopulateUnitSelectionOptions();
        unitSelectionDropdown.onValueChanged.AddListener(delegate { UpdateEditorSelectedUnitData(); });
        UpdateEditorSelectedUnitData();
    }

    private void PopulateUnitSelectionOptions()
    {
        if (unitSelectionDropdown == null)
        {
            unitSelectionDropdown = GetComponent<TMP_Dropdown>();
        }

        List<TMP_Dropdown.OptionData> unitOptions = new();
        UnitData[] unitDataPrefabs = Resources.LoadAll<UnitData>(Paths.UnitDataFolderPath);

        for (int i = 0; i < unitDataPrefabs.Length; i++)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = unitDataPrefabs[i].unitName;
            unitOptions.Add(option);
        }

        unitSelectionDropdown.options = unitOptions;
    }

    private void UpdateEditorSelectedUnitData()
    {
        EditAdditionManager.Instance.selectedUnitIndex = unitSelectionDropdown.value;
    }
}
