using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;

public class PlayerInfo
{
    public string id;
    public List<StageInfo> stageInfos = new List<StageInfo>();
}

public class StageInfo
{
    public bool isClear;
    public float highScore;

    public StageInfo(bool isClear = false, float highScore = 0f)
    {
        this.isClear = isClear;
        this.highScore = highScore;
    }
}

public class DBManager : MonoBehaviour
{
    private static DBManager instance;
    public static DBManager Instance { get { return instance; } }

    public string DBurl = "https://overcooked2-df596-default-rtdb.firebaseio.com/";
    DatabaseReference reference;

    public PlayerInfo playerInfo = new PlayerInfo();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new Uri(DBurl);
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void CreateAccount(string id, string password)
    {
        DatabaseReference duplicateRef = reference.Child("Account").Child("ID").Child(id);
        duplicateRef.GetValueAsync().ContinueWith(task => // �ش� ����� �����͸� �񵿱������� ������
        {
            if (task.IsFaulted)
            {
                Debug.Log("�ߺ� üũ �� ���� �߻�");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists) // ������ �����Ͱ� �����ϴ� ���
                {
                    Debug.Log("�ߺ��� ���̵�");
                    return;
                }
                else
                {
                    DatabaseReference accountRef = reference.Child("Account");
                    accountRef.GetValueAsync().ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            Debug.Log("���� ���� �� ���� �߻�");
                        }
                        else if (task.IsCompleted)
                        {   // �������
                            reference.Child("Account").Child("ID").Child(id).Child("Password").SetValueAsync(password);
                            Debug.Log("���� ���� �Ϸ�");
                            
                            CreateData(id);
                        }
                    });
                }
            }
        });
    }

    public void Login(string id, string password)
    {
        DatabaseReference accountRef = reference.Child("Account").Child("ID").Child(id);
        accountRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("�α��� �� ���� �߻�");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    if (snapshot.Child("Password").Value.ToString() == password)
                    {
                        Debug.Log("�α��� ����");
                        LoadData(snapshot);
                        LoginToStart.LoadStartScene("StartScene");
                    }
                    else
                    {
                        Debug.Log("��� Ʋ��");
                    }
                }
                else
                {
                    Debug.Log("���̵� ����");
                }
            }
        });
    }

    public void CreateData(string id)
    {
        DatabaseReference duplicateRef = reference.Child("Account").Child("ID").Child(id);
        duplicateRef.GetValueAsync().ContinueWith(task => // �ش� ����� �����͸� �񵿱������� ������
        {
            if (task.IsFaulted)
            {
                Debug.Log("������ ���� �� ���� �߻�");
                return;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                StageInfo stageInfo = new StageInfo();
                string stageInfoJson = JsonUtility.ToJson(stageInfo);

                for (int i = 0; i < Enum.GetValues(typeof(StageName)).Length; i++)
                {
                    reference.Child("Account").Child("ID").Child(id).Child("Stage").Child(Enum.GetName(typeof(StageName), i)).SetRawJsonValueAsync(stageInfoJson);
                }
                Debug.Log("�ʱ� ������ ���� �Ϸ�");
            }
        });
    }

    private void LoadData(DataSnapshot snapshot)
    {
        playerInfo.id = snapshot.Key;
        for (int i = 0; i < Enum.GetValues(typeof(StageName)).Length; i++)
        {
            playerInfo.stageInfos.Add(new StageInfo(bool.Parse(snapshot.Child("Stage").Child(Enum.GetName(typeof(StageName), i)).Child("isClear").Value.ToString()),
                int.Parse(snapshot.Child("Stage").Child(Enum.GetName(typeof(StageName), i)).Child("highScore").Value.ToString())));
        }
        Debug.Log("LoadData");
    }

    public void SaveData(PlayerInfo playerInfo)
    {
        DatabaseReference dataRef = reference.Child("Account").Child("ID").Child(playerInfo.id);
        dataRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("������ ���� �� ����");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                for(int i = 0; i < playerInfo.stageInfos.Count; i++)
                {
                    string stageInfoJson = JsonUtility.ToJson(playerInfo.stageInfos[i]);
                    reference.Child("Account").Child("ID").Child(playerInfo.id).Child("Stage").Child(Enum.GetName(typeof(StageName), i)).SetRawJsonValueAsync(stageInfoJson);
                }
                Debug.Log("������ ���� �Ϸ�");
            }
        });
    }
}
