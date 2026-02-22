# Weather App - Backend & Frontend Integration Summary

## Project Structure

### Backend Services
Located in: `WeatherApp/backend/`

| Service | HTTPS Port | HTTP Port | Base URL |
|---------|-----------|-----------|----------|
| AuthenticationService | 7188 | 5020 | https://localhost:7188 |
| WeatherService | 7264 | 5000 | https://localhost:7264 |
| LoggingService | 7131 | 5042 | https://localhost:7131 |

### Frontend
Located in: `WeatherApp/frontend/`
- **Build Command**: `npm run build`
- **Dev Server**: `npm start` (runs on http://localhost:4200)
- **Output**: `dist/frontend/`

## API Endpoints Reference

### Authentication Service
- **Login**: `POST /api/auth/login`
  - Request: `{ email: string, password: string }`
  - Response: `{ success: bool, message: string, user: User, token: string }`

- **Register**: `POST /api/auth/register`
  - Request: `{ email: string, password: string, confirmPassword: string }`
  - Response: `{ success: bool, message: string, user: User, token: string }`

### Weather Service
- **Get Weather by City**: `POST /api/weather/get-weather`
  - Request: `{ city: string }`
  - Response: `{ success: bool, message: string, data: WeatherData }`

- **Get Weather by Coordinates**: `GET /api/weather/get-weather-by-coords?lat={latitude}&lon={longitude}`
  - Response: `{ success: bool, message: string, data: WeatherData }`

### Logging Service
- Endpoints for log retrieval and management
- Base URL: `https://localhost:7131/api/logs`

## Frontend Features

### Components
1. **Navbar** - Navigation with logout option
2. **Login** - User authentication
3. **Register** - New user registration
4. **Home** - Landing page for authenticated users
5. **Search** - Weather search functionality with map integration

### Services
1. **AuthService** - Authentication management
2. **WeatherService** - Weather API communication

### Security
- **JWT Interceptor**: Automatically injects JWT token in request headers
- **Auth Guard**: Protects routes that require authentication
- **Token Storage**: JWT stored in localStorage

## Build Information

### Phase 1: Backend Integration ✅
- ✅ ELMAH error logging
- ✅ AutoMapper (12.0.1) for object mapping
- ✅ Autofac (8.0.0) for dependency injection
- ✅ All services compiled successfully

### Phase 3 & 4: Frontend ✅
- ✅ Angular 21.1.5 setup
- ✅ Routing with lazy loading
- ✅ Minimal UI components
- ✅ JWT interceptor and auth guard
- ✅ Build successful (244 KB initial, 67.57 KB compressed)

## Next Steps

1. **Test the Application**
   - Start all backend services
   - Start Angular dev server: `npm start` in frontend folder
   - Register a new user
   - Login and search for weather

2. **Phase 6: Docker Setup**
   - Create Dockerfile for each service
   - Create docker-compose.yml

3. **Phase 7: Jenkins CI/CD**
   - Create Jenkinsfile
   - Setup SonarQube integration

4. **Phase 8: Azure Deployment**
   - Configure Azure App Service
   - Setup CI/CD pipeline to Azure

## Environment Configuration

**Development**: `environment.ts`
```typescript
{
  production: false,
  apiUrl: 'https://localhost:7188',
  authServiceUrl: 'https://localhost:7188/api/auth',
  weatherServiceUrl: 'https://localhost:7264/api/weather',
  loggingServiceUrl: 'https://localhost:7131/api/logs'
}
```

**Production**: `environment.prod.ts`
```typescript
{
  production: true,
  apiUrl: 'https://yourdomain.com',
  authServiceUrl: 'https://yourdomain.com/auth/api',
  weatherServiceUrl: 'https://yourdomain.com/weather/api',
  loggingServiceUrl: 'https://yourdomain.com/logging/api'
}
```
