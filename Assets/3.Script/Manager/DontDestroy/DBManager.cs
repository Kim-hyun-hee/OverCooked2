using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;

// ����� ������ ��
[System.Serializable]
public class User
{
    public string userId;
    public string userPw;

    public User(string userId, string userPw)
    {
        this.userId = userId;
        this.userPw = userPw;
    }
}

public class DBManager : MonoBehaviour
{
    private static DBManager instance;
    public static DBManager Instance { get { return instance; } }

    public string DBurl = "https://overcooked2-df596-default-rtdb.firebaseio.com/";
    DatabaseReference reference;

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

    public void WriteUserData(string userId, string userPassword)
    {
        reference.Child("Account").Child("ID").Child(userId).SetValueAsync(userPassword);
        reference.Child("Account").Child("ID").Child(userId).Child("Password").SetValueAsync(userPassword);
    }

    public void ReadUserData()
    {
        FirebaseDatabase.DefaultInstance.GetReference("Account")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    // Handle the error...
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    // Do something with snapshot...
                    Debug.Log(snapshot.Child("ID").Child("asdfzxcv0731").Key);

                }
            });
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
}
