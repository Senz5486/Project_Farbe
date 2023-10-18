using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// ゲームのセーブデータを管理するクラス
/// 制作者: 渡邊
/// </summary>
public class CommonUtilsSaveSystem : MonoBehaviour
{
    /// <summary>
    /// Singletonインスタンス
    /// </summary>
    public static CommonUtilsSaveSystem Instance { get; private set; }

    /// <summary>
    /// セーブデータの暗号化キー (16文字指定)
    /// </summary>
    private static readonly byte[] encryptionKey = Encoding.UTF8.GetBytes(".T)!Ss)U#E32U5md");
    /// <summary>
    /// セーブ保存ファイル
    /// </summary>
    private const string filePath = "/save";
    /// <summary>
    /// ゲームデータのセーブスロット数
    /// </summary>
    private const int NumSaveSlots = 3;

    /// <summary>
    /// ゲーム設定
    /// </summary>
    public CommonUtilsGameSettings Settings { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // フォルダーが存在しない場合、作成する
        string folderPath = Application.persistentDataPath + filePath;
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
    }

    private string GetSavePath(int slot)
    {
        return Application.persistentDataPath + filePath + $"/savegame{slot}.dat";
    }

    // 設定を保存するメソッド
    public void SaveSettings(CommonUtilsGameSettings settings)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + filePath + "/gamesettings.dat";

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, settings);
        }
    }

    // 設定をロードするメソッド
    public CommonUtilsGameSettings LoadSettings()
    {
        string path = Application.persistentDataPath + filePath + "/gamesettings.dat";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                CommonUtilsGameSettings settings = (CommonUtilsGameSettings)formatter.Deserialize(stream);
                return settings;
            }
        }
        else
        {
            // ファイルが存在しない場合の処理
            CommonUtilsGameSettings newdata = new CommonUtilsGameSettings();
            SaveSettings(newdata);
            return newdata;
        }
    }
    /// <summary>
    /// 新しいゲームデータを指定したセーブスロットに作成し、保存する
    /// </summary>
    /// <param name="slot">セーブスロット(1-3)</param>
    public void CreateNewGameData(int slot)
    {
        if (slot < 1 || slot > NumSaveSlots)
        {
            CommonUtilsDebugUISystem.Log(CommonUtilsMessageConstants.ERROR_SAVE_SLOTINVALID);
            return;
        }

        // 新しいゲームデータを作成（ここでは例として空のゲームデータを作成）
        CommonUtilsGameSaveData newGameData = new CommonUtilsGameSaveData();

        // 作成したゲームデータを指定したスロットに保存
        SaveGameData(slot, newGameData);
    }

    /// <summary>
    /// ゲームデータを指定したスロットに保存する
    /// </summary>
    /// <param name="slot">セーブスロット(1-3)</param>
    /// <param name="gameData">保存するゲームデータ</param>
    public void SaveGameData(int slot, CommonUtilsGameSaveData gameData)
    {
        if (slot < 1 || slot > NumSaveSlots)
        {
            CommonUtilsDebugUISystem.Log(CommonUtilsMessageConstants.ERROR_SAVE_SLOTINVALID);
            return;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        string path = GetSavePath(slot);

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            byte[] data = ObjectToByteArray(gameData);
            byte[] encryptedData = Encrypt(data);
            formatter.Serialize(stream, encryptedData);
        }
    }

    /// <summary>
    /// ゲームデータを指定したスロットからロードする
    /// </summary>
    /// <param name="slot">ロードするセーブスロット(1-3)</param>
    public CommonUtilsGameSaveData LoadGameData(int slot)
    {
        if (slot < 1 || slot > NumSaveSlots)
        {
            CommonUtilsDebugUISystem.Log(CommonUtilsMessageConstants.ERROR_SAVE_SLOTINVALID); //スロット番号が無効な時のメッセージを表示
            return null;
        }

        string path = GetSavePath(slot);

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    byte[] encryptedData = (byte[])formatter.Deserialize(stream);
                    byte[] decryptedData = Decrypt(encryptedData);

                    CommonUtilsGameSaveData gameData = (CommonUtilsGameSaveData)ByteArrayToObject(decryptedData);
                    return gameData;
                }
            }
            catch //セーブデータが読み込めなかった時の処理
            {
                CommonUtilsDebugUISystem.Log(CommonUtilsMessageConstants.ERROR_SAVE_DATABROKEN); //セーブデータ破損のメッセージを表示
                return null;
            }
        }
        else
        {
            CommonUtilsDebugUISystem.Log(CommonUtilsMessageConstants.ERROR_SAVE_DATANOTEXIST); //セーブデータが無いときの初期を表示
            return null;
        }
    }

    /// <summary>
    /// 指定したスロットのセーブデータを削除する
    /// </summary>
    /// <param name="slot">削除するセーブスロット(1-3)</param>
    public void DeleteSaveData(int slot)
    {
        if (slot < 1 || slot > NumSaveSlots)
        {
            CommonUtilsDebugUISystem.Log(CommonUtilsMessageConstants.ERROR_SAVE_SLOTINVALID);
            return;
        }

        string path = GetSavePath(slot);

        if (File.Exists(path))
        {
            File.Delete(path);
        }
        else
        {
            CommonUtilsDebugUISystem.Log(CommonUtilsMessageConstants.ERROR_SAVE_DATANOTEXIST);
        }
    }


    // 暗号化ロジック
    private byte[] Encrypt(byte[] data)
    {
        using (AesManaged aes = new AesManaged())
        {
            ICryptoTransform encryptor = aes.CreateEncryptor(encryptionKey, aes.IV);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                }
                return ms.ToArray();
            }
        }
    }

    // 復号化ロジック
    private byte[] Decrypt(byte[] data)
    {
        using (AesManaged aes = new AesManaged())
        {
            ICryptoTransform decryptor = aes.CreateDecryptor(encryptionKey, aes.IV);
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    byte[] decryptedData = new byte[data.Length];
                    cs.Read(decryptedData, 0, decryptedData.Length);
                    return decryptedData;
                }
            }
        }
    }

    // オブジェクトをバイト配列に変換
    private byte[] ObjectToByteArray(object obj)
    {
        if (obj == null)
            return null;
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    // バイト配列をオブジェクトに変換
    private object ByteArrayToObject(byte[] arrBytes)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            ms.Write(arrBytes, 0, arrBytes.Length);
            ms.Seek(0, SeekOrigin.Begin);
            BinaryFormatter bf = new BinaryFormatter();
            return bf.Deserialize(ms);
        }
    }
}