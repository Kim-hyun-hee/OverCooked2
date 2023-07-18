using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;

// 사용자 데이터 모델
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
        duplicateRef.GetValueAsync().ContinueWith(task => // 해당 경로의 데이터를 비동기적으로 가져옴
        {
            if (task.IsFaulted)
            {
                Debug.Log("중복 체크 중 오류 발생");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists) // 가져온 데이터가 존재하는 경우
                {
                    Debug.Log("중복된 아이디");
                    return;
                }
                else
                {
                    DatabaseReference accountRef = reference.Child("Account");
                    accountRef.GetValueAsync().ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            Debug.Log("계정 생성 중 오류 발생");
                        }
                        else if (task.IsCompleted)
                        {   // 비번저장
                            reference.Child("Account").Child("ID").Child(id).Child("Password").SetValueAsync(password);
                            Debug.Log("계정 생성 완료");
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
                Debug.Log("로그인 중 오류 발생");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    if (snapshot.Child("Password").Value.ToString() == password)
                    {
                        Debug.Log("로그인 성공");
                    }
                    else
                    {
                        Debug.Log("비번 틀림");
                    }
                }
                else
                {
                    Debug.Log("아이디 없음");
                }
            }
        });
    }
}
