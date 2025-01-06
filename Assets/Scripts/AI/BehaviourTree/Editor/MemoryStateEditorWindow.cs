using UnityEditor;
using UnityEngine;

public class MemoryStateEditorWindow : EditorWindow
{
    private MemoryState _memoryState;
    private string _newKey = "";
    private string _newStringValue = "";
    private int _newIntValue;
    private float _newFloatValue;
    private double _newDoubleValue;
    private bool _newBoolValue;
    private Vector2 _newVector2Value;
    private Vector3 _newVector3Value;
    private MonoBehaviour _newMonoBehaviourValue;
    private Quaternion _newQuaternionValue = Quaternion.identity;
    private Color _newColorValue = Color.white;
    private Transform _newTransformValue;

    private enum DataType { Bool, Int, Float, Double, String, Vector2, Vector3, Quaternion, Color, Transform, MonoBehaviour }
    private DataType _selectedType = DataType.Bool;

    [MenuItem("MeowTools/Memory State Editor")]
    public static void OpenEmptyWindow()
    {
        GetWindow<MemoryStateEditorWindow>("Memory State Editor").Show();
    }
    public static void OpenWindow(MemoryState memoryState)
    {
        var window = GetWindow<MemoryStateEditorWindow>("Memory State Editor");
        window._memoryState = memoryState;
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Memory State Editor", EditorStyles.boldLabel);

        _memoryState = (MemoryState)EditorGUILayout.ObjectField("Memory State", _memoryState, typeof(MemoryState), false);

        if (_memoryState != null)
        {
            GUILayout.Space(10);

            GUILayout.Label("Existing Data", EditorStyles.largeLabel);

            foreach (var key in _memoryState.GetAllData().Keys)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(key, GUILayout.Width(100));
                GUILayout.Label(_memoryState.GetData<object>(key)?.ToString() ?? "null", GUILayout.Width(200));

                if (GUILayout.Button("Remove", GUILayout.Width(80)))
                {
                    _memoryState.RemoveData(key);
                    EditorUtility.SetDirty(_memoryState);
                    AssetDatabase.SaveAssets();
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(10);
            GUILayout.Label("Add New Data", EditorStyles.largeLabel);

            _selectedType = (DataType)EditorGUILayout.EnumPopup("Data Type", _selectedType);

            _newKey = EditorGUILayout.TextField("Key", _newKey);

            switch (_selectedType)
            {
                case DataType.Bool:
                    _newBoolValue = EditorGUILayout.Toggle("Value", _newBoolValue);
                    break;
                case DataType.Int:
                    _newIntValue = EditorGUILayout.IntField("Value", _newIntValue);
                    break;
                case DataType.Float:
                    _newFloatValue = EditorGUILayout.FloatField("Value", _newFloatValue);
                    break;
                case DataType.Double:
                    _newDoubleValue = EditorGUILayout.DoubleField("Value", _newDoubleValue);
                    break;
                case DataType.Vector2:
                    _newVector2Value = EditorGUILayout.Vector2Field("Value", _newVector2Value);
                    break;
                case DataType.Vector3:
                    _newVector3Value = EditorGUILayout.Vector3Field("Value", _newVector3Value);
                    break;
                case DataType.String:
                    _newStringValue = EditorGUILayout.TextField("Value", _newStringValue);
                    break;
                case DataType.Quaternion:
                    Vector4 quaternionVector = new Vector4(_newQuaternionValue.x, _newQuaternionValue.y, _newQuaternionValue.z, _newQuaternionValue.w);
                    quaternionVector = EditorGUILayout.Vector4Field("Value (x, y, z, w)", quaternionVector);
                    _newQuaternionValue = new Quaternion(quaternionVector.x, quaternionVector.y, quaternionVector.z, quaternionVector.w);
                    break;
                case DataType.Color:
                    _newColorValue = EditorGUILayout.ColorField("Value", _newColorValue);
                    break;
                case DataType.Transform:
                    _newTransformValue = (Transform)EditorGUILayout.ObjectField("Value", _newTransformValue, typeof(Transform), true);
                    break;
                case DataType.MonoBehaviour:
                    _newMonoBehaviourValue = (MonoBehaviour)EditorGUILayout.ObjectField("Value", _newMonoBehaviourValue, typeof(MonoBehaviour), true);
                    break;
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Add Data"))
            {
                if (string.IsNullOrEmpty(_newKey))
                    return;

                if (_memoryState.HasData(_newKey))
                {
                    bool overwrite = EditorUtility.DisplayDialog(
                        "Overwrite Data",
                        $"Data with key '{_newKey}' already exists. Do you want to overwrite it?",
                        "Yes",
                        "No"
                    );

                    if (!overwrite)
                        return;
                }

                AddDataToMemoryState();
                ClearInputFields();
                EditorUtility.SetDirty(_memoryState);
                AssetDatabase.SaveAssets();
            }
        }
        else
        {
            GUILayout.Label("Select a Memory State to edit.");
        }
    }

    private void AddDataToMemoryState()
    {
        switch (_selectedType)
        {
            case DataType.Bool:
                _memoryState.SetData(_newKey, _newBoolValue);
                break;
            case DataType.Int:
                _memoryState.SetData(_newKey, _newIntValue);
                break;
            case DataType.Float:
                _memoryState.SetData(_newKey, _newFloatValue);
                break;
            case DataType.Double:
                _memoryState.SetData(_newKey, _newDoubleValue);
                break;
            case DataType.Vector2:
                _memoryState.SetData(_newKey, _newVector2Value);
                break;
            case DataType.Vector3:
                _memoryState.SetData(_newKey, _newVector3Value);
                break;
            case DataType.String:
                _memoryState.SetData(_newKey, _newStringValue);
                break;
            case DataType.Quaternion:
                _memoryState.SetData(_newKey, _newQuaternionValue);
                break;
            case DataType.Color:
                _memoryState.SetData(_newKey, _newColorValue);
                break;
            case DataType.Transform:
                _memoryState.SetData(_newKey, _newTransformValue);
                break;
            case DataType.MonoBehaviour:
                _memoryState.SetData(_newKey, _newMonoBehaviourValue);
                break;
        }
    }
    private void ClearInputFields()
    {
        _newKey = string.Empty;
        _newStringValue = string.Empty;
        _newBoolValue = false;
        _newIntValue = 0;
        _newFloatValue = 0f;
        _newDoubleValue = 0.0;
        _newVector2Value = Vector2.zero;
        _newVector3Value = Vector3.zero;
        _newQuaternionValue = Quaternion.identity;
        _newColorValue = Color.white;
        _newTransformValue = null;
        _newMonoBehaviourValue = null;
    }
}
