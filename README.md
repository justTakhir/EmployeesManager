# Employees Manager

Приложение по управлению персоналом, позволяющее:
1. Нанимать новых сотрудников;
2. Увольнять сотрудников и назначать новых на их место;
3. Просматривать всех сотрудников посредством удобного графического интерфейса.

## Если у вас есть Docker Compose
1. Открыть консоль в папке с решением.
2. docker compose up

## Если у вас нет Docker или Docker Compose

### Start Backend

1. Открыть консоль в папке с решением.
2. dotnet restore
2. cd .\EmployeesManager\
3. dotnet publish -c release -o .\app
4. $Env:CONNECTION_STRING="User ID=postgres;Password=mysecretpassword;Host=localhost;Port=5432;Database=initdb;"
5. cd .\app\
6. .\EmployeesManager.exe

### Start Frontend

1. cd .\EmployeesManager\ClientApp\
2. (Optional) $Env:REACT_APP_PROXY_HOST="http://localhost:5000"
3. npm start

## Generate Data
1. cd .\EmployeesManager\Scripts\
2. pip install -r requirements.txt
3. python generate_database.py
4. Если используется Docker Compose, то хост - http://localhost:7245, а при ручной запуске - http://localhost:5000
