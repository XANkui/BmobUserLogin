using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cn.bmob.api;
using cn.bmob.io;
using cn.bmob.tools;
using UnityEngine.UI;

public class GetBmobData : MonoBehaviour {

	public Text userNmaeText;
	public Text coinText;
	private string objectId;

	private static BmobUnity Bmob;

	private string TABLENAME = "_User";

	//排行数据
	public GameObject rankDataPrefab;
	public Transform rankDataParent;

	// Use this for initialization
	void Start () {

		//Bmob初始化
		BmobInit ();

		//获取保存的用户数据objectId
		objectId = PlayerPrefs.GetString ("objectId");

		//获取对应用户数据
		ToGetBmobData ();

		//金币排行榜数据
		RankData ();
	}

	/// <summary>
	/// Bmobs the init.
	/// </summary>
	void BmobInit () {
		BmobDebug.Register (print);
		BmobDebug.level = BmobDebug.Level.TRACE;
		Bmob = GameObject.Find ("Main Camera").GetComponent <BmobUnity> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.A)) {
			RankData ();
		}
	}

	/// <summary>
	/// Bmob game object.
	/// </summary>
	public class BmobGameObject : BmobTable {

		public string userName;
		public int coin;

		/// <summary>
		/// 把服务端返回的数据转化为本地对象值
		/// </summary>
		/// <param name="input"></param>
		public override void readFields (BmobInput input)
		{
			base.readFields (input);

			userName = input.Get<string> ("username");
			coin = input.Get<int> ("coin");
		}

		/// <summary>
		/// 把本地对象写入到output，后续序列化提交到服务器
		/// </summary>
		/// <param name="output"></param>
		/// <param name="all">用于区分请求/打印输出序列化</param>
		public override void write (BmobOutput output, bool all)
		{
			base.write (output, all);
		}

	}

	/// <summary>
	/// Tos the get bmob data.
	/// </summary>
	void ToGetBmobData(){

		Bmob.Get <BmobGameObject>(TABLENAME,objectId,
			(response, exception)=>{
				if(exception != null) {
					print("数据获取失败" + exception);
					return;
				}

				print ("数据获取成功");

				//获取Bmob的用户数据，并且赋值给UI
				userNmaeText.text = response.userName;
				coinText.text = response.coin.ToString ();

			}
		);
	}

	/// <summary>
	/// Ranks the data.
	/// </summary>
	void RankData() {

		//设置获取表的属性
		BmobQuery queryData = new BmobQuery ();
		queryData.OrderByDescending ("coin");
		queryData.Limit (10);

		//获取对应的表的内容
		Bmob.Find <BmobGameObject>(TABLENAME, queryData,
			(response, exception)=>{
				if (exception != null) {
					print ("数据列表获取失败" + exception);

					return;
				}

				print ("数据列表获取成功");

				List<BmobGameObject> list = response.results;

				foreach(var data in list){
					print(data);

					//生成数据列表
					GameObject rankDataUI = Instantiate (rankDataPrefab);
					rankDataUI.transform.parent = rankDataParent;
					rankDataUI.transform.localRotation = rankDataPrefab.transform.localRotation;
					rankDataUI.transform.localScale = Vector3.one;
					rankDataUI.GetComponent <RankDataAttr>().userNmaeText.text = data.userName;
					rankDataUI.GetComponent <RankDataAttr>().coinText.text = data.coin.ToString ();

				}

			}
		);
	}



}
