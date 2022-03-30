using System.Collections.Generic;
namespace BugTrakerAPI.Model.ReturnModel;

public class CommonResponse {
    public bool? status {get; set;}
    public string? errors {get; set;}
}