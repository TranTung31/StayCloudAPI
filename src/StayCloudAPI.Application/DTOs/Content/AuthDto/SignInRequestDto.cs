namespace StayCloudAPI.Application.DTOs.Content.AuthDto
{
    public class SignInRequestDto
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
