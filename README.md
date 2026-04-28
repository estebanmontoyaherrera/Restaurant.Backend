🍽️ El Buen Sazón - Proyecto de Modernización
Este repositorio contiene la migración integral del sistema legacy de "El Buen Sazón". Se ha transformado una aplicación WebForms monolítica en una solución escalable y moderna utilizando .NET 10 y PostgreSQL.

🎯 Justificación Técnica (Arquitectura)
La nueva arquitectura se basa en un Monolito Modular siguiendo los principios de Clean Architecture, elegida para resolver los problemas de acoplamiento del sistema original:

Separación de Capas: Se eliminó la lógica de los code-behind. Ahora, la lógica de negocio reside en la capa de Application, las reglas de integridad en Domain, y el acceso a datos en Infrastructure.

Módulo de Identity (Valor Agregado): Se implementó un sistema de seguridad basado en JWT y RBAC, permitiendo que la API sea consumible de forma segura por cualquier frontend moderno.

Módulo de Ordering: Implementa el patrón CQRS para gestionar el flujo de pedidos, asegurando que las reglas de negocio (como no abrir dos pedidos en la misma mesa) se validen antes de persistir.

Transaccionalidad: Uso de Unit of Work para garantizar que las operaciones complejas (como crear un pedido con múltiples platos) sean atómicas.

🛠️ Stack Tecnológico
Backend: .NET 10 (C#)

Base de Datos: PostgreSQL 17 (Containerized)

API: RESTful con documentación en Swagger.

Infraestructura: Docker Compose para reproductibilidad.

📦 Instrucciones de Levantamiento
1. Requisitos
Docker Desktop.

.NET 10 SDK.

2. Infraestructura y Base de Datos
Para desplegar la base de datos PostgreSQL diseñada para este sistema, ejecute:

Bash
docker-compose up -d
3. Ejecución del Proyecto
Navegue a la carpeta del API y ejecute:

Bash
dotnet run --project src/Api/Restaurant.Api
La documentación de los endpoints y las pruebas funcionales se pueden realizar en:
👉 https://localhost:7037/swagger/index.html

🔄 Proceso de Migración y Solución de Problemas
En cumplimiento con la prueba, se analizaron los bugs y malas prácticas del sistema legacy (legacy.txt), aplicando las siguientes soluciones:

Eliminación de SQL Injection: Se reemplazaron las queries inline concatenadas por Entity Framework Core, asegurando que todos los parámetros sean tratados de forma segura.

Validación de Reglas de Negocio:

Se implementó la lógica que impide abrir dos pedidos en una misma mesa.

Se aseguró que la máquina de estados sea estricta: un pedido no puede pasar a "En Preparación" si no tiene platos asociados.

Integridad de Datos: El sistema ahora valida que no se puedan eliminar platos que tengan pedidos asociados, manteniendo la consistencia referencial que el legacy manejaba de forma manual.

Mantenibilidad: El código repetido se centralizó en Handlers y Repositorios, permitiendo que el sistema sea fácil de testear y escalar.



Notas de Entrega
Backend: Finalizado y funcional (Identity + Ordering).

Base de Datos: Schema diseñado en PostgreSQL optimizado para el nuevo flujo modular.
