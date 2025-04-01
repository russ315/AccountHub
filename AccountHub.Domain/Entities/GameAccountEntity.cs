using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;
using AccountHub.Domain.Events;
using AccountHub.Domain.Models;
using AccountHub.Domain.ValueObjects;

namespace AccountHub.Domain.Entities;

public class GameAccountEntity : BaseEntity
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    [Required]
    public JsonDocument Characteristics { get; private set; }

    [Required]
    [Range(0, double.MaxValue)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; private set; }

    [Required]
    public AccountStatus Status { get; private set; }

    [JsonIgnore]
    public GameEntity? Game { get; private set; }

    [Required]
    public long GameId { get; private set; }

    [JsonIgnore]
    public UserEntity? Seller { get; private set; }

    [Required]
    [MaxLength(450)]
    public string SellerId { get; private set; }

    [Required]
    [MaxLength(450)]
    public string CurrentOwnerId { get; private set; }

    [JsonIgnore]
    public UserEntity? CurrentOwner { get; private set; }

    [Required]
    public ICollection<AccountImageEntity> Images { get; private set; } = new List<AccountImageEntity>();

    // Credential storage in database
    [Required]
    [Column(TypeName = "jsonb")]
    public string CredentialsJson { get; private set; }

    // Transient credential dictionary (not stored directly)
    [NotMapped]
    private Dictionary<CredentialType, GameCredential>? _credentials;

    [NotMapped]
    private Dictionary<CredentialType, GameCredential> Credentials 
    {
        get
        {
            if (_credentials == null)
            {
                DeserializeCredentials();
            }
            return _credentials!;
        }
    }

    public GameAccountEntity(
        JsonDocument characteristics,
        decimal price,
        AccountStatus status,
        long gameId,
        string sellerId,
        string currentOwnerId)
    {
        Characteristics = characteristics;
        Price = price;
        Status = status;
        GameId = gameId;
        SellerId = sellerId;
        CurrentOwnerId = currentOwnerId;
        CredentialsJson = "{}";
        _credentials = new Dictionary<CredentialType, GameCredential>();

        _domainEvents.Add(new GameAccountCreatedEvent(Id, sellerId, price, status));
    }

    public void UpdateStatus(AccountStatus newStatus)
    {
        if (Status == newStatus) return;

        var oldStatus = Status;
        Status = newStatus;
        _domainEvents.Add(new GameAccountStatusChangedEvent(Id, oldStatus, newStatus));
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (Price == newPrice) return;

        var oldPrice = Price;
        Price = newPrice;
        _domainEvents.Add(new GameAccountUpdatedEvent(Id));
    }

    public void TransferOwnership(string newOwnerId)
    {
        if (CurrentOwnerId == newOwnerId) return;

        var previousOwnerId = CurrentOwnerId;
        CurrentOwnerId = newOwnerId;
        _domainEvents.Add(new GameAccountSoldEvent(Id, previousOwnerId, newOwnerId, Price));
    }

    // Credential management methods
    public void AddCredential(CredentialType type, string value, bool encrypt = true, DateTime? expiresAt = null)
    {
        var credential = GameCredential.Create(type, value, encrypt, expiresAt);
        Credentials[type] = credential;
        SerializeCredentials();
        
        _domainEvents.Add(new GameAccountCredentialsUpdatedEvent(
            Id,
            type.ToString(),
            "Added"
        ));
    }

    public void UpdateCredential(CredentialType type, string value, bool encrypt = true, DateTime? expiresAt = null)
    {
        if (!Credentials.ContainsKey(type))
            throw new KeyNotFoundException($"Credential of type {type} not found");

        Credentials[type] = GameCredential.Create(type, value, encrypt, expiresAt);
        SerializeCredentials();

        _domainEvents.Add(new GameAccountCredentialsUpdatedEvent(
            Id,
            type.ToString(),
            "Updated"
        ));
    }

    public void RemoveCredential(CredentialType type)
    {
        if (!Credentials.ContainsKey(type))
            return;

        Credentials.Remove(type);
        SerializeCredentials();

        _domainEvents.Add(new GameAccountCredentialsUpdatedEvent(
            Id,
            type.ToString(),
            "Removed"
        ));
    }

    public string? GetCredentialValue(CredentialType type)
    {
        if (!Credentials.TryGetValue(type, out var credential))
            return null;

        if (credential.IsExpired())
            return null;

        return credential.GetDecryptedValue();
    }

    public bool HasCredential(CredentialType type)
    {
        return Credentials.ContainsKey(type) && !Credentials[type].IsExpired();
    }

    public IEnumerable<CredentialType> GetAvailableCredentialTypes()
    {
        return Credentials.Keys.ToList();
    }

    private void SerializeCredentials()
    {
        var serializableDict = Credentials.ToDictionary(
            kv => kv.Key.ToString(),
            kv => new CredentialDto
            {
                Value = kv.Value.Value,
                IsEncrypted = kv.Value.IsEncrypted,
                ExpiresAt = kv.Value.ExpiresAt
            }
        );

        CredentialsJson = JsonSerializer.Serialize(serializableDict);
    }

    private void DeserializeCredentials()
    {
        _credentials = new Dictionary<CredentialType, GameCredential>();
        
        if (string.IsNullOrEmpty(CredentialsJson) || CredentialsJson == "{}")
            return;

        try
        {
            var serializableDict = JsonSerializer.Deserialize<Dictionary<string, CredentialDto>>(CredentialsJson);
            
            if (serializableDict == null)
                return;

            foreach (var item in serializableDict)
            {
                if (Enum.TryParse<CredentialType>(item.Key, out var credType))
                {
                    var dto = item.Value;
                    var credential = new GameCredential(
                        credType, 
                        dto.Value, 
                        dto.IsEncrypted, 
                        dto.ExpiresAt);
                        
                    _credentials[credType] = credential;
                }
            }
        }
        catch
        {
            // If deserialization fails, start with empty dictionary
            _credentials = new Dictionary<CredentialType, GameCredential>();
        }
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void Update(JsonDocument characteristics, decimal price, AccountStatus status)
    {
        var hasChanges = false;
        
        if (Characteristics != characteristics)
        {
            Characteristics = characteristics;
            hasChanges = true;
        }

        if (Price != price)
        {
            var oldPrice = Price;
            Price = price;
            hasChanges = true;
            _domainEvents.Add(new GameAccountUpdatedEvent(Id));
        }

        if (Status != status)
        {
            var oldStatus = Status;
            Status = status;
            hasChanges = true;
            _domainEvents.Add(new GameAccountStatusChangedEvent(Id, oldStatus, status));
        }

        if (hasChanges)
        {
            _domainEvents.Add(new GameAccountUpdatedEvent(Id));
        }
    }

    // Helper DTO for serialization
    private class CredentialDto
    {
        public string Value { get; set; } = null!;
        public bool IsEncrypted { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
