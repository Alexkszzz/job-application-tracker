# Security Overview

## Implemented Security Measures

### ✅ Authentication & Authorization

- **JWT Bearer Authentication**: Stateless token-based authentication
- **Password Requirements**:
  - Minimum 8 characters
  - Requires: digit, lowercase, uppercase, and special character
  - At least 4 unique characters
- **Account Lockout**: 5 failed login attempts = 15-minute lockout
- **Rate Limiting**: 5 requests per minute on register/login endpoints
- **Token Expiration**: Strict validation with no grace period (ClockSkew = 0)
- **User Isolation**: All application endpoints filter by authenticated user's ID

### ✅ Input Validation

- **Email**: Max 256 characters, proper format validation
- **Passwords**: 8-100 characters with complexity requirements
- **Company Name/Job Title**: Max 200 characters
- **Job Description**: Max 5000 characters
- **Location**: Max 200 characters
- **Salary**: Range validation (0-10,000,000)
- **Job URL**: URL format validation, max 500 characters
- **Notes**: Max 2000 characters
- **Resume/Cover Letter**: Max 500 characters

### ✅ Configuration Security

- JWT secret externalized to environment variables
- CORS configurable via AllowedOrigins array
- Production secrets kept separate from source code
- `.env.example` templates provided for setup

### ✅ Code Security

- SQL Injection protection via Entity Framework parameterized queries
- XSS protection via React automatic escaping
- Password hashing via ASP.NET Identity (PBKDF2)
- HTTPS redirection enabled

## Known Limitations & Recommended Improvements

### ⚠️ Medium Priority Issues

1. **File Storage Security**

   - **Current**: Resume/CoverLetter stored as strings in database (max 500 chars)
   - **Risk**: Limited to URLs or short text, no actual file upload support
   - **Recommendation**: Implement proper file upload with:
     - Azure Blob Storage or AWS S3
     - File type validation (PDF, DOCX only)
     - Virus scanning
     - Size limits (e.g., 5MB max)
     - Secure file naming (prevent path traversal)

2. **No Email Verification**

   - **Current**: Users can register with any email without verification
   - **Risk**: Fake accounts, email spoofing
   - **Recommendation**: Add email verification flow with confirmation tokens

3. **No Security Logging**

   - **Current**: No audit trail for security events
   - **Risk**: Cannot detect or investigate security incidents
   - **Recommendation**: Log all:
     - Failed login attempts
     - Account lockouts
     - Password changes
     - Suspicious activity patterns

4. **SQLite Database**

   - **Current**: Unencrypted SQLite database
   - **Risk**: If file is accessed, all data is readable
   - **Recommendation**: Migrate to PostgreSQL for production (see DEPLOYMENT.md)

5. **Token Expiration**
   - **Current**: 60 minutes (1 hour)
   - **Risk**: Longer window for token theft exploitation
   - **Recommendation**: Reduce to 15-30 minutes, implement refresh tokens

### 📋 Optional Enhancements

6. **Content Security Policy (CSP)**

   - Add security headers to prevent XSS and injection attacks
   - Implement via middleware or reverse proxy

7. **Two-Factor Authentication (2FA)**

   - Add TOTP-based 2FA for additional account security
   - Use Google Authenticator/Microsoft Authenticator

8. **Session Management**

   - Implement refresh tokens for better UX without sacrificing security
   - Add "remember me" functionality with longer-lived refresh tokens

9. **Password Reset Flow**

   - Add secure password reset via email with time-limited tokens
   - Require old password when changing password while authenticated

10. **API Versioning**
    - Implement API versioning for better backward compatibility
    - Example: `/api/v1/applications`

## Security Testing

### Manual Testing Checklist

- [ ] Try registering with weak password (should fail)
- [ ] Try 5+ failed login attempts (should trigger lockout)
- [ ] Try accessing another user's applications (should return 404/401)
- [ ] Try sending oversized input fields (should return 400 Bad Request)
- [ ] Try using expired JWT token (should return 401 Unauthorized)
- [ ] Try accessing protected endpoints without token (should return 401)

### Automated Testing

- All DTOs have validation attributes
- AuthService unit tests cover authentication flows
- Consider adding integration tests for:
  - Rate limiting enforcement
  - Account lockout behavior
  - User data isolation

## Reporting Security Issues

If you discover a security vulnerability:

1. **DO NOT** open a public GitHub issue
2. Email the maintainer directly with details
3. Include steps to reproduce and potential impact
4. Allow reasonable time for fix before disclosure

## Deployment Security Checklist

Before deploying to production:

- [ ] JWT secret set to cryptographically secure random string (64+ chars)
- [ ] CORS configured with actual frontend URL
- [ ] HTTPS enforced on hosting platform
- [ ] Environment variables configured (not .env files)
- [ ] Database backups configured
- [ ] Monitoring/logging enabled
- [ ] Consider migrating to PostgreSQL from SQLite
- [ ] Review and test rate limiting in production
- [ ] Ensure sensitive data not logged
- [ ] API keys and secrets stored securely (Azure Key Vault, AWS Secrets Manager)

## Security Best Practices for Development

1. **Never commit secrets**: Use `.env` files (gitignored) for local development
2. **Keep dependencies updated**: Run `dotnet outdated` and `npm audit` regularly
3. **Code review**: Review security-critical code changes carefully
4. **Principle of least privilege**: Grant minimum necessary permissions
5. **Defense in depth**: Multiple layers of security (validation, authorization, rate limiting)
6. **Fail securely**: Return generic error messages, don't leak sensitive info

## Resources

- [OWASP Top 10](https://owasp.org/www-project-top-ten/)
- [ASP.NET Core Security Best Practices](https://learn.microsoft.com/en-us/aspnet/core/security/)
- [JWT Security Best Practices](https://curity.io/resources/learn/jwt-best-practices/)
- [React Security Best Practices](https://snyk.io/blog/10-react-security-best-practices/)

---

**Last Updated**: January 2025  
**Security Level**: Development/Demo Ready, Production improvements needed
