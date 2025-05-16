
# EmailServiceProvider

A gRPC microservice in ASP.NET Core that uses Azure Communication Services to send emails. Part of a larger project with
multiple microservices.

## How to use

As a client: Copy the proto-file (***email.proto***) from EmailServiceProvider/Protos to your project.

### Install Packages

- Grpc.Tools
- Grpc.Net.Client
- Grpc.Net.ClientFactory
- Google.Protobuf

### Example use (Mostly AI generated)

```csharp
        // Setup gRPC channel to your service
        using var channel = GrpcChannel.ForAddress("https://localhost:5000");
        var client = new EmailServicer.EmailServicerClient(channel);

        // Construct the email request
        var emailRequest = new EmailRequest
        {
            SenderAddress = "noreply@example.com",
            Subject = "Welcome to our service!",
            PlainText = "Thanks for signing up!",
            Html = "<strong>Thanks for signing up!</strong>"
        };

        // Add recipient/s
        emailRequest.Recipients.Add("user1@example.com");

        // Call the SendEmail RPC
        var response = await client.SendEmailAsync(emailRequest);

        // Handle response
        if (response.IsSuccess)
        {
            Console.WriteLine("Email sent successfully.");
        }
        else
        {
            Console.WriteLine($"Failed to send email: {response.Result}");
        }
```