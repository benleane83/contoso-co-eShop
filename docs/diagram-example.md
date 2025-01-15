```mermaid
graph TD
    A[eShop.sln] --> B[eShop.Web]
    A --> C[eShop.Services]
    A --> D[eShop.Data]
    A --> E[eShop.Core]
    A --> F[eShop.Tests]

    B --> C
    B --> D
    B --> E

    C --> D
    C --> E

    D --> E

    F --> B
    F --> C
    F --> D
    F --> E