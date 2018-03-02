using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cn.bmob.api;
using cn.bmob.tools;
using cn.bmob.io;
using cn.bmob.json;
using cn.bmob.response;
using cn.bmob.config;
using cn.bmob.Extensions;
using cn.bmob.http;

public class BmobInit : MonoBehaviour {

	private static BmobUnity Bmob;

	// Use this for initialization
	void Start ()
	{
		BmobDebug.Register (print);
		BmobDebug.level = BmobDebug.Level.TRACE;
		Bmob = gameObject.GetComponent<BmobUnity> ();
	}

	static string TABLENAME = "UserInfo";

	public class BmobGameObject : BmobTable{

		public BmobInt userId{ get; set;}
		public string userName{ get; set;}

		public override void readFields(BmobInput input){
			base.readFields (input);
			this.userName = input.Get<string> ("username");
			this.userId = input.Get<BmobInt> ("objectid");
		}

		public override void write(BmobOutput output, bool all) {
			base.write (output,all);

			output.Put ("objectid", this.userId);
			output.Put ("username", this.userName);
		}
	}

	void addData() {

		var data = new BmobGameObject ();
		data.userId = 2;
		data.userName = "Billy";

		Bmob.Create (TABLENAME, data,
			(resp, exception) =>{
				if (exception != null) {
					print("加载失败") ;					
				}else{
					print("加载成功"+ resp.createdAt+exception) ;
				}
		}
			);
	}

	void DeleteData() {
		Bmob.Delete (TABLENAME,"abab01b9ba",(resp, exception)=>{
			if(exception != null){
				print("删除失败");
			}else{
				print ("删除成功"+resp);
			}
		}
		);
	}

	void SelectData() {

		Bmob.Get <BmobGameObject>(TABLENAME, "838951fe45", (resp, exception)=> {
			if(exception != null){
				print("查询失败");
			}else{
				print ("查询成功");

				BmobGameObject data = resp;
				print(data);
				print (data.userName);
				print (data.userId);
				print (data.objectId);
				print (data.createdAt);
			}
		}
		);
	}

    void UpdateData() {

        BmobGameObject game = new BmobGameObject();
        game.userId = 11;
        game.userName = "XANkui";

        Bmob.Update(TABLENAME, "838951fe45", game,(resp, exception)=> {
            if (exception != null) {
                print("数据更新失败："+exception);
                return;
            }
            print("数据更新成功");
        }
        );
    }

	void Update() {

		if (Input.GetKeyDown (KeyCode.A)) {
			addData ();
		}

		if (Input.GetKeyDown (KeyCode.S)) {
			DeleteData ();
		}

		if (Input.GetKeyDown (KeyCode.D)) {
			SelectData ();
		}

        if (Input.GetKeyDown(KeyCode.F))
        {
            UpdateData();
        }
    }
}
