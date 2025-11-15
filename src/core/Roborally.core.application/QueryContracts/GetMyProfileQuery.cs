
using FastEndpoints;

namespace Roborally.core.application.QueryContracts;

public class GetMyProfileQuery : ICommand<GetMyProfileResponse>{
    public required string Username { get; set; }

}

public class GetMyProfileResponse {
    public string Username { get; set; }
    public DateOnly Birthday { get; set; }
    public int Rating { get; set; }
}