using UnityEngine;
using System.Collections;
using UnityEditor;


public class DKUMAVersionWin : EditorWindow {

//	Color Green = new Color (0.8f, 1f, 0.8f, 1);
//	Color Red = new Color (0.9f, 0.5f, 0.5f);

	Vector2 scroll;

	void OnGUI () {
		this.minSize = new Vector2(370, 500);

		#region fonts variables
		var bold = new GUIStyle ("label");
		var boldFold = new GUIStyle ("foldout");
		bold.fontStyle = FontStyle.Bold;
		bold.fontSize = 14;
		boldFold.fontStyle = FontStyle.Bold;
		var Slim = new GUIStyle ("label");
		Slim.fontStyle = FontStyle.Normal;
		Slim.fontSize = 10;	
		var style = new GUIStyle ("label");
		style.wordWrap = true;
		#endregion fonts variables

		GUILayout.Label ( "DK U.M.A. Editor v1650 information", "toolbarbutton", GUILayout.ExpandWidth (true));

		using (new ScrollView(ref scroll)) {

			GUI.color = Color.white;
			GUILayout.TextField("- Works with Unity 5 and UMA 1.2." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			GUILayout.TextField("- The new RPG Avatar Editor in its version 0.7, some options are not ready, but you already can use it. More details further." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			GUILayout.TextField("- Manage the head, body and equipment of your avatar in a single window." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			GUILayout.TextField("- The overlays and color presets are displayed and also the various information about the avatar and elements for a simple look reading." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			GUILayout.TextField("- Advanced filtered lists about race and gender, merging the slot elements and the overlay elements eligiable for the selected part of your avatar." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			GUILayout.TextField("- All is dynamic, displaying only the usable content." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			GUILayout.TextField("- Advanced building and rebuilding of your RPG avatars." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			GUILayout.TextField("- Highly evolutive RPG Avatar Editor, with jewels management and also some blood and dirt options, in a near future." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			GUILayout.TextField("- Integration oriented scripts composition, enabling you to easily use any of the DK UMA RPG possibilities in your own game system." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			GUILayout.TextField("- More automated process for the detection of your DK contents." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			GUILayout.TextField("- A very proper system for the cleaning of the legacy slots when equiping elements, for an example, an helmet removes the hair and the chouchou." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			GUILayout.TextField("- Only black eyelash (For the moment)." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			GUILayout.TextField("- Belt and armband management." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			GUILayout.TextField("- A covering wear is now able to hide more elements such as the belt or the new Armband elements." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			GUILayout.TextField("- Lot more modifications." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

		}
	}
}
