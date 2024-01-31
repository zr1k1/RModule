namespace RModule.Runtime.GraphicKitsSystem {
	using System.Collections.Generic;
	using System.Collections;

	public interface IGraphicKitsManager<GraphicKitKey, GraphicKitValueType> {
		void AddElement(GraphicKitKey graphicKitsKey, IGraphicKitElement graphicKitElement);
		void RemoveElement(GraphicKitKey graphicKitsKey, IGraphicKitElement graphicKitElement);
		void UpdateElementsView(GraphicKitKey graphicKitKey);
		T GetGraphicKitValue<T>(GraphicKitKey graphicKitKey, string graphiKitValueKey);
	}
}
