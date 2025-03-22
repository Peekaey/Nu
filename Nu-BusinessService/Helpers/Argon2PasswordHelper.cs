using System.Security.Cryptography;
using System.Text;
using Isopoh.Cryptography.Argon2;
using Isopoh.Cryptography.SecureArray;

namespace Nu_BusinessService.Helpers;

public static class Argon2PasswordHelper
{
    
    public static string HashPassword(string password)
    {
        // 16 Bit Salt as Common
        byte[] salt  = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // Argon 2 Config as per https://github.com/mheyman/Isopoh.Cryptography.Argon2
        // Minus Peppering and Associated Data
        var config = new Argon2Config
        {
            Type = Argon2Type.DataIndependentAddressing,
            Version = Argon2Version.Nineteen,
            TimeCost = 10,
            MemoryCost = 32768,
            Lanes = 5,
            Threads = Environment.ProcessorCount, // higher than "Lanes" doesn't help (or hurt)
            Password = Encoding.UTF8.GetBytes(password),
            Salt = salt, // >= 8 bytes if not null
            // Secret (Pepper) not needed as needs to be stored securely elsewhere as the threat model isn't high enough for this
            // Secret = secret, // from somewhere
            // AssociatedData not needed as its just additional metadata
            // AssociatedData = associatedData, // from somewhere
            HashLength = 20, // >= 4
        };
        
        var argon2A = new Argon2(config);
        string hashString;
        using (SecureArray<byte> hashA = argon2A.Hash())
        {
            hashString = config.EncodeString(hashA.Buffer);
        }
        return hashString;
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        if (Argon2.Verify(hashedPassword, password))
        {
            return true;
        }

        return false;
    }
}