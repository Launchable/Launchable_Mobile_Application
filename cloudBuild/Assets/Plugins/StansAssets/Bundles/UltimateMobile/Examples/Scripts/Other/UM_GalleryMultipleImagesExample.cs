using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UM_GalleryMultipleImagesExample : MonoBehaviour {

	public Image[] Images;

	public void LoadImages() {
		UM_Camera.Instance.OnImagesPicked += HandleOnImagesPicked;
		UM_Camera.Instance.GetImagesFromGallery();
	}

	void HandleOnImagesPicked (UM_ImagesPickResult result)
	{
		UM_Camera.Instance.OnImagesPicked -= HandleOnImagesPicked;

		if (result.IsSucceeded) {
			if (Images != null) {
				int i = 0;
				foreach (KeyValuePair<string, Texture2D> texture in result.Images) {
					if (i < Images.Length) {
						Images[i++].sprite = Sprite.Create(texture.Value, new Rect(0.0f, 0.0f, texture.Value.width, texture.Value.height), new Vector2(0.5f, 0.5f));
					}
				}
			}
		}
	}

}
