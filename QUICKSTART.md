# ForgeMacro - Quick Start Guide

## 🚀 Get Started in 5 Minutes

### Step 1: Build the Project

```bash
cd /Users/k/Documents/Projects/forgego
dotnet restore
dotnet build
```

### Step 2: Prepare Your Models

Create a `models/` folder and add your trained YOLO models:

```
models/
├── yolo_ore_detector.onnx    # Main ore detection model
└── yolo_rock_detector.onnx   # Optional: rock detection
```

**Supported formats:**
- `.onnx` (ONNX Runtime - recommended)
- `.pt` (PyTorch - requires conversion)

### Step 3: Configure Backend (Optional)

Edit `appsettings.json`:

```json
{
  "Backend": {
    "BaseUrl": "http://your-backend-url:5000",
    "ApiKey": "your-api-key",
    "EnableUploadStats": true
  }
}
```

### Step 4: Run the Application

```bash
dotnet run
```

The UI will open with:
- **Dashboard**: Real-time statistics
- **Settings**: Model and automation configuration
- **Detection**: Live detection testing
- **Statistics**: Session analytics
- **Logs**: Application logs

### Step 5: Start Mining

1. Go to **Settings** → Configure model path and thresholds
2. Click **Start** button (top right)
3. Watch the **Dashboard** for real-time stats
4. Check **Logs** for detailed activity

---

## 📋 What's Included

### Core Services

| Service | Purpose |
|---------|---------|
| **ScreenCaptureService** | Captures game screen for analysis |
| **OcrService** | Extracts ore names using Tesseract |
| **ObjectDetectionService** | Detects ores/rocks using YOLO |
| **InputSimulationService** | Controls mouse/keyboard for mining |
| **MacroEngineService** | Orchestrates the automation loop |
| **BackendService** | Syncs with your backend server |

### UI Pages

| Page | Features |
|------|----------|
| **Dashboard** | Live stats, activity log, quick actions |
| **Settings** | Model config, thresholds, automation options |
| **Detection** | Real-time detection testing |
| **Statistics** | Session analytics, ore breakdown |
| **Logs** | Application logs with filtering |

---

## 🎯 Key Features

✅ **YOLO Object Detection** - Detect ores and rocks with high accuracy  
✅ **OCR Integration** - Recognize ore names from screenshots  
✅ **Auto-Mining** - Automatically click detected ores  
✅ **Real-time Monitoring** - Live statistics and performance metrics  
✅ **Backend Sync** - Upload stats and download models  
✅ **Modern UI** - Smooth animations and professional design  
✅ **Single-file Deployment** - No dependencies, just run the .exe  

---

## ⚙️ Configuration

### Model Settings

```json
{
  "Macro": {
    "ModelPath": "models/yolo_ore_detector.onnx",
    "OreConfidenceThreshold": 0.6,      // 0-1, higher = stricter
    "RockConfidenceThreshold": 0.6,
    "ScreenCaptureInterval": 100,       // milliseconds
    "EnableOcr": true,
    "EnableAutoMine": true,
    "EnableGpuAcceleration": false
  }
}
```

### Performance Tips

- **Faster Detection**: Reduce `ScreenCaptureInterval` (50-100ms)
- **Fewer False Positives**: Increase confidence thresholds
- **Better Performance**: Enable GPU acceleration (requires NVIDIA CUDA)

---

## 🔧 Customization

### Add Custom Detection Classes

Edit `Services/ObjectDetectionService.cs`:

```csharp
public List<DetectionResult> DetectCustom(Bitmap image, float confidenceThreshold = 0.6f)
{
    var allDetections = Detect(image, confidenceThreshold);
    var customClasses = new[] { "your_class" };
    
    return allDetections
        .Where(d => customClasses.Any(c => d.Label.Contains(c, StringComparison.OrdinalIgnoreCase)))
        .ToList();
}
```

### Modify Mining Logic

Edit `Services/MacroEngineService.cs` → `RunMacroAsync()` method:

```csharp
// Example: Add delay between clicks
_input.Delay(200);  // 200ms delay

// Example: Custom ore handling
if (ore.Label.Contains("diamond"))
{
    _input.Delay(500);  // Longer delay for valuable ores
}
```

---

## 📦 Building for Release

### Single-file Executable

```bash
dotnet publish -c Release -r win-x64 \
  --self-contained \
  -p:PublishSingleFile=true \
  -p:PublishTrimmed=true \
  -p:PublishReadyToRun=true
```

**Output:** `bin/Release/net8.0-windows/win-x64/publish/ForgeMacro.exe`  
**Size:** ~60-80MB (includes .NET runtime)

---

## 🐛 Troubleshooting

### "Model not found"
- Check `appsettings.json` → `Macro.ModelPath`
- Ensure file exists in `models/` folder
- Use absolute path if relative path fails

### "OCR not working"
- Download Tesseract data from: https://github.com/UB-Mannheim/tesseract/wiki
- Extract to `tessdata/` folder in app directory

### "GPU not detected"
- Install NVIDIA CUDA Toolkit
- Set `EnableGpuAcceleration: true` in settings
- Requires `Microsoft.ML.OnnxRuntime.Gpu` package

### "Detection accuracy low"
- Adjust confidence thresholds in Settings
- Ensure model is trained on your game version
- Check screen resolution matches training data

---

## 📊 Backend Integration

### Expected API Endpoints

```
GET  /api/config              # Get configuration
POST /api/stats               # Upload statistics
POST /api/detections          # Report detections
GET  /api/auth/check          # Check authorization
GET  /api/models/{modelName}  # Download model
```

### Example Stats Payload

```json
{
  "oresMined": 1234,
  "rocksDetected": 567,
  "runningTime": "04:02:15",
  "averageDetectionTime": 45.5
}
```

---

## 📚 Next Steps

1. **Train Your Model**: Use YOLOv8 to train on your game screenshots
2. **Test Detection**: Use the Detection page to verify accuracy
3. **Optimize Settings**: Adjust thresholds for your environment
4. **Deploy**: Build single-file .exe for distribution
5. **Monitor**: Check Statistics and Logs regularly

---

## 💡 Tips & Tricks

- **Faster Mining**: Reduce screen capture interval to 50ms
- **Better Accuracy**: Increase confidence threshold to 0.7+
- **Save Resources**: Disable OCR if not needed
- **Debug Issues**: Check Logs page for detailed error messages
- **Batch Processing**: Use Backend API to upload stats periodically

---

**Happy Mining! ⛏️**

For questions or issues, check the README.md or contact support.
