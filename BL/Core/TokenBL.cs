using BL.Encrypt;
using BL.Hashing;
using BL.Repositories;
using BL.Repositories.Interfaceis;
using FCBankBasicHelper.Models;
using Models.DTO;
using Models.Mappers;

namespace BL.Core
{
    public class TokenBL
    {
        private readonly ITokenRepository tokenRepository;
        private readonly IEncryption encryption;
        public TokenBL(IEncryption encryption, ITokenRepository tokenRepository)
        {
            this.tokenRepository = tokenRepository;
            this.encryption = encryption;
        }
        public void Save(string refreshToken, string userName)
        {
            try
            {
                var tok = new Token
                {
                    Username = encryption.Encrypt(userName),
                    RefreshToken = refreshToken,
                    RefreshTokenExpire = DateTime.UtcNow.AddDays(7)
                };
                tokenRepository.Add(tok);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Save()
        {
            try
            {
                tokenRepository.InsertUser();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Token GetTokens(string? username)
        {
            try
            {
                username = encryption.Decrypt(username);
                return tokenRepository.Tokens(username);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
