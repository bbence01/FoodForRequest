# Food For Request



## A projekt leírása

Weboldal, amely platformot kínál ételek rendeléséhez és azok elkészítéséhez.
A felhasználók ételigénylést hozhatnak létre, megadva az összetevőket, a szállítási és fizetési
Lehetőségeket és egyéb részleteket. Mások böngészhetik a kérések listáját, kiválaszthatják és az teljesíthetik.
Minden felhasználóknak lehetősége van új kérést vagy ajánlatot leadni.
A felhasználó majd kiválaszthatják a számára legszimpatikusabb ajánlatot.

### Componensek és Funkciók
 - **Home**
   - A kezdőlap az alkalmazás nyitóoldalaként szolgál, rövid áttekintést adva a platform céljáról és funkcióiról. 
   - Ezenkívül fontos oldalakra mutató hivatkozásokat is tartalmaz, például regisztrációt, 
   bejelentkezést és kérések létrehozását, 
   lehetővé téve a felhasználók számára, hogy gyorsan elérjék ezeket a funkciókat.
   - Gyors keresési ötletek és fő kategoriák.
   - Példa rendelések listázása. 
- **Registration**
   - A felhasználói regisztráció összetevő lehetővé teszi az új felhasználók számára.
   - Űrlap az olyan alapvető információk gyűjtésére, mint a név, e-mail cím és jelszó.
   - A komponens biztosítja az adatok érvényesítését.
   - Közösségi média regisztráció
- **Login**
    - A felhasználói bejelentkezési komponens lehetővé teszi a regisztrált felhasználók számára, 
   hogy hitelesítsék magukat, és hozzáférjenek a platform szolgáltatásaihoz. 
    - Tartalmaz egy űrlapot az e-mail és a jelszó mezőivel, 
   valamint kezeli a felhasználói hitelesítést és a token generálást.
     - Közösségi média bejelentkezés
- **Request List**
    - A kéréslista komponens megjeleníti az összes elérhető kérést a platformon,
    szűrő- és keresési funkciókat kínálva,
    hogy segítse a felhasználókat a releváns kérések hatékonyabb megtalálásában.
    - Jelenítse meg az összes elérhető ételigény listáját
    - Keresés funkció konkrét kérések megtalálásához
    - Szűrési funkciókkal szűkítheti a kéréseket olyan kritériumok alapján,
  mint a dátum, a sürgősség vagy a kézbesítési lehetőségek
- **Request Details**
    - A kérelem részletei összetevő egy adott kérés részletes nézetét mutatja be.
    - Lehetővé téve a felhasználók számára a kérés teljesítését.
    - Megjegyzések hozzáadását és egymás értékelését.    
 - **Create Request**
    - A kérés létrehozása összetevő lehetővé teszi a hitelesített felhasználók számára,
    hogy új igényt nyújtsanak be, megadva a részleteket.
    - Űrlap a tulajdonságok, összetevők, szállítási lehetőségek, 
    fizetési lehetőségek, dátum, sürgősség és ismétlődő kérések mezőivel    
 - **User Profile**
    - A felhasználói profil összetevő lényeges felhasználói információkat jelenít meg, nevet,
    e-mail-címet és a kérések listáját. 
    - Személyes irányítópultként szolgál a felhasználók számára, 
    akik kezelhetik kéréseiket és megtekinthetik fiókjuk adatait.    
 - **Request History**
    - A kéréselőzmények összetevő megjeleníti a felhasználó korábbi kérések és teljesítési listáját,
    áttekintést adva a platformon végzett tevékenységükről. 
    
    
    
### Technológiák és keretrendszerek

- **Frontend**
  - Angular
  - Bootstrap 
  - Tailwind CSS
  - Sass CSS
  - RxJS
  - HttpClient
  
- **Backend**
  - ASP.NET Core 6.0
  - Entity Framework Core
  - SQL Server
  - Swashbuckle.AspNetCore

### Releváns API Végpontok


- **Register a user**
  - Endpoint: POST /api/users/register
    - Input: { "email": "user@example.com", "password": "password", "name": "John Doe" }
    - Return: { "message": "User registered successfully", "userId": 1 }
    
- **Authenticate a user**
  - Endpoint: POST /api/users/login
    - Input: { "email": "user@example.com", "password": "password" }
    - Return: { "message": "User authenticated", "userId": 1, "token": "abcd1234" }
    
- **Get user profile**
  - Endpoint: GET /api/users/profile/:userId
    - Input: userId
    - Return: { "name": "John Doe", "email": "user@example.com", "profilePicture": "path/to/pic.jpg" }

- **Create a food request**
  - Endpoint: POST /api/requests/create
    - Input: { "userId": 1, "properties": "Vegan", "ingredients": "Avocado, lettuce", "deliveryOptions": "Pickup", "paymentOptions": "Cash", "date": "2023-05-05", "urgency": "High", "reoccurring": "Weekly" }
    - Return: { "message": "Request created successfully", "requestId": 1 }

