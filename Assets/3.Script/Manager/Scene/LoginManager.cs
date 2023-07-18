using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public InputField id;
    public InputField pw;

    public void Login()
    {
        DBManager.Instance.Login(id.text, pw.text);
    }

    public void SignIn()
    {
        DBManager.Instance.CreateAccount(id.text, pw.text);
    }
}
