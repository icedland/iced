namespace Iced.Intel
{
	public struct Label {
		public Label(string name, InstructionBlock block) {
			Name = name;
			Block = block;
		}
		
		public readonly string Name;

		public readonly InstructionBlock Block;

		public bool IsEmpty => Block.Instructions == null;
	}
}
