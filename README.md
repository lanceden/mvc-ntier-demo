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
