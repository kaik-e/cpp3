# ForgeMacro - Backend Integration Setup

## Forge Discord Bot Integration

Your Forge API is a Railway Discord bot located at `../forge` relative to the ForgeMacro project.

### Directory Structure
```
parent-directory/
├── forge/                    (Your Discord bot with API endpoints)
│   ├── index.js
│   ├── package.json
│   ├── commands/
│   ├── events/
│   └── ...
└── forgego/                  (ForgeMacro client)
    ├── appsettings.json
    ├── ForgeMacro.csproj
    └── ...
```

### Configuration

The ForgeMacro client is configured to connect to your Forge Discord bot API at:

```
http://localhost:3001
```

**File**: `appsettings.json`
```json
{
  "Backend": {
    "BaseUrl": "http://localhost:3001",
    "ApiKey": "",
    "EnableUploadStats": true,
    "UploadInterval": 300000,
    "DiscordBotToken": ""
  }
}
```

---

## Setup Instructions

### Step 1: Configure Forge Discord Bot

Add HTTP API endpoints to your Forge Discord bot (`../forge`):

```javascript
// Add to your bot's main file (index.js or similar)
const express = require('express');
const app = express();

app.use(express.json());

// CORS
app.use((req, res, next) => {
  res.header('Access-Control-Allow-Origin', '*');
  res.header('Access-Control-Allow-Methods', 'GET, POST, PUT, DELETE');
  res.header('Access-Control-Allow-Headers', 'Content-Type, Authorization');
  next();
});

// Health check
app.get('/health', (req, res) => {
  res.json({ status: 'ok', bot: client.user.tag });
});

// Start Express server
const PORT = process.env.API_PORT || 3001;
app.listen(PORT, () => {
  console.log(`Forge API running on http://localhost:${PORT}`);
});
```

### Step 2: Start Your Forge Discord Bot

```bash
cd ../forge
npm install
npm start
```

Verify it's running at `http://localhost:3001`

### Step 3: Build ForgeMacro

```bash
cd ../forgego
dotnet restore
dotnet build
dotnet run
```

### Step 4: Verify Connection

In the ForgeMacro UI:
1. Go to **Settings** page
2. Check **Backend URL**: `http://localhost:3001`
3. Enter your **Discord Bot Token** (optional, for authentication)
4. Click **Save Settings**
5. Check **Logs** page for connection messages

---

## Expected API Endpoints

Your Forge API should implement these endpoints:

### Configuration
```
GET /api/config
Response: {
  "modelPath": "models/yolo_ore_detector.onnx",
  "oreConfidenceThreshold": 0.6,
  "rockConfidenceThreshold": 0.6,
  "screenCaptureInterval": 100,
  "enableOcr": true,
  "enableAutoMine": true
}
```

### Statistics Upload
```
POST /api/stats
Request: {
  "oresMined": 1234,
  "rocksDetected": 567,
  "runningTime": "04:02:15",
  "averageDetectionTime": 45.5
}
Response: { "success": true }
```

### Detection Reports
```
POST /api/detections
Request: {
  "oreType": "iron_ore",
  "count": 5,
  "timestamp": "2024-12-11T19:52:00Z"
}
Response: { "success": true }
```

### Authorization Check
```
GET /api/auth/check
Response: { "authorized": true }
```

### Model Download
```
GET /api/models/{modelName}
Response: Binary file (ONNX model)
```

---

## Troubleshooting

### Connection Refused
- **Issue**: `http://localhost:3000` not reachable
- **Solution**: 
  - Ensure Forge API is running
  - Check port 3000 is not blocked
  - Verify no firewall issues

### API Not Found
- **Issue**: Endpoints return 404
- **Solution**:
  - Verify endpoint paths match exactly
  - Check API is using correct port
  - Review API logs for errors

### CORS Issues
- **Issue**: Browser/client blocked by CORS
- **Solution**:
  - Add CORS headers in Forge API:
    ```javascript
    app.use((req, res, next) => {
      res.header('Access-Control-Allow-Origin', '*');
      res.header('Access-Control-Allow-Methods', 'GET, POST, PUT, DELETE');
      res.header('Access-Control-Allow-Headers', 'Content-Type');
      next();
    });
    ```

### Slow Response
- **Issue**: API responses are slow
- **Solution**:
  - Check network latency
  - Optimize API queries
  - Increase timeout in `BackendService.cs`:
    ```csharp
    _httpClient.Timeout = TimeSpan.FromSeconds(60);
    ```

