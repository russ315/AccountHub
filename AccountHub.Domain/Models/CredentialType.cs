namespace AccountHub.Domain.Models;

public enum CredentialType
{
    Login,
    Password,
    Token,
    SteamId,
    SteamApiKey,
    BackupCodes,
    Email,
    PhoneNumber,
    TwoFactorSecret,
    RecoveryCode,
    Custom
} 