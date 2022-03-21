# Cymmetri Login SDK for dotnet core
Login SDK in dotnet core, that can be directly used from Nuget Package Manager.

## Goals
- Allow for Nuget Package Manager to put this as a package.
- Import this as a clean dependency to the Windows Credential Provider.
- Provide interface for API calls made from Cymmetri frontend for the various login flows.
- Provide Stateful function that is internally consistent for performing above mentioned login flows.

## Supported Login Flows
- Password-based Login Flow
- Passwordless Login Flow
- Reset Password Flow
- Unlock Account Flow

## Errors Supported
- Show domain invalid
- Show username invalid
- Show Account Lock status
- Show MFA incorrect
- Show Username/Password combination incorrect

## Important References
- Reference for crypto-js equivalent Encryption - https://stackoverflow.com/questions/5452422/openssl-encryption-using-net-classes#answer-5454692
- NuGet Package Link - https://www.nuget.org/packages/com.cymmetri.login.sdk/
