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
    public string username;
    public string password;

    public User(string username, string password)
    {
        this.username = username;
        this.password = password;
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
        duplicateRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("중복 체크 중 오류 발생");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    //Managers.Event.DBEvent?.Invoke(Define.DB_Event.DuplicateID);
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
                        {
                            reference.Child("Account").Child("ID").Child(id).Child("Password").SetValueAsync(password);
                            //Managers.Event.DBEvent?.Invoke(Define.DB_Event.SuccessCreateNewAccount);
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
                        //CurrecntUserID = id;
                        //if (snapshot.HasChild(_dataSlot1))
                        //{
                        //    LoadData(Slot1Data, Slot1SkillData, Slot1InventoryData, Slot1EquipmantData, snapshot, _dataSlot1);
                        //}
                        //if (snapshot.HasChild(_dataSlot2))
                        //{
                        //    LoadData(Slot2Data, Slot2SkillData, Slot2InventoryData, Slot2EquipmantData, snapshot, _dataSlot2);
                        //}
                        //if (snapshot.HasChild(_dataSlot3))
                        //{
                        //    LoadData(Slot3Data, Slot3SkillData, Slot3InventoryData, Slot3EquipmantData, snapshot, _dataSlot3);
                        //}

                        //Managers.Event.DBEvent?.Invoke(Define.DB_Event.SuccessLogin);
                        Debug.Log("로그인 성공");
                    }
                    else
                    {
                        //Managers.Event.DBEvent?.Invoke(Define.DB_Event.WrongPassword);
                        Debug.Log("비번 틀림");
                    }
                }
                else
                {
                    //Managers.Event.DBEvent?.Invoke(Define.DB_Event.NonExistID);
                    Debug.Log("아이디 없음");
                }
            }
        });
    }
}
