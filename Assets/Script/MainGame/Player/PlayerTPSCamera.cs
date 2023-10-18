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
    [SerializeField] private Vector3 initialPositionOffset; // 初期位置のオフセット
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

        // カーソルを非表示にし、ゲーム中に動かないように固定
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 初期カメラ位置の設定
        transform.position = playerTransform.position + initialPositionOffset;

        // カメラの衝突処理を実行して、カメラがオブジェクトにめり込んでいないか確認
        HandleCameraCollision();
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
        transform.position = AdjustCameraPositionForLookingUp();
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

        // カメラからプレイヤーへのベクトルを計算
        Vector3 dirFromPlayerToCamera = transform.position - playerTransform.position;
        float distance = offset.magnitude; // カメラとプレイヤーとの間の距離

        // プレイヤーからカメラ方向にレイを飛ばし、衝突を検出
        if (Physics.Raycast(playerTransform.position, dirFromPlayerToCamera.normalized, out hit, distance, collisionLayers))
        {
            // プレイヤーが非常に近くにいる場合、ズーム効果は適用しない
            if (hit.distance > 1f)
            {
                transform.position = hit.point;
            }
        }
        else
        {
            // 衝突がない場合は、オリジナルのカメラ位置を維持
            transform.position = playerTransform.position + dirFromPlayerToCamera.normalized * distance;
        }
    }

    // カメラの位置を真上向き時に調整するためのメソッド
    private Vector3 AdjustCameraPositionForLookingUp()
    {
        // pitchが65以上（真上に近い）の場合、カメラの位置を近づける
        if (pitch > 65f)
        {
            float adjustmentFactor = 1.0f - (pitch - 65f) / 20f;  // pitchが65〜85の範囲で0から1に変化するファクター
            return playerTransform.position - transform.forward * offset.magnitude * adjustmentFactor + offset.y * transform.up;
        }
        return playerTransform.position - transform.forward * offset.magnitude + offset.y * transform.up;
    }
}