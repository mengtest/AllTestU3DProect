using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

public class QrCode : MonoBehaviour {
	BarcodeReader m_barcodeRender = new BarcodeReader();
	private RectTransform rectTrans;
	public Image image1;
	public Image image2;
	public Image image3;

	public Texture2D icon;

	// Use this for initialization
	void Start () {
		rectTrans = GetComponent<RectTransform>();
		// image1.sprite = ShowQRCode("13215649845");//生成二维码返回sprite，固定宽高256,256
		// image2.sprite = ShowQRCode("13215649845",256,256);//生成二维码返回sprite，固定宽高256,256
		image3.sprite = ShowQRCode("13215649845",256,256,icon);//生成二维码返回sprite，固定宽高256,256





		// StartCoroutine(ScreenshotSave());

		// 获取文件解码
		// byte[] binary;
		// using (FileStream fs = new FileStream("c:/5.png",FileMode.Open,FileAccess.Read))
		// {
		// 	binary = new byte[fs.Length]; //创建文件长度的buffer
		// 	fs.Read(binary,0,(int)fs.Length);
		// 	fs.Close();
		// 	fs.Dispose();
		// }

		// Debug.Log(binary.Length);
		// Debug.Log(DeCodeQR(binary,486,720));	//byte数组 文件宽高
		// Texture2D img = new Texture2D(486,720);
		// img.LoadImage(binary);
		// Debug.Log(img.GetPixels32().Length);
		// Debug.Log(DeCodeQR(img,486,720));	//byte数组 文件宽高
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}


	
	public void Screenshot(RectTransform _rectTrans){

	}
	public void Screenshot(){

	}

	public void Screenshot(Rect _rectTrans){

	}


	/// <summary>
	/// 截图保存
	/// </summary>
	/// <param name="_rectTrans">要截的大小</param>
	/// <returns></returns>
	IEnumerator ScreenshotSave(RectTransform _rectTrans){
		yield return new WaitForEndOfFrame ();
		float screenWidth = Screen.width;
		float screenHeight = Screen.height;
		float tempWidth = _rectTrans.sizeDelta.x * (screenWidth / 1280);
		float tempHeight = _rectTrans.sizeDelta.y * (screenHeight / 780);
		Texture2D txt2D = new Texture2D((int)tempWidth, (int)tempHeight, UnityEngine.TextureFormat.RGB24, false);
		txt2D.ReadPixels(new Rect(0 + (screenWidth / 2) - (tempWidth / 2),0,tempWidth,tempHeight),0,0,false);
		System.IO.File.WriteAllBytes("C:/5.png", txt2D.EncodeToPNG());
	}


	/// <summary>
	/// 截图保存
	/// </summary>
	/// <param name="_rectTrans">要截的大小</param>
	/// <returns></returns>
	IEnumerator TestScreenshotSave(Vector2 _imgSize,Vector2 _pivot){
		yield return new WaitForEndOfFrame ();
		float screenWidth = Screen.width;
		float screenHeight = Screen.height;
		float imgWidth = _imgSize.x * (screenWidth / 1280);
		float imgHeight = _imgSize.y * (screenHeight / 780);
		Texture2D txt2D = new Texture2D((int)imgWidth, (int)imgHeight, UnityEngine.TextureFormat.RGB24, false);
		txt2D.ReadPixels(new Rect(0 + screenWidth * _pivot.x - (imgWidth / 2),0,imgWidth,imgHeight),0,0,false);
		System.IO.File.WriteAllBytes("C:/5.png", txt2D.EncodeToPNG());
	}

    /// <summary>
    /// 显示绘制的二维码，固定宽高256
    /// </summary>
    /// <param name="_str">扫码信息</param>
    Sprite ShowQRCode(string _str)
    {
		int _width = 256;
		int _height = 256;
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,//码格式（二维码、条形码...）
            Options = new QrCodeEncodingOptions//编码格式（支持的编码格式)
            {
                Width = _width,//设置宽高
                Height = _height,
				CharacterSet = "UTF-8",//设置中文编码格式，否则中文不支持
				Margin = 1,//设置二维码距离边缘的空白距离，0难以识别
            }
        };

