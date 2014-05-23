using UnityEngine;
using UnityEditor;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using Microsoft.CSharp;

public class Mono4UnityEditorWindow : EditorWindow {
	#region public class methods
	[MenuItem ("Window/Mono4Unity/Build DLL")]
	public static void ShowWindow () {
		Mono4UnityEditorWindow w = EditorWindow.GetWindow<Mono4UnityEditorWindow>("Mono4Unity", true);

		if (PlayerPrefs.HasKey (Mono4UnityEditorWindow.PlayerPrefs_BuildBasePath)) {
			w.BuildPath = PlayerPrefs.GetString (Mono4UnityEditorWindow.PlayerPrefs_BuildBasePath);

			DirectoryInfo directory = new DirectoryInfo(w.BuildPath);
			if (!directory.Exists){
				w.BuildPath = new DirectoryInfo(Application.dataPath).Parent.FullName;
			}
		} else {
			w.BuildPath = new DirectoryInfo(Application.dataPath).Parent.FullName;
		}
	}
	#endregion

	#region protected class properties
	protected static readonly string MonoPath = 
	Application.dataPath	+ Path.DirectorySeparatorChar + 
	"Mono4Unity"			+ Path.DirectorySeparatorChar + 
	"Runtime"				+ Path.DirectorySeparatorChar;

	protected static readonly string MonoDll = 
	"Mono4Unity.dll";

	protected static readonly string PlayerPrefs_BuildBasePath = 
	"BuildBasePath";
	#endregion

	#region public instance properties
	public string BuildPath{
		get{
			return this._buildPath;
		}
		set{
			this._buildPath = value;
		}
	}
	#endregion

	#region public instance properties
	protected string _buildPath;
	protected bool _compiling = false;
	#endregion

	#region protected instance methods
	protected string[] GetSourceFiles(string sourceFolder){
		DirectoryInfo dir = new DirectoryInfo(sourceFolder);
		List<string> files = new List<string> ();

		if (dir.Exists) {
			foreach (FileInfo file in dir.GetFiles("*.cs", SearchOption.AllDirectories)){
				files.Add(file.FullName);
			}
		}

		return files.ToArray ();
	}
	#endregion

	#region EditorWindow methods
	public virtual void OnGUI(){
		GUILayout.Space (30);

		if (this._compiling) {
			GUILayout.Label ("Compiling...");
		}else{
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Build Path:");
			this.BuildPath = GUILayout.TextField (this.BuildPath != null ? this.BuildPath : string.Empty);

			if (GUILayout.Button ("+")) {
				this.BuildPath = EditorUtility.SaveFolderPanel(
					"Select the folder where the DLL will be created",
					string.IsNullOrEmpty(this.BuildPath)? Application.dataPath : this.BuildPath,
					string.Empty
				);
			}
			GUILayout.EndHorizontal ();
			GUILayout.Space (10);

			if (GUILayout.Button ("Build DLL")) {
				if (string.IsNullOrEmpty(this.BuildPath)){
					EditorUtility.DisplayDialog(
						"Build path not selected",
						"You must select the folder where the DLL will be created.",
						"OK"
					);
				}else{
					StringBuilder warnings = new StringBuilder();
					StringBuilder errors = new StringBuilder();
					this._compiling = true;

					string monoDll = new FileInfo(
						this.BuildPath + 
						Path.DirectorySeparatorChar + 
						Mono4UnityEditorWindow.MonoDll
					).FullName;

					PlayerPrefs.SetString (
						Mono4UnityEditorWindow.PlayerPrefs_BuildBasePath,
						this.BuildPath
					);

					Debug.Log("Generating DLL...");
					string[] results = EditorUtility.CompileCSharp(
						this.GetSourceFiles(Mono4UnityEditorWindow.MonoPath),
						new string[]{"System.dll", "mscorlib.dll"},
						new string[0],
						new FileInfo(monoDll).FullName
					);
					
					foreach (string result in results){
						if (result.Contains("warning")){
							warnings.AppendLine(result);
						}else if (result.Contains("error")){
							errors.AppendLine(result);
						}
					}
					
					if (warnings.Length > 0){
						Debug.LogWarning(warnings.ToString());
					}

					if (errors.Length > 0){
						Debug.LogError(errors.ToString());
					}else{
						Debug.Log("The DLL were generated successfully"); 
					}

					this._compiling = false;
				}
			}
		}
	}
	#endregion
}
