# UWolframAlpha
WolframAlpha queries now accessible from any Unity Editor/Application nerby!

go to **Tools>WolframAlpha**

or call

```c#
string query = "phosphorus";
var response = await UWolframAlpha.Query( query );
```
```c#
string query = "phosphorus";
string appid = "your own appid goes here";
var response = await UWolframAlpha.Query( query , appid );
string response_raw = await UWolframAlpha.QueryXML( query , appid );
```

![(preview image)](https://i.imgur.com/mhBViEQ.jpg)