- **Get all food requests**
  - Endpoint: GET /api/requests
    - Input: None
    - Return: [ { "requestId": 1, "userId": 1, "properties": "Vegan", "ingredients": "Avocado, lettuce", "deliveryOptions": "Pickup", "paymentOptions": "Cash", "date": "2023-05-05", "urgency": "High", "reoccurring": "Weekly" } ]

- **Get a single food request**
  - Endpoint: GET /api/requests/:requestId
    - Input: requestId
    - Return: { "requestId": 1, "userId": 1, "properties": "Vegan", "ingredients": "Avocado, lettuce", "deliveryOptions": "Pickup", "paymentOptions": "Cash", "date": "2023-05-05", "urgency": "High", "reoccurring": "Weekly" }

- **Fulfill a food request**
  - Endpoint: PUT /api/requests/fulfill/:requestId
    - Input: requestId
    - Return: { "message": "Request fulfilled successfully" }

- **Rate a user**
   - Endpoint: POST /api/ratings
     - Input: { "userId": 1, "ratedUserId": 2, "rating": 4 }
     - Return: { "message": "User rated successfully" }
    
- **Create a new fulfillment request**
  - Endpoint: POST /api/fulfillmentrequests
    - Input: { "requestId": 1, "userId": 2 }
    - Return: { "message": "Fulfillment request created successfully", "fulfillmentRequestId": 1 }
   
- **Update the status of a fulfillment request**
  - Endpoint: PUT /api/fulfillmentrequests/:id
    - Input: { "id": 1, "status": "Accepted" }
    - Return: { "message": "Fulfillment request updated successfully" }

- **Authentication Login**
  - Endpoint: POST /api/Auth
    - Input: {  "userName": "string",  "password": "string" }
    - Return: { "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhNWJjZmY5Mi1jMGJjLTRlY2EtODE1NS1hZjJlOTY1MjBhNjAiLCJuYW1lIjoic2VlZHBsYXllckBnbWFpbC5jb20iLCJuYW1laWQiOiJhNWJjZmY5Mi1jMGJjLTRlY2EtODE1NS1hZjJlOTY1MjBhNjAiLCJleHAiOjE2ODI5NzA4MDcsImlzcyI6Imh0dHA6Ly93d3cuc2VjdXJpdHkub3JnIiwiYXVkIjoiaHR0cDovL3d3dy5zZWN1cml0eS5vcmcifQ.phM71KtceLt-8GHwstPZbbXrnrpJ0LiswobmKpiwMuc", 
  "expiration": "2023-05-01T19:53:27Z", 
  "id": "a5bcff92-c0bc-4eca-8155-af2e96520a60" } 
    
- **Authentication Register**
  - Endpoint: POST /api/Auth/register
    - Input: {  "email": "string",  "playerName": "string",  "password": "string"}
    - Return: { "message": "User registered" }
    
- **Authentication Logout**
  - Endpoint: POST /api/Auth/logout
    - Input: {  }
    - Return: { "message": "User Logged out" }

### Rendelés Lebonyolítása

- A felhasználó a Kérelemlista összetevőben elérhető szűrők és keresési funkciók segítségével keres egy kérést.
- A felhasználó kiválaszt egy kérést, és egy gombra kattintva jelentkezhet teljesítésre.
- A gombra kattintva egy új FulfillmentRequest jön létre a vonatkozó adatokkal 
(pl. RequestId, UserId és Status), és elmentődik az adatbázisban.
- Az eredeti kérelmet létrehozó B felhasználó értesítést kap 
(akár a weboldalon profiljában, akár e-mailben) az új teljesítési kérelemről.
- B felhasználó elfogadhat egy teljesítési kérelmet, és elutasíthatja a többit. 
Az elfogadott FulfillmentRequest állapota „Készítés Allatt”-ra változik, míg a megtagadtak „Elutasított”-ra.
- Az A felhasználó, akinek a FulfillmentRequest elfogadásra került,
értesítést kap az elfogadásról, míg a többi felhasználó értesítést kap az elutasításról.
- A felhasználó megkezdi a kérés teljesítését, és a teljesítés után
frissítheti a FulfillmentRequest állapotát „Kész”-re.
- B felhasználó értesítést kap arról, 
hogy a kérelmet teljesítették, és áttekintheti az eredményt.
- A vizetési és szállítási feltételeket az eredeti kérésben meg határozottak szerint követik.
- Mind A, mind B felhasználó értékelheti egymást, 
és visszajelzést adhat a teljesítési folyamat befejezése után.



[Figma mockup](https://www.figma.com/file/64peNylyskwb2UMwDFDooy/Untitled?type=design&node-id=0%3A1&t=cRTFXqfoCfneD3Lr-1)

![Home](./images/Home.JPG)
![Requests](./images/Requests.JPG)
![Request](./images/Request.JPG)
![newRequest](./images/newRequest.JPG)
![user](./images/user.JPG)
