using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Habby.Storage {
	internal class StorageEditor : EditorWindow {
		private static int _databaseIndex;
		private static Vector2 _scrollValue;
		private static bool[] _foldOutFlags;
		private static string _editingContainerKey;
		private static string _editingPropertyKey;
		private static string _editingPropertyValue;

		[MenuItem("Habby/Storage")]
		public static void ShowEditor() {
			var wnd = GetWindow<StorageEditor>();
			wnd.titleContent = new GUIContent("Storage");
		}

		private void OnGUI() {
			var e = Event.current;
			var databasesKeys = Storage.GetDatabaseKeys();
			var index = GUILayout.Toolbar(Mathf.Clamp(_databaseIndex, 0, databasesKeys.Count - 1), databasesKeys.ToArray());
			var database = databasesKeys.Count > 0 ? databasesKeys[index] : "";
			var containerKeys = Storage.GetContainerKeys(database);
			var style = GUI.skin.button;
			_databaseIndex = index;
			_scrollValue = EditorGUILayout.BeginScrollView(_scrollValue);
			if (_foldOutFlags == null || _foldOutFlags.Length != containerKeys.Count) {
				_foldOutFlags = new bool[containerKeys.Count];
			}
			if (string.IsNullOrEmpty(_editingPropertyKey) && e.isKey && !e.command && !e.control) {
				e.Use();
			}
			for (var i = 0; i < containerKeys.Count; ++i) {
				var containerKey = containerKeys[i];
				var container = Storage.GetContainer(containerKey, database);
				var rect = GUILayoutUtility.GetRect(0f, float.PositiveInfinity, 0f, 0f);
				rect.x = rect.width - 50f - style.margin.right;
				rect.y += style.margin.top;
				rect.width = 50f;
				rect.height = EditorGUIUtility.singleLineHeight;
				GUI.enabled = !e.isMouse || !rect.Contains(e.mousePosition);
				_foldOutFlags[i] = EditorGUILayout.BeginFoldoutHeaderGroup(_foldOutFlags[i], containerKey);
				GUI.enabled = true;
				if (GUI.Button(rect, "Clear") && DoubleCheck()) {
					Storage.Clear(containerKey, database);
				}
				if (_foldOutFlags[i]) {
					var keys = container.Keys.ToArray();
					++EditorGUI.indentLevel;
					foreach (var key in keys) {
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.PrefixLabel(key);
						if (_editingContainerKey == containerKey && _editingPropertyKey == key) {
							GUI.SetNextControlName("EditingProperty");
							_editingPropertyValue = EditorGUILayout.TextField(_editingPropertyValue);
							GUI.FocusControl("EditingProperty");
							if (GUILayout.Button("Save", GUILayout.Width(50f))) {
								var obj = JsonConvert.DeserializeObject(_editingPropertyValue,
									JsonStorageSerializer.SerializerSettings);
								container.Set(key, obj);
								container.Save();
								CancelEditing();
							}
							if (GUILayout.Button("Cancel", GUILayout.Width(50f))) {
								CancelEditing();
							}
						} else {
							var json = JsonConvert.SerializeObject(container.Get<object>(key).Value,
								JsonStorageSerializer.SerializerSettings);
							GUI.enabled = _editingContainerKey == null || _editingPropertyKey == null;
							EditorGUILayout.TextField(json);
							GUI.enabled = true;
							if (GUILayout.Button("Edit", GUILayout.Width(50f))) {
								_editingContainerKey = containerKey;
								_editingPropertyKey = key;
								_editingPropertyValue = json;
							}
							if (GUILayout.Button("Delete", GUILayout.Width(50f)) && DoubleCheck()) {
								container.Delete(key);
								container.Save();
								CancelEditing();
							}
						}
						EditorGUILayout.EndHorizontal();
					}
					--EditorGUI.indentLevel;
				}
				EditorGUILayout.EndFoldoutHeaderGroup();
			}
			EditorGUILayout.EndScrollView();

			if (GUILayout.Button("Clear Database") && DoubleCheck()) {
				CancelEditing();
				Storage.ClearAll(database);
			}
			if (GUILayout.Button("Clear All Data") && DoubleCheck()) {
				CancelEditing();
				Storage.ClearAll();
			}
			if (GUILayout.Button("Export To Clipboard")) {
				CancelEditing();
				GUIUtility.systemCopyBuffer = Storage.ExportToJson();
			}
			if (GUILayout.Button("Import From Clipboard") && DoubleCheck()) {
				CancelEditing();
				Storage.ImportFromJson(GUIUtility.systemCopyBuffer);
			}
		}

		private bool DoubleCheck() {
			return EditorUtility.DisplayDialog("Storage", "Are you sure?", "Yes", "Cancel");
		}

		private void CancelEditing() {
			_editingContainerKey = null;
			_editingPropertyKey = null;
			_editingPropertyValue = null;
			GUI.FocusControl(null);
		}

		private void OnDisable() {
			CancelEditing();
		}
	}
}