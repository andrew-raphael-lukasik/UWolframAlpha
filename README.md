# UWolframAlpha
WolframAlpha queries now accessible from any Unity Editor/Application nerby!

go to **Tools>UWolframAlpha**

or call

```c#
string query = "phosphorus";
var response = await UWolframAlpha.Query.Data( query );
```
```c#
string query = "phosphorus";
string appid = "your own appid goes here";
var response = await UWolframAlpha.Query.Data( query , appid );
string response_raw = await UWolframAlpha.Query.XML( query , appid );
```

**NOTE**: UWolframAlpha uses it's own internal appid when none is provided. Limit of 2000 free requests/month is shared between all such users. Hence it's advised to use your own appid.

You can obtain **free appid** here: http://developer.wolframalpha.com/portal/myapps/index.html

![(preview image)](https://i.imgur.com/dlUKB4p.jpg)

# Installation Unity 2019.x
Add this line in `manifest.json` / `dependencies`:
```
"dependencies": {
    "com.andrewraphaellukasik.uwolframalpha": "https://github.com/andrew-raphael-lukasik/UWolframAlpha.git#upm_2019",
}
```

Or via `Package Manager` / `Add package from git URL`:
```
https://github.com/andrew-raphael-lukasik/UWolframAlpha.git#upm_2019
```
