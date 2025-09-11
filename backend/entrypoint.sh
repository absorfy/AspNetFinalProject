#!/bin/sh
echo "=== ENTRYPOINT STARTED ==="

until dotnet ef database update; do
  echo "Waiting for database migrations..."
  sleep 2
done

echo "=== MIGRATIONS APPLIED ==="

# запускаємо бекенд
exec dotnet watch run --urls http://0.0.0.0:5134
