using System.Collections.Generic;
namespace BugTrakerAPI.Model.ReturnModel;

public class CommonResponse {
    public bool? success { get; set;} = false;
    public List<string>? errors {get; set;}
}