using System.ComponentModel.DataAnnotations;

namespace BicycleService.Milestone05.WebAPI.Dtos;

public class CreateServiceJobRequest : IValidatableObject
{
	[Required]
	[StringLength(100)]
	public string CustomerName { get; set; } = string.Empty;

	[Required]
	[StringLength(100)]
	public string BicycleModel { get; set; } = string.Empty;

	[Required]
	[StringLength(500)]
	public string Issue { get; set; } = string.Empty;

	[Range(typeof(decimal), "0", "79228162514264337593543950335")]
	public decimal EstimatedCost { get; set; }

	public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
	{
		if (string.IsNullOrWhiteSpace(CustomerName))
		{
			yield return new ValidationResult("Customer name is required.", new[] { nameof(CustomerName) });
		}

		if (string.IsNullOrWhiteSpace(BicycleModel))
		{
			yield return new ValidationResult("Bicycle model is required.", new[] { nameof(BicycleModel) });
		}

		if (string.IsNullOrWhiteSpace(Issue))
		{
			yield return new ValidationResult("Issue is required.", new[] { nameof(Issue) });
		}
	}
}

public class UpdateServiceJobRequest : IValidatableObject
{
	[Required]
	[StringLength(500)]
	public string Issue { get; set; } = string.Empty;

	[Required]
	[RegularExpression("^(Requested|In Progress|Completed|Cancelled)$")]
	public string Status { get; set; } = "Requested";

	[Range(typeof(decimal), "0", "79228162514264337593543950335")]
	public decimal EstimatedCost { get; set; }

	[Range(typeof(decimal), "0", "79228162514264337593543950335")]
	public decimal? FinalCost { get; set; }

	public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
	{
		if (string.IsNullOrWhiteSpace(Issue))
		{
			yield return new ValidationResult("Issue is required.", new[] { nameof(Issue) });
		}
	}
}

public class ServiceJobResponse
{
	public int Id { get; set; }
	public string CustomerName { get; set; } = string.Empty;
	public string BicycleModel { get; set; } = string.Empty;
	public string Issue { get; set; } = string.Empty;
	public string Status { get; set; } = string.Empty;
	public decimal EstimatedCost { get; set; }
	public decimal? FinalCost { get; set; }
}