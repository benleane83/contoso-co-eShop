```mermaid
graph TB
    subgraph "Client Applications"
        WebApp[WebApp - .NET Blazor]
        ClientApp[ClientApp - .NET MAUI]
    end

    subgraph "API Gateway"
        Gateway[API Gateway]
    end

    subgraph "Microservices"
        Catalog[Catalog Service]
        Basket[Basket Service]
        Ordering[Ordering Service]
        Identity[Identity Service]
    end

    subgraph "Data Stores"
        CatalogDB[(Catalog DB)]
        BasketDB[(Basket Redis)]
        OrderDB[(Order DB)]
        IdentityDB[(Identity DB)]
    end

    subgraph "Message Bus"
        ServiceBus[Azure Service Bus]
    end

    WebApp --> Gateway
    ClientApp --> Gateway
    Gateway --> Catalog
    Gateway --> Basket
    Gateway --> Ordering
    Gateway --> Identity

    Catalog --> CatalogDB
    Basket --> BasketDB
    Ordering --> OrderDB
    Identity --> IdentityDB

    Ordering --> ServiceBus
    Basket --> ServiceBus
    
    class WebApp,ClientApp client
    class Catalog,Basket,Ordering,Identity service
    class CatalogDB,BasketDB,OrderDB,IdentityDB database
    class ServiceBus messaging