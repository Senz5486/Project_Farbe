using UnityEngine;
using UnityEditor;

/// <summary>
/// MainGameObjectController��Editor�g���N���X�B
/// �C���X�y�N�^��ł̑����Scene View�ł̕\�����J�X�^�}�C�Y����B
/// �܂��ATools���j���[����p�X�G�f�B�^�E�B���h�E���J���@�\���񋟂���B
/// </summary>
[CustomEditor(typeof(MainGameObjectController))]
public class MainGameObjectControllerEditor :  Editor
{
    /// <summary>
    /// �ΏۂƂȂ�MainGameObjectController�ւ̎Q�ƁB
    /// </summary>
    private MainGameObjectController _controller;
    /// <summary>
    /// �����ݒ�B
    /// </summary>
    private void OnEnable()
    {
        _controller = (MainGameObjectController)target;
    }

    /// <summary>
    /// �C���X�y�N�^��ł̕\�����J�X�^�}�C�Y�B
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (_controller.type == MainGameObjectController.ObjectType.MovingFloor)
        {
            for (int i = 0; i < _controller.paths.Count; i++)
            {
                var path = _controller.paths[i];
                for (int j = 0; j < path.points.Count; j++)
                {
                    path.points[j] = EditorGUILayout.Vector3Field($"Point {j + 1}", path.points[j]);
                }

                if (GUILayout.Button($"Add Point to Path {i + 1}"))
                {
                    path.points.Add(Vector3.zero);
                }

                if (path.points.Count > 0 && GUILayout.Button($"Remove Last Point from Path {i + 1}"))
                {
                    path.points.RemoveAt(path.points.Count - 1);
                }
            }
        }
    }

    /// <summary>
    /// Scene View��ł̕\���⑀����J�X�^�}�C�Y�B
    /// </summary>
    private void OnSceneGUI()
    {
        for (int i = 0; i < _controller.paths.Count; i++)
        {
            var path = _controller.paths[i];
            for (int j = 0; j < path.points.Count; j++)
            {
                EditorGUI.BeginChangeCheck();
                var newPosition = Handles.PositionHandle(path.points[j], Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_controller, "Move Path Point");
                    path.points[j] = newPosition;
                    _controller.Update();
                }
            }
        }
    }
}