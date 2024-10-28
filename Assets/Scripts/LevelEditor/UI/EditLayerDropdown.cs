using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EditLayerDropdown : MonoBehaviour
{
    [SerializeField] TMP_Dropdown editLayerDropdown;

    private void Start()
    {
        PopulateEditLayerOptions();
        editLayerDropdown.onValueChanged.AddListener(delegate { UpdateEditorLayerOnSelect(); });
        UpdateEditorLayerOnSelect();
    }

    private void PopulateEditLayerOptions()
    {
        if(editLayerDropdown == null)
        {
            editLayerDropdown = GetComponent<TMP_Dropdown>();
        }

        List<TMP_Dropdown.OptionData> drawStyleOptions = new();
        string[] drawStyles = Enum.GetNames(typeof(GridLayers));

        for (int i = 0; i < drawStyles.Length; i++)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = drawStyles[i];
            drawStyleOptions.Add(option);
        }

        editLayerDropdown.options = drawStyleOptions;
    }

    private void UpdateEditorLayerOnSelect()
    {
        string stringyfiedEnumOption = editLayerDropdown.options[editLayerDropdown.value].text;
        if (!Enum.TryParse(stringyfiedEnumOption, out GridLayers enumOption)) { Debug.LogError("Failed to parse dropdown to enum"); return; }
        LevelEditManager.Instance.editLayer = enumOption;
    }
}
