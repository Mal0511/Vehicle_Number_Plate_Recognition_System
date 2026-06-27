# Module View - Vehicle Number Plate Recognition

## External Systems

1. External OCR System - Google Cloud Vision OCR
   - Module adapter: `Modules/ExternalSystems/PlateRecognition`
   - Responsibility: receive the captured camera image, recognize plate characters, and return a text/number string.
   - Local credential file: `App_Data/google-vision-key.json` (ignored by git).

2. Internet / Location Service
   - Module adapter: `Modules/ExternalSystems/InternetLocation`
   - Responsibility: receive location input, resolve current location, and support notification data.

## 4 Basic Functions

1. Login Police/Admin
   - UI: `Views/Auth/Index.cshtml`
   - Controller: `Controllers/AuthController.cs`
   - Service: `Modules/Auth/Services/AuthService.cs`
   - Test accounts: `admin/admin123`, `police/police123`

2. Scan license plate by Android phone
   - UI: `Views/Scan/Index.cshtml`
   - Controller: `Controllers/ScanController.cs`
   - Service: `Modules/Scanning/Services/ScanService.cs`
   - External dependency: `IPlateRecognitionGateway` implemented by `GoogleVisionPlateRecognitionGateway`

3. Check plate in Blacklist/Whitelist
   - UI: `Views/Check/Index.cshtml`
   - Controller: `Controllers/CheckController.cs`
   - Service: `Modules/VehicleLists/Services/VehicleListService.cs`

4. Notify when blacklist vehicle is detected and save to database
   - UI: `Views/Notifications/Index.cshtml`
   - Controller: `Controllers/NotificationsController.cs`
   - Service: `Modules/Notifications/Services/NotificationService.cs`
   - Database: SQLite file `App_Data/numberplate.db` with seeded test data.
   - External dependency: `IInternetLocationService` and `IAdminAlertGateway`

## Layer Style

- Controllers: receive HTTP request and return Razor views.
- Services: application logic and module orchestration.
- Repositories: database access through SQLite repositories.
- ExternalSystems: adapters that isolate external services from business code.
