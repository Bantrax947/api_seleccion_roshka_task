# 📌 API de Tareas – ROSHKA

API RESTful desarrollada en **.NET 8**, siguiendo buenas prácticas de arquitectura en capas (**Controllers → Services → Repositories**) y utilizando **Entity Framework Core** para la persistencia de datos.  
El objetivo es gestionar **Tareas** y **Subtareas**, aplicando validaciones, logs, autenticación por API Key y documentación con **Swagger**.

---

## 🚀 Características

✅ Arquitectura en capas (Controller → Service → Repository)  
✅ Buenas prácticas REST (códigos de estado correctos, rutas limpias, verbos HTTP adecuados)  
✅ Logs informativos y de advertencia en cada endpoint  
✅ Validaciones con **FluentValidation**  
✅ Autenticación mediante **API Key** (configurable vía `.env`)  
✅ Documentación automática con **Swagger/OpenAPI**  
✅ Uso de **.env** para secretos y configuración sensible  
✅ Organización de carpetas profesional (`Core`, `Infrastructure`, `WebApi`)  

---

## 📦 Requisitos

- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/es-es/sql-server/sql-server-downloads)

---

## ⚙️ Instalación y Configuración

### 1️⃣ Clonar el repositorio

```bash
git clone https://github.com/tu-usuario/api-roshka.git
cd api-roshka
