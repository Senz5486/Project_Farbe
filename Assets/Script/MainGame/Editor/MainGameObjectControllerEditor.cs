using UnityEngine;
using UnityEditor;

/// <summary>
/// MainGameObjectControllerのEditor拡張クラス。
/// インスペクタ上での操作やScene Viewでの表示をカスタマイズする。
/// また、Toolsメニューからパスエディタウィンドウを開く機能も提供する。
/// </summary>
[CustomEditor(typeof(MainGameObjectController))]
public class MainGameObjectControllerEditor :  Editor
{
    /// <summary>
    /// 対象となるMainGameObjectControllerへの参照。
    /// </summary>
    private MainGameObjectController _controller;
    /// <summary>
    /// 初期設定。
    /// </summary>
    private void OnEnable()
    {
        _controller = (MainGameObjectController)target;
    }

    /// <summary>
    /// インスペクタ上での表示をカスタマイズ。
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
    /// Scene View上での表示や操作をカスタマイズ。
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