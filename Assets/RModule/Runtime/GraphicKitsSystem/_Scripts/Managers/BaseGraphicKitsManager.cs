using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RModule.Runtime.GraphicKitsSystem {

	/// <summary>
	/// <para>
	/// GraphicKitKey - Used kits types, like Hero or Location.
	/// </para>
	/// <para>
	/// GraphicKitValueType - Used graphic type to get, like SpriteAddress or Color (You will choise on your graphickitsetter conponent).
	/// </para>
	/// <para>
	/// Setup instruction:
	/// </para>
	/// <para>
	/// 1. Create and add to game object GraphicKitsManager script inherited from BaseGraphicKitsManager.
	/// </para>
	/// <para>
	/// 2. Create graphic kits configs (Example on rigth click -> Create -> RModule/Examples/AppConfigs/GraphicKitsConfigs/...).
	/// </para>
	/// <para>
	/// 3. Setup configs into your GraphicKitsManager game object.
	/// <para>
	/// Сreate YourGraphicSetter with using RModule.Runtime.GraphicKitsSystem, inherit from BaseGraphicSetter(YourGraphicKitKey, YourGraphicKitValueType) and setup to your game object.
	/// </para>
	/// </para>
	/// <para>
	/// You can check ExampleGraphicKitsManager, ExampleGraphicSetter for understand
	/// </para>
	/// </summary>

	public class BaseGraphicKitsManager<GraphicKitKey, GraphicKitValueType> : SingletonMonoBehaviour<BaseGraphicKitsManager<GraphicKitKey, GraphicKitValueType>>
		, IGraphicKitsManager<GraphicKitKey, GraphicKitValueType>

		where GraphicKitKey : Enum {

		// Outlets
		[SerializeField] protected SerializableDictionary<GraphicKitKey, GraphicKitsData> _graphicKitsDict = default;

		// Private vars
		protected IGraphicKitDataProvider _graphicKitDataProvider;
		protected Dictionary<GraphicKitKey, List<IGraphicKitElement>> _graphicKitElements = new Dictionary<GraphicKitKey, List<IGraphicKitElement>>();
		protected bool _initializeFinished = false;

		// Classes
		[Serializable]
		public class GraphicKitsData {
			public GraphicKitsConfig GraphicKitsConfig => _graphicKitsConfig;
			public GraphicKitConfig CurrentGraphicKitConfig => _currentGraphicKitConfig;

			[SerializeField] GraphicKitsConfig _graphicKitsConfig = default;
			[SerializeField] GraphicKitConfig _currentGraphicKitConfig = default;
			[SerializeField] GraphicKitConfig _debugGraphicKitConfig = default;

			[Header("Debug")]
			[SerializeField] internal bool _useDebug = default;

			internal bool TrySetCurrentGraphicKey(GraphicKitConfig currentGraphicKit) {
				_currentGraphicKitConfig = currentGraphicKit;

				return _currentGraphicKitConfig.Key != currentGraphicKit.Key;
			}

			internal void ChangeCurrentToDebugGraphicConfig() {
				_currentGraphicKitConfig = _debugGraphicKitConfig;
			}

			public GraphicKitConfig GetGraphicConfigByKey(string graphicConfigKey) {
				return _graphicKitsConfig.GetGraphicKitConfigByNameKeyOrDefault(graphicConfigKey);
			}
		}

		public interface IGraphicKitDataProvider : IKeyValueSetter<GraphicKitKey, string> , IKeyValueGetter<GraphicKitKey, string> {
		}

		public IEnumerator Initialize(IGraphicKitDataProvider graphicKitDataProvider) {
			_graphicKitDataProvider = graphicKitDataProvider;
			SetupGraphicKits();

			_initializeFinished = true;

			yield return null;
			Debug.Log("GraphicKitsManager : Initialized");
		}

		public override bool IsInitialized() {
			return _initializeFinished;
		}

		void SetupGraphicKits() {
			_graphicKitElements = new Dictionary<GraphicKitKey, List<IGraphicKitElement>>();
			foreach (var keyPair in _graphicKitsDict) {
				GraphicKitKey graphicKitsKey = keyPair.Key;
				SetupGraphicKit(graphicKitsKey, _graphicKitDataProvider.GetValue(graphicKitsKey));
			}
		}

		void SetupGraphicKit(GraphicKitKey graphicKitsKey, string graphicKitKey) {
			if (!_graphicKitsDict.ContainsKey(graphicKitsKey)) {
				Debug.LogError($"Add Graphic kits for key = {graphicKitsKey}");
				return;
			}
			_graphicKitElements[graphicKitsKey] = new List<IGraphicKitElement>();

			var graphicKits = _graphicKitsDict[graphicKitsKey];

			if (_graphicKitsDict[graphicKitsKey]._useDebug && Application.isEditor) {
				graphicKits.ChangeCurrentToDebugGraphicConfig();
			} else {
				graphicKits.TrySetCurrentGraphicKey(graphicKits.GetGraphicConfigByKey(graphicKitKey));
			}
		}

		public virtual void ChangeAndSaveGraphicKitKey(GraphicKitKey graphicKitsKey, string graphicKitKey) {
			_graphicKitDataProvider.SetValue(graphicKitsKey, graphicKitKey);
			SetupGraphicKit(graphicKitsKey, graphicKitKey);
		}

		public virtual void UpdateElementsView(GraphicKitKey graphicKitKey) {
			foreach (var element in _graphicKitElements[graphicKitKey])
				element.UpdateView();
		}

		public virtual void AddElement(GraphicKitKey graphicKitsKey, IGraphicKitElement graphicKitElement) {
			if (!_graphicKitElements.ContainsKey(graphicKitsKey)) {
				_graphicKitElements[graphicKitsKey].Add(graphicKitElement);
			}
		}

		public virtual void RemoveElement(GraphicKitKey graphicKitsKey, IGraphicKitElement graphicKitElement) {
			if (_graphicKitElements.ContainsKey(graphicKitsKey)) {
				_graphicKitElements[graphicKitsKey].Remove(graphicKitElement);
			}
		}

		public virtual T GetGraphicKitValue<T>(GraphicKitKey graphicKitKey, string graphicKitValueKey) {
			_graphicKitsDict[graphicKitKey].CurrentGraphicKitConfig.TryGetValue(graphicKitValueKey, out T value);

			return value;
		}

		public GraphicKitConfig GetCurrentGraphicKitConfig(GraphicKitKey graphicKitKey) {
			return _graphicKitsDict[graphicKitKey].CurrentGraphicKitConfig;
		}
	}
}