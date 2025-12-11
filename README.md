# .NET10 Maui Deep Link

This project shows how to implement deeplink with NET10 MAUI

## Android

```powershell
adb shell am start -W -a android.intent.action.VIEW -c android.intent.category.BROWSABLE -d mdl://test?p=1
```

## Windows

```powershell
start mdl://test?p=1
```