
# Appointment Service API

Just a service to manage your book service if you are a professional that works with 
sheduled time. It´s a simple service developed with .Netcore 3.1 and with MongoDB
as database.




## Installation

To run the project you need to have installed in your machine:
- Visual Studio 2019+ or Visual code
- [dotnet 3.1+](https://dotnet.microsoft.com/en-us/download/dotnet/3.1)
- MongoDB locally or an account in [Mongo Atlas](https://account.mongodb.com/account/login)

Clone this project with:

```bash
  git clone https://github.com/3duardonery/AppointmentsServiceApi.git
```

Or just fork this project.
## Running Locally

Get into root path of project and run this command:

```bash
  dotnet restore
```

Build project:

```bash
  dotnet build
```

Run:

```bash
  dotnet run
```



## Docs

#### Retrieve all Services

```http
  GET /api/services
```
Return all registered services

Example of response
```json
[
    {
        "id": "626015732744f077456f3fe6",
        "description": "Chief Solutions Executive",
        "duration": 60,
        "isEnabled": true
    }
]
```

| Property   | Type       | Description                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `string` | Identification of service|
| `description`      | `string` | Description of service|
| `duration`      | `int` | Duration in minutes of service|
| `isEnabled`      | `bool` | Status of service|


#### Create a new service

```http
  POST /api/services
```

Create a new service

Example of request body
```json
{
    "description": "Hair cut",
    "duration": 30,
    "isEnabled": true
}
```

| Property   | Type       | Description                                   |
| :---------- | :--------- | :------------------------------------------ |
| `description`      | `string` | Description of service|
| `duration`      | `int` | Duration in minutes of service|
| `isEnabled`      | `bool` | Status of service|

#### Retrieve all Professionals

```http
  GET /api/professional
```
Return all registered professionals

Example of response
```json
[
    {
        "id": "62601b2d060d32578ce4601c",
        "name": "Ms. Sergio Vandervort",
        "profilePicture": "http://placeimg.com/640/480",
        "isEnabled": true,
        "services": [
            {
                "id": "626015732744f077456f3fe6",
                "description": "Chief Solutions Executive",
                "duration": 60,
                "isEnabled": true
            }
        ]
    }
]
```

| Property   | Type       | Description                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `string` | Identification of service|
| `name`      | `string` | Description of service|
| `profilePicture`      | `int` | Duration in minutes of service|
| `isEnabled`      | `bool` | Status of service|
| `services`    | `object`  | Services |
| `services.id`      | `string` | Identification of service|
| `services.description`      | `string` | Description of service|
| `services.duration`      | `int` | Duration in minutes of service|
| `services.isEnabled`      | `bool` | Status of service|

#### Create a new professional

```http
  POST /api/professional
```

Create a new professional

Example of request body
```json
{
    "name": "Jhon Doe",
    "profilePicture": "http://placeimg.com/640/480",
    "isEnabled": true
}
```

| Property   | Type       | Description                                   |
| :---------- | :--------- | :------------------------------------------ |
| `name`      | `string` | Description of service|
| `profilePicture`      | `int` | Duration in minutes of service|
| `isEnabled`      | `bool` | Status of service|


#### Update professional services

```http
  PATCH /api/professional
```

Update a list of services from professional record

Example of request body
```json
{
    "professionalId": "62601b2d060d32578ce4601c",
    "serviceIds": [
        "626015732744f077456f3fe6"
    ]   
}
```

| Property   | Type       | Description                                   |
| :---------- | :--------- | :------------------------------------------ |
| `professionalId`      | `string` | professional's Id|
| `servieIds`      | `Array` | String array with services ids|


#### Retrieve all books of a service

```http
  GET /api/books?serviceId={serviceId}
```
Return all available slots of a service

| Property   | Type       | Description                                   |
| :---------- | :--------- | :------------------------------------------ |
| `serviceId`      | `string` | **Required** Identification of service|

Example of response
```json
[
    {
        "date": "2022-04-23T03:00:00Z",
        "bookDateStringValue": "23/04/2022",
        "availableHours": [
            {
                "id": "8c4b17d5-9fdc-4385-a64c-27c4cbc6d77d",
                "availableHour": "08:00",
                "professionalId": null,
                "customerId": null,
                "isCancelled": false
            },
            {
                "id": "276a1169-8031-46f7-9466-ca84188bc72d",
                "availableHour": "10:00",
                "professionalId": null,
                "customerId": null,
                "isCancelled": false
            }
        ],
        "isEnabled": true,
        "serviceReference": {
            "id": "626015732744f077456f3fe6",
            "description": "Chief Solutions Executive",
            "duration": 60,
            "isEnabled": true
        }
    }
]
```

| Property   | Type       | Description                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `string` | Identification of service|
| `date`      | `Date` | Date of a book|
| `bookDateStringValue`      | `string` | String value of date of a book|
| `isEnabled`      | `bool` | Status of service|
| `availableHours`  | `array`  | Array of all slots |
| `availableHours.id` | `string` | Id of slot | 
| `availableHours.availableHour` | `string` | Slot time |
| `availableHours.customerId` | `string` | Customer Id |
| `availableHours.isCancelled` | `bool` | Slot Status |
| `serviceReference`    | `object`  | Reference of a service |
| `services.id`      | `string` | Identification of service|
| `services.description`      | `string` | Description of service|
| `services.duration`      | `int` | Duration in minutes of service|
| `services.isEnabled`      | `bool` | Status of service|


#### Create a book for a professional and service

```http
  POST /api/books
```

Create a new book for professional

Example of request body
```json
{
    "startDate": "2022-04-23",
    "endDate": "2022-04-24",
    "startTime": "08:00:00",
    "endTime": "18:00:00",
    "serviceId": "626015732744f077456f3fe6",
    "professionalId": "62601b2d060d32578ce4601c"
}
```

| Property   | Type       | Description                                   |
| :---------- | :--------- | :------------------------------------------ |
| `startDate`      | `Date` | Start date of a book|
| `endDate`      | `Date` | End date of a book|
| `startTime`      | `string` | Start time for a schedule ex: '08:00'|
| `endTime`      | `string` | End time for a schedule ex: '18:00'|
| `serviceId`      | `string` | Service ID|
| `professionalId`      | `string` | Professional ID|

Example of response
```json
[
    {
        "date": "2022-04-23T03:00:00Z",
        "bookDateStringValue": "23/04/2022",
        "availableHours": [
            {
                "id": "8c4b17d5-9fdc-4385-a64c-27c4cbc6d77d",
                "availableHour": "08:00",
                "professionalId": null,
                "customerId": null,
                "isCancelled": false
            },
            {
                "id": "276a1169-8031-46f7-9466-ca84188bc72d",
                "availableHour": "10:00",
                "professionalId": null,
                "customerId": null,
                "isCancelled": false
            }
        ],
        "isEnabled": true,
        "serviceReference": {
            "id": "626015732744f077456f3fe6",
            "description": "Chief Solutions Executive",
            "duration": 60,
            "isEnabled": true
        }
    }
]
```

| Property   | Type       | Description                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `string` | Identification of service|
| `date`      | `Date` | Date of a book|
| `bookDateStringValue`      | `string` | String value of date of a book|
| `isEnabled`      | `bool` | Status of service|
| `availableHours`  | `array`  | Array of all slots |
| `availableHours.id` | `string` | Id of slot | 
| `availableHours.availableHour` | `string` | Slot time |
| `availableHours.customerId` | `string` | Customer Id |
| `availableHours.isCancelled` | `bool` | Slot Status |
| `serviceReference`    | `object`  | Reference of a service |
| `services.id`      | `string` | Identification of service|
| `services.description`      | `string` | Description of service|
| `services.duration`      | `int` | Duration in minutes of service|
| `services.isEnabled`      | `bool` | Status of service|
