namespace Iced.Intel
{
	public struct Label {
		public Label(string name, ulong rip) {
			Name = name;
			RIP = rip;
		}
		
		public readonly string Name;

		public readonly ulong RIP;

		public bool IsEmpty => RIP == 0;
	}
}
