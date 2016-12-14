## Cake Launcher

Execute Cake tasks from Explorer context menu.

![](Screen/CakeLauncher.png)

## Requirement

**build.cmd** that contain commands like this.

```bat
powershell -File build.ps1 %*
pause
```

## Install

1. Extract CakeLauncher-XXX.zip
2. Execute CakeLauncher.Register.exe

## Uninstall

- Execute CakeLauncher.Deregister.exe

## Chocolatey

- Install: `choco install cake-launcher`
- Uninstall: `choco uninstall cake-launcher`