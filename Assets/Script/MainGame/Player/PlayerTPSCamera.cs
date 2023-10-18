using UnityEngine;

/**
 * <summary>
 * TPSカメラコントローラークラス。
 * プレイヤーを追従し、マウスやアナログスティックの動きに応じてカメラを回転させる機能。
 * プレイヤーはカメラの方向を向くように調整する機能。
 * </summary>
 * 制作者: 渡邊
 */
public class PlayerTPSCamera : MonoBehaviour
{
    /* --- シリアライズフィールド一覧 --- */

    [SerializeField] private Vector3 offset; // プレイヤーからのオフセット
    [SerializeField] private float sensitivity = 5.0f; // カメラの感度
    [SerializeField] private float rotationSmoothTime = 0.1f; // カメラの回転のスムージング時間
    [SerializeField] private float playerRotateSpeed = 5.0f; // プレイヤーの回転速度
    [SerializeField] private LayerMask collisionLayers; // カメラの衝突を検知するレイヤー

    /* --- プライベートフィールド --- */
    private Transform playerTransform; // プレイヤーのTransform
    private float yaw; // 横方向の回転角
    private float pitch; // 縦方向の回転角
    private Vector3 rotationSmoothVelocity; // スムージング用の速度
    private Vector3 currentRotation; // 現在の回転角

    private void Start()
    {
        // Playerタグを持つオブジェクトを検索して、そのTransformを取得
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        // カメラの回転処理
        HandleCameraRotation();
        // プレイヤーの向きを調整する処理
        HandlePlayerRotation();
        // カメラの衝突処理
        HandleCameraCollision();
    }

    /// <summary>
    /// カメラの回転を処理
    /// </summary>
    private void HandleCameraRotation()
    {
        // マウス入力に基づく角度の計算
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, -40, 85); // 縦の角度の制限

        // スムージングを用いてカメラの回転を更新
        Vector3 targetRotation = new Vector3(pitch, yaw);
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        // カメラの位置を更新
        transform.position = playerTransform.position - transform.forward * offset.magnitude + offset.y * transform.up;
    }

    /// <summary>
    /// プレイヤーがカメラの方向に向くように調整
    /// </summary>
    private void HandlePlayerRotation()
    {
        Vector3 lookDirection = new Vector3(transform.forward.x, 0, transform.forward.z);
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, lookRotation, Time.deltaTime * playerRotateSpeed);
    }

    /// <summary>
    /// カメラと他のオブジェクトとの衝突を処理
    /// </summary>
    private void HandleCameraCollision()
    {
        RaycastHit hit;
        // カメラが何かしらのオブジェクトに接触している場合、カメラをプレイヤーに近づける
        if (Physics.Raycast(playerTransform.position, -transform.forward, out hit, offset.magnitude, collisionLayers))
        {
            transform.position = hit.point + hit.normal * 0.5f; // 少しオフセットを加えてカメラを引く
        }
    }
}