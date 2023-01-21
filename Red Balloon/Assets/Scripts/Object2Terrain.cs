using UnityEngine;
using UnityEditor;
 
public class Object2Terrain : EditorWindow {
 
	[MenuItem("Terrain/Object to Terrain", false, 2000)] static void OpenWindow () {
 
		EditorWindow.GetWindow<Object2Terrain>(true);
	}
 
	private int _resolution = 512;
	private Vector3 _addTerrain;
	private int _bottomTopRadioSelected = 0;
	private static readonly string[] BottomTopRadio = { "Bottom Up", "Top Down"};
	private float _shiftHeight = 0f;

	private void OnGUI () {
 
		_resolution = EditorGUILayout.IntField("Resolution", _resolution);
		_addTerrain = EditorGUILayout.Vector3Field("Add terrain", _addTerrain);
		_shiftHeight = EditorGUILayout.Slider("Shift height", _shiftHeight, -1f, 1f);
		_bottomTopRadioSelected = GUILayout.SelectionGrid(_bottomTopRadioSelected, BottomTopRadio, BottomTopRadio.Length, EditorStyles.radioButton);
 
		if(GUILayout.Button("Create Terrain")){
			if(Selection.activeGameObject == null){
				EditorUtility.DisplayDialog("No object selected", "Please select an object.", "Ok");
				return;
			}
			
			CreateTerrain();
		}
	}

	private delegate void CleanUp();

	private void CreateTerrain(){	
 
		//fire up the progress bar
		ShowProgressBar(1, 100);
 
		TerrainData terrain = new TerrainData();
		terrain.heightmapResolution = _resolution;
		GameObject terrainObject = Terrain.CreateTerrainGameObject(terrain);
 
		Undo.RegisterCreatedObjectUndo(terrainObject, "Object to Terrain");
 
		MeshCollider collider = Selection.activeGameObject.GetComponent<MeshCollider>();
		CleanUp cleanUp = null;
 
		//Add a collider to our source object if it does not exist.
		//Otherwise raycasting doesn't work.
		if(!collider){
 
			collider = Selection.activeGameObject.AddComponent<MeshCollider>();
			cleanUp = () => DestroyImmediate(collider);
		}
 
		var bounds = collider.bounds;	
		var sizeFactor = collider.bounds.size.y / (collider.bounds.size.y + _addTerrain.y);
		terrain.size = collider.bounds.size + _addTerrain;
		bounds.size = new Vector3(terrain.size.x, collider.bounds.size.y, terrain.size.z);
 
		// Do raycasting samples over the object to see what terrain heights should be
		var heights = new float[terrain.heightmapResolution, terrain.heightmapResolution];	
		var ray = new Ray(new Vector3(bounds.min.x, bounds.max.y + bounds.size.y, bounds.min.z), -Vector3.up);
		RaycastHit hit;
		var meshHeightInverse = 1 / bounds.size.y;
		var rayOrigin = ray.origin;
 
		var maxHeight = heights.GetLength(0);
		var maxLength = heights.GetLength(1);
 
		var stepXZ = new Vector2(bounds.size.x / maxLength, bounds.size.z / maxHeight);
 
		for(int zCount = 0; zCount < maxHeight; zCount++){
 
			ShowProgressBar(zCount, maxHeight);
 
			for(int xCount = 0; xCount < maxLength; xCount++){
 
				float height = 0.0f;
 
				if(collider.Raycast(ray, out hit, bounds.size.y * 3)){
 
					height = (hit.point.y - bounds.min.y) * meshHeightInverse;
					height += _shiftHeight;
 
					//bottom up
					if(_bottomTopRadioSelected == 0){
 
						height *= sizeFactor;
					}
 
					//clamp
					if(height < 0){
 
						height = 0;
					}
				}
 
				heights[zCount, xCount] = height;
           		rayOrigin.x += stepXZ[0];
           		ray.origin = rayOrigin;
			}
 
			rayOrigin.z += stepXZ[1];
      		rayOrigin.x = bounds.min.x;
      		ray.origin = rayOrigin;
		}
 
		terrain.SetHeights(0, 0, heights);
 
		EditorUtility.ClearProgressBar();
 
		if(cleanUp != null){
 
			cleanUp();    
		}
	}
 
    void ShowProgressBar(float progress, float maxProgress){
 
		float p = progress / maxProgress;
		EditorUtility.DisplayProgressBar("Creating Terrain...", Mathf.RoundToInt(p * 100f)+ " %", p);
	}
}