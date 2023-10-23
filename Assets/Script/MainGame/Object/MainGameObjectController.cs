using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// さまざまなタイプのオブジェクトの動作を制御するクラス。
/// </summary>
public class MainGameObjectController : MonoBehaviour
{
    /// <summary>
    /// オブジェクトのタイプの列挙体。
    /// </summary>
    public enum ObjectType
    {
        MovingFloor, // 移動する床
        Coin,        // コイン
        GoalMedal,   // ゴール用のメダル
        Pipe         // 土管
    }

    // 共通のパラメーター
    public float speed; // 速度

    [Header("基本設定")]

    /// <summary>
    /// このオブジェクトのタイプ。
    /// </summary>
    public ObjectType type;

    /// <summary>
    /// 移動床として動作する際のパスリスト。
    /// </summary>
    public List<Path> paths; // 移動する床のパスのリスト
    private int currentPathIndex = 0; // 現在のパスのインデックス
    private bool isReturning = false; // パスを往復するかどうか

    // コインのためのパラメーター
    public float rotationSpeed; // 回転速度

    // 土管のためのパラメーター
    public Transform entry; // 入口
    public Transform exit;  // 出口
    public bool isBidirectional; // 両方向に移動可能かどうか

    public void Update()
    {
        switch (type)
        {
            case ObjectType.MovingFloor:
                HandleMovingFloor();
                break;

            case ObjectType.Coin:
                HandleCoin();
                break;
        }
    }

    /// <summary>
    /// 移動する床の動作を制御するメソッド。
    /// </summary>
    private void HandleMovingFloor()
    {
        if (paths.Count <= 0) return;

        transform.position = Vector3.MoveTowards(transform.position, paths[currentPathIndex].position, speed * Time.deltaTime);

        if (transform.position == paths[currentPathIndex].position)
        {
            if (isReturning)
            {
                currentPathIndex--;
                if (currentPathIndex < 0)
                {
                    isReturning = false;
                    currentPathIndex = 0;
                }
            }
            else
            {
                currentPathIndex++;
                if (currentPathIndex >= paths.Count)
                {
                    isReturning = true;
                    currentPathIndex = paths.Count - 1;
                }
            }
        }
    }

    /// <summary>
    /// コインの動作を制御するメソッド。
    /// </summary>
    private void HandleCoin()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (type == ObjectType.Coin)
            {
                // コイン取得の処理
                Destroy(gameObject);
            }
            else if (type == ObjectType.GoalMedal)
            {
                // ゴール時の処理
                Debug.Log("Goal!");
            }
        }
    }
}

[System.Serializable]
public class Path
{
    public Vector3 position; // 位置情報
    public Quaternion rotation; // 回転情報
    public float speed = 1.0f;  // このポイントでの速度
    public float delay = 0.0f; // このポイントでの停止時間
    public Vector3 startPoint = Vector3.zero; // パスの始点
    public Vector3 endPoint = Vector3.zero;   // パスの終点
    public List<Vector3> points = new List<Vector3>();
}
