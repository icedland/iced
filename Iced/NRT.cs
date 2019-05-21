namespace System.Runtime.CompilerServices {
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
	sealed class NotNullWhenFalseAttribute : Attribute {
		public NotNullWhenFalseAttribute() { }
	}
}
