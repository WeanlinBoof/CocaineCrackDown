using Nez.ImGuiTools.TypeInspectors;

using System.Collections.Generic;


namespace Nez.ImGuiTools.ObjectInspectors {
	public abstract class AbstractComponentInspector : IComponentInspector {
		public abstract Entity Entity { get; }
		public abstract Component Component { get; }

		protected List<AbstractTypeInspector> _inspectors;
		protected int _scopeId = NezImGui.GetScopeId();

		public abstract void Draw();
	}
}