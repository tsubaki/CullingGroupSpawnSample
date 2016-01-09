using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class SpawnTest : MonoBehaviour 
{
	private IEnumerator spawnFlow = null;
	private CullingGroup cullingGroup = null;

	[SerializeField] Transform[] targetPositions = null;
	[SerializeField] GameObject spawnObject = null;
	[SerializeField] float interval = 1;

	void Awake()
	{
		spawnFlow = SpawnCoroutine();
	}

	void OnEnable()
	{
		// cullinggroupを生成してコルーチンを開始
		SetupCullinggroup ();
		StartCoroutine (spawnFlow);
	}

	void OnDisable()
	{
		// コルーチンを停止してcullinggroupを破棄
		StopCoroutine (spawnFlow);
		cullingGroup.Dispose ();
		cullingGroup = null;
	}

	void SetupCullinggroup()
	{
		// culling groupの初期化
		cullingGroup = new CullingGroup ();
		cullingGroup.targetCamera = Camera.main;

		// 敵を生成する座標の登録
		BoundingSphere[] bounds = new BoundingSphere[targetPositions.Length];
		for (int i = 0; i < targetPositions.Length; i++) {
			bounds [i].position = targetPositions [i].position;
			bounds [i].radius = 1;
		}
		cullingGroup.SetBoundingSpheres (bounds);
		cullingGroup.SetBoundingSphereCount (targetPositions.Length);
	}

	IEnumerator SpawnCoroutine()
	{
		List<int> countList = new List<int> ();
		while (true) {

			// 視界外にある生成ポイント一覧を取得
			countList.Clear ();
			for (int i = 0; i < targetPositions.Length; i++) {
				if (cullingGroup.IsVisible (i) == false ) {
					countList.Add (i);
				}
			}
			if (countList.Count != 0) {
				var newPos = countList[Random.Range (0, countList.Count)];
				GameObject.Instantiate (spawnObject, targetPositions [newPos].position, Quaternion.identity);
			}

			// 0.5秒待つ
			yield return new WaitForSeconds (interval);
		}
	}
}