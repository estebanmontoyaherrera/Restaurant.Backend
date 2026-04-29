🍽️ El Buen Sazón - Modernización Legacy
Este proyecto documenta la migración de un sistema ASP.NET WebForms hacia una arquitectura moderna, escalable y diseñada para el mantenimiento evolutivo.

🎯 Justificación Técnica (Arquitectura)
Se implementó un Monolito Modular bajo los principios de Clean Architecture para garantizar la mantenibilidad a largo plazo y resolver la rigidez del legacy:

Modularidad: Separación de dominios en módulos independientes (Identity, Ordering), facilitando pruebas y futuras migraciones a microservicios.

Clean Architecture: Independencia de la lógica de negocio frente a frameworks y bases de datos, blindando el sistema contra la obsolescencia técnica.

Seguridad Enterprise: Autenticación JWT y RBAC, permitiendo una API stateless y segura para clientes modernos.

Patrones de Diseño: Implementación de CQRS, Repository y Unit of Work para asegurar la integridad transaccional y la claridad del código.

🛠️ Stack Tecnológico
Backend: .NET 10 (C#)

Base de Datos: PostgreSQL 17 (Containerized)

Infraestructura: Docker Compose

Frontend: Angular (Entrega: 30 de abril)

📦 Instrucciones de Levantamiento
1. Requisitos
Docker Desktop

.NET 10 SDK

2. Infraestructura (Base de Datos)
El docker-compose.yml incluido levanta automáticamente PostgreSQL 17. Ejecute:

Bash
docker-compose up -d

3. Ejecución del API
Bash

dotnet run --project src/Api/Restaurant.Api

Acceda a la documentación en: https://localhost:7037/swagger/index.html

### 🔐 Autenticación y Pruebas (Swagger)
Para probar los endpoints protegidos, siga estos pasos:
1. **Iniciar Sesión**: Use el endpoint `POST /api/Auth/Login` con las siguientes credenciales:
   - **Email**: `admin@gmail.com`
   - **Password**: `12345`
2. **Obtener Token**: Copie el valor del `accessToken` de la respuesta JSON.
3. **Autorizar**: Haga clic en el botón **"Authorize"** (icono de candado) en la parte superior de Swagger.
4. **Configurar**: Escriba el token en el campo de valor y haga clic en Authorize.

🔄 Proceso de Migración (Solución de Requisitos)
Siguiendo las especificaciones de la prueba, se resolvieron las deficiencias del sistema original:

Seguridad: Eliminación de SQL Injection mediante el uso de consultas parametrizadas con EF Core.

Lógica de Negocio: Migración de reglas desde el code-behind hacia Handlers desacoplados.

Máquina de Estados: Validación estricta que impide el avance de pedidos sin platos asociados.

Integridad: Validación de reglas de mesa (un pedido activo por mesa) y bloqueo de eliminación de platos con historial.
