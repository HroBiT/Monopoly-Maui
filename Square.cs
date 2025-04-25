namespace MonopolyBoard
{
	public enum SquareType
	{
		Property,
		GoToJail,
		Jail,
		FreeParking,
		Go,
		Chance,
		CommunityChest,
		Tax
	}

	public class Square
	{
		public int Position { get; set; }
		public string Name { get; set; }
		public SquareType Type { get; set; }
		public Property? Property { get; set; }

		public Square(int position, string name, SquareType type)
		{
			Position = position;
			Name = name;
			Type = type;
			Property = null;
		}

		public Square(int position, string name, SquareType type, Property property)
		{
			Position = position;
			Name = name;
			Type = type;
			Property = property;
		}
	}
}
