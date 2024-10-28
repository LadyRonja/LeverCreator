using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EditModesDropdown : MonoBehaviour
{
    [SerializeField] TMP_Dropdown editModesDropdown;

    private void Start()
    {
        PopulateEditModesDropdown();
        editModesDropdown.onValueChanged.AddListener(delegate { UpdateEditorModeOnSelect(); });
        UpdateEditorModeOnSelect();
    }

    private void PopulateEditModesDropdown()
    {
        if (editModesDropdown == null)
        {
            editModesDropdown = GetComponent<TMP_Dropdown>();
        }

        List<TMP_Dropdown.OptionData> editStyleOptions = new();
        string[] drawStyles = Enum.GetNames(typeof(ClickMode));

        for (int i = 0; i < drawStyles.Length; i++)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = drawStyles[i];
            editStyleOptions.Add(option);
        }

        editModesDropdown.options = editStyleOptions;
    }

    private void UpdateEditorModeOnSelect()
    {
        string stringyfiedEnumOption = editModesDropdown.options[editModesDropdown.value].text;
        if(!Enum.TryParse(stringyfiedEnumOption, out ClickMode enumOption)) { Debug.LogError("Failed to parse dropdown to enum"); return; }
        LevelEditManager.Instance.editMethod = enumOption;
    }

}
