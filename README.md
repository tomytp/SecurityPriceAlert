# Stock Price Alert
Simple stock price (b3 only) alerts on email.

## Getting Started

1. Clone or download the project.
2. Open the solution in your preferred IDE.
3. Update the SMTP credentials in the `appsettings.json` file with your own secure credentials. Do not use the example credentials provided.

```json
{
  "AppSettings": {
    "SmtpCredentials": {
      "Host": "your.smtp.host",
      "Port": 587,
      "UserName": "your.smtp.username",
      "Password": "your.smtp.password"
    },
    "DestinationEmailAddress": "your.destination.email",
    "BrApiToken": "your.br.api.token"
  }
}
```

4. Note that the BrApiToken provided is for example purposes and is only allowed 15000 requests per month and only updates every 30 minutes.
5. Build and run the project. 


### Command-Line Arguments

The application expects three command-line arguments:

1. `securitySymbol`: The symbol of the security for which you want to monitor the price. (For a list of available symbols, check the options [here](https://www.dadosdemercado.com.br/acoes) under the 'Ticker' column)
2. `sellTriggerPrice`: The price threshold for selling the security (you will be notified if the price rises above this limit).
3. `buyTriggerPrice`: The price threshold for buying the security (you will be notified if the price falls below this limit).

For example, to run the application with the symbol "PETR4" and sell and buy trigger prices of 38.5 and 36, respectively, you can use the following command:

```
dotnet run PETR4 38.5 36
```

If the command-line arguments are not provided or are invalid, the application will display an error message and exit.
