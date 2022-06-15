namespace EFCoreMigrator.UnitTest.Data;

using System.ComponentModel.DataAnnotations;

public class CarModel
{
	public CarModel()
	{
	}

	public CarModel(string model, string year)
	{
		this.Model = model;
		this.Year = year;
	}

	[Key]
	public int Id { get; private set; }

	public string Model { get; }

	public string Year { get; }
}
