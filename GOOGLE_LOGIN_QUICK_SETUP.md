# Google Login - Quick Setup Checklist

## What's Already Done ✅

- [x] Backend GoogleAuthService implemented
- [x] AuthService.GoogleLoginAsync() method
- [x] API endpoints: `/api/auth/google` and `/api/auth/google-login`
- [x] Frontend "Sign in with Google" button on login page
- [x] Google Sign-In library loaded in index.html
- [x] JWT interceptor configured
- [x] AuthService.googleLogin() method
- [x] All DTOs and models created

---

## 3 Simple Steps to Enable Google Login

### 1. Get Google Client ID
   - Go to [Google Cloud Console](https://console.cloud.google.com/)
   - Create OAuth 2.0 Web credentials
   - Copy your **Client ID**

### 2. Update Backend Config
   Edit: `backend/AuthenticationService/AuthenticationService.API/appsettings.json`
   ```json
   "Google": {
     "ClientId": "YOUR_CLIENT_ID.apps.googleusercontent.com"
   }
   ```

### 3. Update Frontend Config (if needed)
   Edit: `frontend/src/app/components/login/login.component.ts`
   ```typescript
   client_id: 'YOUR_CLIENT_ID.apps.googleusercontent.com',
   ```

---

## Test It

```powershell
# Terminal 1: Start Backend
cd backend/AuthenticationService/AuthenticationService.API
dotnet run

# Terminal 2: Start Frontend
cd frontend
npm start
```

Then:
1. Go to http://localhost:4200
2. Click "Sign in with Google"
3. Authenticate
4. You're logged in! 🎉

---

## Full Setup Guide

See [GOOGLE_LOGIN_SETUP.md](GOOGLE_LOGIN_SETUP.md) for detailed instructions.

---

## Key Files

| File | Purpose |
|------|---------|
| `AuthController.cs` | API endpoints `/google` & `/google-login` |
| `GoogleAuthService.cs` | Validates tokens with Google |
| `AuthService.cs` | Creates/finds users |
| `login.component.ts` | Frontend Google Sign-In logic |
| `login.component.html` | Google button UI |
| `appsettings.json` | Client ID configuration |

---

## API Endpoint

**POST** `/api/auth/google-login`

**Request:**
```json
{
  "token": "GOOGLE_ID_TOKEN"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Google login successful",
  "token": "JWT_TOKEN",
  "email": "user@gmail.com",
  "role": "User"
}
```
