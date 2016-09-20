﻿using System.Collections;

using UnityEngine;
# if Editor
using UnityEditor;
# endif
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

public class ColorizeTextureRectCoroutine : WorkerCoroutine
{

	Color32[] finalList;
	Color32[] insertList;
	Color32[] maskList;
	Color32 colorAdjust;
	Rect insertRect;
	Vector2 finalResolution;
	Vector2 textureResolution;
	int maxPixel;
	
    public void Prepare(Color32[] _finalList, Color32[] _insertList, Color32[] _maskList,Color32 _ColorAdjust, Rect _insertRect,Vector2 _finalResolution,Vector2 _textureResolution,int _MaxPixel)
    {
		finalList = _finalList;
		insertList = _insertList;
		maskList = _maskList;
		insertRect = _insertRect;
		finalResolution = _finalResolution;
		textureResolution = _textureResolution;
		maxPixel = _MaxPixel;
		colorAdjust = _ColorAdjust;
    }

    protected override void Start()
    {

	}

    protected override IEnumerator workerMethod()
    {			
		int startingIndexAtlas = 0;
		int startingIndexTexture = 0;
		float pixelCount = 0;
		
		int ColorAdjR = colorAdjust.r;
        int ColorAdjG = colorAdjust.g;
        int ColorAdjB = colorAdjust.b;
        int ColorAdjA = colorAdjust.a;
		
		for(float y = 0; y < insertRect.height; y++){
			startingIndexAtlas = Vector2ToIndex(insertRect.x,y + insertRect.y,finalResolution);
			startingIndexTexture = Vector2ToIndex(0,y,textureResolution);		
			
			for(float x = 0; x < insertRect.width; x++){		
				try{
				//To correct value change, might be temporary
				finalList[startingIndexAtlas] = Color32.Lerp(finalList[startingIndexAtlas],new Color32(
					(byte)(insertList[startingIndexTexture].r * ColorAdjR >> 8),
					(byte)(insertList[startingIndexTexture].g * ColorAdjG >> 8),
					(byte)(insertList[startingIndexTexture].b * ColorAdjB >> 8),
					(byte)(insertList[startingIndexTexture].a * ColorAdjA >> 8)
					),maskList[startingIndexTexture].a/255.0f);
				}
				catch ( IndexOutOfRangeException /*e*/ ) { 
				//	Debug.LogError ( startingIndexAtlas.ToString()+"/"+finalList.Length.ToString()+" ."+ e ); 
				}


//				int mask = maskList[startingIndexTexture].a;
//	            int inverseRatio = 255 - mask;
//				
//	            finalList[startingIndexAtlas] = new Color32(
//	            (byte)((finalList[startingIndexAtlas].r * inverseRatio >> 8) + (insertList[startingIndexTexture].r * ColorAdjR * mask >> 16)),
//	            (byte)((finalList[startingIndexAtlas].g * inverseRatio >> 8) + (insertList[startingIndexTexture].g * ColorAdjG * mask >> 16)),
//	            (byte)((finalList[startingIndexAtlas].b * inverseRatio >> 8) + (insertList[startingIndexTexture].b * ColorAdjB * mask >> 16)),
//	            (byte)((finalList[startingIndexAtlas].a * inverseRatio >> 8) + (insertList[startingIndexTexture].a * ColorAdjA * mask >> 16)));
				
				startingIndexAtlas ++;
				startingIndexTexture ++;
			}
			pixelCount += insertRect.width;
			
			if(pixelCount > maxPixel){
				yield return null;
				pixelCount = 0;
			}
		}
	
		yield return null;	
    }
	

	
	public Vector2 IndexToVector2(int index,Vector2 gridSize){
		if(index > (gridSize.x*gridSize.y)-1 || index < 0){
			return new Vector2(-1,-1);
		}

		float tempVectorX = index % gridSize.x;
		float tempVectorY = (index - tempVectorX) / gridSize.x;
		
		return new Vector2(tempVectorX,tempVectorY);
	}
	
	public int Vector2ToIndex(float nodePositionX,float nodePositionY,Vector2 gridSize){
		if(nodePositionX >= gridSize.x || nodePositionX < 0 || nodePositionY >= gridSize.y || nodePositionY < 0){
			return -1;
		}
	
		int tempIndex;
		tempIndex = Mathf.FloorToInt((nodePositionY * gridSize.x) + nodePositionX);
	
		return tempIndex;
	}
	
	
    protected override void Stop()
    {
		finalList = null;
		insertList = null; 
    }
}