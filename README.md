# mvc-ntier-demo_beta 0.5
## Preview
![Lance](http://portal.wegames.tw/Demo.png)
--
---
# Remark
1. Using N-Tier With AspNetMVC
2. Dependency injection with Autofac
3. Video Preview Online

---
use VIPService;
go
ALTER TABLE VIP_INVITE 
ADD IsDelete bit
DEFAULT 0 NOT NULL;
go
ALTER TABLE VIP_INVITE 
ADD IsDeleteDateTime DATETIME
NULL DEFAULT NULL;


---

- 協助設計官方Line帳號功能
- 使用Line Liff V2 SDK 讓會員能更便利的使用相關功能，減少填寫資料
- 設計後台讓管理人員可以快速查詢會員相關資訊，並可下載報表



---

https://test-impcust.skl.com.tw/sub/Views/SKL/VIP.zip
https://test-impcust.skl.com.tw/sub/Views/SKL/VIP-API.zip
https://test-impcust.skl.com.tw/sub/Views/SKL/CheckList-V6.xlsx
https://test-impcust.skl.com.tw/sub/Views/SKL/new.xlsx
https://test-impcust.skl.com.tw/sub/Views/SKL/kpi.xlsx
https://test-impcust.skl.com.tw/sub/Views/SKL/SignatureVerificationV3_Prod.zip

-- 查詢排程工作
schtasks |more
-- 設定排程(五分鐘一次)
schtasks /create /tn bpmTask /tr C:\Users\ek1008\source\repos\bpmRecognize\bpmNotify\bin\Debug\bpmNotify.exe /sc minute /mo 15
-- 設定排程(立即執行，需更改時間)
schtasks /create /tn bpmTask2 /tr C:\Users\ek1008\source\repos\bpmRecognize\bpmNotify\bin\Debug\bpmNotify.exe /sc once /st 17:22
-- 刪除排程
schtasks /delete /tn 排程名稱
