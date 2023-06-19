namespace UserAndShopAuth.Identity.Contracts
{
    public interface IJwtService
    {
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
        Task<AccessToken> GenerateAsync(User user);
        string GenerateRefreshToken();
    }
}