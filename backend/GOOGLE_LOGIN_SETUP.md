# Google Federated Login Setup Guide

## Overview

Your Weather App backend already has Google login implemented! This guide walks you through the final configuration steps needed to make it fully functional.

## Backend Setup (Already Implemented ✅)

### What's Already in Place:

1. **GoogleAuthService** - Validates Google ID tokens using Google's official library
2. **AuthService.GoogleLoginAsync()** - Handles user creation/login with Google accounts
3. **AuthController Endpoints**:
   - `POST /api/auth/google` - Original endpoint (uses `GoogleAuthDto`)
   - `POST /api/auth/google-login` - Frontend-compatible endpoint (uses `GoogleLoginRequestDto`)

### Key Files:

- [AuthController.cs](AuthenticationService/AuthenticationService.API/Controllers/AuthController.cs) - API endpoints
- [AuthService.cs](AuthenticationService/AuthenticationService.Infrastructure/Services/AuthService.cs) - Business logic
- [GoogleAuthService.cs](AuthenticationService/AuthenticationService.Infrastructure/Services/GoogleAuthService.cs) - Token validation
- [GoogleAuthDto.cs](AuthenticationService/AuthenticationService.Application/DTOs/GoogleAuthDto.cs) - Request DTO
- [GoogleLoginRequestDto.cs](AuthenticationService/AuthenticationService.Application/DTOs/GoogleLoginRequestDto.cs) - Frontend request DTO

---

## Step 1: Get Your Google Client ID

1. Go to [Google Cloud Console](https://console.cloud.google.com/)
2. Create a new project or select an existing one
3. Enable the **Google+ API**:
   - Search for "Google+ API" in the search bar
   - Click **Enable**

4. Create an OAuth 2.0 Credential:
   - Go to **Credentials** → **Create Credentials** → **OAuth 2.0 Client ID**
   - Select **Web application**
   - Add Authorized JavaScript origins:
     - `http://localhost:4200` (frontend dev)
     - `http://localhost:5020` (auth service)
     - Your production domain
   - Add Authorized Redirect URIs:
     - `http://localhost:4200/home` (frontend)
     - Your production URLs
   - Click **Create**
   - Copy your **Client ID**

---

## Step 2: Configure Backend

### Update appsettings.json

Add your Google Client ID to `AuthenticationService.API/appsettings.json`:

```json
{
  "Google": {
    "ClientId": "YOUR_GOOGLE_CLIENT_ID_HERE.apps.googleusercontent.com"
  }
}
```

Example:
```json
{
  "Google": {
    "ClientId": "715486754865-9e3f7hm2i8mfh4c8n9v5p6q7r8s9t0u.apps.googleusercontent.com"
  }
}
```

---

## Step 3: Update Frontend

### Update Login Component

The frontend login component is already updated, but verify it has your Client ID.

File: `frontend/src/app/components/login/login.component.ts`

Change this line with your actual Google Client ID:
```typescript
client_id: 'YOUR_GOOGLE_CLIENT_ID.apps.googleusercontent.com',
```

Current placeholder:
```typescript
client_id: '715486754865-9e3f7hm2i8mfh4c8n9v5p6q7r8s9t0u.apps.googleusercontent.com',
```

---

## Step 4: Test Google Login

### 1. Start the Backend

```powershell
cd backend/AuthenticationService/AuthenticationService.API
dotnet run
```

Service will run on `http://localhost:5020`

### 2. Start the Frontend

```powershell
cd frontend
npm start
```

App will run on `http://localhost:4200`

### 3. Test the Login Flow

1. Open http://localhost:4200
2. Click the **"Sign in with Google"** button on the login page
3. Select your Google account
4. You should be logged in and redirected to the home page
5. You can now search for weather!

---

## How It Works

### Frontend Flow:
1. User clicks "Sign in with Google"
2. Google Sign-In dialog appears
3. User authenticates with Google
4. Google returns an ID token to the frontend
5. Frontend sends ID token to backend: `POST /api/auth/google-login`
   ```json
   {
     "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
   }
   ```

### Backend Flow:
1. `GoogleLoginRequestDto` receives the token
2. `GoogleAuthService.ValidateGoogleTokenAsync()` validates it with Google's servers
3. Token payload is extracted (email, name, picture, Google ID)
4. If user doesn't exist → new user is created
5. JWT token is generated and returned
6. Frontend stores JWT in localStorage for API calls

### Database:
New users created via Google login are stored with:
- `Email` - from Google account
- `Name` - from Google profile
- `ProfilePicture` - from Google profile
- `GoogleId` - Google's user ID
- `AuthProvider` = "Google"
- Empty `PasswordHash` (not needed for federated auth)

---

## API Endpoints Reference

### Google Login
- **Endpoint**: `POST /api/auth/google-login`
- **Request Body**:
  ```json
  {
    "token": "GOOGLE_ID_TOKEN"
  }
  ```
- **Response Success**:
  ```json
  {
    "success": true,
    "message": "Google login successful",
    "token": "JWT_TOKEN",
    "email": "user@gmail.com",
    "role": "User"
  }
  ```
- **Response Error**:
  ```json
  {
    "success": false,
    "message": "Invalid Google token"
  }
  ```

### Regular Login (for comparison)
- **Endpoint**: `POST /api/auth/login`
- **Request Body**:
  ```json
  {
    "email": "user@example.com",
    "password": "password123"
  }
  ```

---

## Troubleshooting

### "Invalid Google token" error
- **Cause**: Google Client ID in backend doesn't match frontend
- **Fix**: Ensure both have the same Client ID

### "Email already registered. Please use regular login."
- **Cause**: Email is already registered with a different auth provider
- **Fix**: User needs to use the original login method or contact support

### CORS errors
- **Cause**: Origin not whitelisted
- **Fix**: Add your domain to Google Cloud Console's Authorized Origins and CORS policy in backend

### Token validation fails silently
- **Cause**: Google API not enabled or credentials misconfigured
- **Fix**: Check Google Cloud Console settings

---

## Security Notes

1. **Never commit** your Google Client ID to frontend production code
2. **Always validate** tokens on the backend (already implemented)
3. **Use HTTPS** in production
4. **Store JWT securely** - currently in localStorage (consider upgrading to httpOnly cookies)
5. **Implement token expiration** - already configured (check `Jwt:ExpiryMinutes`)

---

## Next Steps

After testing:
1. Add Google button styling improvements
2. Implement "Sign up" flow if needed
3. Add profile picture display from Google
4. Implement token refresh for longer sessions
5. Set up production environment with real domain

---

## Support

For issues with Google API setup:
- [Google Cloud Documentation](https://cloud.google.com/docs)
- [Google Sign-In Guide](https://developers.google.com/identity/sign-in)
- [JWT Bearer Documentation](https://jwt.io/)

For application-specific issues:
- Check backend logs: `backend/AuthenticationService/logs/`
- Check browser console (F12) for frontend errors