---

## Development Workflow

### Running Both Services

**Terminal 1 - Forge API**
```bash
cd ../forge
npm start
```

**Terminal 2 - ForgeMacro**
```bash
cd ../forgego
dotnet run
```

### Testing API Endpoints

Use curl or Postman to test:

```bash
# Get config
curl http://localhost:3000/api/config

# Upload stats
curl -X POST http://localhost:3000/api/stats \
  -H "Content-Type: application/json" \
  -d '{"oresMined":100,"rocksDetected":50,"runningTime":"01:00:00","averageDetectionTime":45}'

# Check auth
curl http://localhost:3000/api/auth/check
```

---

## API Implementation Example (Node.js)

```javascript
const express = require('express');
const app = express();

app.use(express.json());

// CORS
app.use((req, res, next) => {
  res.header('Access-Control-Allow-Origin', '*');
  res.header('Access-Control-Allow-Methods', 'GET, POST, PUT, DELETE');
  res.header('Access-Control-Allow-Headers', 'Content-Type');
  next();
});

// Get config
app.get('/api/config', (req, res) => {
  res.json({
    modelPath: 'models/yolo_ore_detector.onnx',
    oreConfidenceThreshold: 0.6,
    rockConfidenceThreshold: 0.6,
    screenCaptureInterval: 100,
    enableOcr: true,
    enableAutoMine: true
  });
});

// Upload stats
app.post('/api/stats', (req, res) => {
  console.log('Stats received:', req.body);
  res.json({ success: true });
});

// Report detection
app.post('/api/detections', (req, res) => {
  console.log('Detection reported:', req.body);
  res.json({ success: true });
});

// Check auth
app.get('/api/auth/check', (req, res) => {
  res.json({ authorized: true });
});

// Download model
app.get('/api/models/:modelName', (req, res) => {
  const fs = require('fs');
  const path = require('path');
  const modelPath = path.join(__dirname, 'models', req.params.modelName);
  
  if (fs.existsSync(modelPath)) {
    res.download(modelPath);
  } else {
    res.status(404).json({ error: 'Model not found' });
  }
});

app.listen(3000, () => {
  console.log('Forge API running on http://localhost:3000');
});
```

---

## Environment Variables

For production, use environment variables:

**Forge API (.env)**
```
PORT=3000
NODE_ENV=development
DB_URL=mongodb://localhost:27017/forge
API_KEY_SECRET=your-secret-key
```

**ForgeMacro (appsettings.json)**
```json
{
  "Backend": {
    "BaseUrl": "${BACKEND_URL:http://localhost:3000}",
    "ApiKey": "${API_KEY:}",
    "EnableUploadStats": true
  }
}
```

---

## Security Considerations

1. **API Key Authentication**
   - Implement API key validation in Forge API
   - Store keys securely (environment variables)
   - Rotate keys regularly

2. **HTTPS in Production**
   - Use HTTPS instead of HTTP
   - Install SSL certificates
   - Update `appsettings.json`:
     ```json
     "BaseUrl": "https://your-domain.com"
     ```

3. **Rate Limiting**
   - Implement rate limiting on API endpoints
   - Prevent abuse and DDoS attacks

4. **Input Validation**
   - Validate all incoming data
   - Sanitize inputs
   - Check data types and ranges

---

## Monitoring & Logging

### ForgeMacro Logs
Check `logs/forgego-YYYY-MM-DD.txt` for:
- API connection status
- Request/response details
- Errors and warnings

### Forge API Logs
Monitor your API server logs for:
- Incoming requests
- Response times
- Errors and exceptions

---

## Performance Tips

1. **Batch Statistics**: Upload stats every 5 minutes instead of real-time
2. **Cache Configuration**: Cache API config locally
3. **Async Requests**: Use async/await for non-blocking calls
4. **Connection Pooling**: Reuse HTTP connections

---

## Next Steps

1. ✅ Configure backend URL in `appsettings.json`
2. ✅ Implement API endpoints in Forge API
3. ✅ Test endpoints with curl/Postman
4. ✅ Start both services
5. ✅ Verify connection in ForgeMacro UI
6. ✅ Monitor logs for issues

---

**Backend Configuration**: Ready  
**Last Updated**: December 11, 2024
