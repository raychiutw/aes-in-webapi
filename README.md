# AES in WebApi

ASP.NET Core 6 WebApi 專案使用 AES 範例

## 情境描述

呼叫 API 的 Request 與 Response 由 AES 加密(pkcs7padding + base64)

> 使用客製化 ModelBinder, 將 Request Body AES 解密之後 Binding 至 parameter model

> 使用 Filter 將 Response 做 AES 加密

## 步驟

### 開啟 ASP.NET CORE 6 WebApi 專案

[請參照](https://docs.microsoft.com/zh-tw/aspnet/core/tutorials/first-web-api?view=aspnetcore-6.0&tabs=visual-studio)

### 安裝套件

AES 加密套件為: NETCore.Encrypt (可自行替換習慣的套件)

```powershell
Install-Package NETCore.Encrypt -Version 2.1.0
```

## 步驟說明

### AesHelper 相關程式複製到專案

[程式路徑請點我](/src/AESSample/AesHelper/)

- IAesCryptor.cs: 密碼器介面, 定義加解密方法
- AesCryptor.cs: 密碼器實作
- AesOptions.cs: 密碼器選項類別, 取得 AES KEY 與 IV
- AesModelBinder: AES 解密 ModelBinder
- AesResponseFilter: AES 加密 Filter

### 相關設定

Program.cs

```csharp
// AES Cryptor DI
builder.Services.AddScoped<IAesCryptor, AesCryptor>();

// AES Option Binding
builder.Services.Configure<AesOptions>(
    builder.Configuration.GetSection(nameof(AesOptions)));
```

appsettings.

```json
  "AesOptions": {
    "Key": "7571B17C2CB2F3E0ED6F9E92277AFA2A",
    "Iv": "E21E75D88C5B76C9"
  }
```

### 使用教學

請先建立參數類別 `WeatherForecastParameter.cs`

```csharp
    public class WeatherForecastParameter
    {
        /// <summary>
        /// 產生筆數, 預設 5 筆
        /// </summary>
        public int Size { get; set; } = 5;
    }
```

> 加入參數類別, 將 `numerable.Range(1, 5)` 改為 ` Enumerable.Range(1, parameter.Size)`

```csharp
        [HttpPost]
        [Route("raw")]
        public IEnumerable<WeatherForecast> PostRaw(WeatherForecastParameter parameter)
        {
            return Enumerable.Range(1, parameter.Size)
                .Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();
        }
```

> 將參數類別綁定 AesModleBinder

`[ModelBinder(BinderType = typeof(AesModelBinder))] WeatherForecastParameter parameter`

> 將 Response 加密輸出

`[TypeFilter(typeof(AesResponseFilter))]`

```csharp
[HttpPost]
        [Route("aes")]
        [TypeFilter(typeof(AesResponseFilter))]
        public IEnumerable<WeatherForecast> PostAes(
           [FromBody][ModelBinder(BinderType = typeof(AesModelBinder))] WeatherForecastParameter parameter)
        {
           // 略
        }
```

### 使用 Postman 測試

### 補充

[線上 AES 加解密工具](https://www.010tools.com/AES)

### 完整範例

[請點這裡](/src/AESSample/)
