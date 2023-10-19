using UnityEngine;

/**
 * <summary>
 * シンプルなキャラクターコントローラークラス。
 * 基本的な移動、ジャンプ、ダッシュ、二段ジャンプ、急斜面での滑りの機能クラス
 * </summary>
 * 制作者: 渡邊
 */
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    /* --- シリアライズフィールド一覧 --- */

    [SerializeField] private float speed = 5.0f;            // キャラクターの移動速度
    [SerializeField] private float jumpHeight = 2.0f;       // ジャンプの高さ
    [SerializeField] private float gravity = 9.81f;         // 重力の強さ
    [SerializeField] private LayerMask groundLayer;         // 地面レイヤー
    [SerializeField] private float slopeLimit = 45.0f;      // どれくらいの傾斜までなら上れるかの制限
    [SerializeField] private float slideSpeed = 3.0f;       // 急斜面での滑る速度
    [SerializeField] private float dashSpeedMultiplier = 2.0f; // ダッシュ時の速度倍率
    [SerializeField] private float maxDashTime = 5.0f;      // ダッシュできる最大時間
    [SerializeField] private float dashRecoveryRate = 1.0f; // ダッシュ時間の回復速度
    [SerializeField] private int maxJumpCount = 2;          // 最大ジャンプ回数 (二段ジャンプ用)

    /* --- プライベートフィールド --- */

    private Vector3 velocity;                              // 現在のキャラクターの速度
    private bool isGrounded;                               // キャラクターが地面に接触しているか
    private int currentJumpCount = 0;                      // 現在のジャンプ回数
    [SerializeField]private float currentDashTime;                         // 現在のダッシュ残り時間
    private CharacterController controller;                // キャラクターコントローラーのリファレンス
    private Animator playerAnimator;                       // アニメーションを制御するためのAnimator
    [SerializeField] private bool canDash = true;                           // ダッシュが可能かを示すブール変数

    /* --- カプセル化 --- */

    public float Speed { get => speed; set => speed = value; }
    public float JumpHeight { get => jumpHeight; set => jumpHeight = value; }
    public float Gravity { get => gravity; set => gravity = value; }

    private void Start()
    {
        controller = this.GetComponent<CharacterController>();
        currentDashTime = maxDashTime;
        // Animatorコンポーネントの取得
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        //キャラクターの基本的な移動処理
        HandleMovement();
        //ジャンプと二段ジャンプ処理
        HandleJumping();
        //ダッシュ処理
        HandleDashing();
        //急斜面での滑りを処理
        HandleSliding();
        // アニメーションの状態を更新
        UpdateAnimations();
    }

    /// <summary>
    /// キャラクターの基本的な移動処理
    /// </summary>
    private void HandleMovement()
    {
        // 地面に接しているかの判定
        isGrounded = controller.isGrounded;
        // 地面に接しているときの速度を初期化
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
            // 地面に触れたらジャンプカウントをリセット
            currentJumpCount = 0;
        }

        // キーボードの入力を取得し、それを基にキャラクターを移動
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);
    }

    /// <summary>
    /// ジャンプと二段ジャンプ処理
    /// </summary>
    private void HandleJumping()
    {
        // ジャンプボタンが押され、ジャンプの回数制限内であればジャンプ
        if (Input.GetButtonDown("Jump") && currentJumpCount < maxJumpCount)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
            currentJumpCount++;
        }

        // 重力を加えてキャラクターを落下させる
        velocity.y -= gravity * Time.deltaTime;

        // 計算された速度でキャラクターを移動
        controller.Move(velocity * Time.deltaTime);
    }

    /// <summary>
    /// ダッシュ処理
    /// </summary>
    private void HandleDashing()
    {
        if (canDash && currentDashTime <= 0)
        {
            canDash = false; // ダッシュの時間が0になったので、次のダッシュが不可能になる
        }
        if (!canDash && currentDashTime >= maxDashTime)
        {
            canDash = true; // ダッシュの時間が最大に達したので、再びダッシュが可能になる
        }
        // ダッシュキーが押されていて、ダッシュの残り時間がある場合、そしてダッシュが可能な場合、ダッシュ
        if (Input.GetKey(KeyCode.LeftShift) && currentDashTime > 0 && canDash)
        {
            Vector3 dashDirection = transform.forward * dashSpeedMultiplier;
            controller.Move(dashDirection * Time.deltaTime);
            currentDashTime -= Time.deltaTime;
 
        }
        else
        {
            // ダッシュ時間を回復
            currentDashTime += dashRecoveryRate * Time.deltaTime;
        }
        currentDashTime = Mathf.Clamp(currentDashTime, 0, maxDashTime);
    }

    /// <summary>
    /// 急斜面での滑りを処理
    /// </summary>
    private void HandleSliding()
    {
        // キャラクターが急斜面にいる場合、滑る
        if (IsOnSteepSlope() && !controller.isGrounded)
        {
            Vector3 slideDirection = Vector3.down * slideSpeed;
            controller.Move(slideDirection * Time.deltaTime);
        }
    }

    /// <summary>
    /// キャラクターが急斜面にいるかの判定
    /// </summary>
    /// <returns>急斜面にいる場合はtrue、それ以外はfalse。</returns>
    private bool IsOnSteepSlope()
    {
        // 地面に接している場合、急斜面判定を行う
        if (isGrounded)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f, groundLayer))
            {
                float angle = Vector3.Angle(hit.normal, Vector3.up);
                return angle > slopeLimit;
            }
        }
        return false;
    }
    /// <summary>
    /// アニメーションの状態を更新するメソッド
    /// </summary>
    private void UpdateAnimations()
    {
        // IsRunを設定
        bool isRunning = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
        playerAnimator.SetBool("IsRun", isRunning);

        // IsDashを設定
        bool isDashing = Input.GetKey(KeyCode.LeftShift) && canDash;
        playerAnimator.SetBool("IsDash", isDashing);
    }
}