		//调用api写入二维码信息，返回颜色数组
		Color32[] color32 = writer.Write(_str);
		//定义Texture2D并且填充
        Texture2D tTexture = new Texture2D(_width, _height);
        tTexture.SetPixels32(color32);
        tTexture.Apply();
		//创景精灵返回
        return Sprite.Create(tTexture, new Rect(0, 0, _width, _height), new Vector2(0.5f, 0.5f));
    }
	

    /// <summary>
    /// 显示绘制的二维码，任意尺寸正方形
    /// </summary>
    /// <param name="_str">扫码信息</param>
    Sprite ShowQRCode(string _str,int _width,int _height)
    {
        MultiFormatWriter writer = new MultiFormatWriter();
        Dictionary<EncodeHintType, object> hints = new Dictionary<EncodeHintType, object>()
		{
			{EncodeHintType.CHARACTER_SET,"UTF-8"},
			{EncodeHintType.MARGIN,1},
			{EncodeHintType.ERROR_CORRECTION,ZXing.QrCode.Internal.ErrorCorrectionLevel.M},
		};
		BitMatrix bitMatrix = writer.encode(_str, BarcodeFormat.QR_CODE, _width, _height, hints);
        int w = bitMatrix.Width;
        int h = bitMatrix.Height;
        Texture2D texture = new Texture2D(_width, _height);
        for (int x=0; x<h; x++)
        {
            for(int y=0; y<w; y++)
            {
                if(bitMatrix[x,y])
                {
                    texture.SetPixel(y, x, Color.black);
                }
                else
                {
                    texture.SetPixel(y, x, Color.white);
                }
            }
        }
        texture.Apply();
		//创景精灵返回
        return Sprite.Create(texture, new Rect(0, 0, _width, _height), new Vector2(0.5f, 0.5f));
    }


    /// <summary>
    /// 显示绘制的二维码，任意尺寸正方形
    /// </summary>
    /// <param name="_str">扫码信息</param>
    Sprite ShowQRCode(string _str,int _width,int _height,Texture2D centerIcon)
    {
        MultiFormatWriter writer = new MultiFormatWriter();
        Dictionary<EncodeHintType, object> hints = new Dictionary<EncodeHintType, object>()
		{
			{EncodeHintType.CHARACTER_SET,"UTF-8"},
			{EncodeHintType.MARGIN,1},
			{EncodeHintType.ERROR_CORRECTION,ZXing.QrCode.Internal.ErrorCorrectionLevel.M},
		};
		BitMatrix bitMatrix = writer.encode(_str, BarcodeFormat.QR_CODE, _width, _height, hints);
        int w = bitMatrix.Width;
        int h = bitMatrix.Height;
        Texture2D texture = new Texture2D(_width, _height);
        for (int x=0; x<h; x++)
        {
            for(int y=0; y<w; y++)
            {
                if(bitMatrix[x,y])
                {
                    texture.SetPixel(y, x, Color.black);
                }
                else
                {
                    texture.SetPixel(y, x, Color.white);
                }
            }
        }
		
		if (centerIcon != null){
			// 添加小图
			int halfWidth = texture.width / 2;
			int halfHeight = texture.height / 2;
			int halfWidthOfIcon = centerIcon.width / 2;
			int halfHeightOfIcon = centerIcon.height / 2;
			int centerOffsetX = 0;
			int centerOffsetY = 0;
			for (int x = 0; x < h; x++)
			{
				for (int y = 0; y < w; y++)
				{
					centerOffsetX = x - halfWidth;
					centerOffsetY = y - halfHeight;
					if(Mathf.Abs(centerOffsetX) <= halfWidthOfIcon && Mathf.Abs(centerOffsetY) <= halfHeightOfIcon)
					{
						texture.SetPixel(x, y, centerIcon.GetPixel(centerOffsetX + halfWidthOfIcon, centerOffsetY + halfHeightOfIcon));
					}
				}
			}
		}

        texture.Apply();
		//创景精灵返回
        return Sprite.Create(texture, new Rect(0, 0, _width, _height), new Vector2(0.5f, 0.5f));
    }

	public string DeCodeQR(byte[] binary,int width,int height){
		try
		{
			// 将画面中的二维码信息检索出来
			Result tResult = m_barcodeRender.Decode(binary, width, height,RGBLuminanceSource.BitmapFormat.RGB24);
			if (tResult != null)
			{
				return tResult.Text;
			}
		}
		catch (System.Exception ex)
		{
			Debug.Log(ex);
			throw;
		}
		return "";
	}


	public string DeCodeQR(Texture2D _texture,int width,int height){
		try
		{
			// 将画面中的二维码信息检索出来
			Result tResult = m_barcodeRender.Decode(_texture.GetPixels32(), width, height);
			if (tResult != null)
			{
				return tResult.Text;
			}
		}
		catch (System.Exception ex)
		{
			Debug.Log(ex);
			throw;
		}
		return "";
	}

		
	public string save(Texture2D pic) 
	{ 
		byte[] data = pic.EncodeToPNG(); 
		string base64str = System.Convert.ToBase64String(data);
		return base64str;
		// PlayerPrefs.SetString("save_date",base64str);
	} 


	Texture2D load(string str) 
	{ 

		// string str = PlayerPrefs.GetString("save_date");
		Texture2D pic = new Texture2D(200,200);
		byte[] data = System.Convert.FromBase64String(str);
		pic.LoadImage(data);

	return pic; 
	}

}