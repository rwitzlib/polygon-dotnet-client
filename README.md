# polygon-dotnet-client

![image](https://github.com/rwitzlib/polygon-dotnet-client/blob/master/docs/polygon_icon.png)

.NET client for getting stocks, crypto, forex, and indices data from Polygon.io API.

## Prerequisites
Create an account with [Polygon](https://www.polygon.io).  Upon account creation, an API key will be provided for you.

You can manage your API keys on the [Dashboard](https://polygon.io/dashboard/api-keys)

## Setup

#### With Dependency Injection
Using the API key that was generated for you earlier, you can create a Polygon client in the following ways.

```c#
services.AddPolygonClient("API KEY GOES HERE");
```

#### Without Dependency Injection

```c#
using var client = new PolygonClient("API KEY GOES HERE");
```

## Usage

```c#
public class SomeClass
{
	private readonly IPolygonClient _polygonClient;

	public SomeClass(IPolygonClient polygonApi)
	{
		_polygonClient = polygonApi;
	}

	public async Task<PolygonAggregateResponse> DoSomething()
	{
        // This will get all of the 1-minute bars for 2024-04-20
        var request = new PolygonAggregatesRequest
        {
            Ticker = "SPY",
            Multuplier = 1
            Timespan = "minute",
            From = "2024-04-20",
            To = "2024-04-20"
        };

		var response = await _polygonClient.GetAggregatesAsync(request);

        return response;
	}
}
```


Independently developed, this is not an official library and I am not affiliated with Polygon.