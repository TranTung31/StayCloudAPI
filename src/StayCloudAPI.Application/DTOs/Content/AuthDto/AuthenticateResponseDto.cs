namespace StayCloudAPI.Application.DTOs.Content.AuthDto
{
    public class AuthenticateResponseDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
