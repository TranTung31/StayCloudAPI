namespace StayCloudAPI.Application.DTOs.Content.AuthDto
{
    public class SignUpRequestDto
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}
