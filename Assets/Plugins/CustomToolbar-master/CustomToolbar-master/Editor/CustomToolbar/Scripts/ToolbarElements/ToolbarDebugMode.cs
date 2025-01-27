using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityToolbarExtender;

[Serializable]
internal class ToolbarDebugMode : BaseToolbarElement
{
    int selectedDebugMode;

    string[] debugModeOptions =
    {
        "Debug Mode OFF",
        "Debug Mode ON",
    };

    public override string NameInList => "[Dropdown] Debug Mode Options";

    private static ScriptableObject _settings;

    public static ScriptableObject Instance
    {
        get
        {
            if (_settings == null)
                _settings = Resources.Load("OXO/Game Settings") as ScriptableObject;
            return _settings;
        }
    }

    public override void Init()
    {
        if (Instance != null)
        {
            var type = Instance.GetType();
            var propertyInfo = type.GetProperty("IsDebugModeActive",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);

            if (propertyInfo != null)
            {
                bool debugModeValue = (bool)propertyInfo.GetValue(Instance);
                selectedDebugMode = debugModeValue ? 1 : 0;
            }
            else
            {
                Debug.Log("Property doesn't exist.");
            }
        }
    }

    protected override void OnDrawInList(Rect position)
    {
    }

   protected override void OnDrawInToolbar()
{
	EditorGUI.BeginChangeCheck();
	selectedDebugMode = EditorGUILayout.Popup(selectedDebugMode, debugModeOptions, GUILayout.Width(WidthInToolbar * 1.2f));

	if (EditorGUI.EndChangeCheck())
	{
		if (Instance != null)
		{
			var type = Instance.GetType();
			var propertyInfo = type.GetProperty("IsDebugModeActive",
				BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
			if (propertyInfo != null)
			{
				propertyInfo.SetValue(Instance, selectedDebugMode == 1);
				EditorUtility.SetDirty(Instance);
			}
			else
			{
				Debug.Log("Property doesn't exist.");
			}
		}

		Debug.Log("Debug Mode changed to: " + debugModeOptions[selectedDebugMode]);
		switch (debugModeOptions[selectedDebugMode])
		{
			case "Debug Mode OFF":
				// Your custom logic here
				break;
		}
	}
}
}