# 單機版應用程式

* .NET 8
* Vue 3
* Electron .NET
* SQLite

---

## Pre-requirement

1. .NET CLI

2. .EFCore CLI
	```sh
    dotnet tool install dotnet-ef -g
    # 檢查是否安裝成功
    dotnet ef
	```	

3. ElectronNET.CLI
	```sh
    dotnet tool install ElectronNET.CLI -g
    # 檢查是否安裝成功
    electronize
	```
4. Node.js
	```sh
    # 檢查版本
    node -v 
    # 最低版本要求:16.17.1
	```

## Installation

1. 安裝相依套件
	```sh
    cd wwwroot
    npm install
	```

---

## Develop

### Code First

1. 新增或編輯 `Database/Entities`
2. 若為新增，要編輯 `Database/Contexts/DatabaseContext.cs`，將新增的Entity加入。例如：
   ```csharp
   /// <summary> 帳號資料 </summary>
   public virtual DbSet<UserProfiles> UserProfiles { get; set; }
   ```
3. 新增 Migration
   ```pwsh
   dotnet ef migrations add [MigrationName] --context DatabaseContext
   ```
4. 專案執行時，Migration 會自動執行

### Database

* 使用SQLite
* 資料庫檔案位於 `C:\ProgramData\Medicine`
* 可使用 `SQLiteStudio 免安裝版` 來檢視資料庫
  
### Debug

主要使用以下兩個設定檔，可在 `Properties/launchSettings.json` 中檢視詳細設定

1. Kestrel Watch

	使用 `Kestrel` 作為 Web Server，開發.NET相關功能。

2. Electron .NET Watch
							
	打包成應用程式，開發Electron相關功能。

### Others

* 一律使用本機資源，不使用網路資源，例如CDN。

---

## Publish

1. 編輯 `electron.manifest.json`，修改 `build.buildVersion` 版本號
	
2. 執行打包指令
    ```sh
    electronize build /target win
    ```
3. 成品會在 `bin\Desktop\` 中