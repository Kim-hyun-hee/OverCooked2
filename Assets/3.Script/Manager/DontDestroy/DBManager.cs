using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System;

public class DBManager : MonoBehaviour
{
    public string DBurl = "https://overcooked2-df596-default-rtdb.firebaseio.com/";
    DatabaseReference reference;

    private void Start()
    {

    }

    public void Init()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new Uri(DBurl);
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }
}
