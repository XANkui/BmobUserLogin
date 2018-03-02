using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cn.bmob.api;
using cn.bmob.io;
using cn.bmob.tools;
using UnityEngine.UI;

public class BmobRigister : MonoBehaviour {

    private static BmobUnity Bmob;
    public InputField userNameText;
    public InputField userCodeText;
    public InputField userCodeIsOkText;
    public Text tipText;
    public GameObject rigisterButton;
    public GameObject okButton;
    public GameObject loginButton;
    public GameObject userCodeisOkPanel;

    // Use this for initialization
    void Start() {
        BmobDebug.Register(print);
        BmobDebug.level = BmobDebug.Level.TRACE;
        Bmob = GameObject.Find("Main Camera").GetComponent<BmobUnity>();
    }

    public class MyBmobUser : BmobUser {

        public override void readFields(BmobInput input)
        {
            base.readFields(input);
        }

        public override void write(BmobOutput output, bool all)
        {
            base.write(output, all);
        }
    }

    public void RigisterAccount() {

        if (userNameText.text == "") {
            tipText.text = "请输入用户名";
            return;
        }

        if (userCodeText.text == "")
        {
            tipText.text = "请输入密码";
            return;
        }

        if (userCodeIsOkText.text == "")
        {
            tipText.text = "请确认密码";
            return;
        }

        if (userCodeIsOkText.text != userCodeText.text)
        {
            tipText.text = "两次密码不一致！";
            return;
        }

        MyBmobUser user = new MyBmobUser();
        user.username = userNameText.text;
        user.password = userCodeText.text;
        Bmob.Signup<MyBmobUser>(user, (response, exception) => {
            if (exception != null) {
                print("注册失败" + exception);
                tipText.text = "注册失败";
                return;
            }

            print("注册成功");
            tipText.text = "注册成功";
            RigisterPanelHide();
        }
        );
    }

    /// <summary>
    /// 不应该添加在这里，临时
    /// </summary>
    public void RigisterPanelShow(){
        ClearInputField();

        rigisterButton.SetActive(false);
        okButton.SetActive(true);
        userCodeisOkPanel.SetActive(true);
        loginButton.SetActive(false);
    }

    /// <summary>
    /// 不应该添加在这里，临时
    /// </summary>
    public void RigisterPanelHide()
    {
        ClearInputField();

        rigisterButton.SetActive(true);
        okButton.SetActive(false);
        userCodeisOkPanel.SetActive(false);
        loginButton.SetActive(true);
    }

    /// <summary>
    /// 不应该添加在这里，临时
    /// </summary>
    private void ClearInputField() {
        userNameText.text = "";
        userCodeText.text = "";
        userCodeIsOkText.text = "";
    }

    public void LoginAccount() {
        

        if (userNameText.text == "")
        {
            tipText.text = "请输入用户名";
            return;
        }

        if (userCodeText.text == "")
        {
            tipText.text = "请输入密码";
            return;
        }


        Bmob.Login<MyBmobUser>(userNameText.text, userCodeText.text, (response, exception) =>
        {
            if (exception != null)
            {
                print("登录失败" + exception);
                tipText.text = "登录失败";
                return;
            }

		        print("登录成功");

				//保存用户登录的数据objectId
				PlayerPrefs.SetString ("objectId", response.objectId);

		        tipText.text = "登录成功";
		        
				UnityEngine.SceneManagement.SceneManager.LoadScene (1);
        }
        );

        
    }

	// Update is called once per frame
	void Update () {
		
	}
}
