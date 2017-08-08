using UnityEngine;
using System.Collections.Generic;

public class UM_ImagesPickResult : UM_BaseResult {

	private Dictionary<string, Texture2D> _Images;

	public UM_ImagesPickResult(bool isSuccess, Dictionary<string, Texture2D> images) {
		_IsSucceeded = isSuccess;
		_Images = images;
	}

	public Dictionary<string, Texture2D> Images {
		get {
			return _Images;
		}
	}
}
