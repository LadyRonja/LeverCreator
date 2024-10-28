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

        List<TMP_Dropdown.OptionData> drawStyleOptions = new();
        string[] drawStyles = Enum.GetNames(typeof(ClickMode));

        for (int i = 0; i < drawStyles.Length; i++)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = drawStyles[i];
            drawStyleOptions.Add(option);
        }

        editModesDropdown.options = drawStyleOptions;
    }

    private void UpdateEditorModeOnSelect()
    {
        string stringyfiedEnumOption = editModesDropdown.options[editModesDropdown.value].text;
        if(!Enum.TryParse(stringyfiedEnumOption, out ClickMode enumOption)) { Debug.LogError("Failed to parse dropdown to enum"); return; }
        LevelEditManager.Instance.editMethod = enumOption;
    }

}
