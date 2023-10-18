using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// �Q�[���̃Z�[�u�f�[�^���Ǘ�����N���X
/// �����: �n�
/// </summary>
public class CommonUtilsSaveSystem : MonoBehaviour
{
    /// <summary>
    /// Singleton�C���X�^���X
    /// </summary>
    public static CommonUtilsSaveSystem Instance { get; private set; }

    /// <summary>
    /// �Z�[�u�f�[�^�̈Í����L�[ (16�����w��)
    /// </summary>
    private static readonly byte[] encryptionKey = Encoding.UTF8.GetBytes(".T)!Ss)U#E32U5md");
    /// <summary>
    /// �Z�[�u�ۑ��t�@�C��
    /// </summary>
    private const string filePath = "/save";
    /// <summary>
    /// �Q�[���f�[�^�̃Z�[�u�X���b�g��
    /// </summary>
    private const int NumSaveSlots = 3;

    /// <summary>
    /// �Q�[���ݒ�
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

        // �t�H���_�[�����݂��Ȃ��ꍇ�A�쐬����
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

    // �ݒ��ۑ����郁�\�b�h
    public void SaveSettings(CommonUtilsGameSettings settings)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + filePath + "/gamesettings.dat";

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, settings);
        }
    }

    // �ݒ�����[�h���郁�\�b�h
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
            // �t�@�C�������݂��Ȃ��ꍇ�̏���
            CommonUtilsGameSettings newdata = new CommonUtilsGameSettings();
            SaveSettings(newdata);
            return newdata;
        }
    }
    /// <summary>
    /// �V�����Q�[���f�[�^���w�肵���Z�[�u�X���b�g�ɍ쐬���A�ۑ�����
    /// </summary>
    /// <param name="slot">�Z�[�u�X���b�g(1-3)</param>
    public void CreateNewGameData(int slot)
    {
        if (slot < 1 || slot > NumSaveSlots)
        {
            CommonUtilsDebugUISystem.Log(CommonUtilsMessageConstants.ERROR_SAVE_SLOTINVALID);
            return;
        }

        // �V�����Q�[���f�[�^���쐬�i�����ł͗�Ƃ��ċ�̃Q�[���f�[�^���쐬�j
        CommonUtilsGameSaveData newGameData = new CommonUtilsGameSaveData();

        // �쐬�����Q�[���f�[�^���w�肵���X���b�g�ɕۑ�
        SaveGameData(slot, newGameData);
    }

    /// <summary>
    /// �Q�[���f�[�^���w�肵���X���b�g�ɕۑ�����
    /// </summary>
    /// <param name="slot">�Z�[�u�X���b�g(1-3)</param>
    /// <param name="gameData">�ۑ�����Q�[���f�[�^</param>
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
    /// �Q�[���f�[�^���w�肵���X���b�g���烍�[�h����
    /// </summary>
    /// <param name="slot">���[�h����Z�[�u�X���b�g(1-3)</param>
    public CommonUtilsGameSaveData LoadGameData(int slot)
    {
        if (slot < 1 || slot > NumSaveSlots)
        {
            CommonUtilsDebugUISystem.Log(CommonUtilsMessageConstants.ERROR_SAVE_SLOTINVALID); //�X���b�g�ԍ��������Ȏ��̃��b�Z�[�W��\��
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
            catch //�Z�[�u�f�[�^���ǂݍ��߂Ȃ��������̏���
            {
                CommonUtilsDebugUISystem.Log(CommonUtilsMessageConstants.ERROR_SAVE_DATABROKEN); //�Z�[�u�f�[�^�j���̃��b�Z�[�W��\��
                return null;
            }
        }
        else
        {
            CommonUtilsDebugUISystem.Log(CommonUtilsMessageConstants.ERROR_SAVE_DATANOTEXIST); //�Z�[�u�f�[�^�������Ƃ��̏�����\��
            return null;
        }
    }

    /// <summary>
    /// �w�肵���X���b�g�̃Z�[�u�f�[�^���폜����
    /// </summary>
    /// <param name="slot">�폜����Z�[�u�X���b�g(1-3)</param>
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


    // �Í������W�b�N
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

    // ���������W�b�N
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

    // �I�u�W�F�N�g���o�C�g�z��ɕϊ�
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

    // �o�C�g�z����I�u�W�F�N�g�ɕϊ�
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