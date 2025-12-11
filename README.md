# ForgeMacro - AI-Powered Mining Automation

A professional C# WPF application for automated mining in Forge with OCR, object detection, and backend integration.

## Features

- **🤖 Object Detection**: YOLO-based ore and rock detection using your trained models (.pt, .onnx)
- **📖 OCR Integration**: Tesseract Sharp for ore name recognition
- **⚙️ Automation Engine**: Intelligent screen capture, detection, and automated mining
- **🎨 Modern UI**: Beautiful WPF interface with smooth animations
- **📊 Real-time Statistics**: Track ores mined, detection accuracy, and performance metrics
- **🌐 Backend Integration**: REST API connectivity for configuration and data sync
- **💾 Single-file Deployment**: Compiles to a standalone .exe (~60-80MB)

## Project Structure

```
ForgeMacro/
├── Services/
│   ├── IScreenCaptureService.cs
│   ├── IOcrService.cs
│   ├── IObjectDetectionService.cs
│   ├── IInputSimulationService.cs
│   ├── IBackendService.cs
│   ├── IMacroEngineService.cs
│   └── [Implementation files]
├── Views/
│   ├── MainWindow.xaml
│   ├── DashboardPage.xaml
│   ├── SettingsPage.xaml
│   ├── DetectionPage.xaml
│   ├── StatisticsPage.xaml
│   └── LogsPage.xaml
├── ViewModels/
│   └── MainWindowViewModel.cs
├── ForgeMacro.csproj
├── App.xaml
├── appsettings.json
└── README.md
```

## Requirements

- **.NET 8.0** or later
- **Windows 10/11** (x64)
- **GPU** (optional, for faster detection)

## Setup

### 1. Clone and Build

```bash
cd /Users/k/Documents/Projects/forgego
dotnet restore
dotnet build
```

### 2. Add Your Trained Models

Place your YOLO models in the `models/` directory:

```
ForgeMacro/
└── models/
    ├── yolo_ore_detector.onnx  (or .pt)
    └── yolo_rock_detector.onnx (optional)
```

### 3. Configure Settings

Edit `appsettings.json`:

```json
{
  "Macro": {
    "ModelPath": "models/yolo_ore_detector.onnx",
    "OreConfidenceThreshold": 0.6,
    "RockConfidenceThreshold": 0.6,
    "ScreenCaptureInterval": 100,
    "EnableOcr": true,
    "EnableAutoMine": true
  },
  "Backend": {
    "BaseUrl": "http://your-backend-url:5000",
    "ApiKey": "your-api-key"
  }
}
```

### 4. Run

```bash
dotnet run
```

## Building for Deployment

### Single-file Executable

```bash
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -p:PublishTrimmed=true -p:PublishReadyToRun=true
```

Output: `bin/Release/net8.0-windows/win-x64/publish/ForgeMacro.exe` (~60-80MB)

## Key Libraries

| Feature | Library | Version |
|---------|---------|---------|
| **OCR** | Tesseract | 5.3.0 |
| **Object Detection** | ONNX Runtime | 1.17.0 |
| **Image Processing** | OpenCV.NET | 4.8.1.26 |
| **Input Control** | InputSimulator | 1.0.4 |
| **Logging** | Serilog | 3.1.1 |
| **UI Framework** | WPF | Built-in |

## Usage

1. **Load Model**: Go to Settings → Model Path and select your YOLO model
2. **Configure Detection**: Adjust confidence thresholds for ores and rocks
3. **Start Mining**: Click "Start" button to begin automation
4. **Monitor**: Watch real-time statistics on the Dashboard
5. **View Logs**: Check the Logs page for detailed activity

## API Integration

### Backend Endpoints (Expected)

```
GET  /api/config              - Get macro configuration
POST /api/stats               - Upload session statistics
POST /api/detections          - Report detected ores
GET  /api/auth/check          - Check authorization
GET  /api/models/{modelName}  - Download model
```

## Customization

### Adding New Detection Classes

Edit `ObjectDetectionService.cs`:

```csharp
public List<DetectionResult> DetectCustom(Bitmap image, float confidenceThreshold = 0.6f)
{
    var allDetections = Detect(image, confidenceThreshold);
    var customClasses = new[] { "your_class_1", "your_class_2" };
    
    return allDetections
        .Where(d => customClasses.Any(c => d.Label.Contains(c, StringComparison.OrdinalIgnoreCase)))
        .ToList();
}
```

### Custom Automation Logic

Edit `MacroEngineService.cs` → `RunMacroAsync()` method to implement your mining strategy.

## Troubleshooting

### Model Not Loading
- Ensure model file exists at the configured path
- Check that model format is ONNX or PyTorch (.pt)
- Verify file permissions

### OCR Not Working
- Download Tesseract data: https://github.com/UB-Mannheim/tesseract/wiki
- Place `tessdata/` folder in application directory

### GPU Not Detected
- Install NVIDIA CUDA Toolkit
- Use `Microsoft.ML.OnnxRuntime.Gpu` package
- Enable in settings

## Performance Tips

- **Reduce Screen Capture Interval**: Lower = faster detection (CPU intensive)
- **Increase Confidence Threshold**: Higher = fewer false positives
- **Enable GPU**: Significantly faster inference
- **Trim Unused Features**: Disable OCR if not needed

## License

Proprietary - All rights reserved

## Support

For issues or feature requests, contact the development team.

---

**Built with C# .NET 8.0 | WPF | YOLO | Tesseract**
