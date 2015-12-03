﻿using UnityEngine;
using System.Collections;

public class AnimateTextureThroughTiling : MonoBehaviour {

	private float offsetChange;
	private float offsetChange2;
	public float speed = 0.1f;
	public float speed2 = 0.1f;
	public Material material;


	void Start () {
	}
	
	void Update () {

		offsetChange += Time.deltaTime * speed;
		offsetChange2 += Time.deltaTime * speed2;
		material.SetTextureOffset("_MainTex", new Vector2 (offsetChange, offsetChange));
		material.SetTextureOffset("_DetailAlbedoMap", new Vector2 (offsetChange2, offsetChange2));
	}
}