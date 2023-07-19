using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public InputField id;
    public InputField pw;

    public delegate void OnLogin();
    public static event OnLogin OnSuccessLogin;

    public void Login()
    {
        DBManager.Instance.Login(id.text, pw.text);
    }

    public void SignIn()
    {
        DBManager.Instance.CreateAccount(id.text, pw.text);
    }

    private void Update()
    {
        if(OnSuccessLogin != null)
        {
            OnSuccessLogin();
        }
    }
}
