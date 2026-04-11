using System.ComponentModel.DataAnnotations;


namespace Word.API.Contracts.Tests;

public class StartTestRequest
{
    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }
}
