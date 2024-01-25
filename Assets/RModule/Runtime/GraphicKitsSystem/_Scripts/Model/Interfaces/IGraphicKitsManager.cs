namespace RModule.Runtime.GraphicKitsSystem {

	public interface IGraphicKitsManager<GraphicKitKey, GraphicKitValueType> {
		void AddElement(GraphicKitKey graphicKitsKey, IGraphicKitElement graphicKitElement);
		void RemoveElement(GraphicKitKey graphicKitsKey, IGraphicKitElement graphicKitElement);
		void UpdateElementsView(GraphicKitKey graphicKitKey);
		T GetGraphicKitValue<T>(GraphicKitKey graphicKitKey, string graphiKitValueKey);
	}

}
