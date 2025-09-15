docker-compose down
rem docker system prune -af
docker-compose build
docker-compose up -d
PAUSE