using SimpleFirebaseUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LoginManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject seleccion;

    public GameObject registro;
    public Button Btn_continuarRegistro;
    public Button Btn_listoRegistro;
    public TextMeshProUGUI estatusRegistro;
    private string nombreString;
    private string usuarioString;
    private string contraString;
    private string correoString;
    private bool nombre_listo = false;
    private bool usuario_listo = false;
    private bool contra_listo = false;
    private bool correo_listo = false;

    [Space]
    public GameObject login;
    public Button Btn_Login;
    public TextMeshProUGUI estatusLogin;

    [Header("Firebase")]
    private readonly string FirebaseURL = "https://pokefake-go-default-rtdb.firebaseio.com/";
    private readonly string FirebaseKey = "xTlR9Su6uBGOmoMlAHaHXrydQtlY4PZAaM5jqqPs";
    private Firebase _firebase;

    private void Awake()
    {
        seleccion.SetActive(true);
        registro.SetActive(false);
        login.SetActive(false);

        Btn_continuarRegistro.interactable = false;
        Btn_listoRegistro.interactable = false;
        estatusRegistro.text = "Completa los datos";

        Btn_Login.interactable = false;
        estatusLogin.text = "";
    }

    private void Start()
    {
        _firebase = Firebase.CreateNew(FirebaseURL, FirebaseKey);
    }

    #region Registro

    public void Registrarse()
    {
        estatusRegistro.text = "Cargando...";

        StartCoroutine(ComprobarUsuarioCheck());
        //comprobar que no esta ese correo
    }

    public void UpdateNombre(string txt)
    {
        if (txt != null)
        {
            nombre_listo = true;
            nombreString = txt;
        }
        else
        {
            nombre_listo = false;
        }

        CompruebaRegistroListo();
    }
    public void UpdateUsuario(string txt)
    {
        if (txt != null)
        {
            usuario_listo = true;
            usuarioString = txt;
        }
        else
        {
            usuario_listo = false;
        }

        CompruebaRegistroListo();
    }
    public void UpdateContra(string txt)
    {
        if (txt != null)
        {
            contra_listo = true;
            contraString = txt;
        }
        else
        {
            contra_listo = false;
        }

        CompruebaRegistroListo();
    }
    public void UpdateCorreo(string txt)
    {
        if (txt != null)
        {
            correo_listo = true;
            correoString = txt;
        }
        else
        {
            correo_listo = false;
        }

        CompruebaRegistroListo();
    }

    public void CompruebaRegistroListo()
    {
        if (nombre_listo && usuario_listo && contra_listo && correo_listo)
        {
            Btn_continuarRegistro.interactable = true;
        }
        else
        {
            Btn_continuarRegistro.interactable = false;
        }
    }

    #region Default Callbacks

    void GetOKHandler(Firebase sender, DataSnapshot snapshot)
    {
        DebugLog("[OK] Get from key: <" + sender.FullKey + ">");
        DebugLog("[OK] Raw Json: " + snapshot.RawJson);

        Dictionary<string, object> dict = snapshot.Value<Dictionary<string, object>>();
        List<string> keys = snapshot.Keys;

        if (keys != null)
            foreach (string key in keys)
            {
                DebugLog(key + " = " + dict[key].ToString());
            }
    }

    void GetFailHandler(Firebase sender, FirebaseError err)
    {
        DebugError("[ERR] Get from key: <" + sender.FullKey + ">,  " + err.Message + " (" + (int)err.Status + ")");
    }

    void SetOKHandler(Firebase sender, DataSnapshot snapshot)
    {
        DebugLog("[OK] Set from key: <" + sender.FullKey + ">");
    }

    void SetFailHandler(Firebase sender, FirebaseError err)
    {
        DebugError("[ERR] Set from key: <" + sender.FullKey + ">, " + err.Message + " (" + (int)err.Status + ")");
    }

    void UpdateOKHandler(Firebase sender, DataSnapshot snapshot)
    {
        DebugLog("[OK] Update from key: <" + sender.FullKey + ">");
    }

    void UpdateFailHandler(Firebase sender, FirebaseError err)
    {
        DebugError("[ERR] Update from key: <" + sender.FullKey + ">, " + err.Message + " (" + (int)err.Status + ")");
    }

    void DelOKHandler(Firebase sender, DataSnapshot snapshot)
    {
        DebugLog("[OK] Del from key: <" + sender.FullKey + ">");
    }

    void DelFailHandler(Firebase sender, FirebaseError err)
    {
        DebugError("[ERR] Del from key: <" + sender.FullKey + ">, " + err.Message + " (" + (int)err.Status + ")");
    }

    void PushOKHandler(Firebase sender, DataSnapshot snapshot)
    {
        DebugLog("[OK] Push from key: <" + sender.FullKey + ">");
    }

    void PushFailHandler(Firebase sender, FirebaseError err)
    {
        DebugError("[ERR] Push from key: <" + sender.FullKey + ">, " + err.Message + " (" + (int)err.Status + ")");
    }

    void GetRulesOKHandler(Firebase sender, DataSnapshot snapshot)
    {
        DebugLog("[OK] GetRules");
        DebugLog("[OK] Raw Json: " + snapshot.RawJson);
    }

    void GetRulesFailHandler(Firebase sender, FirebaseError err)
    {
        DebugError("[ERR] GetRules,  " + err.Message + " (" + (int)err.Status + ")");
    }

    void GetTimeStamp(Firebase sender, DataSnapshot snapshot)
    {
        long timeStamp = snapshot.Value<long>();
        DateTime dateTime = Firebase.TimeStampToDateTime(timeStamp);

        DebugLog("[OK] Get on timestamp key: <" + sender.FullKey + ">");
        DebugLog("Date: " + timeStamp + " --> " + dateTime.ToString());
    }

    void DebugLog(string str)
    {
        Debug.Log(str);
        //if (textMesh != null)
        //{
        //    textMesh.text += (++debug_idx + ". " + str) + "\n";
        //}
    }

    void DebugWarning(string str)
    {
        Debug.LogWarning(str);
        //if (textMesh != null)
        //{
        //    textMesh.text += (++debug_idx + ". " + str) + "\n";
        //}
    }

    void DebugError(string str)
    {
        Debug.LogError(str);
        //if (textMesh != null)
        //{
        //    textMesh.text += (++debug_idx + ". " + str) + "\n";
        //}
    }

    #endregion

    IEnumerator ComprobarUsuarioCheck()
    {
        Firebase comprobarUsuario = _firebase.Child("Base de datos").Child("Usuarios").Child(usuarioString);

        _firebase.OnGetSuccess += GetOKHandler;
        _firebase.OnGetFailed += GetFailHandler;
        _firebase.OnSetSuccess += SetOKHandler;
        _firebase.OnSetFailed += SetFailHandler;
        _firebase.OnUpdateSuccess += UpdateOKHandler;
        _firebase.OnUpdateFailed += UpdateFailHandler;
        _firebase.OnPushSuccess += PushOKHandler;
        _firebase.OnPushFailed += PushFailHandler;
        _firebase.OnDeleteSuccess += DelOKHandler;
        _firebase.OnDeleteFailed += DelFailHandler;

        comprobarUsuario.OnGetSuccess += ResultadoUsuario;
        comprobarUsuario.GetValue();

        yield return null;
    }

    private void ResultadoUsuario(Firebase sender, DataSnapshot snapshot)
    {
        if (snapshot.RawJson == "null")
        {
            Registrar();
        }
        else
        {
            StopAllCoroutines();
            estatusRegistro.text = "Usuario ya registrado";
        }
    }

    private void Registrar()
    {
        _firebase.Child("Base de datos").Child("Usuarios").Child(usuarioString).Child("Contra").SetValue(contraString);
        _firebase.Child("Base de datos").Child("Usuarios").Child(usuarioString).Child("Correo").SetValue(correoString);
        _firebase.Child("Base de datos").Child("Usuarios").Child(usuarioString).Child("Nombre").SetValue(nombreString);
        estatusRegistro.text = "Registrado!";
        Btn_continuarRegistro.interactable = false;
        Btn_listoRegistro.interactable = true;
    }

    #endregion

    #region Login

    public void UpdateUsuario2(string txt)
    {
        if (txt != null)
        {
            usuario_listo = true;
            usuarioString = txt;
        }
        else
        {
            usuario_listo = false;
        }

        CompruebaRegistroListo2();
    }
    public void UpdateContra2(string txt)
    {
        if (txt != null)
        {
            contra_listo = true;
            contraString = txt;
        }
        else
        {
            contra_listo = false;
        }

        CompruebaRegistroListo2();
    }
    public void CompruebaRegistroListo2()
    {
        if (usuario_listo && contra_listo)
        {
            Btn_Login.interactable = true;
        }
        else
        {
            Btn_Login.interactable = false;
        }
    }

    public void Login()
    {
        estatusLogin.text = "Comprobando...";
        StartCoroutine(LoginUsuario());
    }
    IEnumerator LoginUsuario()
    {
        Firebase Login = _firebase.Child("Base de datos").Child("Usuarios").Child(usuarioString);

        _firebase.OnGetSuccess += GetOKHandler;
        _firebase.OnGetFailed += GetFailHandler;
        _firebase.OnSetSuccess += SetOKHandler;
        _firebase.OnSetFailed += SetFailHandler;
        _firebase.OnUpdateSuccess += UpdateOKHandler;
        _firebase.OnUpdateFailed += UpdateFailHandler;
        _firebase.OnPushSuccess += PushOKHandler;
        _firebase.OnPushFailed += PushFailHandler;
        _firebase.OnDeleteSuccess += DelOKHandler;
        _firebase.OnDeleteFailed += DelFailHandler;

        Login.OnGetSuccess += ResultadoUsuarioLogin;
        Login.GetValue();

        yield return null;
    }

    private void ResultadoUsuarioLogin(Firebase sender, DataSnapshot snapshot)
    {
        if (snapshot.RawJson != "null")
        {
            StartCoroutine(LoginContra());
        }
        else
        {
            StopAllCoroutines();
            estatusLogin.text = "El usuario no existe";
        }
    }

    IEnumerator LoginContra()
    {
        Firebase LoginContra = _firebase.Child("Base de datos").Child("Usuarios").Child(usuarioString).Child("Contra");

        _firebase.OnGetSuccess += GetOKHandler;
        _firebase.OnGetFailed += GetFailHandler;
        _firebase.OnSetSuccess += SetOKHandler;
        _firebase.OnSetFailed += SetFailHandler;
        _firebase.OnUpdateSuccess += UpdateOKHandler;
        _firebase.OnUpdateFailed += UpdateFailHandler;
        _firebase.OnPushSuccess += PushOKHandler;
        _firebase.OnPushFailed += PushFailHandler;
        _firebase.OnDeleteSuccess += DelOKHandler;
        _firebase.OnDeleteFailed += DelFailHandler;

        LoginContra.OnGetSuccess += ResultadoContraLogin;
        LoginContra.GetValue();

        yield return null;
    }
    private void ResultadoContraLogin(Firebase sender, DataSnapshot snapshot)
    {
        string contra = snapshot.RawJson.TrimStart('"').TrimEnd('"');
        if (contra == contraString)
        {
            LoginCorrecto();
        }
        else
        {
            StopAllCoroutines();
            estatusLogin.text = "Contraseña incorrecta";
        }
    }

    private void LoginCorrecto()
    {
        estatusLogin.text = "Logeado";
    }

    #endregion
}