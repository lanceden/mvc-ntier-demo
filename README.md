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
