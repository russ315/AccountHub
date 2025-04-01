using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using AccountHub.Domain.Exceptions;
using AccountHub.Domain.Models;

namespace AccountHub.Domain.ValueObjects;

public class GameCredential
{
    public CredentialType Type { get; }
    public string Value { get; }
    
    public bool IsEncrypted { get; }
    public DateTime? ExpiresAt { get; }

    // For deserialization
    public GameCredential(CredentialType type, string value, bool isEncrypted, DateTime? expiresAt = null)
    {
        Type = type;
        Value = value;
        IsEncrypted = isEncrypted;
        ExpiresAt = expiresAt;
    }

    public static GameCredential Create(CredentialType type, string value, bool encrypt = true, DateTime? expiresAt = null)
    {
        ValidateCredential(type, value);

        if (encrypt && ShouldEncrypt(type))
        {
            value = EncryptValue(value);
        }

        return new GameCredential(type, value, encrypt && ShouldEncrypt(type), expiresAt);
    }

    public string GetDecryptedValue()
    {
        if (!IsEncrypted) return Value;
        return DecryptValue(Value);
    }

    public bool IsExpired() => ExpiresAt.HasValue && ExpiresAt.Value < DateTime.UtcNow;

    private static void ValidateCredential(CredentialType type, string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new BadRequestException("Invalid credential","Credential value cannot be null or empty");

        switch (type)
        {
            case CredentialType.Email:
                if (!new EmailAddressAttribute().IsValid(value))
                    throw new BadRequestException("Invalid credential", $"Credential value is not valid:{value}.Credential type is {type}");
                break;

            case CredentialType.PhoneNumber:
                if (!new PhoneAttribute().IsValid(value))
                    throw new BadRequestException("Invalid credential", $"Credential value is not valid:{value}.Credential type is {type}");
                break;

            case CredentialType.SteamId:
                if (!ulong.TryParse(value, out _))
                    throw new BadRequestException("Invalid credential",$"Credential value is not valid:{value}.Credential type is {type}");
                break;

            case CredentialType.Token:
                if (string.IsNullOrWhiteSpace(value))
                    throw new BadRequestException("Token cannot be empty", $"Credential value is not valid:{value}.Credential type is {type}");
                break;

            case CredentialType.Password:
                if (value.Length < 8)
                    throw new BadRequestException("Password must be at least 8 characters", $"Credential value is not valid:{value}.Credential type is {type}");
                break;

            case CredentialType.TwoFactorSecret:
                if (value.Length < 16)
                    throw new BadRequestException("Two-factor secret must be at least 16 characters", $"Credential value is not valid:{value}.Credential type is {type}");
                break;

            case CredentialType.BackupCodes:
                var codes = value.Split(',', ';');
                if (codes.Length < 1)
                    throw new BadRequestException("At least one backup code is required", $"Credential value is not valid:{value}.Credential type is {type}");
                break;
        }
    }

    private static bool ShouldEncrypt(CredentialType type)
    {
        return type switch
        {
            CredentialType.Password => true,
            CredentialType.Token => true,
            CredentialType.SteamApiKey => true,
            CredentialType.TwoFactorSecret => true,
            CredentialType.RecoveryCode => true,
            CredentialType.BackupCodes => true,
            _ => false
        };
    }

    private static string EncryptValue(string value)
    {
        try
        {
            
            string encryptionKey = "MySecureEncryptionKey12345!";
            
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using MemoryStream memoryStream = new();
                using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);
                using (StreamWriter streamWriter = new(cryptoStream))
                {
                    streamWriter.Write(value);
                }

                array = memoryStream.ToArray();
            }

            return Convert.ToBase64String(array);
        }
        catch
        {
            // Fallback to simple Base64 encoding if advanced encryption fails
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        }
    }

    private static string DecryptValue(string encryptedValue)
    {
        try
        {
            // In production, use a secure key management system
            // This is a simplified version for demonstration
            string encryptionKey = "YourSecureEncryptionKey12345!";
            
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(encryptedValue);

            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using MemoryStream memoryStream = new(buffer);
            using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
            using StreamReader streamReader = new(cryptoStream);
            
            return streamReader.ReadToEnd();
        }
        catch
        {
            // Fallback to simple Base64 decoding if advanced decryption fails
            return Encoding.UTF8.GetString(Convert.FromBase64String(encryptedValue));
        }
    }

    public override string ToString() => Value;
} 