# MazTUB-TelegramUserBot

Userbot de Telegram en C# para filtrar y procesar mensajes.

## Requisitos previos

- .NET 8 o superior
- Cuenta de Telegram

## Configuración

1. Obtén tus credentials en [https://my.telegram.org](https://my.telegram.org):
   - Ve a "API Development tools"
   - Completa el formulario con nombre y descripción
   - Obtendrás `api_id` y `api_hash`

2. Copia el archivo `.env.example` a `.env`:
   ```bash
   copy .env.example .env
   ```

3. Edita `.env` con tus valores:
   ```
   API_ID=tu_api_id_aqui
   API_HASH=tu_api_hash_aqui
   PHONE_NUMBER=tu_numero_de_telefono_con_codigo_pais #
   USER_ID=tu_usuario_id_opcional # Se obtiene automáticamente al iniciar sesión
   ```

## Ejecución

```bash
dotnet run
```

El bot se conectará a Telegram y pedirá el código de verificación la primera vez.