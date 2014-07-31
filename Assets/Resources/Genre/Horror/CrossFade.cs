using UnityEngine;
using System.Collections;

public class CrossFade : MonoBehaviour
{
	private Texture    newTexture;
	private Vector2    newOffset;
	private Vector2    newTiling;
	
	public  float    BlendSpeed = 3.0f;
	
	public bool    trigger = true;
	private float    fader = 0f;
	
	void Start ()
	{

		renderer.material.SetFloat( "_Blend", 0f );
	}
	
	void Update ()
	{
		if ( fader >= 1.0f )
		{
			trigger = false;
		}
		else if(fader <= 0f)
		{
			trigger = true;
		}

		if (trigger == true)
		{
			fader += Time.deltaTime * BlendSpeed;
			
			renderer.material.SetFloat( "_Blend", fader );
		}
		else if (trigger == false)
		{
			fader -= Time.deltaTime * BlendSpeed;

			renderer.material.SetFloat( "_Blend", fader );
		}

//		renderer.material.SetTexture ("_Texture1", newTexture );
//		renderer.material.SetTextureOffset ( "_Texture1", newOffset );
//		renderer.material.SetTextureScale ( "_Texture1", newTiling );

		//}
		//}
	}
	
//	public void CrossFadeTo( Texture curTexture, Vector2 offset, Vector2 tiling )
//	{
//		newOffset = offset;
//		newTiling = tiling;
//		newTexture = curTexture;
//		renderer.material.SetTexture( "_Texture2", curTexture );
//		renderer.material.SetTextureOffset ( "_Texture2", newOffset );
//		renderer.material.SetTextureScale ( "_Texture2", newTiling );
//		trigger = true;
//	}
}