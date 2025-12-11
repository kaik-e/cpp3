# ForgeMacro - Development Checklist

## 📋 Setup & Configuration

- [ ] Clone repository
- [ ] Run `dotnet restore`
- [ ] Run `dotnet build`
- [ ] Create `models/` folder
- [ ] Add trained YOLO model(s)
- [ ] Configure `appsettings.json`
- [ ] Download Tesseract `tessdata/` (if using OCR)
- [ ] Test `dotnet run`

## 🤖 Model Integration

- [ ] Place YOLO model in `models/` folder
- [ ] Update `appsettings.json` → `Macro.ModelPath`
- [ ] Implement `ObjectDetectionService.ParseDetectionOutput()`
  - [ ] Parse YOLO output format
  - [ ] Extract bounding boxes
  - [ ] Extract confidence scores
  - [ ] Extract class labels
- [ ] Test model loading in Settings page
- [ ] Test detection on sample images
- [ ] Verify confidence thresholds work correctly

## 🎮 Game Integration

- [ ] Implement `ScreenCaptureService.CaptureGameWindow()`
  - [ ] Find game window by title
  - [ ] Capture only game area
  - [ ] Handle window not found
- [ ] Test screen capture with actual game
- [ ] Verify ore/rock detection on game screenshots
- [ ] Calibrate confidence thresholds for game

## ⚙️ Automation Logic

- [ ] Customize `MacroEngineService.RunMacroAsync()`
  - [ ] Define mining strategy
  - [ ] Implement ore prioritization
  - [ ] Add movement patterns
  - [ ] Handle edge cases
- [ ] Test auto-mining on game
- [ ] Add safety checks (e.g., pause if no ores detected)
- [ ] Implement anti-detection measures (if needed)

## 📖 OCR Enhancement (Optional)

- [ ] Download Tesseract data files
- [ ] Extract to `tessdata/` folder
- [ ] Test OCR on ore names
- [ ] Implement ore type detection
- [ ] Add OCR result logging

## 🌐 Backend Integration

- [ ] Design backend API endpoints
- [ ] Implement backend server
- [ ] Update `BackendService` endpoints
  - [ ] `GET /api/config`
  - [ ] `POST /api/stats`
  - [ ] `POST /api/detections`
  - [ ] `GET /api/auth/check`
  - [ ] `GET /api/models/{modelName}`
- [ ] Test configuration sync
- [ ] Test statistics upload
- [ ] Implement authentication

## 📊 UI Enhancements

- [ ] Connect Dashboard stats to real data
- [ ] Implement live statistics updates
- [ ] Add activity log entries
- [ ] Connect Settings page to configuration
- [ ] Implement detection page live feed
- [ ] Add ore breakdown charts
- [ ] Implement log filtering
- [ ] Add export functionality

## 🧪 Testing

- [ ] Unit tests for detection pipeline
- [ ] Unit tests for input simulation
- [ ] Integration tests with sample images
- [ ] Performance benchmarks
- [ ] Test with different screen resolutions
- [ ] Test with different game versions
- [ ] Stress test (long-running sessions)
- [ ] Memory leak detection

## 🐛 Debugging & Optimization

- [ ] Enable detailed logging
- [ ] Profile CPU usage
- [ ] Profile memory usage
- [ ] Optimize screen capture
- [ ] Optimize detection pipeline
- [ ] Test GPU acceleration (if available)
- [ ] Reduce binary size
- [ ] Test single-file deployment

## 📦 Deployment

- [ ] Create release build
- [ ] Test .exe on clean Windows system
- [ ] Verify all dependencies included
- [ ] Test model loading from .exe
- [ ] Create installer (optional)
- [ ] Document system requirements
- [ ] Create user manual

## 🔒 Security & Compliance

- [ ] Remove hardcoded API keys
- [ ] Implement secure config storage
- [ ] Validate all inputs
- [ ] Sanitize logs
- [ ] Test with antivirus software
- [ ] Document privacy policy
- [ ] Add license headers to files

## 📚 Documentation

- [ ] Complete README.md
- [ ] Complete QUICKSTART.md
- [ ] Add code comments
- [ ] Create API documentation
- [ ] Create troubleshooting guide
- [ ] Record demo video
- [ ] Create setup guide

## 🚀 Release Preparation

- [ ] Version bump (1.0.0)
- [ ] Update CHANGELOG
- [ ] Final testing
- [ ] Security audit
- [ ] Performance review
- [ ] Create release notes
- [ ] Tag release in git
- [ ] Build final .exe

## 📈 Post-Release

- [ ] Monitor user feedback
- [ ] Track bug reports
- [ ] Monitor performance metrics
- [ ] Plan version 1.1 features
- [ ] Implement hotfixes
- [ ] Update documentation

---

## Priority Levels

### 🔴 Critical (Must Have)
- [ ] Model integration
- [ ] Game window capture
- [ ] Auto-mining logic
- [ ] Basic UI functionality
- [ ] Logging

### 🟡 Important (Should Have)
- [ ] Backend integration
- [ ] OCR enhancement
- [ ] Statistics tracking
- [ ] Settings persistence
- [ ] Error handling

### 🟢 Nice to Have (Could Have)
- [ ] GPU acceleration
- [ ] Advanced filtering
- [ ] Custom scripts
- [ ] Mobile app
- [ ] Cloud sync

---

## Timeline Estimate

| Phase | Tasks | Duration |
|-------|-------|----------|
| Setup | 1-3 | 1 hour |
| Model Integration | 4-8 | 2-3 hours |
| Game Integration | 9-12 | 3-4 hours |
| Automation | 13-16 | 4-5 hours |
| Backend | 17-22 | 3-4 hours |
| UI Polish | 23-27 | 2-3 hours |
| Testing | 28-34 | 4-5 hours |
| Deployment | 35-39 | 2-3 hours |
| **Total** | **39** | **~24-30 hours** |

---

## Notes

- Start with model integration first
- Test each component independently
- Use logging extensively for debugging
- Keep commits small and focused
- Document as you go
- Get user feedback early

---

## Quick Commands

```bash
# Build
dotnet build

# Run
dotnet run

# Publish (single-file)
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -p:PublishTrimmed=true -p:PublishReadyToRun=true

# Clean
dotnet clean

# Format code
dotnet format

# Run tests
dotnet test
```

---

**Last Updated**: December 11, 2024  
**Status**: Ready for Development
